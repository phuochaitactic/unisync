import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { UserService } from "src/app/corelogic/model/common/cvht-sv/user.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponentSV {
  username: string | undefined;
  mssv: string | undefined;
  khoa: string | undefined;
  bhn: string | undefined;
  nganh: string | undefined;
  lop: string | undefined;
  feed?: string;
  subFeed?: string;
  manageActivityFeed = "Quản lý Hoạt động ngoại khoá";
  dataNHHK: any;
  showPopup: boolean = false;
  myQRCode: any;

  activeButton: string | null = null;

  constructor(private router: Router, private api: UserService) {
    this.buttonXLHDNK();
  }

  ngOnInit(): void {
    this.getUser();
    // console.log(this.dataHK)
  }

  buttonDKHDNK() {
    this.activeButton = "DKHDNK";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/sinhvien/dangkyhdnk"]);
    this.feed =
      " > " + this.manageActivityFeed + " > " + "Đăng ký hoạt động ngoại khóa";
    this.subFeed = "Đăng ký hoạt động ngoại khóa";
  }

  buttonKQDKHDNK() {
    this.activeButton = "KQDKHDNK";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/sinhvien/ketquadkhdnk"]);
    this.feed =
      " > " +
      this.manageActivityFeed +
      " > " +
      "Kết quả đăng ký hoạt động ngoại khóa";
    this.subFeed = "Kết quả đăng ký hoạt động ngoại khóa";
  }

  buttonXLHDNK() {
    this.activeButton = "XLHDNK";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/sinhvien/xemlichhdnk"]);
    this.feed =
      " > " + this.manageActivityFeed + " > " + "Xem lịch hoạt động ngoại khóa";
    this.subFeed = "Xem lịch hoạt động ngoại khóa";
  }

  buttonNMC() {
    this.activeButton = "NMC";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/sinhvien/nopminhchung"]);
    this.feed = " > " + this.manageActivityFeed + " > " + "Sự kiện của tôi";
    this.subFeed = "Sự kiện của tôi";
  }

  buttonThongKe() {
    this.activeButton = "thongke";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/sinhvien/thongke"]);
    this.feed = "Thống kê dữ liệu của sinh viên";
  }

  getUser() {
    this.api.getDataCurrentUser().subscribe(
      (res) => {
        console.log(res);
        if (res.code === 200) {
          this.myQRCode = JSON.stringify(res.data);
          localStorage.setItem("userInfo", JSON.stringify(res.data[0]));
          //console.log(res.data[0])
          this.username = res.data[0].hoTenSinhVien;
          this.mssv = res.data[0].userId;
          this.khoa = res.data[0].tenKhoa;
          this.bhn = res.data[0].tenBhNgh;
          this.nganh = res.data[0].tenNganh;
          this.lop = res.data[0].tenLop;
          this.checkCurrentTimePeriod();
        }
      },
      (error) => {
        console.error(error);
        this.router.navigate([""]);
      }
    );
  }

  logOut() {
    this.api.logout().subscribe((res) => {
      if (res.statusCode === 200) {
        localStorage.removeItem("userInfo");
        this.router.navigate(["/"]);
      }
    });
  }

  checkCurrentTimePeriod() {
    this.api.getYearSemester().subscribe((res) => {
      if (res.code === 200) {
        this.dataNHHK = res.data;
        const currentDate = new Date().toISOString();
        const currentSemester = this.dataNHHK.find((nhhk: any) =>
          this.isCurrentTimeInRange(
            nhhk.ngayBatDau,
            nhhk.ngayKetThuc,
            currentDate
          )
        );

        if (currentSemester) {
          console.log("Đang trong khoảng thời gian của:", currentSemester);
          localStorage.setItem(
            "currentSemester",
            JSON.stringify(currentSemester)
          );

          // Lấy ngayKetThuc của khoảng thời gian hiện tại
          const currentSemesterEndDate = new Date(currentSemester.ngayKetThuc);

          // Hiển thị dataHK có thời gian kết thúc lớn hơn ngày hiện tại
          const nextSemester = this.dataNHHK.filter(
            (nhhk: any) => new Date(nhhk.ngayKetThuc) > currentSemesterEndDate
          );
          nextSemester.sort((a: any, b: any) => {
            const endDateA = new Date(a.ngayKetThuc).getTime();
            const endDateB = new Date(b.ngayKetThuc).getTime();

            return endDateA - endDateB;
          });
          localStorage.setItem("nextSemester", JSON.stringify(nextSemester[0]));
          console.log("DataHK có thời gian kết thúc lớn hơn:", nextSemester);
        }
      }
    });
  }

  isCurrentTimeInRange(
    startDate: string,
    endDate: string,
    currentDate: string
  ): boolean {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const currentDateFormated = new Date(currentDate);
    return currentDateFormated >= start && currentDateFormated <= end;
  }

  openPopup() {
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }

  dataHK = [
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNhhk: "Học kỳ 1 - Năm học 2020-2021",
      tenNganh: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
      nienKhoa: "2019-2020",
      hocKi: "Học kỳ 1",
      ngayBatDau: "2020-09-04T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-01-04T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNganh: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
      tenNhhk: "Học kỳ 2 - Năm học 2020-2021",
      nienKhoa: "2020-2021",
      hocKi: "Học kỳ 2",
      ngayBatDau: "2021-01-05T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-06-07T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNhhk: "Học kỳ 3 - Năm học 2020-2021",
      tenNganh: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
      nienKhoa: "2020-2021",
      hocKi: "Học kỳ 3",
      ngayBatDau: "2021-06-08T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-11-07T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNhhk: "Học kỳ 4 - Năm học 2021-2022",
      tenNganh: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
      nienKhoa: "2021-2022",
      hocKi: "Học kỳ 3",
      ngayBatDau: "2021-11-08T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2022-03-07T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNhhk: "Học kỳ 1 - Năm học 2020-2021",
      tenNganh: "Công nghệ Thông tin",
      nienKhoa: "2020-2021",
      hocKi: "Học kỳ 1",
      ngayBatDau: "2020-10-04T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-02-04T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNganh: "Công nghệ Thông tin",
      tenNhhk: "Học kỳ 2 - Năm học 2020-2021",
      nienKhoa: "2020-2021",
      hocKi: "Học kỳ 2",
      ngayBatDau: "2021-02-05T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-07-07T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNhhk: "Học kỳ 3 - Năm học 2020-2021",
      tenNganh: "Công nghệ thông tin",
      nienKhoa: "2020-2021",
      hocKi: "Học kỳ 3",
      ngayBatDau: "2021-07-08T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-12-07T00:00:00",
    },
    {
      idnhhk: -630735430,
      maNhhk: 20202,
      tenNhhk: "Học kỳ 3 - Năm học 2020-2021",
      tenNganh: "Công nghệ thông tin",
      nienKhoa: "2020-2021",
      hocKi: "Học kỳ 3",
      ngayBatDau: "2021-07-08T00:00:00",
      tuanBatDau: 23,
      soTuanHk: 22,
      ngayKetThuc: "2021-12-07T00:00:00",
    },
  ];
}
