import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { CvhtService } from "src/app/corelogic/model/common/cvht-sv/cvht.service";
import { SinhVien_GV } from "src/app/corelogic/interface/cvht-sv/sinhvien_gv.model";

@Component({
  selector: "app-danhsach-sinhvien",
  templateUrl: "./danhsach-sinhvien.component.html",
  styleUrls: ["./danhsach-sinhvien.component.css"],
})
export class DanhsachSinhvienComponent {
  dataNHHK: any;
  stt = 0;
  students: SinhVien_GV[] = [];
  yearSemester: [] = [];
  currentUserInfo: any;
  trainingPeriod: any;
  selectedYear: string = "";
  selectedSemester: string = "";
  selectedStatus: string = "";
  selectedYear_Semester: any;
  dataYear: string[] = [];
  dataSemester: string[] = [];
  navigate: boolean = false;
  isDisplay: boolean = true;

  constructor(private router: Router, private api: CvhtService) {}

  ngOnInit(): void {
    this.selectedStatus = this.dataTinhTrang[0].tinhTrang;
    this.getYearSemester();
  }

  getData() {
    const userInfo = localStorage.getItem("userInfo");
    if (userInfo) {
      const userInfoJSON = JSON.parse(userInfo);
      this.currentUserInfo = userInfoJSON;
      this.api
        .getDataStudentByAdvisor(
          this.selectedYear_Semester.tenNhhk,
          userInfoJSON.userId
        )
        .subscribe((res) => {
          if (res.code === 200 && res.data.length > 0) {
            console.log(res.data);
            const filterData = res.data.filter(
              (item: any) =>
                item.academicYear !== "Chưa Đến Niên Khóa" &&
                item.academicYear !== "Hết Niên Khóa"
            );
            if (filterData) {
              const filterStatusData = filterData.filter(
                (item: any) => item.trangThaiSinhVien === "Đang học"
              );
              this.students = filterStatusData;
              this.checkDateReview(userInfoJSON);
            }
          }
        });
      // this.api.getDataStudent().subscribe((data) => {
      //   if (data.data) {
      //     const filterData = data.data.filter((item: any) => item.tenGv === userInfoJSON.hoTen)
      //     if (filterData) {
      //       const filterStatusData = filterData.filter((item: any) => item.trangThaiSinhVien === "Đang học")
      //       this.students = filterStatusData
      //     }
      //   }
      // })
    }
  }

  getYearSemester() {
    this.api.getYearSemester().subscribe((res) => {
      if (res.code === 200) {
        this.yearSemester = res.data;
        if (res.data.length > 0) {
          const a = localStorage.getItem("currentSemester");
          if (a) {
            this.selectedYear_Semester = JSON.parse(a);
            this.selectedYear = this.extractYear(
              this.selectedYear_Semester.tenNhhk
            );
            this.selectedSemester = this.extractSemester(
              this.selectedYear_Semester.tenNhhk
            );
            this.getData();
            console.log(JSON.parse(a));
          }
        }
        this.sortSemestersByDate();
        this.checkCurrentTimePeriod();
      }
    });
  }

  sortSemestersByDate() {
    this.yearSemester.sort((a: any, b: any) => {
      const endDateA = new Date(a.ngayKetThuc).getTime();
      const endDateB = new Date(b.ngayKetThuc).getTime();

      return endDateA - endDateB;
    });
  }

  extractSemester(input: string) {
    const parts = input.split("-");
    return parts[0].trim();
  }

  extractYear(input: string) {
    const words = input.split(" ");

    const year = words.slice(-1).join(" ");
    return year.trim();
  }

  checkCurrentTimePeriod() {
    if (this.yearSemester) {
      const currentDate = new Date().toISOString();
      this.dataNHHK = this.yearSemester.filter((item: any) => {
        const startDate = new Date(item.ngayBatDau).toISOString();
        const endDate = new Date(item.ngayKetThuc).toISOString();

        return (
          currentDate > endDate ||
          (currentDate >= startDate && currentDate <= endDate)
        );
      });
      if (this.dataNHHK) {
        this.dataNHHK.forEach((t: { tenNhhk: any }) => {
          const semester = this.extractSemester(t.tenNhhk);
          const year = this.extractYear(t.tenNhhk);

          // Kiểm tra xem termYear đã tồn tại trong mảng chưa
          if (!this.dataYear.includes(year)) {
            this.dataYear.push(year);
          }

          if (!this.dataSemester.includes(semester)) {
            this.dataSemester.push(semester);
          }
        });
        console.log(this.dataYear);
        console.log(this.dataSemester);
      }
      console.log("ddddd", this.dataNHHK);
    }
  }

  findNHHKItem(): any {
    console.log(this.selectedSemester + " - Năm học " + this.selectedYear);
    const selectedItem = this.dataNHHK.find(
      (item: any) =>
        item.tenNhhk ===
        this.selectedSemester + " - Năm học " + this.selectedYear
    );

    return selectedItem;
  }

  onButtonClicked() {
    this.students = [];
    const selectedItem = this.findNHHKItem();

    if (selectedItem) {
      this.selectedYear_Semester = selectedItem;
      this.getData();
      // this.getDataReg_Result()
      console.log("Selected Item:", selectedItem);

      // Perform other actions with the selected item
    } else {
      console.log("Item not found for selected year and semester");
    }
  }

  checkDateReview(userInfo: any) {
    this.api.getDataReviewSchedule().subscribe((res) => {
      if (res.code === 200 && res.data.length > 0) {
        const currentDate = new Date().toISOString();
        const reviewableData = res.data.filter((item: any) => {
          const startDate = new Date(item.ngayBatDau).toISOString();
          const endDate = new Date(item.ngayKetThuc).toISOString();

          // const startDate = item.ngayBatDau;
          // const endDate = item.ngayKetThuc;

          console.log("start", startDate);
          console.log("end", currentDate);

          return (
            currentDate >= startDate &&
            currentDate <= endDate &&
            item.tenKhoa === userInfo.tenKhoa
          );
        });

        if (reviewableData.length > 0) {
          this.navigate = true;
        }
      }
    });
  }

  onClickDetail(id: any, tenSinhVien: any) {
    // console.log(this.selectedYear_Semester)
    // console.log(id);
    if (this.navigate) {
      this.router.navigate([
        "dashboard/giangvien/duyethdnk",
        { id: id, tenSinhVien: tenSinhVien },
      ]);
    } else {
      console.log("chưa đến thời gian duyệt");
    }
  }

  dataTinhTrang = [
    {
      tinhTrang: "Duyệt",
      isTinhTrang: true,
    },
    {
      tinhTrang: "Chưa duyệt",
      isTinhTrang: false,
    },
  ];

  dataLichDuyet = [
    {
      idlichDuyet: 1539324944,
      tenKhoa: "Khoa Công nghệ sáng tạo",
      maKhoa: "KCNSC007",
      tenNhhk: "Học kỳ 2 - Năm học 2023-2024",
      ngayBatDau: "2024-03-01T00:00:00",
      ngayKetThuc: "2024-03-02T00:00:00",
    },
    {
      idlichDuyet: 1539324944,
      tenKhoa: "Khoa Công nghệ sáng tạo",
      maKhoa: "KCNSC007",
      tenNhhk: "Học kỳ 2 - Năm học 2023-2024",
      ngayBatDau: "2024-03-04T00:00:00",
      ngayKetThuc: "2024-03-18T00:00:00",
    },
  ];
}
