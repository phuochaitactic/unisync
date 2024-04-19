import { Component, OnInit } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { DanhSachHdnk } from "src/app/corelogic/interface/khoa/danhsachhdnk.model";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import { DanhsachhdnkService } from "src/app/corelogic/model/common/khoa/danhsachhdnk.service";
import { NamhocService } from "src/app/corelogic/model/common/khoa/namhoc.service";
import * as XLSX from "xlsx";
import { catchError } from "rxjs/operators";
import { KhoaService } from "src/app/corelogic/model/common/khoa/khoa.service";
import { switchMap } from "rxjs/operators";
import { Observable, Subscriber } from "rxjs";

@Component({
  selector: "app-danhsachhdnk",
  templateUrl: "./danhsachhdnk.component.html",
  styleUrls: ["./danhsachhdnk.component.css"],
})
export class DanhsachhdnkComponent {
  title = "Hoạt Động Ngoại Khoá";
  showpassword: boolean = false;
  quanLyLopData: any;
  // initialData: QuanLyBacHe[] = [];
  p: number = 1;
  stt: number = 1;
  searchText = "";
  isTenSV: boolean = true;
  isMaSV: boolean = true;
  tenSV: string = "";
  trangthaiSV: string = "";
  bh: string = "";
  tbh = "Tên Hoạt Động";
  addLop = new DanhSachHdnk();
  editingLop: DanhSachHdnk = new DanhSachHdnk();
  checkAll: boolean = false;
  selectedItems: boolean[] = [];
  currentBanCanSuStatus: boolean | null = null;
  isIcon = true;
  isIcons = true;
  isIcon3 = true;
  isIcon4 = true;
  isIcon5 = true;
  isIcon6 = true;
  isIcon7 = true;
  checked: boolean = false;
  selectedTeacher: string = "";
  selectedLocation: string = "";
  selectedRoom: string = "";
  dropdownTeacher: boolean = false;
  dropdownTeacher1: boolean = false;
  dropdownLocation: boolean = false;
  dropdownRoom: boolean = false;
  showModal: boolean = false;
  showModaladd: boolean = false;
  showModaladd2: boolean = false;
  showModaladd3: boolean = false;
  showModalEdit: boolean = false;
  showModaledit: boolean = false;
  // tbh = 'Tên Bậc Hệ';
  showExcel: boolean = false;
  checkDowload = false;
  checkimport = false;
  selectedFileName: string = "";
  ondelete: boolean = false;
  ondelete2: boolean = false;
  ondelete3: boolean = false;
  selectedLop: any;
  loading = false;
  showDeleteAllButton: boolean = false;
  selectedRowCount: number = 0;
  onalldelete = false;
  checkExport = false;
  filter2 = false;
  activeButton: string | null = null;
  dropDieu: any;
  dropBHN: any;
  dropNH: any;
  dropGV: any;
  dropDD: any;
  dropK: any;
  dropP: any;
  dropP1: any;
  dropP2: any;
  dropN: any;
  dropNDMC: any;
  allND: any;
  showOptionsND = false;
  selectedOptionND = "";
  showOptions: boolean = false;
  selectedOption: string = "";
  showOptionsBHN: boolean = false;
  selectedOptionBHN: string = "";
  showOptionsNH: boolean = false;
  selectedOptionNH: string = "";
  showOptionsGV: boolean = false;
  selectedOptionGV: string = "";
  selectedOptionGV1: string = "";
  showOptionsDD: boolean = false;
  selectedOptionDD: string = "";
  showOptionsDD1: boolean = false;
  selectedOptionDD1: string = "";
  showOptionsP1: boolean = false;
  selectedOptionP1: string = "";
  showOptionsK: boolean = false;
  selectedOptionK: string = "";
  showOptionsP: boolean = false;
  selectedOptionP: string = "";
  showOptionsHD: boolean = false;
  selectedOptionHD: string = "";
  showOptionsN: boolean = false;
  selectedOptionN: string = "";
  showOptionsN1: boolean = false;
  selectedOptionN1: string = "";
  showOptionsBuoi: boolean = false;
  selectedOptionBuoi: string = "";
  tenHD = "";
  tenNhhk = "";
  tenKhoa: any;
  tenBHN = "";
  tenNganh: any;
  tenNganh1: any;
  tenGV: any;
  tenGV1: any;
  fBHN: any;
  ftenNhhk: any;
  initialData: DanhSachHdnk[] = [];
  idhdnk: any;
  idduLieuHdnk: any;
  hdnkData: any;
  dlhdnkData: any;
  taoboi: any;
  currentDate: any;
  image: any;
  base64!: any;
  base64e: any;
  newImage: any;
  temporaryImage: string | ArrayBuffer | null = null;
  showdetail = false;

  constructor(
    private service: DanhsachhdnkService,
    private toastr: ToastrService,
    private api: ApifullService,
    private getNam: NamhocService,
    private khoaService: KhoaService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.api.getUser().subscribe((data) => {
      this.tenKhoa = data.data[0].tenKhoa;
      this.taoboi = this.tenKhoa;
      this.service.getHDbyKhoa(this.tenKhoa).subscribe((data) => {
        this.quanLyLopData = data.data;
        this.getNam.selectedYear$.subscribe((newYear: string) => {
          this.filterNHHK();
        });
        this.api.getHDNKData().subscribe((data) => {
          this.hdnkData = data.data;
        });
        this.api.getDLHDData().subscribe((data) => {
          this.dlhdnkData = data.data;
        });
        this.api.getDieuData().subscribe((data) => {
          this.dropDieu = data.data;
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
        this.api.getGVData().subscribe((data) => {
          this.dropGV = data.data;
          this.tenGV1 = this.dropGV.filter(
            (data: any) => data.tenKhoa === this.tenKhoa
          );
        });
        this.api.getDiadiemData().subscribe((data) => {
          this.dropDD = data.data;
        });
        this.api.getNganhData().subscribe((data) => {
          this.dropN = data.data;
          this.tenNganh1 = this.dropN.filter(
            (data: any) => data.tenKhoa === this.tenKhoa
          );
        });
        this.api.getUser().subscribe((data) => {
          this.dropK = data.data[0].tenKhoa;
          this.khoaService.getBHNData(this.dropK).subscribe((data) => {
            this.dropBHN = data.data;
          });
        });
        this.api.getPhongData().subscribe((data: any) => {
          this.dropP = data.data;
        });
        this.api.getNDMC().subscribe((data) => {
          this.allND = data.data;
          this.dropNDMC = this.allND;
        });
        this.selectedItems = new Array(data.length).fill(false);
        this.initialData = this.ftenNhhk;
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
    this.selectedItems = new Array(this.ftenNhhk.length).fill(false);
    this.selectedRowCount = 0;
    this.checkAll = false;
    this.showDeleteAllButton = false;
  }
  selectOne(event: any, i: number): void {
    this.selectedItems[i] = event.target.checked;
    this.selectedRowCount = this.selectedItems.filter((item) => item).length;
    this.checkAll = this.selectedRowCount === this.ftenNhhk.length;
    this.showDeleteAllButton = this.selectedRowCount >= 2;
  }

  checkIfAnyRowSelected(): boolean {
    const selectedCount = this.selectedItems.filter((item) => item).length;
    return selectedCount >= 2;
  }

  checkAllRows(): void {
    this.checkAll = !this.checkAll;
    const selectedCount = this.ftenNhhk.length;
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
        const selectedLop = this.ftenNhhk[index];
        if (selectedLop) {
          this.service.deleteHD(selectedLop.idtthdnk).subscribe(() => {
            this.ftenNhhk = this.ftenNhhk.filter(
              (lop: any) => lop.idtthdnk !== selectedLop.idtthdnk
            );
            this.selectedItems[index] = false;
            this.selectedRowCount--;
            this.showDeleteAllButton = this.selectedRowCount > 0;
            this.checkAll = false;
            this.toastr.success("Xoá thành công");
            this.loadData();
          });
        }
      });
    }
  }

  loadData() {
    this.loading = true;
    this.service.getHDbyKhoa(this.tenKhoa).subscribe((data) => {
      this.quanLyLopData = data.data;
      this.filterNHHK();
      this.loading = false;
    });
  }

  filterNHHK(): void {
    this.loading = true;
    const tenNHHK = this.getNam.getSelectedYear();
    this.ftenNhhk = this.quanLyLopData.filter(
      (nh: any) => nh.tenNHHK === tenNHHK
    );

    this.checkExport = this.ftenNhhk.length > 0;
    this.loading = false;
  }

  // dropdow filter tên Hoạt động
  filterNam(slectedNK: any) {
    this.tbh = "Tên: " + slectedNK;
    this.bh = slectedNK;
  }
  resetFilterNK() {
    this.bh = "";
    this.tbh = "Tất Cả Bậc Hệ";
  }

  resetModal() {
    this.addLop = new DanhSachHdnk();
  }

  toggleOptions() {
    this.showOptions = !this.showOptions;
  }

  selectOption(option: string) {
    this.selectedOption = option;
    this.addLop.maDieu = option;
    this.showOptions = false;
    this.filterLopData();
  }

  filterLopData() {
    this.dropNDMC = this.allND.filter(
      (lop: any) => lop.maDieu === this.selectedOption
    );
  }
  filterPhongData() {
    this.dropP1 = this.dropP.filter(
      (lop: any) => lop.tenDiaDiem === this.selectedOptionDD1
    );
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
    XLSX.writeFile(wb, "File_DANHSACHHDNK.xlsx");
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
        }) as DanhSachHdnk[];

        for (const item of excelData) {
          this.quanLyLopData
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
  onDowload() {
    this.checkDowload = true;
  }

  onFileSelectede(event: any) {
    const file = event.target.files[0];
    this.convertBase64e(file);
  }

  convertBase64e(file: File) {
    const filereader = new FileReader();

    filereader.readAsDataURL(file);

    filereader.onload = () => {
      this.temporaryImage = filereader.result;
      this.base64e = this.temporaryImage;
    };
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    this.convertBase64(file);
    this.selectedFileName = file ? file.name : "";
  }
  convertBase64(file: File) {
    const observable = new Observable((subscriber: Subscriber<any>) => {
      this.readFile(file, subscriber);
    });
    observable.subscribe((item) => {
      this.image = item;
      this.base64 = item;
    });
  }
  readFile(file: File, subscriber: Subscriber<any>) {
    const filereader = new FileReader();

    filereader.readAsDataURL(file);

    filereader.onload = () => {
      subscriber.next(filereader.result);
      subscriber.complete();
    };
    filereader.onerror = () => {
      subscriber.error();
      subscriber.complete();
    };
  }
  onContinue() {
    if (this.checkimport) {
      this.importExcel();
      this.closeExcels();
    }
  }

  exportToExcel() {
    const dataWithoutId = this.ftenNhhk.map((item: any) => {
      const {
        idtthdnk,
        idHdnk,
        lastUpdate,
        createdTime,
        isCanPhong,
        tinhTrangDuyet,

        ...rest
      } = item;
      rest.isCanPhong = isCanPhong ? "Có" : "Không";
      rest.tinhTrangDuyet = tinhTrangDuyet ? "Đã duyệt" : "Chưa duyệt";

      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["A1"].v = "Tên năm học - học kỳ";
    worksheet["B1"].v = "Bậc hệ ngành";
    worksheet["C1"].v = "Mã khoa";
    worksheet["D1"].v = "Tên khoa";
    worksheet["E1"].v = "Mã hoạt động";
    worksheet["F1"].v = "Tên hoạt động";
    worksheet["G1"].v = "Điểm";
    worksheet["H1"].v = "Cổ vũ";
    worksheet["I1"].v = "Ban tổ chức";
    worksheet["J1"].v = "Kỹ năng";
    worksheet["K1"].v = "Nội dung minh chứng";
    worksheet["F1"].v = "Tên giảng viên";
    worksheet["L1"].v = "Phòng";
    worksheet["M1"].v = "Địa điểm";
    worksheet["N1"].v = "Phạm vi";
    worksheet["O1"].v = "Số lượng thực tế";
    worksheet["Y1"].v = "Số lượng dự kiến";
    worksheet["M1"].v = "Ghi chú";
    worksheet["T1"].v = "Buổi ưu tiên";
    worksheet["Q1"].v = "Thời lượng tổ chức";
    worksheet["W1"].v = "Ngày bắt đầu";
    worksheet["P1"].v = "Ngày kết thúc";
    worksheet["Z1"].v = "Cần phòng";
    worksheet["X2"].v = "Tạo bởi";
    worksheet["U1"].v = "Cần minh chứng";
    worksheet["I2"].v = "Tình trạng";
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "danhsachhdnk.xlsx");
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

  toggleOptionsHD() {
    this.showOptionsHD = !this.showOptionsHD;
  }

  selectOptionHD(option: string) {
    this.selectedOptionHD = option;
    this.addLop.kyNangHdnk = option;
    this.showOptionsHD = false;
  }
  clickHT() {
    this.selectedOptionHD = "Học thuật, kỹ năng chuyên ngành";
    this.addLop.kyNangHdnk = this.selectedOptionHD;
    this.showOptionsHD = false;
  }
  clickVH() {
    this.selectedOptionHD = " Văn hoá, Văn Nghệ ";
    this.addLop.kyNangHdnk = this.selectedOptionHD;
    this.showOptionsHD = false;
  }
  clickTT() {
    this.selectedOptionHD = "Thể thao ";
    this.addLop.kyNangHdnk = this.selectedOptionHD;
    this.showOptionsHD = false;
  }
  clickCD() {
    this.selectedOptionHD = "Vì cộng đồng tình nguyện";
    this.addLop.kyNangHdnk = this.selectedOptionHD;
    this.showOptionsHD = false;
  }
  clickDD() {
    this.selectedOptionHD = "Diễn đàn, hội thảo";
    this.addLop.kyNangHdnk = this.selectedOptionHD;
    this.showOptionsHD = false;
  }
  clickHN() {
    this.selectedOptionHD = "Hội nhập toàn cầu";
    this.addLop.kyNangHdnk = this.selectedOptionHD;
    this.showOptionsHD = false;
  }

  addHDNKMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()+{}\[\]:;<>.?~\\/]/);

    if (
      !this.addLop.maHdnk ||
      !this.addLop.tenHdnk ||
      !this.addLop.diemhdnk ||
      !this.addLop.coVu ||
      !this.addLop.banToChuc ||
      !this.addLop.kyNangHdnk ||
      !this.addLop.maDieu ||
      !this.addLop.noiDungMinhChung
    ) {
      this.toastr.warning("Vui lòng nhập đủ thông tin.");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.maHdnk)) {
      this.toastr.warning("Mã hoạt động không được chứa ký tự đặc biệt!");
      return;
    }
    if (kytudacbietRegex.test(this.addLop.tenHdnk)) {
      this.toastr.warning("Tên hoạt động được chứa ký tự đặc biệt!");
      return;
    }
    if (isNaN(Number(this.addLop.diemhdnk))) {
      this.toastr.warning("Điểm hoạt động phải là một số!");
      return;
    }
    if (isNaN(Number(this.addLop.coVu))) {
      this.toastr.warning("Cổ vũ phải là một số!");
      return;
    }
    if (isNaN(Number(this.addLop.banToChuc))) {
      this.toastr.warning("Ban tổ chức phải là một số!");
      return;
    }

    this.service.postAddHDNK(this.addLop).subscribe((data) => {
      if (data.code === 200) {
        this.addLop = new DanhSachHdnk();
        this.tenHD = data.data[0].tenHdnk;
        this.toastr.success("Thêm hoạt động ngoại khoá thành công");
        this.loadData();
        this.resetModal();
        this.idhdnk = data.data[0].idhdnk;
        this.addLop.tenHdnk = this.tenHD;
        this.resetDrop();
        this.closeModalAdd();
        this.OpenModalAdd2();
      } else if (data.code === 500) {
        this.resetDrop();
        this.resetModal();
        this.toastr.warning("Hoạt động đã tồn tại");
      }
    });
  }

  toggleOptionsND() {
    this.showOptionsND = !this.showOptionsND;
  }
  selectOptionND(option: string) {
    this.selectedOptionND = option;
    this.addLop.noiDungMinhChung = option;
    this.showOptionsND = false;
  }
  toggleOptionsBHN() {
    this.showOptionsBHN = !this.showOptionsBHN;
  }

  selectOptionBHN(option: string) {
    this.selectedOptionBHN = option;
    this.addLop.tenBhngh = option;
    this.showOptionsBHN = false;
  }
  toggleOptionsNH() {
    this.showOptionsNH = !this.showOptionsNH;
  }

  selectOptionNH(option: string) {
    this.selectedOptionNH = option;
    this.addLop.tenNhhk = option;
    this.showOptionsNH = false;
  }
  getKhoa() {
    this.api.getUserKData().subscribe((data) => {
      this.tenKhoa = data.data[0].tenKhoa;
    });
    this.filterNganh();
  }
  // filterBHNganh() {
  //   this.fBHN = this.dropBHN.filter(
  //     (bhn: any) => bhn.tenNganh === this.dropK
  //   );
  //   this.filterGV();
  // }

  addHDNKMoi2() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()+{}\[\]:;<>,.?~\\/]/);
    this.getKhoa();
    this.addLop.tenHdnk = this.tenHD;
    this.addLop.tenHdnk = this.tenHD;
    this.tenNhhk = this.addLop.tenNhhk;
    this.tenBHN = this.addLop.tenBhngh;
    this.toastr.success("Thêm dữ liệu hoạt động ngoại khoá thành công");
    this.resetDrop();
    this.closeModalAdd();
    this.OpenModalAdd3();

    // this.service.postAddHDNK2(this.addLop).subscribe((data) => {
    //   this.getKhoa();
    //   const idduLieuHdnk = data.data[0].idduLieuHdnk;
    //   this.addLop = new DanhSachHdnk();
    //   this.toastr.success("Thêm dữ liệu hoạt động ngoại khoá thành công");
    //   this.loadData();
    //   this.resetModal();
    //   this.addLop.tenHdnk = this.tenHD;
    //   this.service.getHdnkbyId(idduLieuHdnk).subscribe((data) => {
    //     this.tenNhhk = data.data[0].tenNhhk;
    //     this.tenBHN = data.data[0].tenBhngh;
    //   });
    //   this.idduLieuHdnk = data.data[0].idduLieuHdnk;

    //   this.showModaladd2 = false;
    //   this.resetDrop();
    //   this.closeModalAdd();
    //   this.OpenModalAdd3();
    // });
  }

  toggleOptionsGV() {
    this.showOptionsGV = !this.showOptionsGV;
  }

  selectOptionGV(option: string) {
    this.selectedOptionGV = option;
    this.addLop.tenGiangVien = option;
    this.dropdownTeacher = false;
  }
  selectOptionGV1(option: string) {
    this.selectedOptionGV1 = option;
    this.editingLop.tenGiangVien = option;
    this.dropdownTeacher1 = false;
  }

  toggleOptionsDD() {
    this.showOptionsDD = !this.showOptionsDD;
  }

  selectOptionDD(option: string) {
    this.selectedOptionDD = option;
    this.addLop.tenDiaDiem = option;
    this.filterPhong1Data();
    this.showOptionsDD = false;
  }
  filterPhong1Data() {
    this.dropP2 = this.dropP.filter(
      (lop: any) => lop.tenDiaDiem === this.selectedOptionDD
    );
  }

  toggleOptionsDD1() {
    this.showOptionsDD1 = !this.showOptionsDD1;
  }

  selectOptionDD1(option: string) {
    this.selectedOptionDD1 = option;
    this.editingLop.tenDiaDiem = option;
    this.filterPhongData();
    this.showOptionsDD1 = false;
  }
  toggleOptionsP1() {
    this.showOptionsP1 = !this.showOptionsP1;
  }

  selectOptionP1(option: string) {
    this.selectedOptionP1 = option;
    this.editingLop.tenPhong = option;
    this.showOptionsP1 = false;
  }
  toggleOptionsK() {
    this.showOptionsK = !this.showOptionsK;
  }

  selectOptionK(option: string) {
    this.selectedOptionK = option;
    this.addLop.tenKhoa = option;
    this.showOptionsK = false;
  }
  toggleOptionsP() {
    this.showOptionsP = !this.showOptionsP;
  }

  selectOptionP(option: string) {
    this.selectedOptionP = option;
    this.addLop.tenPhong = option;
    this.showOptionsP = false;
  }
  toggleOptionsN() {
    this.showOptionsN = !this.showOptionsN;
  }

  selectOptionN(option: string) {
    this.selectedOptionN = option;
    this.addLop.phamVi = option;
    this.showOptionsN = false;
  }

  toggleOptionsN1() {
    this.showOptionsN1 = !this.showOptionsN1;
  }

  selectOptionN1(option: string) {
    this.selectedOptionN1 = option;
    this.editingLop.phamVi = option;
    this.showOptionsN1 = false;
  }
  toggleOptionsBuoi() {
    this.showOptionsBuoi = !this.showOptionsBuoi;
  }

  selectOptionBuoi() {
    this.selectedOptionBuoi = "Buổi sáng";
    this.addLop.buoiUuTien = "Buổi sáng";
    this.showOptionsBuoi = false;
  }
  selectOptionBuoiTT() {
    this.selectedOptionBuoi = "Buổi trưa";
    this.addLop.buoiUuTien = "Buổi trưa";
    this.showOptionsBuoi = false;
  }
  selectOptionBuoiC() {
    this.selectedOptionBuoi = "Buổi chiều";
    this.addLop.buoiUuTien = "Buổi chiều";
    this.showOptionsBuoi = false;
  }
  selectOptionBuoiT() {
    this.selectedOptionBuoi = "Buổi tối";
    this.addLop.buoiUuTien = "Buổi tối";
    this.showOptionsBuoi = false;
  }
  selectOptionBuoiCN() {
    this.selectedOptionBuoi = "Cả ngày";
    this.addLop.buoiUuTien = "Cả ngày";
    this.showOptionsBuoi = false;
  }

  filterNganh() {
    this.tenNganh = this.dropN.filter(
      (nganh: any) => nganh.tenKhoa === this.tenKhoa
    );
    this.filterGV();
  }

  filterGV() {
    this.tenGV = this.dropGV.filter((gv: any) => gv.tenKhoa === this.dropK);
  }
  addHDNKMoi3() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()+{}\[\]:;<>,.?~\\/]/);
    const thoiLuongToChucFormatted = this.addLop.thoiLuongToChuc + ":00";
    this.addLop.thoiLuongToChuc = thoiLuongToChucFormatted;
    this.addLop.tenNhhk = this.tenNhhk;
    this.addLop.tenKhoa = this.taoboi;
    this.addLop.TenBHNgChng = this.tenBHN;
    this.addLop.createdBy = this.taoboi;
    this.addLop.tenHdnk = this.tenHD;
    this.addLop.isCanPhong = this.checked;
    this.addLop.tinhTrangDuyet = false;
    this.addLop.hinhAnh = this.base64;
    this.service.postAddHDNK3(this.addLop).subscribe((data) => {
      if (data.code === 200) {
        this.idduLieuHdnk = data.data[0].idduLieuHdnk;
        this.idhdnk = data.data[0].idhdnk;
        this.addLop = new DanhSachHdnk();
        this.base64 = null;
        this.toastr.success("Hoàn thành tạo hoạt động ngoại khoá");
        this.loadData();
        this.resetDrop();
        this.resetModal();
        this.showModaladd2 = false;
        this.showModaladd3 = false;
        this.showModaladd = false;
      } else if (data.status === 400) {
        this.resetDrop();
        this.toastr.warning("Hãy điền đủ thông tin");
      } else {
        this.resetDrop();
        this.resetModal();
        this.toastr.warning("Lỗi tạo không thành công");
      }
    });
  }
  detail(lop: DanhSachHdnk) {
    this.editingLop = { ...lop };
    this.checked = this.editingLop.tinhTrangDuyet;
    this.showdetail = true;
  }
  closeModalD() {
    this.showdetail = false;
  }

  editLop(lop: DanhSachHdnk) {
    this.editingLop = { ...lop };
    this.checked = this.editingLop.tinhTrangDuyet;
    this.showModalEdit = true;
  }

  onNgaySinhChanges(event: any) {
    this.editingLop.ngayBd = event.target.value;
  }
  onNgaySinhChange(event: any) {
    this.editingLop.ngayKt = event.target.value;
  }
  updateHdnkMoi() {
    const kytudacbietRegex = new RegExp(/[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/);

    this.editingLop.tinhTrangDuyet = this.checked;
    this.editingLop.createdBy = this.taoboi;
    this.editingLop.hinhAnh = this.base64e;
    if (this.editingLop) {
      this.service.putEditHdnk(this.editingLop).subscribe((data) => {
        const index = this.quanLyLopData.findIndex(
          (k: any) => k.idtthdnk === this.editingLop.idtthdnk
        );
        if (index !== -1) {
          this.quanLyLopData[index] = { ...this.editingLop };
        }
        this.temporaryImage = null;
        this.toastr.success("Cập nhật thành công");
        this.resetDrop();
        this.loadData();
        this.closeModalEdit();
      });
    }
  }

  onDelete(idtthdnk: any) {
    this.selectedLop = idtthdnk;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  closeDelete1() {
    this.ondelete2 = false;
  }
  closeDelete2() {
    this.ondelete3 = false;
  }
  deleteHD() {
    if (this.selectedLop) {
      const idtthdnk = this.selectedLop.idtthdnk;
      const idduLieuHdnk = this.selectedLop.idDuLieu;
      const idhdnk = this.selectedLop.idHdnk;
      this.service.deleteHD(idtthdnk).subscribe((data) => {
        this.service.deleteDLhd(idduLieuHdnk).subscribe((data) => {
          this.service.deleteHDNK(idhdnk).subscribe((data) => {
            this.selectedLop = null;
            this.loadData();
            this.resetDrop();
            this.ondelete = false;
            this.toastr.success("Xoá thành công");
          });
        });
      });
    }
  }

  deleteHDNK() {
    if (this.idhdnk) {
      this.service.deleteHDNK(this.idhdnk).subscribe((d) => {
        this.hdnkData = this.hdnkData.filter(
          (lop: any) => lop.idhdnk !== this.idhdnk
        );
        this.idhdnk = null;
        this.loadData();
        console.log("idHDNK", this.idhdnk);
        this.ondelete2 = false;
        this.showModaladd2 = false;
        this.ondelete3 = false;
        this.showModaladd3 = false;
        this.toastr.success("Xoá thành công");
      });
    }
  }
  deleteDLHD() {
    if (this.idduLieuHdnk) {
      this.service.deleteDLhd(this.idduLieuHdnk).subscribe(() => {
        this.dlhdnkData = this.dlhdnkData.filter(
          (lop: any) => lop.idduLieuHdnk !== this.idduLieuHdnk
        );
        this.deleteHDNK();
        this.loadData();
        console.log("idDLHDNK", this.idduLieuHdnk);
        this.showModaladd3 = false;
        this.ondelete3 = false;
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
  closeModal() {
    this.showModal = false;
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
  toggleI5() {
    this.isIcon5 = !this.isIcon5;
  }
  toggleI6() {
    this.isIcon6 = !this.isIcon6;
  }
  toggleI7() {
    this.isIcon7 = !this.isIcon7;
  }

  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.activeButton = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }

  showDropdownTeacher() {
    this.dropdownTeacher = !this.dropdownTeacher;
  }

  showDropdownTeacher1() {
    this.dropdownTeacher1 = !this.dropdownTeacher1;
  }

  showDropdownLocation() {
    this.dropdownLocation = !this.dropdownLocation;
  }

  showDropdownRoom() {
    this.dropdownRoom = !this.dropdownRoom;
  }
  showAdd() {
    this.showModaladd = !this.showModaladd;
  }
  closeModalAdd() {
    this.showModaladd = false;
    this.resetDrop();
  }
  OpenModalAdd2() {
    this.showModaladd2 = true;
  }
  closeModalAdd2() {
    this.ondelete2 = true;
  }
  OpenModalAdd3() {
    this.showModaladd3 = true;
  }
  closeModalAdd3() {
    this.ondelete3 = true;
  }
  OpenModalEdit() {
    this.showModalEdit = true;
  }
  closeModalEdit() {
    this.showModalEdit = false;
    this.temporaryImage = null;
  }
  resetDrop() {
    this.selectedOptionHD = "";
    this.selectedOption = "";
    this.selectedOptionND = "";
    this.selectedOptionBHN = "";
    this.selectedOptionNH = "";
    this.selectedOptionN = "";
    this.selectedOptionGV = "";
    this.selectedOptionDD = "";
    this.selectedOptionP = "";
    this.selectedOptionBuoi = "";
  }
  onTimeChange(event: any): void {
    const timeString = event?.target?.value;
    if (timeString) {
      const [hour, minute] = timeString.split(":");
      const timeValue = `${hour.padStart(2, "0")}:${minute.padStart(
        2,
        "0"
      )}:00`;
      console.log(timeValue);
    }
  }
}
