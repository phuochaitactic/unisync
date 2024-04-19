import { Component, OnInit } from "@angular/core";
import { QuanlynguoidungService } from "src/app/corelogic/model/common/admin/quanlynguoidung.service";
import { QuanLyNguoiDung } from "src/app/corelogic/interface/admin/quanlynguoidung.model";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";
@Component({
  selector: "app-quanlynguoidung",
  templateUrl: "./quanlynguoidung.component.html",
  styleUrls: ["./quanlynguoidung.component.css"],
})
export class QuanlynguoidungComponent implements OnInit {
  title = "Quản Lý Tài Khoản";
  quanLyLopData: QuanLyNguoiDung[] = [];
  initialData: QuanLyNguoiDung[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  nk: string = "";
  addLop = new QuanLyNguoiDung();
  editingLop: QuanLyNguoiDung = new QuanLyNguoiDung();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  showpassword: boolean = false;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  nameu = "Quyền Quản Lý";
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

  constructor(
    private nguoidungService: QuanlynguoidungService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.nguoidungService.getNDData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.selectedItems = new Array(data.length).fill(false);
      this.initialData = [...this.quanLyLopData];
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.loadData();
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
          this.nguoidungService.deleteND(selectedLop.id).subscribe(() => {
            this.quanLyLopData = this.quanLyLopData.filter(
              (lop) => lop.id !== selectedLop.id
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
    XLSX.writeFile(wb, "File_QuanLyNguoiDung.xlsx");
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
        }) as QuanLyNguoiDung[];
        for (const item of excelData) {
          this.nguoidungService
            .postDataToDatabase(item)
            .pipe(
              catchError((error) => {
                if (error.status === 400) {
                  this.toast.warning(
                    "Dữ liệu không hợp lệ. Vui lòng kiểm tra và thử lại."
                  );
                } else {
                  this.toast.warning(
                    "Có lỗi khi thêm dữ liệu. Vui lòng thử lại."
                  );
                }
                return throwError(error);
              })
            )
            .subscribe(() => {
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
      const { idadmin, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["B1"].v = "Tài khoản";
    worksheet["C1"].v = "Mật khẩu";
    worksheet["D1"].v = "Vai trò";
    worksheet["E1"].v = "Họ tên";
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlytaikhoan.xlsx");
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
      const comparison = a.tenLop.localeCompare(b.tenLop);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortMaSV() {
    this.isMaSV = !this.isMaSV;
    this.isIcon = !this.isIcon;

    this.quanLyLopData.sort((a: any, b: any) => {
      if (this.isMaSV) {
        return a.loai.localeCompare(b.loai);
      } else {
        return b.loai.localeCompare(a.loai);
      }
    });
  }

  loadData() {
    this.loading = true;
    this.nguoidungService.getNDData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterNam(slectedNK: any) {
    this.nk = slectedNK;
    this.nameu = "Tài Khoản: " + slectedNK;
  }
  resetFilterNK() {
    this.nk = "";
    this.nameu = "Tất Cả Tài Khoản";
  }
  resetModal() {
    this.addLop = new QuanLyNguoiDung();
    this.showModal = false;
  }

  addNDMoi() {
    this.nguoidungService.postThemND(this.addLop).subscribe((data) => {
      this.addLop = new QuanLyNguoiDung();
      this.toast.success("Thêm thành công");
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editND(lop: QuanLyNguoiDung) {
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

  updateND() {
    if (this.editingLop) {
      this.nguoidungService.putEditND(this.editingLop).subscribe((data) => {
        const index = this.quanLyLopData.findIndex(
          (k) => k.id === this.editingLop.id
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

  onDelete(id: any) {
    this.selectedLop = id;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteND() {
    if (this.selectedLop) {
      this.nguoidungService.deleteND(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.id !== this.selectedLop
        );
        this.selectedLop = null;
        this.ondelete = false;
        this.toast.success("Xoá thành công");
        this.loadData();
      });
    }
  }

  toogleClick() {
    this.showpassword = !this.showpassword;
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
    this.activeButton = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
}
