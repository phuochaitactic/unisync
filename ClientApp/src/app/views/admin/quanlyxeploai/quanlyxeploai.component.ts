import { Component, OnInit } from "@angular/core";
import { QuanlyxeploaiService } from "src/app/corelogic/model/common/admin/quanlyxeploai.service";
import { QuanLyXepLoai } from "src/app/corelogic/interface/admin/quanlyxeploai.model";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { initFlowbite } from "flowbite";
import * as XLSX from "xlsx";
import { ToastrService } from "ngx-toastr";
import { QuanlyvanbanService } from "src/app/corelogic/model/common/admin/quanlyvanban.service";
@Component({
  selector: "app-quanlyxeploai",
  templateUrl: "./quanlyxeploai.component.html",
  styleUrls: ["./quanlyxeploai.component.css"],
})
export class QuanlyxeploaiComponent implements OnInit {
  title = "Quản Lý Xếp Loại Hoạt Động Ngoại Khoá";
  quanLyLopData: QuanLyXepLoai[] = [];
  initialData: QuanLyXepLoai[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  nk: string = "";
  addLop = new QuanLyXepLoai();
  editingLop: QuanLyXepLoai = new QuanLyXepLoai();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  isIcon = true;
  isIcons = true;
  namexl = "Xếp Loại";
  showModal: boolean = false;
  showModalAdd: boolean = false;
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
  showNH = false;
  slectedNH = "";
  dropNH: any;
  showNHE = false;
  slectedNHE = "";

  constructor(
    private xeploaiService: QuanlyxeploaiService,
    private toast: ToastrService,
    private vanban: QuanlyvanbanService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.xeploaiService.getXLData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.vanban.getVanBanData().subscribe((data) => {
        this.dropNH = data.data;
      });
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
          this.xeploaiService.deleteXL(selectedLop.idxepLoai).subscribe(() => {
            this.quanLyLopData = this.quanLyLopData.filter(
              (lop) => lop.idxepLoai !== selectedLop.idxepLoai
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

  detail(lop: QuanLyXepLoai) {
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
    XLSX.writeFile(wb, "File_QuanLyNamXepLoai.xlsx");
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
        }) as QuanLyXepLoai[];
        for (const item of excelData) {
          const tenvanBan = item.tenvanBan;
          this.xeploaiService
            .postDataToDatabase(item, tenvanBan)
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
      const { idxepLoai, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet['A1'].v = 'Tên văn bản';
    worksheet['B1'].v = 'Mã loại điểm rèn luyện';
    worksheet['C1'].v = 'Điểm';
    worksheet['D1'].v = 'Xếp loại';
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlyxeploai.xlsx");
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
    this.xeploaiService.getXLData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.checkExport = this.quanLyLopData.length > 0;
      this.resetSelection();
      this.loading = false;
    });
  }

  filterNam(slectedNK: any) {
    this.nk = slectedNK;
    this.namexl = "Loại: " + slectedNK;
  }
  resetFilterNK() {
    this.nk = "";
    this.namexl = "Tất Cả";
  }
  resetModal() {
    this.addLop = new QuanLyXepLoai();
    this.showModal = false;
  }

  addXeploaiMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,.?~\\/]/);

    // if (!this.addLop.diem || !this.addLop.xepLoai || !this.addLop.tenvanBan) {
    //   this.toast.warning("Vui lòng nhập đủ thông tin.");
    //   return;
    // }
    if (kytudacbietRegex.test(this.addLop.xepLoai)) {
      this.toast.warning("Xếp loại không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenvanBan)) {
      this.toast.warning("Tên văn bản không được chứa ký tự đặc biệt!");
      return;
    }
    // if (isNaN(Number(this.addLop.diem))) {
    //   this.toast.warning("Điểm phải là một số!");
    //   return;
    // }
    if (/\d/.test(this.addLop.xepLoai)) {
      this.toast.warning("Vui lòng không nhập số trong Xếp loại.");
      return;
    }
    const tenvanBan = this.addLop.tenvanBan;
    this.xeploaiService.postThemXL(this.addLop, tenvanBan).subscribe((data) => {
      this.addLop = new QuanLyXepLoai();
      this.toast.success("Thêm thành công");
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editXeploai(lop: QuanLyXepLoai) {
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

  updateXeploai() {
    const kytudacbietRegex = new RegExp(/[!@#$%^*+{}\[\]:;<>,.?~\\/]/);

    if (
      !this.editingLop.diem ||
      !this.editingLop.xepLoai ||
      !this.editingLop.tenvanBan
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.xepLoai)) {
      this.toast.warning("Xếp loại không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.editingLop.tenvanBan)) {
      this.toast.warning("Tên văn bản không được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.editingLop.diem))) {
      this.toast.warning("Điểm phải là một số!");
      return;
    }
    if (/\d/.test(this.editingLop.xepLoai)) {
      this.toast.warning("Vui lòng không nhập số trong Xếp loại.");
      return;
    }
    const tenvanBan = this.editingLop.tenvanBan;
    if (this.editingLop) {
      this.xeploaiService
        .putEditXL(this.editingLop, tenvanBan)
        .subscribe((data) => {
          const index = this.quanLyLopData.findIndex(
            (k) => k.idxepLoai === this.editingLop.idxepLoai
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
  deleteXL() {
    if (this.selectedLop) {
      this.xeploaiService.deleteXL(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop) => lop.idxepLoai !== this.selectedLop
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

  clickshowNH() {
    this.showNH = !this.showNH;
  }
  slecteddropNH(option: string) {
    this.slectedNH = option;
    this.addLop.tenvanBan = option;
    this.showNH = false;
  }
  clickshowNHE() {
    this.showNHE = !this.showNHE;
  }
  slecteddropNHE(option: string) {
    this.slectedNHE = option;
    this.editingLop.tenvanBan = option;
    this.showNHE = false;
  }
}
