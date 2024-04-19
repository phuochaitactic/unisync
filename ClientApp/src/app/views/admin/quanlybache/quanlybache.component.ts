import { Component, OnInit } from "@angular/core";
import { QuanLyBacHe } from "src/app/corelogic/interface/admin/quanlybache.model";
import { QuanlybacheService } from "src/app/corelogic/model/common/admin/quanlybache.service";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";
@Component({
  selector: "app-quanlybache",
  templateUrl: "./quanlybache.component.html",
  styleUrls: ["./quanlybache.component.css"],
})
export class QuanlybacheComponent implements OnInit {
  title = "Quản Lý Bậc Hệ";
  showpassword: boolean = false;
  quanLyLopData: QuanLyBacHe[] = [];
  initialData: QuanLyBacHe[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  bh: string = "";
  addLop = new QuanLyBacHe();
  editingLop: QuanLyBacHe = new QuanLyBacHe();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModaladd: boolean = false;
  tbh = "Tên Bậc Hệ";
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
  filter2 = false;
  activeButton: string | null = null;
  showdetail = false;

  constructor(
    private bacheService: QuanlybacheService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadData();
    initFlowbite();
    this.loading = true;
    const token = sessionStorage.getItem("token");
    if (token) {
      this.bacheService.getBacHeData().subscribe((data) => {
        this.quanLyLopData = data.data;
        this.selectedItems = new Array(data.length).fill(false);
        this.initialData = [...this.quanLyLopData];
        this.loading = false;
        this.selectedItems = new Array(data.length).fill(false);
        this.selectedRowCount = 0;
      });
    }
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
          this.bacheService.deleteBH(selectedLop.idbh).subscribe(() => {
            this.quanLyLopData = this.quanLyLopData.filter(
              (lop) => lop.idbh !== selectedLop.idbh
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
  detail(lop: QuanLyBacHe) {
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
    XLSX.writeFile(wb, "File_QuanLyBacHe.xlsx");
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
        }) as QuanLyBacHe[];

        for (const item of excelData) {
          this.bacheService
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

  exportToExcel() {
    const dataWithoutId = this.quanLyLopData.map((item) => {
      const { idbh, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Mã bậc hệ';
    worksheet['B1'].v = 'Tên bậc hệ';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlybache.xlsx");
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
      const comparison = a.TenBacHe.localeCompare(b.TenBacHe);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortMaSV() {
    this.isMaSV = !this.isMaSV;
    this.isIcon = !this.isIcon;

    this.quanLyLopData.sort((a: any, b: any) => {
      if (this.isMaSV) {
        return a.MaBacHe.localeCompare(b.MaBacHe);
      } else {
        return b.MaBacHe.localeCompare(a.MaBacHe);
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
    this.bacheService.getBacHeData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterNam(slectedNK: any) {
    this.tbh = "Bậc Hệ: " + slectedNK;
    this.bh = slectedNK;
  }
  resetFilterNK() {
    this.bh = "";
    this.tbh = "Tất Cả Bậc Hệ";
  }
  resetModal() {
    this.addLop = new QuanLyBacHe();
    this.showModal = false;
  }

  addBacHeMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);

    if (!this.addLop.maBh || !this.addLop.tenBh) {
      this.toastr.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maBh)) {
      this.toastr.warning("Mã bậc hệ không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenBh)) {
      this.toastr.warning("Tên không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.addLop.tenBh)) {
      this.toastr.warning("Vui lòng không nhập số trong Tên.");
      return;
    }
    this.bacheService.postThemBH(this.addLop).subscribe((data) => {
      this.addLop = new QuanLyBacHe();
      this.toastr.success("Thêm thành công");
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editBacHe(lop: QuanLyBacHe) {
    this.editingLop = { ...lop };
    this.showModal = true;
  }
  closeModal() {
    this.showModal = false;
  }
  showAdd() {
    this.showModaladd = true;
  }
  closeModalAdd() {
    this.showModaladd = false;
  }

  updateBacHe() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);

    if (!this.editingLop.maBh || !this.editingLop.tenBh) {
      this.toastr.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maBh)) {
      this.toastr.warning("Mã bậc hệ không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenBh)) {
      this.toastr.warning("Tên không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.editingLop.tenBh)) {
      this.toastr.warning("Vui lòng không nhập số trong Tên.");
      return;
    }
    if (this.editingLop) {
      this.bacheService.putEditBH(this.editingLop).subscribe((data) => {
        const index = this.quanLyLopData.findIndex(
          (k) => k.idbh === this.editingLop.idbh
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
  onDelete(idbh: any) {
    this.selectedLop = idbh;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteBH() {
    if (this.selectedLop) {
      this.bacheService.deleteBH(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idbh !== this.selectedLop
        );
        this.selectedLop = null;
        this.loadData();
        this.ondelete = false;
        this.toastr.success("Xoá thành công");
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
  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.activeButton = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
}
