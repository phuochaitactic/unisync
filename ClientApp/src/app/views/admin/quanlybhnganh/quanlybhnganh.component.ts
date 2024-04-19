import { Component, OnInit } from "@angular/core";
import { QuanlybhnganhService } from "src/app/corelogic/model/common/admin/quanlybhnganh.service";
import { QuanLyBHNGnh } from "src/app/corelogic/interface/admin/quanlyBHNgh.model";
import { catchError } from "rxjs/operators";
import { HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { initFlowbite } from "flowbite";
import { ToastrService } from "ngx-toastr";
import * as XLSX from "xlsx";
import { ApiService } from "src/app/corelogic/model/common/admin/api.service";

@Component({
  selector: "app-quanlybhnganh",
  templateUrl: "./quanlybhnganh.component.html",
  styleUrls: ["./quanlybhnganh.component.css"],
})
export class QuanlybhnganhComponent implements OnInit {
  title = "Quản Lý Bậc Hệ Ngành";
  showpassword: boolean = false;
  quanLyLopData: QuanLyBHNGnh[] = [];
  initialData: QuanLyBHNGnh[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  tbhn: string = "";
  tn: string = "";
  tbh: string = "";
  addLop = new QuanLyBHNGnh();
  editingLop: QuanLyBHNGnh = new QuanLyBHNGnh();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namebhn = "Tên Bậc Hệ Ngành";
  namebh = "Tên Bậc Hệ";
  namen = "Tên Ngành";
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
  filter3 = false;
  showdetail = false;
  dropBH: any;
  dropdownBH = false;
  selectedOptionBH = "";
  dropN: any;
  dropdownN = false;
  selectedOptionN = "";
  dropBHE: any;
  dropdownBHE = false;
  selectedOptionBHE = "";
  dropNE: any;
  dropdownNE = false;
  selectedOptionNE = "";
  selectedOptionBHN = "";

  constructor(
    private quanlybhnganhService: QuanlybhnganhService,
    private toast: ToastrService,
    private api: ApiService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.quanlybhnganhService.getBHNData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.api.getBHData().subscribe((data) => {
        this.dropBH = data.data;
      });
      this.api.getNganhData().subscribe((data) => {
        this.dropN = data.data;
      });
      this.initialData = [...this.quanLyLopData];
      this.selectedItems = new Array(data.length).fill(false);
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.loadData();
  }

  showDropdownBH() {
    this.dropdownBH = !this.dropdownBH;
  }
  selectOptionBH(option: string) {
    this.selectedOptionBH = option;
    this.addLop.tenBh = option;
    this.dropdownBH = false;
    this.updateSelectedOptionBHN();
  }

  showDropdownN() {
    this.dropdownN = !this.dropdownN;
  }
  selectOptionN(option: string) {
    this.selectedOptionN = option;
    this.addLop.tenNganh = option;
    this.dropdownN = false;
    this.updateSelectedOptionBHN();
  }
  showDropdownBHE() {
    this.dropdownBHE = !this.dropdownBHE;
  }
  selectOptionBHE(option: string) {
    this.selectedOptionBHE = option;
    this.editingLop.tenBh = option;
    this.dropdownBHE = false;
    this.updateSelectedOptionBHNE();
  }

  showDropdownNE() {
    this.dropdownNE = !this.dropdownNE;
  }
  selectOptionNE(option: string) {
    this.selectedOptionNE = option;
    this.editingLop.tenNganh = option;
    this.dropdownNE = false;
    this.updateSelectedOptionBHNE();
  }
  updateSelectedOptionBHN() {
    this.selectedOptionBHN = this.selectedOptionBH + " " + this.selectedOptionN;
  }
  updateSelectedOptionBHNE() {
    this.editingLop.tenBhngChng =
      this.editingLop.tenBh + " " + this.editingLop.tenNganh;
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
          this.quanlybhnganhService
            .deleteBHN(selectedLop.idbhngChng)
            .subscribe(() => {
              this.quanLyLopData = this.quanLyLopData.filter(
                (lop) => lop.idbhngChng !== selectedLop.idbhngChng
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

  detail(lop: QuanLyBHNGnh) {
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
    XLSX.writeFile(wb, "File_QuanLyBacHeNganh.xlsx");
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
        }) as QuanLyBHNGnh[];

        for (const item of excelData) {
          const tenNganh = item.tenNganh;
          const tenBh = item.tenBh;
          this.quanlybhnganhService
            .postDataToDatabase(item, tenBh, tenNganh)
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
      const { idbhngChng, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Mã bậc hệ ngành chung';
    worksheet['B1'].v = 'Tên bậc hệ';
    worksheet['C1'].v = 'Tên ngành';
    worksheet['D1'].v = 'Tên bậc hệ ngành';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlybachenganh.xlsx");
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
        return a.maLop.localeCompare(b.maLop);
      } else {
        return b.maLop.localeCompare(a.maLop);
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
    this.quanlybhnganhService.getBHNData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterNam(slectedNK: any) {
    this.tbhn = slectedNK;
    this.namebhn = "Bậc Hệ Ngành: " + slectedNK;
  }
  resetFilterNK() {
    this.tbhn = "";
    this.namebhn = "Tất Cả Bậc Hệ Ngành";
  }

  filterBHN(slectedBH: any) {
    this.tbh = slectedBH;
    this.namebh = "Bậc Hệ: " + slectedBH;
  }
  resetFilterBH() {
    this.tbh = "";
    this.namebh = "Tất Cả Bậc Hệ";
  }

  filterLop(slectedLop: any) {
    this.tn = slectedLop;
    this.namen = "Ngành: " + slectedLop;
  }
  resetFilter() {
    this.tn = "";
    this.namen = "Tất Cả Ngành";
  }
  resetModal() {
    this.addLop = new QuanLyBHNGnh();
    this.showModal = false;
  }

  addBHNganhMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*+{}\[\]:;<>,.?~\\/]/);
    if (
      !this.addLop.maBhngChng ||
      !this.addLop.tenNganh ||
      !this.addLop.tenBh
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maBhngChng)) {
      this.toast.warning("Mã bậc hệ ngành không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenNganh)) {
      this.toast.warning("Tên ngành không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenBh)) {
      this.toast.warning("Tên bậc hệ không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.addLop.tenBh)) {
      this.toast.warning("Vui lòng không nhập số trong Tên Bậc Hệ.");
      return;
    }
    if (/\d/.test(this.addLop.tenNganh)) {
      this.toast.warning("Vui lòng không nhập số trong Tên Ngành.");
      return;
    }

    const tenNganh = this.addLop.tenNganh;
    const tenBh = this.addLop.tenBh;
    this.quanlybhnganhService
      .postThemBHN(this.addLop, tenBh, tenNganh)
      .subscribe((data) => {
        this.addLop = new QuanLyBHNGnh();
        this.toast.success("Thêm thành công");
        this.loadData();
        this.resetModal();
        this.closeModalAdd();
      });
  }

  editBHNganh(lop: QuanLyBHNGnh) {
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

  updateBHNganh() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*+{}\[\]:;<>,.?~\\/]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.editingLop.maBhngChng ||
      !this.editingLop.tenNganh ||
      !this.editingLop.tenBh
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maBhngChng)) {
      this.toast.warning("Mã bậc hệ ngành không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenNganh)) {
      this.toast.warning("Tên ngành không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenBh)) {
      this.toast.warning("Tên bậc hệ không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.editingLop.tenBh)) {
      this.toast.warning("Vui lòng không nhập số trong Tên Bậc Hệ.");
      return;
    }
    if (/\d/.test(this.editingLop.tenNganh)) {
      this.toast.warning("Vui lòng không nhập số trong Tên Ngành.");
      return;
    }
    const tenBh = this.editingLop.tenBh;
    const tenNganh = this.editingLop.tenNganh;
    if (this.editingLop) {
      this.quanlybhnganhService
        .putEditBHN(this.editingLop, tenBh, tenNganh)
        .subscribe((data) => {
          const index = this.quanLyLopData.findIndex(
            (k) => k.idbhngChng === this.editingLop.idbhngChng
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

  onDelete(idbhngChng: any) {
    this.selectedLop = idbhngChng;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteBHN() {
    if (this.selectedLop) {
      this.quanlybhnganhService.deleteBHN(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idbhngChng !== this.selectedLop
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
    this.activeButton = "FT Test";
    if (this.filter3 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
}
