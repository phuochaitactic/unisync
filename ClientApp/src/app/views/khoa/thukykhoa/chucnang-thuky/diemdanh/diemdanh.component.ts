import { Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { DiemdanhService } from "src/app/corelogic/model/common/khoa/diemdanh.service";
import { ScannerQRCodeConfig, ScannerQRCodeResult } from "ngx-scanner-qrcode";
import { DiemDanh } from "src/app/corelogic/interface/thukykhoa/diemdanh.model";
import { DanhSachHdnk } from "src/app/corelogic/interface/khoa/danhsachhdnk.model";
import { ToastrService } from "ngx-toastr";
import * as XLSX from "xlsx";
import { DanhsachhdnkService } from "src/app/corelogic/model/common/khoa/danhsachhdnk.service";
import { NamhocService } from "src/app/corelogic/model/common/khoa/namhoc.service";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import emailjs, { type EmailJSResponseStatus } from "@emailjs/browser";
import { Chart, registerables } from "chart.js";
Chart.register(...registerables);

@Component({
  selector: "app-diemdanh",
  templateUrl: "./diemdanh.component.html",
  styleUrls: ["./diemdanh.component.css"],
})
export class DiemdanhComponent implements OnInit, OnDestroy {
  title = "Tổ chức Hoạt động";
  @ViewChild("action") action: any;
  dataDiemDanh: any;
  dataThamgia: any;
  dataResult: any;
  editingLop: any;
  searchText = "";
  p: number = 1;
  stt: number = 1;
  bh = "";
  tbh = "Lớp";
  vaitro = "";
  tvaitro = "Tất Cả Vai Trò";
  showFirst = true;
  showCheckin = false;
  showDropdow = false;
  showResult = false;
  showCamera = false;
  remainingTime = 20;
  countdownInterval: any;
  countdownStarted = false;
  filter1 = false;
  filter2 = false;
  loading = false;
  activeButton: string | null = null;
  activeButton1: string | null = null;
  editingSV: DiemDanh = new DiemDanh();
  showModal = false;
  ondelete: boolean = false;
  selectedLop: any;
  selectedOptionHD = "";
  showOptionsHD = false;
  user: any;
  showDeleteAllButton = false;
  showTT = false;
  showTG = true;
  showTTR = false;
  showTGR = true;
  idHdnk: any;
  idHdnk2: any;
  idSinhVien: any;
  slsv = 0;
  isScanning: boolean = false;
  showModalEdit = false;

  constructor(
    private diemdanhService: DiemdanhService,
    private danhsachHdnk: DanhsachhdnkService,
    private getUser: ApifullService,
    private getNam: NamhocService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    // this.Render();
    this.loading = true;
    this.getUser.getUser().subscribe((data: any) => {
      this.user = data.data[0].tenKhoa;
      this.getNam.selectedYear$.subscribe((data: any) => {
        const namHoc = data;
        this.danhsachHdnk.getHDbyKhoa(this.user).subscribe((data) => {
          const dataHd = data.data;
          const dataHDs = dataHd.filter(
            (hd: any) => hd.tenNHHK === namHoc && hd.tinhTrangDuyet === true
          );
          this.dataDiemDanh = dataHDs;
          this.loading = false;
        });
      });
    });
    window.addEventListener("beforeunload", this.confirmNavigation.bind(this));
  }

  ngOnDestroy(): void {
    this.stopCountdown();
    window.removeEventListener(
      "beforeunload",
      this.confirmNavigation.bind(this)
    );
  }
  Render() {
    const myChart = new Chart("ctx", {
      type: "pie",
      data: {
        labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
        datasets: [
          {
            label: "# of Votes",
            data: [12, 19, 3, 5, 2, 3],
            borderWidth: 1,
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });
  }

  confirmNavigation(event: BeforeUnloadEvent): string | undefined {
    if (this.remainingTime > 0) {
      const confirmationMessage =
        "Bạn đang trong thời gian điểm danh. Bạn có chắc chắn muốn rời khỏi trang?";
      (event || window.event).returnValue = confirmationMessage;
      return confirmationMessage;
    } else {
      return undefined;
    }
  }

  startCountdown(): void {
    this.countdownInterval = setInterval(() => {
      if (this.remainingTime > 0) {
        this.remainingTime--;
      } else {
        this.stopCountdown();
        this.getDataRe();
        // this.showResult = true;
        // this.showCheckin = false;
      }
    }, 1000);
  }

  stopCountdown(): void {
    if (this.countdownInterval) {
      clearInterval(this.countdownInterval);
    }
  }

  formatTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${remainingSeconds < 10 ? "0" : ""}${remainingSeconds}`;
  }

  loadData() {
    this.getDataDD(this.idHdnk);
  }

  exportToExcel() {
    const dataWithoutId = this.dataResult.map((item: any) => {
      const { idSinhVien, ...rest } = item;
      return rest;
    });
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(dataWithoutId);
    const workbook: XLSX.WorkBook = {
      Sheets: { data: worksheet },
      SheetNames: ["data"],
    };
    worksheet["A1"].v = "Tên Sinh Viên";
    worksheet["B1"].v = "Mã Sinh Viên";
    worksheet["C1"].v = "Tên lớp";
    worksheet["D1"].v = "Bậc Hệ Ngành";
    worksheet["E1"].v = "Niên Khoá";
    worksheet["F1"].v = "Vai Trò";
    worksheet["G1"].v = "Cố Vấn Học Tập";
    const excelBuffer: any = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    this.saveAsExcelFile(excelBuffer, "quanlydiemdanh.xlsx");
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
  // clickCD() {
  //   this.selectedOptionHD = "Cổ Vũ";
  //   this.editingSV.vaitro = "Cổ Vũ";
  //   this.showOptionsHD = false;
  // }
  // clickHT() {
  //   this.selectedOptionHD = "Tham Gia";
  //   this.editingSV.vaitro = "Tham Gia";
  //   this.showOptionsHD = false;
  // }
  // clickDD() {
  //   this.selectedOptionHD = "Ban Tổ Chức";
  //   this.editingSV.vaitro = "Ban Tổ Chức";
  //   this.showOptionsHD = false;
  // }

  filterNam(slectedNK: any) {
    this.tbh = "Lớp: " + slectedNK;
    this.bh = slectedNK;
  }
  resetFilterNK() {
    this.bh = "";
    this.tbh = "Tất Cả Lớp";
  }
  filterVT(slectedVT: any) {
    this.tvaitro = "Vai trò: " + slectedVT;
    this.vaitro = slectedVT;
  }
  resetFilterVT() {
    this.vaitro = "";
    this.tvaitro = "Tất Cả";
  }
  toggleDropDown1() {
    this.filter1 = !this.filter1;
    this.activeButton1 = "FT VT";
    if (this.filter1 === false) {
      this.activeButton1 = "";
    }
    localStorage.setItem("activeButton", this.activeButton1);
  }
  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.activeButton = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }

  onData(event: ScannerQRCodeResult[], action?: any): void {
    event?.length && action && action.stop();
    for (const result of event) {
      const jsonData = JSON.parse(result.value);
      if (typeof jsonData === "object") {
        const datas = jsonData;

        for (const data of datas) {
          const idHdnk = this.editingSV.idHdnk;
          this.idHdnk = idHdnk;
          const idSinhVien = data.userId;
          this.idSinhVien = idSinhVien;
          const ngayThamGia = this.editingSV.ngayBd;
          this.diemdanhService
            .putDDData(this.idHdnk, this.idSinhVien, ngayThamGia)
            .subscribe(
              (res) => {
                if (res.code === 200) {
                  this.toast.success("Điểm danh thành công");
                  this.getDataDD(this.idHdnk);
                  action.start();
                } else {
                  this.toast.error(res.message);
                  action.start();
                }
              },
              (error) => {
                this.toast.error("Lỗi điểm danh");
              }
            );
        }
      } else {
        this.toast.warning("Mã QR không hợp lệ");
      }
    }
  }

  sendEmail(e: Event) {
    e.preventDefault();

    emailjs
      .sendForm(
        "service_uffhc2d",
        "template_yx340oc",
        e.target as HTMLFormElement,
        {
          publicKey: "8AeydCazQvl3svUig",
        }
      )
      .then(
        () => {
          console.log("SUCCESS!");
        },
        (error) => {
          console.log("FAILED...", (error as EmailJSResponseStatus).text);
        }
      );
  }

  continueScanning(action: any) {
    if (this.isScanning) {
      action.start();
    } else {
      action.stop();
    }
  }

  getDataDD(idHdnk: any) {
    this.loading = true;
    this.idHdnk = idHdnk;
    this.diemdanhService.getDataDD(this.idHdnk).subscribe((data) => {
      this.dataThamgia = data.data;
      this.loading = false;
    });
  }

  editSV(sinhvien: DiemDanh) {
    this.editingSV = { ...sinhvien };

    this.idHdnk2 = sinhvien.idHdnk;
    const toDay = new Date();
    const ngayKt = new Date(sinhvien.ngayKt);
    const ngayBd = new Date(sinhvien.ngayBd);

    const toDays = new Date(
      toDay.getFullYear(),
      toDay.getMonth(),
      toDay.getDate()
    );
    const ngayKts = new Date(
      ngayKt.getFullYear(),
      ngayKt.getMonth(),
      ngayKt.getDate()
    );
    const ngayBds = new Date(
      ngayBd.getFullYear(),
      ngayBd.getMonth(),
      ngayBd.getDate()
    );
    // this.showResult = true;
    this.showCheckin = true;
    this.showFirst = false;
    // this.getDataRe(this.idHdnk2);

    // if (toDays.getTime() >= ngayKts.getTime()) {
    //   this.showResult = true;
    // this.getDataRe();
    //   this.showFirst = false;
    // } else if (toDays.getTime() === ngayBds.getTime()) {
    //   this.showCheckin = true;
    //   this.showFirst = false;
    // } else {
    //   this.toast.warning("Chưa tới ngày điểm danh");
    // }
  }
  getDataRe() {
    this.loading = true;
    this.diemdanhService.getDataRE(this.idHdnk2).subscribe((data: any) => {
      if (data.code === 200) {
        this.dataResult = data.data;
        this.slsvTG();
        this.loading = false;
      } else {
        this.toast.warning("Dữ liệu lỗi");
        this.loading = false;
      }
    });
  }
  slsvTG() {
    this.slsv = this.dataResult.length;
  }
  closeModal() {
    this.showModal = false;
  }
  editDD(sinhvien: DiemDanh) {
    this.editingLop = { ...sinhvien };
    this.showModalEdit = true;
  }
  updateDD() {
    if (this.editingLop) {
      this.diemdanhService.putTGData(this.editingLop).subscribe((data) => {
        const index = this.dataThamgia.findIndex(
          (k: any) => k.idSinhVien === this.editingLop.idSinhVien
        );
        if (index !== -1) {
          this.dataThamgia[index] = { ...this.editingLop };
        }
        this.toast.success("Cập nhật thành công");
        this.loadData();
        this.closeModal();
      });
    }
  }

  onDelete(idSinhVien: any) {
    this.selectedLop = idSinhVien;
    this.ondelete = true;
  }
  closeDelete() {
    this.ondelete = false;
  }
  deleteBH() {
    if (this.selectedLop) {
      this.diemdanhService
        .deleteDD(this.idHdnk2, this.selectedLop, false)
        .subscribe(() => {
          const updatedDataThamgia = this.dataThamgia.map((lop: any) => {
            if (lop.id === this.selectedLop) {
              return { ...lop, trangthai: false };
            }
            return lop;
          });

          this.dataThamgia = updatedDataThamgia;
          this.selectedLop = null;
          this.ondelete = false;
          this.loadData();
          this.toast.success("Cập nhật trạng thái thành công");
        });
    }
  }
  clickStart() {
    this.showCamera = !this.showCamera;
    if (!this.countdownStarted) {
      this.countdownStarted = true;
      this.startCountdown();
    }
  }
  closeModaledit() {
    this.showModalEdit = false;
  }
  clickShowCheck() {
    this.showCheckin = !this.showCheckin;
    this.showFirst = false;
  }
  clickCloseCheck() {
    this.showCheckin = false;
    this.showFirst = true;
    this.showResult = false;
  }
  clickShowResult() {
    this.showResult = true;
    this.showCheckin = false;
  }
  clickCloseRe() {
    this.showResult = false;
    this.showCheckin = false;
    this.showFirst = true;
  }
  btnTG() {
    this.showTG = true;
    this.showTT = false;
  }
  btnTT() {
    this.showTT = true;
    this.showTG = false;
  }
  btnTGR() {
    this.showTGR = true;
    this.showTTR = false;
  }
  btnTTR() {
    this.showTTR = true;
    this.showTGR = false;
  }
}
