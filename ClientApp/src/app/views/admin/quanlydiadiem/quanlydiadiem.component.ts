import { Component, OnInit } from "@angular/core";
import { QuanlydiadiemService } from "src/app/corelogic/model/common/admin/quanlydiadiem.service";
import { QuanLyDiaDiem } from "src/app/corelogic/interface/admin/quanlydiadiem.model";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { initFlowbite } from "flowbite";
import { ToastrService } from "ngx-toastr";
import * as XLSX from "xlsx";
@Component({
  selector: "app-quanlydiadiem",
  templateUrl: "./quanlydiadiem.component.html",
  styleUrls: ["./quanlydiadiem.component.css"],
})
export class QuanlydiadiemComponent implements OnInit {
  title = "Quản Lý Địa Điểm";
  quanLyDDData: QuanLyDiaDiem[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenKhoa: boolean = true;
  isMakhoa: boolean = true;
  tendiadiem: string = "";
  tendiachi: string = "";
  adddiadiem = new QuanLyDiaDiem();
  editingkhoa: QuanLyDiaDiem = new QuanLyDiaDiem();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  isIcon = true;
  isIcons = true;
  showModal: boolean = false;
  showModalAdd: boolean = false;
  namedd = "Tên Địa Điểm";
  namedc = "Tên Địa Chỉ";
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
    private diadiemService: QuanlydiadiemService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.diadiemService.getDiadiemData().subscribe((data) => {
      this.quanLyDDData = data.data;
      this.selectedItems = new Array(data.length).fill(false);
      this.loading = false;
      this.selectedItems = new Array(data.length).fill(false);
      this.selectedRowCount = 0;
    });

    this.loadData();
  }
  resetSelection() {
    this.selectedItems = new Array(this.quanLyDDData.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }

  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.quanLyDDData.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }
  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.quanLyDDData.length;
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
        const selectedLop = this.quanLyDDData[index];
        if (selectedLop) {
          this.diadiemService
            .deleteDiadiem(selectedLop.iddiaDiem)
            .subscribe(() => {
              this.quanLyDDData = this.quanLyDDData.filter(
                (lop) => lop.iddiaDiem !== selectedLop.iddiaDiem
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
    XLSX.writeFile(wb, "File_QuanLyDiaDiem.xlsx");
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
        }) as QuanLyDiaDiem[];
        for (const item of excelData) {
          this.diadiemService
            .postDataToDatabase(item)
            .pipe(
              catchError((error) => {
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
    const dataWithoutId = this.quanLyDDData.map((item) => {
      const { iddiaDiem, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Tên địa điểm';
    worksheet['B1'].v = 'Tên địa chỉ';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlydiadiem.xlsx");
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
    this.quanLyDDData.sort((a: any, b: any) => {
      const comparison = a.tenDiaChi.localeCompare(b.tenDiaChi);
      return order === "asc" ? comparison : -comparison;
    });
  }

  Sort() {
    this.isTenKhoa = !this.isTenKhoa;
    this.isIcons = !this.isIcons;
    this.sortDataByTenkhoa(this.isTenKhoa ? "asc" : "desc");
  }
  filterDiachi(slectedKhoa: any) {
    this.tendiachi = slectedKhoa;
    this.namedc = "Địa Chỉ: " + slectedKhoa;
  }
  resetFilterDiachi() {
    this.tendiachi = "";
    this.namedc = "Tất Cả Địa Chỉ";
  }

  filterDiadiem(slectedKhoa: any) {
    this.tendiadiem = slectedKhoa;
    this.namedd = "Địa Điểm: " + slectedKhoa;
  }
  resetFilter() {
    this.tendiadiem = "";
    this.namedd = "Tất Cả Địa Điểm";
  }

  loadData() {
    this.loading = true;
    this.diadiemService.getDiadiemData().subscribe((data) => {
      this.quanLyDDData = data.data;
      this.checkExport = this.quanLyDDData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }
  resetModal() {
    this.adddiadiem = new QuanLyDiaDiem();
    this.showModal = false;
  }

  addDiaDiemMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);
    const kytudacbietDCRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\]/);

    if (!this.adddiadiem.tenDiaDiem || !this.adddiadiem.diaChi) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.adddiadiem.tenDiaDiem)) {
      this.toast.warning("Tên địa điểm không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietDCRegex.test(this.adddiadiem.diaChi)) {
      this.toast.warning("Tên địa chỉ không được chứa ký tự đặc biệt!");
      return;
    }

    if (/\d/.test(this.adddiadiem.tenDiaDiem)) {
      this.toast.warning("Vui lòng không nhập số trong Tên địa chỉ.");
      return;
    }
    this.diadiemService.postThemDiadiem(this.adddiadiem).subscribe((data) => {
      this.adddiadiem = new QuanLyDiaDiem();
      this.toast.success("Thêm thành công");
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  detail(lop: QuanLyDiaDiem) {
    this.editingkhoa = { ...lop };
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
  }
  editDD(diadiem: QuanLyDiaDiem) {
    this.editingkhoa = { ...diadiem };
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
  updateDiaDiem() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);
    const kytudacbietDCRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\]/);

    if (!this.editingkhoa.tenDiaDiem || !this.editingkhoa.diaChi) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingkhoa.tenDiaDiem)) {
      this.toast.warning("Tên địa điểm không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietDCRegex.test(this.editingkhoa.diaChi)) {
      this.toast.warning("Tên địa chỉ không được chứa ký tự đặc biệt!");
      return;
    }

    if (/\d/.test(this.editingkhoa.tenDiaDiem)) {
      this.toast.warning("Vui lòng không nhập số trong Tên địa chỉ.");
      return;
    }
    if (this.editingkhoa) {
      this.diadiemService.putEditDiadiem(this.editingkhoa).subscribe((data) => {
        const index = this.quanLyDDData.findIndex(
          (k) => k.iddiaDiem === this.editingkhoa.iddiaDiem
        );
        if (index !== -1) {
          this.quanLyDDData[index] = { ...this.editingkhoa };
        }
        this.toast.success("Cập nhật thành công");
        this.loadData();
        this.closeModal();
      });
    }
  }

  onDelete(iddiaDiem: any) {
    this.selectedLop = iddiaDiem;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteDiaDiem() {
    if (this.selectedLop) {
      this.diadiemService.deleteDiadiem(this.selectedLop).subscribe(() => {
        this.quanLyDDData = this.quanLyDDData.filter(
          (lop) => lop.iddiaDiem !== this.selectedLop
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
