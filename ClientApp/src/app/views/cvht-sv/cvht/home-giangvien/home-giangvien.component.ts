import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { UserService } from "src/app/corelogic/model/common/cvht-sv/user.service";

@Component({
  selector: "app-home-giangvien",
  templateUrl: "./home-giangvien.component.html",
  styleUrls: ["./home-giangvien.component.css"],
})
export class HomeGiangvienComponent implements OnInit {
  username: string | undefined;
  msgv: string | undefined;
  khoa: string | undefined;
  feed?: string;
  subFeed?: string;
  dataNHHK: any;

  activeButton: string | null = null;

  constructor(private router: Router, private api: UserService) {}

  ngOnInit(): void {
    this.getUser();
    this.buttonDHDNK();
  }

  buttonDHDNK() {
    this.activeButton = "DHDNK";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/giangvien/danhsachsv"]);
    this.feed = "> Duyệt hoạt động ngoại khóa";
    this.subFeed = "Duyệt hoạt động ngoại khóa";
  }

  buttonLSD() {
    this.activeButton = "LSD";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/giangvien/danhsachlichsu"]);
    this.feed = "> Lịch sử duyệt hoạt động ngoại khoá";
    this.subFeed = "Lịch sử duyệt hoạt động ngoại khoá";
  }

  getUser() {
    this.api.getDataCurrentAdvisor().subscribe(
      (res) => {
        if (res.code === 200) {
          localStorage.setItem("userInfo", JSON.stringify(res.data[0]));
          //console.log(res.data[0])
          this.username = res.data[0].hoTen;
          this.msgv = res.data[0].username;
          this.khoa = res.data[0].tenKhoa;
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
  ];
}
