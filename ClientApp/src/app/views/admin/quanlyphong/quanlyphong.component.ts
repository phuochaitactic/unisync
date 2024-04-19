import { Component, OnInit } from "@angular/core";
import { QuanLyPhong } from "src/app/corelogic/interface/admin/quanlyphong.model";
import { QuanlyphongService } from "src/app/corelogic/model/common/admin/quanlyphong.service";
import { initFlowbite } from "flowbite";
import { catchError } from "rxjs/operators";
import * as XLSX from "xlsx";
import { ToastrService } from "ngx-toastr";
import { ApiService } from "src/app/corelogic/model/common/admin/api.service";
@Component({
  selector: "app-quanlyphong",
  templateUrl: "./quanlyphong.component.html",
  styleUrls: ["./quanlyphong.component.css"],
})
export class QuanlyphongComponent {
  title = "Quản Lý Phòng";
  showpassword: boolean = false;
  quanLyLopData: QuanLyPhong[] = [];
  initialData: QuanLyPhong[] = [];
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
  addLop = new QuanLyPhong();
  editingLop: QuanLyPhong = new QuanLyPhong();
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
  tuan = "Cơ Sở";
  nam = "Tính Chất";
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
  dropP: any;
  dropdowP = false;
  selectedOptionP = "";
  dropDD: any;
  dropdowDD = false;
  selectedOptionDD = "";
  dropdowPE = false;
  selectedOptionPE = "";
  dropdowDDE = false;
  selectedOptionDDE = "";

  constructor(
    private nhhkService: QuanlyphongService,
    private toastr: ToastrService,
    private api: ApiService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.nhhkService.getPhongData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.api.getPhongData().subscribe((data) => {
        this.dropP = data.data;
      });
      this.api.getDiadiemData().subscribe((data) => {
        this.dropDD = data.data;
      });
      this.selectedItems = new Array(data.length).fill(false);
      this.initialData = [...this.quanLyLopData];
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.loadData();
  }

  showDropdownP() {
    this.dropdowP = !this.dropdowP;
  }
  selectOptionP(option: string) {
    this.selectedOptionP = option;
    this.addLop.tenPhong = option;
    this.dropdowP = false;
  }
  showDropdownPE() {
    this.dropdowPE = !this.dropdowPE;
  }
  selectOptionPE(option: string) {
    this.selectedOptionPE = option;
    this.editingLop.tenPhong = option;
    this.dropdowPE = false;
  }

  showDropdownDD() {
    this.dropdowDD = !this.dropdowDD;
  }
  selectOptionDD(option: string) {
    this.selectedOptionDD = option;
    this.addLop.tenDiaDiem = option;
    this.dropdowDD = false;
  }

  showDropdownDDE() {
    this.dropdowDDE = !this.dropdowDDE;
  }
  selectOptionDDE(option: string) {
    this.selectedOptionDDE = option;
    this.editingLop.tenDiaDiem = option;
    this.dropdowDDE = false;
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
          this.nhhkService.deletePhong(selectedLop.idphong).subscribe(() => {
            this.quanLyLopData = this.quanLyLopData.filter(
              (lop) => lop.idphong !== selectedLop.idphong
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

  detail(lop: QuanLyPhong) {
    this.editingLop = { ...lop };
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
  }
  closeExcels() {
    this.showExcel = false;
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
        }) as QuanLyPhong[];
        for (const item of excelData) {
          const tenDiaDiem = item.tenDiaDiem;
          this.nhhkService
            .postDataToDatabase(item, tenDiaDiem)
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
      const { idphong, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Mã phòng';
    worksheet['B1'].v = 'Tên phòng';
    worksheet['C1'].v = 'Tên địa điểm';
    worksheet['D1'].v = 'Sức chứa';
    worksheet['E1'].v = 'Dãy phòng';
    worksheet['F1'].v = 'Cơ sở';
    worksheet['G1'].v = 'Tính chất phòng';
    worksheet['H1'].v = 'Diện tích';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlyphong.xlsx");
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
    this.nhhkService.getPhongData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }
  filterNam(slectedNK: any) {
    this.tuan = "Cơ sở: " + slectedNK;
    this.TuanBatDau = slectedNK;
  }
  resetFilterNK() {
    this.TuanBatDau = "";
    this.tuan = "Tất cả cơ sở";
  }

  filterLop(slectedLop: any) {
    this.nam = "Tính chất: " + slectedLop;
    this.TenNhhk = slectedLop;
  }
  resetFilter() {
    this.TenNhhk = "";
    this.nam = "Tất cả";
  }
  resetModal() {
    this.addLop = new QuanLyPhong();
    this.showModal = false;
  }
  addPhongMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,?~\\/]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\]/);

    if (
      !this.addLop.maPhong ||
      !this.addLop.tenPhong ||
      !this.addLop.tenDiaDiem ||
      !this.addLop.sucChua ||
      !this.addLop.dayPhong ||
      !this.addLop.tinhChatPhong ||
      !this.addLop.dienTichSuDung ||
      !this.addLop.coSo
    ) {
      this.toastr.warning("Vui lòng không để trống thông tin");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maPhong)) {
      this.toastr.warning("Mã phòng không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenPhong)) {
      this.toastr.warning("Tên phòng không được chứa ký tự đặc biệt!");
      return;
    }

    if (kytudacbietngayRegex.test(this.addLop.dayPhong)) {
      this.toastr.warning("Dãy phòng không được chứa ký tự đặc biệt!");
      return;
    }

    if (kytudacbietRegex.test(this.addLop.tinhChatPhong)) {
      this.toastr.warning("Tính chất phòng không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.addLop.sucChua))) {
      this.toastr.warning("Sức chứa phải là một số!");
      return;
    }
    if (isNaN(Number(this.addLop.dienTichSuDung))) {
      this.toastr.warning("Diện tích sử dụng phải là một số!");
      return;
    }

    const tenDiaDiem = this.addLop.tenDiaDiem;
    this.nhhkService
      .postThemPhong(this.addLop, tenDiaDiem)
      .subscribe((data) => {
        this.showModal = false;
        this.toastr.success("Thêm thành công");
        this.loadData();
        this.resetModal();
        this.closeModalAdd();
      });
  }

  editPhong(lop: QuanLyPhong) {
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

  updatePhong() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,?~\\/]/);
    const kytudacbietngayRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\]/);

    if (
      !this.editingLop.maPhong ||
      !this.editingLop.tenPhong ||
      !this.editingLop.tenDiaDiem ||
      !this.editingLop.sucChua ||
      !this.editingLop.dayPhong ||
      !this.editingLop.coSo ||
      !this.editingLop.tinhChatPhong ||
      !this.editingLop.dienTichSuDung
    ) {
      this.toastr.warning("Vui lòng không để trống thông tin");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.maPhong)) {
      this.toastr.warning("Mã phòng không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenPhong)) {
      this.toastr.warning("Tên phòng không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietngayRegex.test(this.editingLop.dayPhong)) {
      this.toastr.warning("Dãy phòng không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietngayRegex.test(this.editingLop.coSo)) {
      this.toastr.warning("Cơ sở không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tinhChatPhong)) {
      this.toastr.warning("Tính chất phòng không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.editingLop.sucChua))) {
      this.toastr.warning("Sức chứa phải là một số!");
      return;
    }
    if (isNaN(Number(this.editingLop.dienTichSuDung))) {
      this.toastr.warning("Diện tích sử dụng phải là một số!");
      return;
    }
    const tenDiaDiem = this.editingLop.tenDiaDiem;
    if (this.editingLop) {
      this.nhhkService
        .putEditPhong(this.editingLop, tenDiaDiem)
        .subscribe((data) => {
          const index = this.quanLyLopData.findIndex(
            (k) => k.idphong === this.editingLop.idphong
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
  deletePhong() {
    if (this.selectedLop) {
      this.nhhkService.deletePhong(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idphong !== this.selectedLop
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
