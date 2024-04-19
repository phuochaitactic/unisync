import { Component, OnInit } from "@angular/core";
import { QuanlynhhkService } from "src/app/corelogic/model/common/admin/quanlynhhk.service";
import { QuanLyNHHK } from "src/app/corelogic/interface/admin/quanlynhhk.model";
import { initFlowbite } from "flowbite";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import * as XLSX from "xlsx";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-quanlynhhk",
  templateUrl: "./quanlynhhk.component.html",
  styleUrls: ["./quanlynhhk.component.css"],
})
export class QuanlynhhkComponent implements OnInit {
  title = "Quản Lý Năm Học - Học Kỳ";
  showpassword: boolean = false;
  quanLyLopData: QuanLyNHHK[] = [];
  initialData: QuanLyNHHK[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  khoa: string = "";
  TuanBatDau: string = "";
  TenNhhk: string = "";
  addLop = new QuanLyNHHK();
  editingLop: QuanLyNHHK = new QuanLyNHHK();
  chitietLop: QuanLyNHHK = new QuanLyNHHK();
  showchitiet = false;
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  isIcon3 = true;
  isIcon4 = true;
  isIcon5 = true;
  isIcon6 = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  tuan = "Tuần Bắt Đầu";
  nam = "Số Tuần Học";
  showExcel: boolean = false;
  checkDowload = false;
  checkimport = false;
  selectedFileName: string = "";
  ondelete: boolean = false;
  selectedLop: any;
  loading = false;
  showDeleteAllButton: boolean = false;
  selectedRowCount: number = 0;
  onalldelete = false;
  checkExport = false;
  activeButton: string | null = null;
  filter1 = false;
  filter2 = false;
  showdetail = false;
  currentDate: any;

  constructor(
    private nhhkService: QuanlynhhkService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.nhhkService.getNHHKData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.initialData = [...this.quanLyLopData];

      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.checkDay();
    this.loadData();
  }

  checkDay() {
    const today = new Date();
    const day = String(today.getDate()).padStart(2, "0");
    const month = String(today.getMonth() + 1).padStart(2, "0");
    const year = today.getFullYear();

    this.currentDate = `${year}-${month}-${day}`;
  }

  toogleClick() {
    this.showpassword = !this.showpassword;
  }
  resetSelection() {
    this.selectedItems = new Array(this.quanLyLopData.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }

  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.quanLyLopData.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.quanLyLopData.length;
    if (this.checkAll) {
      this.selectedItems = new Array(selectedCount).fill(true);
      this.selectedRowCount = selectedCount;
    } else {
      this.selectedItems = new Array(selectedCount).fill(false);
      this.selectedRowCount = 0;
    }
    this.showDeleteAllButton = this.checkIfAnyRowSelected();
  }

  deleteSelectedRows(): void {
    const selectedRows = this.selectedItems
      .map((selected, index) => (selected ? index : -1))
      .filter((index) => index !== -1);
    if (selectedRows.length > 0) {
      selectedRows.forEach((index) => {
        const selectedLop = this.quanLyLopData[index];
        if (selectedLop) {
          this.nhhkService.deleteNH(selectedLop.idnhhk).subscribe(() => {
            this.quanLyLopData = this.quanLyLopData.filter(
              (lop) => lop.idnhhk !== selectedLop.idnhhk
            );
            this.selectedItems[index] = false;
            this.selectedRowCount--;
            this.showDeleteAllButton = this.selectedRowCount > 0;
            this.checkAll = false;
            this.loadData();
          });
        }
      });
    }
  }

  onDowload() {
    this.checkDowload = true;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];

    this.selectedFileName = file ? file.name : "";
    this.checkimport = !!this.selectedFileName;
  }

  onContinue() {
    if (this.checkimport) {
      this.importExcel();
      this.closeExcels();
    }
  }

  sortSTT() {}
  showExcels() {
    this.showExcel = true;
  }

  closeExcels() {
    this.showExcel = false;
  }

  showInfo(lop: QuanLyNHHK) {
    this.chitietLop = { ...lop };
    this.showchitiet = true;
  }
  cautrucExcel() {
    const element = document.getElementById("excel-table");
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    XLSX.writeFile(wb, "File_QuanLyNamHocHocKy.xlsx");
  }

  importExcel() {
    const fileInput = document.getElementById(
      "dropzone-file"
    ) as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      const file = fileInput.files[0];
      const fileReader = new FileReader();

      fileReader.onload = (e: any) => {
        const binaryString = fileReader.result;
        const workbook = XLSX.read(binaryString, { type: "binary" });
        const sheetNameList = workbook.SheetNames;
        const sheetName = sheetNameList[0];
        const excelData = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName], {
          raw: false,
        }) as QuanLyNHHK[];

        for (const item of excelData) {
          item.ngayBatDau = this.convertDateFormat(item.ngayBatDau);
          item.ngayKetThuc = this.convertDateFormat(item.ngayKetThuc);
          this.nhhkService
            .postDataToDatabase(item)
            .pipe(
              catchError((error) => {
                if (error.status === 400) {
                  this.toastr.error(
                    "Dữ liệu không hợp lệ. Vui lòng kiểm tra và thử lại."
                  );
                  return error;
                } else {
                  this.toastr.error(
                    "Có lỗi khi thêm dữ liệu. Vui lòng thử lại."
                  );
                  return error;
                }
              })
            )
            .subscribe(() => {
              this.loadData();
            });
        }
        this.toastr.success("Thêm dữ liệu thành công");
      };

      fileReader.readAsBinaryString(file);
    }
  }

  convertDateFormat(dateString: string): string {
    const parts = dateString.split("/");
    let year = parseInt(parts[2], 10);
    if (year < 100) {
      year += 2000;
    }
    const formattedDate = year + "-" + parts[1] + "-" + parts[0];
    return formattedDate;
  }
  exportToExcel() {
    const dataWithoutId = this.quanLyLopData.map((item) => {
      const { idnhhk, ngayBatDau, ngayKetThuc, ...rest } = item;
      const dateFormat = /\d{4}-\d{2}-\d{2}/; // YYYY-MM-DD
      const isValidFormat =
        dateFormat.test(ngayBatDau) && dateFormat.test(ngayKetThuc);

      const batDauDate = isValidFormat
        ? new Date(ngayBatDau)
        : new Date(ngayBatDau.replace(/(\d{2})-(\d{2})-(\d{4})/, "$3-$2-$1"));
      const ketThucDate = isValidFormat
        ? new Date(ngayKetThuc)
        : new Date(ngayKetThuc.replace(/(\d{2})-(\d{2})-(\d{4})/, "$3-$2-$1"));

      return { ...rest, ngayBatDau: batDauDate, ngayKetThuc: ketThucDate };
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["A1"].v = "Mã NHHK";
    worksheet["B1"].v = "Tên NHHK";
    worksheet["C1"].v = "Tuần bắt đầu";
    worksheet["D1"].v = "Số tuần trong học kì";
    worksheet["E1"].v = "Ngày bắt đầu";
    worksheet["F1"].v = "Ngày kết thúc";
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlynamhoc.xlsx");
  }

  saveAsExcelFile(buffer: any, fileName: string) {
    const data: Blob = new Blob([buffer], { type: "application/octet-stream" });
    const url: string = window.URL.createObjectURL(data);
    const a: HTMLAnchorElement = document.createElement("a");
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    window.URL.revokeObjectURL(url);
  }

  sortDataBySinhVien(order: "asc" | "desc" = "asc") {
    this.quanLyLopData.sort((a: any, b: any) => {
      const comparison = a.tenNhhk.localeCompare(b.tenNhhk);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortMaSV() {
    this.isMaSV = !this.isMaSV;
    this.isIcon = !this.isIcon;

    this.quanLyLopData.sort((a: any, b: any) => {
      if (this.isMaSV) {
        return a.maNhhk.localeCompare(b.maNhhk);
      } else {
        return b.maNhhk.localeCompare(a.maNhhk);
      }
    });
  }

  sortTenSV() {
    this.isTenSV = !this.isTenSV;
    this.isIcons = !this.isIcons;
    this.sortDataBySinhVien(this.isTenSV ? "asc" : "desc");
  }
  loadData() {
    this.loading = true;
    this.nhhkService.getNHHKData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }
  filterNam(slectedNK: any) {
    this.tuan = "Tuần Bắt Đầu: " + slectedNK;
    this.TuanBatDau = slectedNK;
  }
  resetFilterNK() {
    this.TuanBatDau = "";
    this.tuan = "Tuần Bắt Đầu";
  }

  filterLop(slectedLop: any) {
    this.nam = "Tuần Học: " + slectedLop;
    this.TenNhhk = slectedLop;
  }
  resetFilter() {
    this.TenNhhk = "";
    this.nam = "Số Tuần Học";
  }
  resetModal() {
    this.addLop = new QuanLyNHHK();
    this.showModal = false;
  }
  onNgaySinhChanges(event: any) {
    this.editingLop.ngayKetThuc = event.target.value;
  }
  onNgaySinhChange(event: any) {
    this.editingLop.ngayBatDau = event.target.value;
  }
  addNhhkMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.addLop.maNhhk ||
      !this.addLop.tenNhhk ||
      !this.addLop.ngayBatDau ||
      !this.addLop.tuanBatDau ||
      !this.addLop.soTuanHk
    ) {
      this.toastr.warning("Vui lòng không để trống thông tin");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maNhhk)) {
      this.toastr.warning("Mã năm học-học kỳ không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.addLop.maNhhk))) {
      this.toastr.warning("Mã năm học-học kỳ phải là kiểu số!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenNhhk)) {
      this.toastr.warning("Tên năm học-học kỳ không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tuanBatDau)) {
      this.toastr.warning("Tuần bắt đầu không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.soTuanHk)) {
      this.toastr.warning("Số tuần không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.addLop.soTuanHk))) {
      this.toastr.warning("Số tuần phải là một số!");
      return;
    }
    if (isNaN(Number(this.addLop.tuanBatDau))) {
      this.toastr.warning("Tuần bắt đầu phải là một số!");
      return;
    }
    this.nhhkService.postThemNH(this.addLop).subscribe((data) => {
      this.showModal = false;
      this.toastr.success("Thêm thành công");
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editNhhk(lop: QuanLyNHHK) {
    this.editingLop = { ...lop };
    this.showModal = true;
  }
  detail(lop: QuanLyNHHK) {
    this.editingLop = { ...lop };
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
  }
  closeModal() {
    this.showModal = false;
  }

  showAdd() {
    this.showModalAdd = true;
  }
  closeModalAdd() {
    this.showModalAdd = false;
  }

  updateNhhk() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.editingLop.maNhhk ||
      !this.editingLop.tenNhhk ||
      !this.editingLop.ngayBatDau ||
      !this.editingLop.tuanBatDau ||
      !this.editingLop.soTuanHk
    ) {
      this.toastr.warning("Vui lòng không để trống thông tin");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maNhhk)) {
      this.toastr.warning("Mã năm học-học kỳ không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.editingLop.maNhhk))) {
      this.toastr.warning("Mã năm học-học kỳ phải là kiểu số!");
      return;
    }
    if (kytudacbietngayRegex.test(this.editingLop.tenNhhk)) {
      this.toastr.warning("Tên năm học-học kỳ không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tuanBatDau)) {
      this.toastr.warning("Tuần bắt đầu không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.soTuanHk)) {
      this.toastr.warning("Số tuần không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.editingLop.soTuanHk))) {
      this.toastr.warning("Số tuần phải là một số!");
      return;
    }
    if (isNaN(Number(this.editingLop.tuanBatDau))) {
      this.toastr.warning("Tuần bắt đầu phải là một số!");
      return;
    }
    if (this.editingLop) {
      this.nhhkService.putEditNH(this.editingLop).subscribe((data) => {
        const index = this.quanLyLopData.findIndex(
          (k) => k.idnhhk === this.editingLop.idnhhk
        );
        if (index !== -1) {
          this.quanLyLopData[index] = { ...this.editingLop };
        }
        this.toastr.success("Cập nhật thành công");
        this.loadData();
        this.closeModal();
      });
    }
  }

  onDelete(idnhhk: any) {
    this.selectedLop = idnhhk;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteNhhk() {
    if (this.selectedLop) {
      this.nhhkService.deleteNH(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idnhhk !== this.selectedLop
        );
        this.selectedLop = null;
        this.ondelete = false;
        this.toastr.success("Xoá thành công");
        this.loadData();
      });
    }
  }

  onDeleteAll() {
    this.onalldelete = true;
  }
  closeDeleteAll() {
    this.onalldelete = false;
  }
  deleteLopAll() {
    this.deleteSelectedRows();
    this.onalldelete = false;
  }

  toggleDropDown() {
    this.filter1 = !this.filter1;
    this.filter2 = false;
    this.activeButton = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.filter1 = false;
    this.activeButton = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
  clickIcon3() {
    this.isIcon3 = !this.isIcon3;
  }
  clickIcon4() {
    this.isIcon4 = !this.isIcon4;
  }
  clickIcon5() {
    this.isIcon5 = !this.isIcon5;
  }
  clickIcon6() {
    this.isIcon6 = !this.isIcon6;
  }
}
