import { Component } from '@angular/core';
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import emailjs, { type EmailJSResponseStatus } from '@emailjs/browser';
import { KetquadkService } from "src/app/corelogic/model/common/cvht-sv/ketquadk.service";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";
declare let Email: any;

@Component({
  selector: 'app-nopminhchung',
  templateUrl: './nopminhchung.component.html',
  styleUrls: ['./nopminhchung.component.css']
})
export class NopminhchungComponent {
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
  semesterData: any[] = [
    { name: "Học kỳ 1", data: [] },
    { name: "Học kỳ 2", data: [] },
    { name: "Học kỳ 3", data: [] }
  ];

  constructor(
    private toastr: ToastrService,
    private router: Router, 
    private api: KetquadkService
  ) {}

  ngOnInit(): void {
    this.getYearSemester();
    this.getData();
  }

  sendEmail(e: Event) {
    e.preventDefault();

    emailjs
      .sendForm('service_uffhc2d', 'template_yx340oc', e.target as HTMLFormElement, {
        publicKey: '8AeydCazQvl3svUig',
      })
      .then(
        () => {
          console.log('SUCCESS!');
        },
        (error) => {
          console.log('FAILED...', (error as EmailJSResponseStatus).text);
        },
      );
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
     this.onButtonClicked()
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
    if (this.trainingPeriod && this.trainingPeriod.length > 0) {
      const selectedItem = this.trainingPeriod.filter(
        (item: any) => this.extractYear(item.tenNhhk) === this.selectedYear
      );
      return selectedItem;
    } else {
      console.log("Training period is not defined or empty.");
      return [];
    }
  }

  onButtonClicked() {
    const selectedItem = this.findNHHKItem();
    console.log('mmmm', selectedItem)
    if (selectedItem && selectedItem.length > 0) {
      selectedItem.forEach((selectedItem: any) => {
        this.selectedYear_Semester = selectedItem;
      });
      this.getDataReg_Result(selectedItem);

      // Perform other actions with the selected item
    } else {
      console.log("Item not found for selected year and semester");
    }
  }

  getDataReg_Result(selectedItem: any) {
    this.semesterData= [
      { name: "Học kỳ 1", data: [] },
      { name: "Học kỳ 2", data: [] },
      { name: "Học kỳ 3", data: [] }
    ];
    
    this.api.getDataReg_Result().subscribe((res) => {
      if (res.code === 200) {
        this.dataReg_Result = res.data;
        const selectedYear_Semester = selectedItem;

        this.dataReg_Result = res.data.filter((item: any) => {
          if (item.ngayThamGia != null) {
            const ngayBd = new Date(item.ngayBd).getTime();
            const ngayKt = new Date(item.ngayKt).getTime();

            selectedYear_Semester.forEach((semester: any) => {
              const khoangNgayBd = new Date(semester.ngayBatDau).getTime();
              const khoangNgayKt = new Date(semester.ngayKetThuc).getTime();

              if (ngayBd >= khoangNgayBd && ngayKt <= khoangNgayKt) {
                const semesterName = this.extractSemester(semester.tenNhhk);
                const semesterIndex = this.semesterData.findIndex((data: any) => data.name === semesterName);
                
                if (semesterIndex !== -1) {
                  this.semesterData[semesterIndex].data.push(item);
                }
              }
            });
          }
          // console.log(this.semesterData)
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

  openPopupComment(data: any) {
    this.isOpenPopup = !this.isOpenPopup;
    this.activitySelected = data;
  }

  closePopupComment() {
    this.isOpenPopup = !this.isOpenPopup;
  }
}
