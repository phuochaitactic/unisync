import { Component, OnInit } from "@angular/core";
import { QuanlylopService } from "src/app/corelogic/model/common/admin/quanlylop.service";
import { QuanLyLop } from "src/app/corelogic/interface/admin/quanlylop.model";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";
import { ToastrService } from "ngx-toastr";
import { ApiService } from "src/app/corelogic/model/common/admin/api.service";

@Component({
  selector: "app-quanlylop",
  templateUrl: "./quanlylop.component.html",
  styleUrls: ["./quanlylop.component.css"],
})
export class QuanlylopComponent implements OnInit {
  title = "Quản Lý Lớp";
  showpassword: boolean = false;
  quanLyLopData: QuanLyLop[] = [];
  initialData: QuanLyLop[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  khoa: string = "";
  nk: string = "";
  addLop = new QuanLyLop();
  editingLop: QuanLyLop = new QuanLyLop();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namelop = "Tên Lớp";
  namek = "Tên Khoa";
  namebhn = "Bậc Hệ Ngành";
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
  dropK: any;
  showK = false;
  slectedK = "";
  tenKhoadropBHN: any;
  showBHN = false;
  dropNH: any;
  showNH = false;
  slectedNH = "";
  slectedBHN = "";
  dropKE: any;
  showKE = false;
  slectedKE = "";
  dropBHNE: any;
  showBHNE = false;
  slectedBHNE = "";
  showNHE = false;
  slectedNHE = "";
  tenKhoa = "";
  dropBHN: any;

  constructor(
    private lopService: QuanlylopService,
    private toast: ToastrService,
    private api: ApiService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.lopService.getLopData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.api.getNHData().subscribe((data) => {
        this.dropNH = data.data;
      });
      this.api.getKhoaData().subscribe((data) => {
        this.dropK = data.data;
        this.tenKhoa = data.data[0].tenKhoa;
        this.api.getBHNKData(this.tenKhoa).subscribe((data) => {
          this.dropBHN = data.data;
        });
      });

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
          this.lopService.deleteLop(selectedLop.idlop).subscribe(() => {
            this.quanLyLopData = this.quanLyLopData.filter(
              (lop) => lop.idlop !== selectedLop.idlop
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

  clickshowK() {
    this.showK = !this.showK;
    this.showBHN = false;
  }
  slecteddropK(option: string) {
    this.slectedK = option;
    this.addLop.tenKhoa = option;
    this.showK = false;
  }

  clickshowKE() {
    this.showKE = !this.showKE;
    this.showBHNE = false;
  }
  slecteddropKE(option: string) {
    this.slectedKE = option;
    this.editingLop.tenKhoa = option;
    this.showKE = false;
  }
  clickshowNH() {
    this.showNH = !this.showNH;
  }
  slecteddropNH(option: string) {
    this.slectedNH = option;
    this.addLop.tenNhhk = option;
    this.showNH = false;
  }
  clickshowNHE() {
    this.showNHE = !this.showNHE;
  }
  slecteddropNHE(option: string) {
    this.slectedNHE = option;
    this.editingLop.tenNhhk = option;
    this.showNHE = false;
  }

  clickshowBHN() {
    this.showBHN = !this.showBHN;
    this.showK = false;
  }
  slecteddropBHN(option: string) {
    this.slectedBHN = option;
    this.addLop.tenBhngChng = option;
    this.showBHN = false;
  }

  clickshowBHNE() {
    this.showBHNE = !this.showBHNE;
    this.showKE = false;
  }
  slecteddropBHNE(option: string) {
    this.slectedBHNE = option;
    this.editingLop.tenBhngChng = option;
    this.showBHNE = false;
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

  detail(lop: QuanLyLop) {
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
    XLSX.writeFile(wb, "File_QuanLyLop.xlsx");
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
        }) as QuanLyLop[];
        for (const item of excelData) {
          const tenKhoa = item.tenKhoa;
          const TenBHNganh = item.tenBhngChng;
          this.lopService
            .postDataToDatabase(item, tenKhoa, TenBHNganh)
            .pipe(
              catchError((error) => {
                if (error.status === 400) {
                  this.toast.error(
                    "Dữ liệu không hợp lệ. Vui lòng kiểm tra và thử lại."
                  );
                  return error;
                } else {
                  this.toast.error(
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
        this.toast.success("Thêm dữ liệu thành công");
      };
      fileReader.readAsBinaryString(file);
    }
  }

  exportToExcel() {
    const dataWithoutId = this.quanLyLopData.map((item) => {
      const { idlop, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Mã lớp';
    worksheet['B1'].v = 'Tên lớp';
    worksheet['C1'].v = 'Năm vào';
    worksheet['D1'].v = 'Tên khoa';
    worksheet['E1'].v = 'Tên bậc hệ ngành';
    worksheet['F1'].v = 'Tên NHHK';
    worksheet['G1'].v = 'Niên khoá';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlylop.xlsx");
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
    this.lopService.getLopData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterLop(slectedNK: any) {
    this.nk = slectedNK;
    this.namelop = "Lớp: " + slectedNK;
  }
  resetFilterLop() {
    this.nk = "";
    this.namelop = "Tất Cả Các Lớp";
  }

  resetModal() {
    this.addLop = new QuanLyLop();
    this.showModal = false;
  }

  filterKhoa(slectedLop: any) {
    this.khoa = slectedLop;
    this.namek = "Khoa: " + slectedLop;
  }
  resetFilterKhoa() {
    this.khoa = "";
    this.namek = "Tất Cả Khoa";
  }

  addLopMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,.?~\\/]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.addLop.maLop ||
      !this.addLop.tenLop ||
      !this.addLop.namVao ||
      !this.addLop.nienKhoa ||
      !this.addLop.tenKhoa ||
      !this.addLop.tenBhngChng
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maLop)) {
      this.toast.warning("Mã lớp không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenLop)) {
      this.toast.warning("Tên lớp không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenKhoa)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenBhngChng)) {
      this.toast.warning("Tên bậc hệ ngành không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietngayRegex.test(this.addLop.nienKhoa)) {
      this.toast.warning("Niên khoá không được chứa ký tự đặc biệt!");

      return;
    }
    if (isNaN(Number(this.addLop.namVao))) {
      this.toast.warning("Năm vào phải là một số!");

      return;
    }
    if (/\d/.test(this.addLop.tenBhngChng)) {
      this.toast.warning("Vui lòng không nhập số trong tên bậc hệ ngành.");

      return;
    }
    if (/\d/.test(this.addLop.tenKhoa)) {
      this.toast.warning("Vui lòng không nhập số trong tên khoa.");
      return;
    }

    const tenKhoa = this.addLop.tenKhoa;
    const TenBHNganh = this.addLop.tenBhngChng;
    this.lopService
      .postThemLop(this.addLop, tenKhoa, TenBHNganh)
      .subscribe((data) => {
        this.addLop = new QuanLyLop();
        this.toast.success("Thêm lớp thành công.");
        this.loadData();
        this.resetModal();
        this.closeModalAdd();
      });
  }

  editLop(lop: QuanLyLop) {
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

  updateLop() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,.?~\\/]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/]/);
    if (
      !this.editingLop.maLop ||
      !this.editingLop.tenLop ||
      !this.editingLop.namVao ||
      !this.editingLop.nienKhoa ||
      !this.editingLop.tenKhoa ||
      !this.editingLop.tenBhngChng
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maLop)) {
      this.toast.warning("Mã lớp không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenLop)) {
      this.toast.warning("Tên lớp không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenKhoa)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenBhngChng)) {
      this.toast.warning("Tên bậc hệ ngành không được chứa ký tự đặc biệt!");

      return;
    }
    if (kytudacbietngayRegex.test(this.editingLop.nienKhoa)) {
      this.toast.warning("Niên khoá không được chứa ký tự đặc biệt!");

      return;
    }
    if (isNaN(Number(this.editingLop.namVao))) {
      this.toast.warning("Năm vào phải là một số!");

      return;
    }
    if (/\d/.test(this.editingLop.tenBhngChng)) {
      this.toast.warning("Vui lòng không nhập số trong tên bậc hệ ngành.");

      return;
    }
    if (/\d/.test(this.editingLop.tenKhoa)) {
      this.toast.warning("Vui lòng không nhập số trong tên khoa.");
      return;
    }
    const tenKhoa = this.editingLop.tenKhoa;
    const TenBHNganh = this.editingLop.tenBhngChng;
    if (this.editingLop) {
      this.lopService
        .putEditLop(this.editingLop, tenKhoa, TenBHNganh)
        .subscribe((data) => {
          const index = this.quanLyLopData.findIndex(
            (k) => k.idlop === this.editingLop.idlop
          );
          if (index !== -1) {
            this.quanLyLopData[index] = { ...this.editingLop };
          }
          this.toast.success("Cập nhật lớp thành công");
          this.loadData();
          this.closeModal();
        });
    }
  }

  onDelete(idlop: any) {
    this.selectedLop = idlop;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteLop() {
    if (this.selectedLop) {
      this.lopService.deleteLop(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idlop !== this.selectedLop
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
