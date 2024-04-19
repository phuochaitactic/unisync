import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { KetquadkService } from "src/app/corelogic/model/common/cvht-sv/ketquadk.service";
import { CvhtService } from "src/app/corelogic/model/common/cvht-sv/cvht.service";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";

@Component({
  selector: "app-duyethdnk-giangvien",
  templateUrl: "./duyethdnk-giangvien.component.html",
  styleUrls: ["./duyethdnk-giangvien.component.css"],
})
export class DuyethdnkGiangvienComponent {
  dataReg_Result: KetquaDK[] = [];
  stt: number = 1;
  isOpenPopup: boolean = false;
  activitySelected: any;
  checked: boolean = false;
  textValue: string = "";
  selectedOption: string = "false";
  selectedOption_Boolean: boolean = false;
  title: string = "false";
  dataSchedule: any[] = [];
  idStudent: any;
  nameStudent: any;
  checkActivity: any;
  currentYear_Semester: any;
  nextYear_Semester: any;

  constructor(
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private api: KetquadkService,
    private api_cvht: CvhtService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const id = params["id"];
      const tenSinhVien = params["tenSinhVien"];

      this.idStudent = id;
      this.nameStudent = tenSinhVien;
    });
    this.getData();
  }

  getData() {
    const a = localStorage.getItem("userInfo");
    if (a) {
      const userInfoObject = JSON.parse(a);
      console.log(userInfoObject);
      this.api.setUserInfo(userInfoObject);
      this.getDataReg_ResultByAdviser();
    } else {
      console.log("No userInfo found in localStorage");
    }
  }

  // isCurrentTimeInRange(ngayBatDau: any, ngayKetThuc: any): boolean {
  //   const currentTime = new Date('Mar 5 2024 00:00:00'); //'Jan 12 2024 00:00:00'
  //   const startDate = new Date(ngayBatDau)
  //   const endDate = new Date(ngayKetThuc)

  //   // console.log(startDate)
  //   // console.log(endDate)
  //   // console.log(currentTime)

  //   if (currentTime >= startDate && currentTime <= endDate) {
  //     console.log('Current time is within the range')
  //     return true;
  //   }
  //   console.log('Current time is outside the range');
  //   return false;
  // }

  getDataReg_ResultByAdviser() {
    this.api.getDataReg_ResultFromAdviser(this.idStudent).subscribe((res) => {
      if (res.code === 200) {
        // this.dataReg_Result = res.data
        // console.log(this.dataReg_Result)
        const a = localStorage.getItem("nextSemester");
        const c = localStorage.getItem("currentSemester");
        if (a && c) {
          const b = JSON.parse(a);
          this.nextYear_Semester = JSON.parse(a);
          this.currentYear_Semester = JSON.parse(c);
          console.log("currentYear", JSON.parse(c));
          this.dataReg_Result = res.data.filter((item: any) => {
            const startDate = item.ngayBd;
            const endDate = item.ngayKt;

            return b.ngayBatDau <= startDate && b.ngayKetThuc >= endDate;
          });
        }
      } else {
        this.toastr.error("Xảy ra lỗi trong quá trình tải");
      }
    });
  }

  openPopupComment(data: any) {
    this.isOpenPopup = !this.isOpenPopup;
    this.activitySelected = data;
    this.api.setActivityId(this.activitySelected);
    console.log(this.activitySelected.iddangKy);
  }

  closePopupComment() {
    this.isOpenPopup = !this.isOpenPopup;
    this.textValue = "";
    this.checked = false;
    this.selectedOption_Boolean = false;
  }

  onCheckboxChange() {
    console.log(this.checked);
  }

  putDataReg_Result(event: boolean, data: any) {
    //console.log(this.selectedOption)
    console.log(event, data);
    this.api.putDataReg_ResultEachActivityFromAdviser(event, data).subscribe(
      (res) => {
        if (res) {
          //this.closePopupComment()
          this.toastr.success("Cập nhật thành công");
          this.getDataReg_ResultByAdviser();
          // this.textValue = ''
          // this.checked = false
          // this.selectedOption_Boolean = false
        }
      },
      (error) => {
        this.toastr.error(error);
      }
    );
  }

  toggleOptions(option: any) {
    this.title = String(option);
    this.selectedOption = option;
    this.selectedOption_Boolean = option == "true";
    console.log(option);
  }

  onCheckDataActivity(event: boolean, data: any) {
    //this.checkActivity = event
    this.putDataReg_Result(event, data);
  }

  submit() {
    this.api_cvht
      .postCommentActivities(
        this.nameStudent,
        this.nextYear_Semester.tenNhhk,
        this.textValue
      )
      .subscribe((res) => {
        if (res.code === 200) {
          this.toastr.success("Duyệt thành công");
          this.router.navigate(["dashboard/giangvien/danhsachsv"]);
        } else {
          this.toastr.error("Duyệt thất bại, Vui lòng kiểm tra lại !");
        }
      });
  }

  dataLichDuyetHDNK = {
    idlichDuyet: -1079334342,
    tenLop: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
    tenNhhk: "Học kỳ 2 - Năm học 2020-2021", //khong cần
    ngayBatDau: "2024-12-08T00:00:00",
    ngayKetThuc: "2024-12-14T00:00:00",
  };
}
