import { Component, OnInit } from "@angular/core";
import { QuanLyKhoa } from "src/app/corelogic/interface/admin/quanlykhoa.model";
import { QuanlykhoaService } from "src/app/corelogic/model/common/admin/quanlykhoa.service";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";

@Component({
  selector: "app-quanlykhoa",
  templateUrl: "./quanlykhoa.component.html",
  styleUrls: ["./quanlykhoa.component.css"],
})
export class QuanlykhoaComponent implements OnInit {
  title = "Quản Lý Khoa";
  quanLyKhoaData: QuanLyKhoa[] = [];
  p: number = 1;
  stt: number = 1;
  isTenKhoa: boolean = true;
  isMakhoa: boolean = true;
  searchText = "";
  tenkhoa: string = "";
  addKhoa = new QuanLyKhoa();
  editingkhoa: QuanLyKhoa = new QuanLyKhoa();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  isIcon = true;
  isIcons = true;
  successMessage: string = "";
  isPopupVisible = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namek = "Tên Khoa";
  showpassword: boolean = false;
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
  showdetail = false;

  constructor(
    private khoaService: QuanlykhoaService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.khoaService.getKhoaData().subscribe((data) => {
      this.quanLyKhoaData = data.data;
      this.selectedItems = new Array(data.length).fill(false);
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });
    this.loadData();
  }
  resetSelection() {
    this.selectedItems = new Array(this.quanLyKhoaData.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }

  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.quanLyKhoaData.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.quanLyKhoaData.length;
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
        const selectedLop = this.quanLyKhoaData[index];
        if (selectedLop) {
          this.khoaService.deleteKhoa(selectedLop.idkhoa).subscribe(() => {
            this.quanLyKhoaData = this.quanLyKhoaData.filter(
              (lop) => lop.idkhoa !== selectedLop.idkhoa
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

  detail(lop: QuanLyKhoa) {
    this.editingkhoa = { ...lop };
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
    XLSX.writeFile(wb, "File_QuanLyKhoa.xlsx");
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
        }) as QuanLyKhoa[];
        for (const item of excelData) {
          this.khoaService
            .postDataToDatabase(item)
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
    const dataWithoutId = this.quanLyKhoaData.map((item) => {
      const { idkhoa, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Mã khoa';
    worksheet['B1'].v = 'Tên khoa';
    worksheet['C1'].v = 'Mật khẩu';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlykhoa.xlsx");
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

  sortDataByTenkhoa(order: "asc" | "desc" = "asc") {
    this.quanLyKhoaData.sort((a: any, b: any) => {
      const comparison = a.tenKhoa.localeCompare(b.tenKhoa);
      return order === "asc" ? comparison : -comparison;
    });
  }
  sortDataByMakhoa() {
    this.isMakhoa = !this.isMakhoa;
    this.isIcon = !this.isIcon;
    this.quanLyKhoaData.sort((a: any, b: any) => {
      if (this.isMakhoa) {
        return a.mKkhoa.localeCompare(b.maKhoa);
      } else {
        return b.maKhoa.localeCompare(a.maKhoa);
      }
    });
  }

  Sort() {
    this.isTenKhoa = !this.isTenKhoa;
    this.isIcons = !this.isIcons;
    this.sortDataByTenkhoa(this.isTenKhoa ? "asc" : "desc");
  }
  loadData() {
    this.loading = true;
    this.khoaService.getKhoaData().subscribe((data) => {
      this.quanLyKhoaData = data.data;
      this.checkExport = this.quanLyKhoaData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterKhoa(slectedKhoa: any) {
    this.tenkhoa = slectedKhoa;
    this.namek = "Khoa:" + this.tenkhoa;
  }
  resetFilter() {
    this.tenkhoa = "";
    this.namek = "Tất Cả Khoa";
  }
  resetModal() {
    this.addKhoa = new QuanLyKhoa();
    this.showModal = false;
  }

  addKhoaMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.addKhoa.maKhoa ||
      !this.addKhoa.tenKhoa ||
      !this.addKhoa.matKhau
    ) {
      this.toast.warning("Vui lòng điền đầy đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addKhoa.maKhoa)) {
      this.toast.warning("Mã khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addKhoa.tenKhoa)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.addKhoa.tenKhoa)) {
      this.toast.warning("Vui lòng không nhập số trong tên khoa.");
      return;
    }
    this.khoaService.postThemKhoa(this.addKhoa).subscribe((data) => {
      this.addKhoa = new QuanLyKhoa();
      this.toast.success("Thêm thành công");

      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editKhoa(khoa: QuanLyKhoa) {
    this.editingkhoa = { ...khoa };
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

  toogleClick() {
    this.showpassword = !this.showpassword;
  }

  updateKhoa() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.editingkhoa.maKhoa ||
      !this.editingkhoa.tenKhoa ||
      !this.editingkhoa.matKhau
    ) {
      this.toast.warning("Vui lòng điền đầy đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingkhoa.maKhoa)) {
      this.toast.warning("Mã khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingkhoa.tenKhoa)) {
      this.toast.warning("Tên khoa không được chứa ký tự đặc biệt!");
      return;
    }
    if (/\d/.test(this.editingkhoa.tenKhoa)) {
      this.toast.warning("Vui lòng không nhập số trong tên khoa.");
      return;
    }
    if (this.editingkhoa) {
      this.khoaService.putEditKhoa(this.editingkhoa).subscribe((data) => {
        const index = this.quanLyKhoaData.findIndex(
          (k) => k.idkhoa === this.editingkhoa.idkhoa
        );
        if (index !== -1) {
          this.quanLyKhoaData[index] = { ...this.editingkhoa };
        }
        this.toast.success("Cập nhật thành công");
        this.loadData();
        this.closeModal();
      });
    }
  }

  onDelete(idkhoa: any) {
    this.selectedLop = idkhoa;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteKhoa() {
    if (this.selectedLop) {
      this.khoaService.deleteKhoa(this.selectedLop).subscribe(() => {
        this.quanLyKhoaData = this.quanLyKhoaData.filter(
          (lop) => lop.idkhoa !== this.selectedLop
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
    this.activeButton = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
}
