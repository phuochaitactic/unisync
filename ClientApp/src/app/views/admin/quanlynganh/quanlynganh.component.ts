import { Component, OnInit } from "@angular/core";
import { QuanlynganhService } from "src/app/corelogic/model/common/admin/quanlynganh.service";
import { QuanLyNganh } from "src/app/corelogic/interface/admin/quanlynganh.model";
import { ApiService } from "src/app/corelogic/model/common/admin/api.service";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { HttpErrorResponse } from "@angular/common/http";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";

@Component({
  selector: "app-quanlynganh",
  templateUrl: "./quanlynganh.component.html",
  styleUrls: ["./quanlynganh.component.css"],
})
export class QuanlynganhComponent implements OnInit {
  title = "Quản Lý Ngành";
  showpassword: boolean = false;
  quanLyLopData: QuanLyNganh[] = [];
  initialData: QuanLyNganh[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  mk: string = "";
  cn: string = "";
  addLop = new QuanLyNganh();
  editingLop: QuanLyNganh = new QuanLyNganh();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  makhoa = "Đơn vị quản lý";
  ten = "Chuyên Ngành";
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
  dropKhoa: any;
  selectedOption: string = "";
  showOptions: boolean = false;
  selectedOptioE: string = "";
  showOptionsE: boolean = false;
  activeButton: string | null = null;
  filter1 = false;
  filter2 = false;
  showdetail = false;
  dropK: any;
  dropdownK = false;
  selectedOptionK = "";

  constructor(
    private quanlynganhService: QuanlynganhService,
    private api: ApiService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.quanlynganhService.getNganhData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.api.getKhoaData().subscribe((data) => {
        this.dropK = data.data;
      });
      this.selectedItems = new Array(data.length).fill(false);
      this.initialData = [...this.quanLyLopData];
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.api.getKhoaData().subscribe((data) => {
      this.dropKhoa = data;
    });
    this.loadData();
  }
  showDropdownK() {
    this.dropdownK = !this.dropdownK;
  }
  selectOptionK(option: string) {
    this.selectedOptionK = option;
    this.addLop.tenKhoa = option;
    this.dropdownK = false;
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
          this.quanlynganhService
            .deleteNganh(selectedLop.idngh)
            .subscribe(() => {
              this.quanLyLopData = this.quanLyLopData.filter(
                (lop) => lop.idngh !== selectedLop.idngh
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
  detail(lop: QuanLyNganh) {
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
    XLSX.writeFile(wb, "File_QuanLyNganh.xlsx");
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
        }) as QuanLyNganh[];
        for (const item of excelData) {
          const tenKhoa = item.tenKhoa;
          this.quanlynganhService
            .postDataToDatabase(item, tenKhoa)
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
      const { idngh, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Tên khoa';
    worksheet['B1'].v = 'Mã ngành';
    worksheet['C1'].v = 'Tên ngành';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlynganh.xlsx");
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
      const comparison = a.tenNgh.localeCompare(b.tenNgh);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortMaSV() {
    this.isMaSV = !this.isMaSV;
    this.isIcon = !this.isIcon;

    this.quanLyLopData.sort((a: any, b: any) => {
      if (this.isMaSV) {
        return a.maNgh.localeCompare(b.maNgh);
      } else {
        return b.maNgh.localeCompare(a.maNgh);
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
    this.quanlynganhService.getNganhData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterTen(slectedCN: any) {
    this.cn = slectedCN;
    this.ten = "Ngành: " + slectedCN;
  }
  resetFilterTen() {
    this.cn = "";
    this.ten = "Tất Cả Chuyên Ngành";
  }
  filterNam(slectedNK: any) {
    this.mk = slectedNK;
    this.makhoa = "Khoa: " + slectedNK;
  }
  resetFilterNK() {
    this.mk = "";
    this.makhoa = "Tất Cả Đơn Vị Quản Lý";
  }
  resetModal() {
    this.addLop = new QuanLyNganh();
    this.showModal = false;
  }

  addNganhMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);

    if (!this.addLop.maNgh || !this.addLop.tenNgh || !this.addLop.tenKhoa) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maNgh)) {
      this.toast.warning("Mã ngành không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenNgh)) {
      this.toast.warning("Tên ngành không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenKhoa)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.addLop.tenNgh)) {
      this.toast.warning("Vui lòng không nhập số trong tên ngành.");
      return;
    }
    if (/\d/.test(this.addLop.tenKhoa)) {
      this.toast.warning("Vui lòng không nhập số trong tên khoa.");
      return;
    }
    const tenKhoa = this.addLop.tenKhoa;
    this.quanlynganhService
      .postThemNganh(this.addLop, tenKhoa)
      .subscribe((data) => {
        this.addLop = new QuanLyNganh();
        this.toast.success("Thêm thành công");
        this.loadData();
        this.resetModal();
        this.closeModalAdd();
      });
  }

  editNganh(lop: QuanLyNganh) {
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

  updateNganh() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);
    if (
      !this.editingLop.maNgh ||
      !this.editingLop.tenNgh ||
      !this.editingLop.tenKhoa
    ) {
      alert("Vui lòng nhập đủ thông tin.");
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maNgh)) {
      this.toast.warning("Mã ngành không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenNgh)) {
      this.toast.warning("Tên ngành không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenKhoa)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.editingLop.tenNgh)) {
      this.toast.warning("Vui lòng không nhập số trong tên ngành.");
      return;
    }
    if (/\d/.test(this.editingLop.tenKhoa)) {
      this.toast.warning("Vui lòng không nhập số trong tên khoa.");
      return;
    }
    const tenKhoa = this.editingLop.tenKhoa;
    if (this.editingLop) {
      this.quanlynganhService
        .putEditNganh(this.editingLop, tenKhoa)
        .subscribe((data) => {
          const index = this.quanLyLopData.findIndex(
            (k) => k.idngh === this.editingLop.idngh
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

  onDelete(idngh: any) {
    this.selectedLop = idngh;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteNganh() {
    if (this.selectedLop) {
      this.quanlynganhService.deleteNganh(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idngh !== this.selectedLop
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

  toggleOptions() {
    this.showOptions = !this.showOptions;
  }
  selectOption(option: string) {
    this.selectedOption = option;
    this.addLop.tenKhoa = option;
    this.showOptions = false;
  }

  toggleOptionsE() {
    this.showOptionsE = !this.showOptionsE;
  }
  selectOptionE(option: string) {
    this.selectedOptioE = option;
    this.editingLop.tenKhoa = option;
    this.showOptionsE = false;
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
