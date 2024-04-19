import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { KetquadkService } from "src/app/corelogic/model/common/cvht-sv/ketquadk.service";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";

@Component({
  selector: "app-ketquadkhdnk",
  templateUrl: "./ketquadkhdnk.component.html",
  styleUrls: ["./ketquadkhdnk.component.css"],
})
export class KetquadkhdnkComponent {
  dataReg_Result: KetquaDK[] = [];
  stt: number = 1;
  isOpenPopup: boolean = false;
  activitySelected: any;
  yearSemester: [] = [];
  userInfo: any;
  trainingPeriod: any;
  dataYear: string[] = [];
  dataSemester: string[] = [];
  selectedYear: string = "";
  selectedSemester: string = "";
  selectedYear_Semester: any;
  stateData: string = "display";
  commentBySemester: any;
  textValue: string = "";

  constructor(private router: Router, private api: KetquadkService) {}

  ngOnInit(): void {
    this.getYearSemester();
    this.getData();
  }

  getCurrentTrainingPeriod() {
    const currentDate = new Date().getTime();

    const currentTrainingPeriod = this.trainingPeriod.find((period: any) => {
      const startDate = new Date(period.ngayBatDau).getTime();
      const endDate = new Date(period.ngayKetThuc).getTime();

      return currentDate >= startDate && currentDate <= endDate;
    });

    return currentTrainingPeriod;
  }

  // Add this method to your component class

  checkCurrentTrainingPeriod() {
    const currentPeriod = this.getCurrentTrainingPeriod();

    if (currentPeriod) {
      const semester = this.extractSemester(currentPeriod.tenNhhk);
      const year = this.extractYear(currentPeriod.tenNhhk);
      this.selectedYear = year;
      this.selectedSemester = semester;
      this.selectedYear_Semester = currentPeriod;
      this.getData();
      console.log("Current Training Period:", currentPeriod);
    } else {
      console.log("No current training period found.");
    }
  }

  getData() {
    const a = localStorage.getItem("userInfo");
    if (a) {
      const userInfoObject = JSON.parse(a);
      this.userInfo = userInfoObject;
      console.log(userInfoObject);
      this.api.setUserInfo(userInfoObject);
      this.getDataReg_Result();
      this.getComment();
    } else {
      console.log("No userInfo found in localStorage");
    }
  }

  getYearSemester() {
    this.api.getYearSemester().subscribe((res) => {
      if (res.code === 200) {
        this.yearSemester = res.data;
        this.sortSemestersByDate();
        this.checkYearSemester();
        this.checkCurrentTrainingPeriod();
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

  checkYearSemester() {
    const trainingTime = 15; //5 năm
    const index = this.yearSemester.findIndex(
      (item: any) => item.tenNhhk === this.userInfo.tenNhhk
    );

    if (index !== -1) {
      console.log(`Index của ${this.userInfo.tenNhhk} là ${index}`);
      this.trainingPeriod = this.yearSemester.slice(
        index,
        index + trainingTime
      );

      this.trainingPeriod.forEach((t: { tenNhhk: any }) => {
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
      // console.log(this.dataYear)
      // console.log(this.dataSemester)
    } else {
      console.log(`${this.userInfo.tenNhhk} không nằm trong mảng.`);
    }
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

  findNHHKItem(): any {
    console.log(this.selectedSemester + " - Năm học " + this.selectedYear);
    const selectedItem = this.trainingPeriod.find(
      (item: any) =>
        item.tenNhhk ===
        this.selectedSemester + " - Năm học " + this.selectedYear
    );

    return selectedItem;
  }

  onButtonClicked() {
    const selectedItem = this.findNHHKItem();

    if (selectedItem) {
      this.selectedYear_Semester = selectedItem;
      this.getDataReg_Result();
      this.getComment();
      console.log("Selected Item:", selectedItem);

      // Perform other actions with the selected item
    } else {
      console.log("Item not found for selected year and semester");
    }
  }

  getDataReg_Result() {
    this.api.getDataReg_Result().subscribe((res) => {
      if (res.code === 200) {
        this.dataReg_Result = res.data;
        const selectedYear_Semester = this.selectedYear_Semester;

        this.dataReg_Result = res.data.filter((item: any) => {
          const ngayBd = new Date(item.ngayBd).getTime();
          const ngayKt = new Date(item.ngayKt).getTime();

          const khoangNgayBd = new Date(
            selectedYear_Semester.ngayBatDau
          ).getTime();
          const khoangNgayKt = new Date(
            selectedYear_Semester.ngayKetThuc
          ).getTime();

          // Check if ngayBd or ngayKt is within the specified range
          return ngayBd >= khoangNgayBd && ngayKt <= khoangNgayKt;
        });
        if (this.dataReg_Result.length === 0) {
          console.log("khong co data");
          this.stateData = "noDisplay";
        } else {
          this.stateData = "display";
        }
      }
    });
  }

  getComment() {
    this.api.getComment().subscribe((res) => {
      if (res.code === 200 && res.data.length > 0) {
        const commentBySelected = res.data.filter(
          (item: any) => item.idNHHK === this.selectedYear_Semester.idnhhk
        );
        if (commentBySelected.length > 0) {
          this.commentBySemester = commentBySelected[0].loiNhan;
        } else {
          console.log("khong có data");
        }
      } else {
        console.log("error");
      }
    });
  }

  openPopupComment(data: any) {
    this.isOpenPopup = !this.isOpenPopup;
    this.activitySelected = data;
  }

  closePopupComment() {
    this.isOpenPopup = !this.isOpenPopup;
  }

  dataLichDK = {
    idlichDangKy: -1079362313,
    tenLop: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
    tenNhhk: "Học kỳ 2 - Năm học 2020-2021", //khong cần
    ngayBatDau: "2020-12-01T00:00:00",
    ngayKetThuc: "2020-12-07T00:00:00",
  };
}
