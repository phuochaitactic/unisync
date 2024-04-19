import { Component, OnInit } from "@angular/core";
import { QuanlyhoatdongnkService } from "src/app/corelogic/model/common/admin/quanlyhoatdongnk.service";
import { QuanLyHDNK } from "src/app/corelogic/interface/admin/quanlyhdnk.model";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { HttpErrorResponse } from "@angular/common/http";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";
import { QuanLyNHHK } from "src/app/corelogic/interface/admin/quanlynhhk.model";

@Component({
  selector: "app-quanlyhoatdongnk",
  templateUrl: "./quanlyhoatdongnk.component.html",
  styleUrls: ["./quanlyhoatdongnk.component.css"],
})
export class QuanlyhoatdongnkComponent implements OnInit {
  title = "Quản Lý Hoạt Động Ngoại khoá";
  showpassword: boolean = false;
  quanLyLopData: QuanLyHDNK[] = [];
  initialData: QuanLyHDNK[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  nk: string = "";
  addLop = new QuanLyHDNK();
  editingLop: QuanLyHDNK = new QuanLyHDNK();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namehdnk = "Hoạt Động Ngoại Khoá";
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

  constructor(
    private hoatdongnkService: QuanlyhoatdongnkService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.hoatdongnkService.getHDNKData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.selectedItems = new Array(data.length).fill(false);
      this.initialData = [...this.quanLyLopData];
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.loadData();
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
          this.hoatdongnkService
            .deleteHDNK(selectedLop.idhdnk)
            .subscribe(() => {
              this.quanLyLopData = this.quanLyLopData.filter(
                (lop) => lop.idhdnk !== selectedLop.idhdnk
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
  detail(lop: QuanLyHDNK) {
    this.editingLop = { ...lop };
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
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
    XLSX.writeFile(wb, "File_QuanLyHoatDongNK.xlsx");
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
        }) as QuanLyHDNK[];
        for (const item of excelData) {
          const maDieu = item.maDieu;
          this.hoatdongnkService
            .postDataToDatabase(item, maDieu)
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
        this.toast.success("Thêm dữ liệu thành công");
      };

      fileReader.readAsBinaryString(file);
    }
  }

  exportToExcel() {
    const dataWithoutId = this.quanLyLopData.map((item) => {
      const { idhdnk, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["A1"].v = "Nội dung minh chứng";
    worksheet["B1"].v = "Mã hoạt động";
    worksheet["C1"].v = "Tên đoạt động";
    worksheet["D1"].v = "Điểm";
    worksheet["E1"].v = "Cổ vũ";
    worksheet["F1"].v = "Ban tổ chức";
    worksheet["G1"].v = "Kỹ năng hoạt động";
    worksheet["J1"].v = "Mã Điều";
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlyhoatdongnk.xlsx");
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
      const comparison = a.tenHdnk.localeCompare(b.tenHdnk);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortMaSV() {
    this.isMaSV = !this.isMaSV;
    this.isIcon = !this.isIcon;

    this.quanLyLopData.sort((a: any, b: any) => {
      if (this.isMaSV) {
        return a.maHdnk.localeCompare(b.maHdnk);
      } else {
        return b.maHdnk.localeCompare(a.maHdnk);
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
    this.hoatdongnkService.getHDNKData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterNam(slectedNK: any) {
    this.nk = slectedNK;
    this.namehdnk = "Hoạt Động: " + slectedNK;
  }
  resetFilterNK() {
    this.nk = "";
    this.namehdnk = "Tất Cả Hoạt Động";
  }
  resetModal() {
    this.addLop = new QuanLyHDNK();
    this.showModal = false;
  }

  addHdnkMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,.?~\\/]/);

    if (!this.addLop.maHdnk || !this.addLop.tenHdnk || !this.addLop.diemhdnk) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maHdnk)) {
      this.toast.warning(
        "Mã hoạt động ngoại khoá không được chứa ký tự đặc biệt!"
      );
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenHdnk)) {
      this.toast.warning(
        "Tên hoạt động ngoại khoá được không chứa ký tự đặc biệt!"
      );
      return;
    }

    if (isNaN(Number(this.addLop.diemhdnk))) {
      this.toast.warning("Điểm phải là một số!");
      return;
    }
    const maDieu = this.addLop.maDieu;
    this.hoatdongnkService
      .postThemHDNK(this.addLop, maDieu)
      .subscribe((data) => {
        this.addLop = new QuanLyHDNK();
        this.toast.success("Thêm thành công");
        this.loadData();
        this.resetModal();
        this.closeModalAdd();
      });
  }

  editHdnk(lop: QuanLyHDNK) {
    this.editingLop = { ...lop };
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

  updateHdnk() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.editingLop.maHdnk ||
      !this.editingLop.tenHdnk ||
      !this.editingLop.diemhdnk
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maHdnk)) {
      this.toast.warning(
        "Mã hoạt động ngoại khoá không được chứa ký tự đặc biệt!"
      );
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenHdnk)) {
      this.toast.warning(
        "Tên hoạt động ngoại khoá được không chứa ký tự đặc biệt!"
      );
      return;
    }

    if (isNaN(Number(this.editingLop.diemhdnk))) {
      this.toast.warning("Điểm phải là một số!");
      return;
    }

    const maDieu = this.editingLop.maDieu;
    if (this.editingLop) {
      this.hoatdongnkService
        .putEditHDNK(this.editingLop, maDieu)
        .subscribe((data) => {
          const index = this.quanLyLopData.findIndex(
            (k) => k.idhdnk === this.editingLop.idhdnk
          );
          if (index !== -1) {
            this.quanLyLopData[index] = { ...this.editingLop };
          }
          this.toast.success("Cập nhật thành công");
          this.loadData();
          this.closeModal();
        });
    }
  }

  onDelete(idhdnk: any) {
    this.selectedLop = idhdnk;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteHdnk() {
    if (this.selectedLop) {
      this.hoatdongnkService.deleteHDNK(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idhdnk !== this.selectedLop
        );
        this.selectedLop = null;
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
}
