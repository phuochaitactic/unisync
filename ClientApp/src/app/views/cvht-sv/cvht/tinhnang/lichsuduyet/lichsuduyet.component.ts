import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ActivatedRoute } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { KetquadkService } from "src/app/corelogic/model/common/cvht-sv/ketquadk.service";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";

@Component({
  selector: "app-lichsuduyet",
  templateUrl: "./lichsuduyet.component.html",
  styleUrls: ["./lichsuduyet.component.css"],
})
export class LichsuduyetComponent {
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
  checkActivity: any;

  constructor(
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private router: Router,
    private api: KetquadkService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const id = params["id"];
      const startDate = params["ngayBatDau"];
      const endDate = params["ngayKetThuc"];
      console.log(id);
      console.log("startDate", startDate);
      this.getDataReg_ResultByAdviser(id, startDate, endDate);
    });
  }

  // isCurrentTimeInRange(ngayBatDau: any, ngayKetThuc: any): boolean {
  //   const currentTime = new Date("Mar 5 2024 00:00:00"); //'Jan 12 2024 00:00:00'
  //   const startDate = new Date(ngayBatDau);
  //   const endDate = new Date(ngayKetThuc);

  //   // console.log(startDate)
  //   // console.log(endDate)
  //   // console.log(currentTime)

  //   if (currentTime >= startDate && currentTime <= endDate) {
  //     console.log("Current time is within the range");
  //     return true;
  //   }
  //   console.log("Current time is outside the range");
  //   return false;
  // }

  getDataReg_ResultByAdviser(idStudent: any, startDate: any, endDate: any) {
    this.api.getDataReg_ResultFromAdviser(idStudent).subscribe((res) => {
      if (res.code === 200) {
        // this.dataReg_Result = res.data
        // console.log(this.dataReg_Result)
        console.log('dsd', startDate);
        this.dataReg_Result = res.data.filter((item: any) => {
          const startDateItem = item.ngayBd;
          const endDateItem = item.ngayKt;
          console.log('startDateItem', startDateItem)

          return startDate <= startDateItem && endDate >= endDateItem;
        });
        console.log('Filtered data:', this.dataReg_Result);
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

  // putDataReg_Result(event: boolean, data: any) {
  //   //console.log(this.selectedOption)
  //   console.log(event, data);
  //   this.api.putDataReg_ResultEachActivityFromAdviser(event, data).subscribe(
  //     (res) => {
  //       if (res) {
  //         //this.closePopupComment()
  //         this.toastr.success("Cập nhật thành công");
  //         this.getDataReg_ResultByAdviser();
  //         // this.textValue = ''
  //         // this.checked = false
  //         // this.selectedOption_Boolean = false
  //       }
  //     },
  //     (error) => {
  //       this.toastr.error(error);
  //     }
  //   );
  // }

  toggleOptions(option: any) {
    this.title = String(option);
    this.selectedOption = option;
    this.selectedOption_Boolean = option == "true";
    console.log(option);
  }

  // onCheckDataActivity(event: boolean, data: any) {
  //   //this.checkActivity = event
  //   this.putDataReg_Result(event, data);
  // }

  dataLichDuyetHDNK = {
    idlichDuyet: -1079334342,
    tenLop: "Công nghệ Thông tin (dạy bằng tiếng Anh)",
    tenNhhk: "Học kỳ 2 - Năm học 2020-2021", //khong cần
    ngayBatDau: "2024-12-08T00:00:00",
    ngayKetThuc: "2024-12-14T00:00:00",
  };
}
