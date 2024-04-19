import { Component } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import { TaolichService } from "src/app/corelogic/model/common/khoa/taolich.service";
import { LichDuyet } from "src/app/corelogic/interface/khoa/taolichduyet.model";
import * as XLSX from "xlsx";
import { catchError } from "rxjs/operators";
import { NamhocService } from "src/app/corelogic/model/common/khoa/namhoc.service";
import { KhoaService } from "src/app/corelogic/model/common/khoa/khoa.service";

@Component({
  selector: "app-taolichduyet-tk",
  templateUrl: "./taolichduyet-tk.component.html",
  styleUrls: ["./taolichduyet-tk.component.css"],
})
export class TaolichduyetTkComponent {
  title = "Tạo Lịch Duyệt Hoạt Động Ngoại Khoá";
  showpassword: boolean = false;
  quanLyLopData: any;
  initialData: any;
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  bh: string = "";
  addLop = new LichDuyet();
  editingLop: LichDuyet = new LichDuyet();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  isIcon3 = true;
  isIcon4 = true;
  showModal: boolean = false;
  showModaladd: boolean = false;
  tbh = "Tên Khoa";
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
  dropKhoa: any;
  showOptionsKhoa: boolean = false;
  selectedOptionKhoa: string = "";
  showOptionsNH: boolean = false;
  selectedOptionNH: string = "";
  dropNH: any;
  dropBHN: any;
  allLopData: any;
  showOptionsNHK: boolean = false;
  selectedOptionNHK: string = "";
  showOptionsNHK1: boolean = false;
  selectedOptionNHK1: string = "";
  showModalEdit = false;
  ftenlichd: any;
  showdetail = false;
  tenKhoa: any;
  currentDate: any;

  constructor(
    private service: TaolichService,
    private toast: ToastrService,
    private api: ApifullService,
    private getNam: NamhocService,
    private khoaService: KhoaService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.api.getUser().subscribe((data) => {
      this.dropKhoa = data.data[0].tenKhoa;
      this.tenKhoa = this.dropKhoa;
      this.service.getLichDuyetData().subscribe((data) => {
        this.quanLyLopData = data.data;
        this.getNam.selectedYear$.subscribe((newYear: string) => {
          this.filterNHHK();
        });
        this.khoaService.getTenNhhk().subscribe((nhhkData) => {
          this.api.getNHData().subscribe((data) => {
            const filteredData = data.data
              .filter((item: any) => item.tenNhhk.includes(nhhkData))
              .sort((a: any, b: any) => {
                const aHK = parseInt(a.tenNhhk.match(/\d+/)?.[0] || "0");
                const bHK = parseInt(b.tenNhhk.match(/\d+/)?.[0] || "0");
                return bHK - aHK;
              });
            this.dropNH = [...filteredData];
          });
        });

        this.selectedItems = new Array(data.length).fill(false);
        this.initialData = [...this.ftenlichd];
        this.loading = false;
        this.selectedItems = new Array(data.length).fill(false);
        this.selectedRowCount = 0;
      });
      this.checkDay();
    });
  }

  checkDay() {
    const today = new Date();
    const day = String(today.getDate()).padStart(2, "0");
    const month = String(today.getMonth() + 1).padStart(2, "0");
    const year = today.getFullYear();

    this.currentDate = `${year}-${month}-${day}`;
  }

  resetSelection() {
    this.selectedItems = new Array(this.ftenlichd.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }
  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.ftenlichd.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.ftenlichd.length;
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
        const selectedLop = this.ftenlichd[index];
        if (selectedLop) {
          this.service.deleteLichD(selectedLop.idlichDuyet).subscribe(() => {
            this.ftenlichd = this.ftenlichd.filter(
              (lop: any) => lop.idlichDuyet !== selectedLop.idlichDuyet
            );
            this.selectedItems[index] = false;
            this.selectedRowCount--;
            this.showDeleteAllButton = this.selectedRowCount > 0;
            this.checkAll = false;
            this.toast.success("Xoá thành công");
            this.loadData();
          });
        }
      });
    }
  }

  loadData() {
    this.loading = true;
    this.service.getLichDuyetData().subscribe((data) => {
      this.quanLyLopData = data.data;
      this.filterNHHK();
      this.loading = false;
    });
  }

  filterNHHK(): void {
    this.loading = true;
    const tenNHHK = this.getNam.getSelectedYear();
    const data = this.quanLyLopData.filter((nh: any) => nh.tenNhhk === tenNHHK);
    const datas = [...data].filter(
      (data: any) => data.tenKhoa === this.tenKhoa
    );
    this.ftenlichd = datas;
    this.checkExport = this.ftenlichd.length > 0;
    this.loading = false;
  }
  filterNam(slectedNK: any) {
    this.tbh = "Khoa: " + slectedNK;
    this.bh = slectedNK;
  }
  resetFilterNK() {
    this.bh = "";
    this.tbh = "Tất Cả Khoa";
  }
  resetModal() {
    this.addLop = new LichDuyet();
    this.showModal = false;
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
  detail(lop: LichDuyet) {
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
    XLSX.writeFile(wb, "File_LichDuyet.xlsx");
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
        }) as LichDuyet[];

        for (const item of excelData) {
          this.service
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
    const dataWithoutId = this.ftenlichd.map((item: any) => {
      const { idlichDuyet, ngayBatDau, ngayKetThuc, ...rest } = item;
      const dateFormat = /\d{4}-\d{2}-\d{2}/; // YYYY-MM-DD
      const isValidFormat = dateFormat.test(ngayBatDau);
      const isValidFormats = dateFormat.test(ngayKetThuc);

      const batDauDate = isValidFormat
        ? new Date(ngayBatDau)
        : new Date(ngayBatDau.replace(/(\d{2})-(\d{2})-(\d{4})/, "$3-$2-$1"));

      const ketThucDate = isValidFormats
        ? new Date(ngayKetThuc)
        : new Date(ngayKetThuc.replace(/(\d{2})-(\d{2})-(\d{4})/, "$3-$2-$1"));
      return { ...rest, ngayBatDau: batDauDate, ngayKetThuc: ketThucDate };
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["A1"].v = "Khoa";
    worksheet["B1"].v = "Mã Khoa";
    worksheet["C1"].v = " Năm học - học kỳ";
    worksheet["D1"].v = " Ngày bắt đầu";
    worksheet["E1"].v = " Ngày kết thúc";
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "lichduyet.xlsx");
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

  toggleI1() {
    this.isIcon = !this.isIcon;
  }
  toggleI2() {
    this.isIcons = !this.isIcons;
  }
  toggleI3() {
    this.isIcon3 = !this.isIcon3;
  }
  toggleI4() {
    this.isIcon4 = !this.isIcon4;
  }
  toggleOptionsKhoa() {
    this.showOptionsKhoa = !this.showOptionsKhoa;
    this.showOptionsNH = false;
    this.showOptionsNHK = false;
  }

  selectOptionKhoa(option: string) {
    this.selectedOptionKhoa = option;
    this.addLop.tenKhoa = option;
    this.showOptionsKhoa = false;
    this.showOptionsNHK = false;
  }
  // filterLopData() {
  //   this.dropNH = this.allLopData.filter(
  //     (lop: any) => lop.tenBhngChng === this.selectedOptionBHN
  //   );
  // }

  toggleOptionsNHK() {
    this.showOptionsNHK = !this.showOptionsNHK;
    this.showOptionsNH = false;
  }

  selectOptionNHK(option: string) {
    this.selectedOptionNHK = option;
    this.addLop.tenNhhk = option;
    this.showOptionsNHK = false;
    this.showOptionsNH = false;
  }
  toggleOptionsNHK1() {
    this.showOptionsNHK1 = !this.showOptionsNHK1;
  }

  selectOptionNHK1(option: string) {
    this.selectedOptionNHK1 = option;
    this.editingLop.tenNhhk = option;
    this.showOptionsNHK1 = false;
  }
  addLichDMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()+{}\[\]:;<>,.?~\\/]/);
    this.addLop.tenKhoa = this.dropKhoa;

    if (
      !this.addLop.tenKhoa ||
      !this.addLop.tenNhhk ||
      !this.addLop.ngayBatDau ||
      !this.addLop.ngayKetThuc
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    this.service.postLichDuyetD(this.addLop).subscribe((data) => {
      this.addLop = new LichDuyet();
      this.toast.success("Tạo lịch duyệt thành công");
      this.resetDrop();
      this.loadData();
      this.resetModal();
      this.closeModalAdd();
    });
  }

  editLichD(lop: LichDuyet) {
    this.editingLop = { ...lop };
    this.showModalEdit = true;
  }
  onNgaySinhChanges(event: any) {
    this.editingLop.ngayBatDau = event.target.value;
  }
  onNgaySinhChange(event: any) {
    this.editingLop.ngayKetThuc = event.target.value;
  }
  updateLichD() {
    this.editingLop.tenKhoa = this.dropKhoa;
    if (
      !this.editingLop.tenKhoa ||
      !this.editingLop.tenNhhk ||
      !this.editingLop.ngayBatDau ||
      !this.editingLop.ngayKetThuc
    ) {
      this.toast.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (this.editingLop) {
      this.service.putEditLichD(this.editingLop).subscribe((data) => {
        const index = this.quanLyLopData.findIndex(
          (k: any) => k.idlichDuyet === this.editingLop.idlichDuyet
        );
        if (index !== -1) {
          this.quanLyLopData[index] = { ...this.editingLop };
        }
        this.resetDrop();
        this.loadData();
        this.toast.success("Cập nhật thành công");
        this.closeModaledit();
      });
    }
  }
  onDelete(idlichDuyet: any) {
    this.selectedLop = idlichDuyet;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteBH() {
    if (this.selectedLop) {
      this.service.deleteLichD(this.selectedLop).subscribe(() => {
        this.quanLyLopData = this.quanLyLopData.filter(
          (lop: any) => lop.idlichDuyet !== this.selectedLop
        );
        this.selectedLop = null;
        this.resetDrop();
        this.loadData();
        this.ondelete = false;
        this.toast.success("Xoá thành công");
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

  showAdd() {
    this.showModaladd = !this.showModaladd;
  }
  closeModalAdd() {
    this.showModaladd = false;
    this.resetDrop();
  }
  showEdit() {
    this.showModalEdit = true;
  }
  closeModaledit() {
    this.showModalEdit = false;
  }
  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.activeButton = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
  resetDrop() {
    this.selectedOptionNHK = "";
  }
}
