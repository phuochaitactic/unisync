import { Component, OnInit, Input } from "@angular/core";
import { ApiService } from "src/app/corelogic/model/common/admin/api.service";
import { QuanLySinhVien } from "src/app/corelogic/interface/admin/quanlysv.model";
import { QuanlysinhvienService } from "src/app/corelogic/model/common/admin/quanlysinhvien.service";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";
import { DatePipe } from "@angular/common";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-quanlysinhvien",
  templateUrl: "./quanlysinhvien.component.html",
  styleUrls: ["./quanlysinhvien.component.css"],
})
export class QuanlysinhvienComponent implements OnInit {
  title = "Quản Lý Sinh Viên";
  showpassword: boolean = false;
  quanLySVData: QuanLySinhVien[] = [];
  initialData: QuanLySinhVien[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  bc = "";
  tt = "";
  trangthaiSV: string = "";
  tenLop: string = "";
  addSV = new QuanLySinhVien();
  editingSV: QuanLySinhVien = new QuanLySinhVien();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  firstSVData: any[] = [];
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namett = "Trạng Thái";
  namel = "Lớp";
  namebcs = "Ban Cán Sự";
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
  dropNHHK: any;
  dropGV: any;
  dropLop: any;
  showOptions: boolean = false;
  showOptionsLop: boolean = false;
  showOptionsGV: boolean = false;
  selectedOption: string = "";
  selectedOptionLop: string = "";
  selectedOptionGV: string = "";
  showOptionsENH: boolean = false;
  showOptionsELop: boolean = false;
  showOptionsEGV: boolean = false;
  selectedOptionENH: string = "";
  selectedOptionEGV: string = "";
  selectedOptionELop: string = "";
  activeButton: string | null = null;
  filter1 = false;
  filter2 = false;
  filter3 = false;
  showdetail = false;
  dropdownVT = false;
  selectedOptionVT = "";
  dropdownVTE = false;
  selectedOptionVTE = "";
  currentDate: any;

  constructor(
    private sinhvienService: QuanlysinhvienService,
    private api: ApiService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.sinhvienService.getSVData().subscribe((data) => {
      this.quanLySVData = data.data;
      this.api.getLopData().subscribe((data) => {
        this.dropLop = data.data;
      });
      this.api.getNHData().subscribe((data) => {
        this.dropNHHK = data.data;
      });
      this.api.getGVData().subscribe((data) => {
        this.dropGV = data.data;
      });
      this.selectedItems = new Array(data.length).fill(false);
      this.initialData = [...this.quanLySVData];
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.api.getNHData().subscribe((nhhk) => {
      this.dropNHHK = nhhk;
    });
    this.api.getGVData().subscribe((gv) => {
      this.dropGV = gv;
    });
    this.api.getLopData().subscribe((lop) => {
      this.dropLop = lop;
    });
    this.loadData();
  }

  checkDay() {
    const today = new Date();
    const day = String(today.getDate()).padStart(2, "0");
    const month = String(today.getMonth() + 1).padStart(2, "0");
    const year = today.getFullYear();

    this.currentDate = `${year}-${month}-${day}`;
  }

  showDropdownVT() {
    this.dropdownVT = !this.dropdownVT;
  }
  selectOptionVT() {
    this.selectedOptionVT = "Đang Học";
    this.addSV.trangThaiSinhVien = "Đang học";
    this.dropdownVT = false;
  }
  selectOptionVT1() {
    this.selectedOptionVT = "Thôi học";
    this.addSV.trangThaiSinhVien = "Thôi học";
    this.dropdownVT = false;
  }

  showDropdownVTE() {
    this.dropdownVTE = !this.dropdownVTE;
  }
  selectOptionVTE() {
    this.selectedOptionVTE = "Đang Học";
    this.editingSV.trangThaiSinhVien = "Đang học";
    this.dropdownVTE = false;
  }
  selectOptionVTE1() {
    this.selectedOptionVTE = "Thôi học";
    this.editingSV.trangThaiSinhVien = "Thôi học";
    this.dropdownVTE = false;
  }

  toogleClick() {
    this.showpassword = !this.showpassword;
  }

  resetSelection() {
    this.selectedItems = new Array(this.quanLySVData.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }

  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.quanLySVData.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.quanLySVData.length;
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
        const selectedLop = this.quanLySVData[index];
        if (selectedLop) {
          this.sinhvienService
            .deleteSV(selectedLop.idsinhVien)
            .subscribe(() => {
              this.quanLySVData = this.quanLySVData.filter(
                (lop) => lop.idsinhVien !== selectedLop.idsinhVien
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

  showExcels() {
    this.showExcel = true;
  }

  closeExcels() {
    this.showExcel = false;
  }

  cautrucExcel() {
    const element = document.getElementById("excel-table");
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    XLSX.writeFile(wb, "File_QuanLySinhVien.xlsx");
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
        }) as QuanLySinhVien[];
        for (const item of excelData) {
          item.ngaySinh = this.convertDateFormat(item.ngaySinh);
          if (typeof item.phai === "string") {
            item.phai = item.phai === "TRUE";
          }
          if (typeof item.isBanCanSu === "string") {
            item.isBanCanSu = item.isBanCanSu === "TRUE";
          }
          if (
            item.tenLop &&
            item.tenGv &&
            item.phai !== undefined &&
            item.isBanCanSu !== undefined &&
            item.ngaySinh &&
            item.tenNhhk
          ) {
            const tenLop = item.tenLop;
            const tenNHHK = item.tenNhhk;
            const tenGiangVien = item.tenGv;
            this.sinhvienService
              .postDataToDatabase(item, tenLop, tenNHHK, tenGiangVien)
              .pipe(
                catchError((error: HttpErrorResponse) => {
                  if (error.status === 400) {
                    this.toast.error(
                      "Dữ liệu không hợp lệ. Vui lòng kiểm tra và thử lại."
                    );
                  } else {
                    this.toast.error(
                      "Có lỗi khi thêm dữ liệu. Vui lòng thử lại."
                    );
                  }
                  return throwError(error);
                })
              )
              .subscribe((data) => {
                this.loadData();
              });
          }
        }
        this.toast.success("Thêm dữ liệu thành công");
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
    const dataWithoutId = this.quanLySVData.map((item) => {
      const { idsinhVien, ngaySinh, ...rest } = item;
      const dateFormat = /\d{4}-\d{2}-\d{2}/; // YYYY-MM-DD
      const isValidFormat = dateFormat.test(ngaySinh);

      const batDauDate = isValidFormat
        ? new Date(ngaySinh)
        : new Date(ngaySinh.replace(/(\d{2})-(\d{2})-(\d{4})/, "$3-$2-$1"));

      return { ...rest, ngayBatDau: batDauDate };
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["A1"].v = "Mã sinh viên";
    worksheet["B1"].v = "Họ tên sinh viên";
    worksheet["C1"].v = "Mật khẩu";
    worksheet["D1"].v = "Phái";
    worksheet["E1"].v = "Trạng thái sinh viên";
    worksheet["F1"].v = "Ban cán sự";
    worksheet["G1"].v = "Tên giảng viên";
    worksheet["H1"].v = "Tên lớp";
    worksheet["I1"].v = "Tên NHHK";
    worksheet["J1"].v = "Liên Hệ";
    worksheet["K1"].v = "Ngày bắt đầu";

    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlysinhvien.xlsx");
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
    this.quanLySVData.sort((a: any, b: any) => {
      const comparison = a.hoTenSinhVien.localeCompare(b.hoTenSinhVien);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortMaSV() {
    this.isMaSV = !this.isMaSV;
    this.isIcon = !this.isIcon;

    this.quanLySVData.sort((a: any, b: any) => {
      if (this.isMaSV) {
        return a.maSinhVien.localeCompare(b.maSinhVien);
      } else {
        return b.maSinhVien.localeCompare(a.maSinhVien);
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
    this.sinhvienService.getSVData().subscribe((data) => {
      this.resetSelection();
      this.quanLySVData = data.data;
      this.checkExport = this.quanLySVData.length > 0;
      this.loading = false;
    });
  }

  filterBanCanSu(status: boolean | null) {
    if (status === true) {
      this.namebcs = "Chức Vụ: Lớp Trưởng";
    } else if (status === false) {
      this.namebcs = "Chức Vụ: Không";
    } else {
      this.namebcs = "Tất Cả Chức Vụ";
    }
    if (status === null) {
      this.quanLySVData = [...this.initialData];
    } else {
      this.quanLySVData = this.initialData.filter(
        (sinhvien) => sinhvien.isBanCanSu === status
      );
    }
  }

  filterStatus(status: any) {
    this.tt = status;
    this.namett = "Trạng Thái: " + status;
  }
  resetStatus() {
    this.tt = "";
    this.namett = "Tất Cả Trạng Thái";
  }

  filterSV(slectedSV: any) {
    this.tenLop = slectedSV;
    this.namel = "Lớp: " + slectedSV;
  }
  resetFilter() {
    this.tenLop = "";
    this.namel = "Tất Cả Lớp";
  }
  resetModal() {
    this.addSV = new QuanLySinhVien();
    this.showModal = false;
  }

  addSinhVienMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.addSV.hoTenSinhVien ||
      !this.addSV.ngaySinh ||
      !this.addSV.tenLop ||
      !this.addSV.trangThaiSinhVien ||
      !this.addSV.maSinhVien ||
      !this.addSV.tenNhhk ||
      !this.addSV.tenGv ||
      !this.addSV.matKhau ||
      this.addSV.phai === null ||
      this.addSV.isBanCanSu === null
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addSV.hoTenSinhVien)) {
      this.toast.warning("Họ tên sinh viên không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addSV.tenLop)) {
      this.toast.warning("Tên lớp không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addSV.maSinhVien)) {
      this.toast.warning("Mã sinh viên không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addSV.tenGv)) {
      this.toast.warning("Tên giảng viên không được chứa ký tự đặc biệt!");
      return;
    }

    if (/\d/.test(this.addSV.hoTenSinhVien)) {
      this.toast.warning("Vui lòng không nhập số trong họ và tên sinh viên .");
      return;
    }
    if (/\d/.test(this.addSV.tenGv)) {
      this.toast.warning("Vui lòng không nhập số trong tên giảng viên .");
      return;
    }

    if (kytudacbietngayRegex.test(this.addSV.tenNhhk)) {
      this.toast.warning("Tên năm học-học kỳ không được chứa ký tự đặc biệt!");
      return;
    }
    const tenLop = this.addSV.tenLop;
    const tenNHHK = this.addSV.tenNhhk;
    const tenGiangVien = this.addSV.tenGv;
    this.sinhvienService
      .postThemSV(this.addSV, tenLop, tenNHHK, tenGiangVien)
      .subscribe((data) => {
        this.addSV = new QuanLySinhVien();
        this.toast.success("Thêm thành công");
        this.loadData();
        this.resetModal();
        this.closeModalAdd();
      });
  }

  detail(lop: QuanLySinhVien) {
    this.editingSV = { ...lop };
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
  }
  editSV(sinhvien: QuanLySinhVien) {
    this.editingSV = { ...sinhvien };
    this.showModal = true;
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
  onNgaySinhChange(event: any) {
    this.editingSV.ngaySinh = event.target.value;
  }
  updateSV() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.editingSV.hoTenSinhVien ||
      !this.editingSV.ngaySinh ||
      !this.editingSV.tenLop ||
      !this.editingSV.trangThaiSinhVien ||
      !this.editingSV.maSinhVien ||
      !this.editingSV.tenGv ||
      !this.editingSV.matKhau ||
      !this.editingSV.tenNhhk ||
      this.editingSV.phai === null ||
      this.editingSV.isBanCanSu === null
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingSV.hoTenSinhVien)) {
      this.toast.warning("Họ tên sinh viên không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingSV.tenLop)) {
      this.toast.warning("Tên lớp không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingSV.maSinhVien)) {
      this.toast.warning("Mã sinh viên không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingSV.tenGv)) {
      this.toast.warning("Tên giảng viên không được chứa ký tự đặc biệt!");
      return;
    }

    if (/\d/.test(this.editingSV.hoTenSinhVien)) {
      this.toast.warning("Vui lòng không nhập số trong họ và tên sinh viên .");
      return;
    }
    if (/\d/.test(this.editingSV.tenGv)) {
      this.toast.warning("Vui lòng không nhập số trong tên giảng viên .");
      return;
    }
    if (kytudacbietngayRegex.test(this.editingSV.tenNhhk)) {
      this.toast.warning("Tên năm học-học kỳ không được chứa ký tự đặc biệt!");
      return;
    }
    const tenLop = this.editingSV.tenLop;
    const tenNHHK = this.editingSV.tenNhhk;
    const tenGiangVien = this.editingSV.tenGv;
    if (this.editingSV) {
      this.sinhvienService
        .putEditSV(this.editingSV, tenLop, tenNHHK, tenGiangVien)
        .subscribe((data) => {
          const index = this.quanLySVData.findIndex(
            (k) => k.idsinhVien === this.editingSV.idsinhVien
          );
          if (index !== -1) {
            this.quanLySVData[index] = { ...this.editingSV };
          }
          this.toast.success("Cập nhật thành công");
          this.loadData();
          this.closeModal();
        });
    }
  }

  onDelete(idsinhVien: any) {
    this.selectedLop = idsinhVien;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteSV() {
    if (this.selectedLop) {
      this.sinhvienService.deleteSV(this.selectedLop).subscribe(() => {
        this.quanLySVData = this.quanLySVData.filter(
          (lop) => lop.idsinhVien !== this.selectedLop
        );
        this.selectedLop = null;
        this.loadData();
        this.ondelete = false;
        this.toast.success("Xoá thành công");
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

  toggleOptions() {
    this.showOptions = !this.showOptions;
  }

  selectOption(option: string) {
    this.selectedOption = option;
    this.addSV.tenNhhk = option;
    this.showOptions = false;
  }
  toggleOptionsLop() {
    this.showOptionsLop = !this.showOptionsLop;
  }

  selectOptionLop(option: string) {
    this.selectedOptionLop = option;
    this.addSV.tenLop = option;
    this.showOptionsLop = false;
  }

  toggleOptionsGV() {
    this.showOptionsGV = !this.showOptionsGV;
  }

  selectOptionGV(option: string) {
    this.selectedOptionGV = option;
    this.addSV.tenGv = option;
    this.showOptionsGV = false;
  }
  toggleOptionsELop() {
    this.showOptionsELop = !this.showOptionsELop;
  }

  selectOptionELop(option: string) {
    this.selectedOptionELop = option;
    this.editingSV.tenLop = option;
    this.showOptionsELop = false;
  }
  toggleOptionsENH() {
    this.showOptionsENH = !this.showOptionsENH;
  }

  selectOptionENH(option: string) {
    this.selectedOptionENH = option;
    this.editingSV.tenNhhk = option;
    this.showOptionsENH = false;
  }
  toggleOptionsEGV() {
    this.showOptionsEGV = !this.showOptionsEGV;
  }

  selectOptionEGV(option: string) {
    this.selectedOptionEGV = option;
    this.editingSV.tenGv = option;
    this.showOptionsEGV = false;
  }
  Date(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().split("T")[0];
  }
  toggleDropDown() {
    this.filter1 = !this.filter1;
    this.filter2 = false;
    this.filter3 = false;
    this.activeButton = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.filter1 = false;
    this.filter3 = false;
    this.activeButton = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
  toggleDropDown3() {
    this.filter3 = !this.filter3;
    this.filter1 = false;
    this.filter2 = false;
    this.activeButton = "FT Tên";
    if (this.filter3 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
}
