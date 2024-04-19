import { Component, ElementRef, ViewChild } from "@angular/core";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import html2canvas from "html2canvas";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { DangkyhdnkService } from "src/app/corelogic/model/common/cvht-sv/dangkyhdnk.service";
import { DangkyHDNK } from "src/app/corelogic/interface/cvht-sv/dangkyhdnk.model";
import { LichDK } from "src/app/corelogic/interface/cvht-sv/lichdangky.model";
import { KetquadkService } from "src/app/corelogic/model/common/cvht-sv/ketquadk.service";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";

@Component({
  selector: "app-dangkyhdnk",
  templateUrl: "./dangkyhdnk.component.html",
  styleUrls: ["./dangkyhdnk.component.css"],
})
export class DangkyhdnkComponent {
  @ViewChild("content") el!: ElementRef;

  dataHDNK: DangkyHDNK[] = [];
  dataReg_Result: KetquaDK[] = [];
  stt: number = 1;
  checkedItems: any[] = [];
  dataSchedule: any[] = [];
  filteredDataHDNK: DangkyHDNK[] = [];
  userInfo: any;
  selectedOption: string = "trongKhoa";
  startDateNextSemester: any;
  endDateNextSemester: any;
  isOpenPopup: boolean = false;
  detailActivity: any;

  ngOnInit(): void {
    this.getTimeNextSemester();
    this.getDataReg_Schedule();
    //this.isCurrentTimeInRange()
    //this.getData()
    this.getDataReg_Result();
  }

  constructor(
    private toastr: ToastrService,
    private router: Router,
    private api: DangkyhdnkService,
    private apiKq: KetquadkService
  ) {}

  isCurrentTimeInRange(ngayBatDau: any, ngayKetThuc: any): boolean {
    const currentTime = new Date(); //'Jan 12 2024 00:00:00'
    const startDate = new Date(ngayBatDau);
    const endDate = new Date(ngayKetThuc);

    // console.log(startDate)
    // console.log(endDate)
    // console.log(currentTime)

    if (currentTime >= startDate && currentTime <= endDate) {
      console.log("Current time is within the range");
      return true;
    }
    console.log("Current time is outside the range");
    return false;
  }

  getTimeNextSemester() {
    const dates = localStorage.getItem("nextSemester");
    if (dates) {
      const displayDataTime = JSON.parse(dates);
      console.log("sdsd", displayDataTime.ngayBatDau);
      this.startDateNextSemester = new Date(displayDataTime.ngayBatDau);
      this.endDateNextSemester = new Date(displayDataTime.ngayKetThuc);
    } else {
      this.toastr.error("Xảy ra lỗi trong quá trình tải");
    }
  }

  getData() {
    this.filteredDataHDNK = [];
    this.api.getDataReg_ExActivity().subscribe((res) => {
      console.log("datatrongkkhoa", res.data);
      if (res.code === 200) {
        this.dataHDNK = res.data;
        this.filteredDataHDNK = this.dataHDNK.filter((item) => {
          const itemStartDate = new Date(item.ngayBd);
          const itemEndDate = new Date(item.ngayKt);

          // Kiểm tra xem item.ngayBD và ngayKT có nằm trong khoảng của displayDataTime không
          return (
            itemStartDate >= this.startDateNextSemester &&
            itemEndDate <= this.endDateNextSemester &&
            item.tenKhoa === this.userInfo.tenKhoa &&
            item.tinhTrangDuyet === true
          );
        });
      }
    });
  }

  getDataAllDepartment() {
    this.filteredDataHDNK = [];
    this.api.getDataReg_ExActivity().subscribe((res) => {
      console.log("datangoaikkhoa", res.data);
      if (res.code === 200) {
        this.dataHDNK = res.data;
        console.log("dsd", this.startDateNextSemester);
        this.filteredDataHDNK = this.dataHDNK.filter((item) => {
          console.log(item.ngayBd);
          console.log(item.ngayKt);
          const itemStartDate = new Date(item.ngayBd);
          const itemEndDate = new Date(item.ngayKt);

          // Kiểm tra xem item.ngayBD và ngayKT có nằm trong khoảng của displayDataTime không
          return (
            itemStartDate >= this.startDateNextSemester &&
            itemEndDate <= this.endDateNextSemester &&
            item.tenKhoa !== this.userInfo.tenKhoa &&
            item.tinhTrangDuyet === true
          );
        });
        console.log(this.filteredDataHDNK);
      }
    });
  }

  onCheckboxChange(data: any, event: any) {
    const isChecked = event.target.checked;

    if (isChecked) {
      const existingItem = this.checkedItems.find(
        (item) => item.idtthdnk === data.idtthdnk
      );

      if (!existingItem) {
        this.checkedItems.push(data);
      }
    } else {
      this.checkedItems = this.checkedItems.filter(
        (item) => item.idtthdnk !== data.idtthdnk
      );
    }

    console.log(this.checkedItems);
    this.api.setDataExActivity(this.checkedItems);
  }

  postDataReg_ExActivity() {
    this.api.postDataReg_ExActivity().subscribe((res) => {
      if (res) {
        this.toastr.success("Đăng ký thành công");
        this.getDataReg_Result();
      } else {
        this.toastr.error("Đăng ký không thành công");
      }
    });
  }

  getDataReg_Schedule() {
    const getUserInfo = localStorage.getItem("userInfo");
    if (getUserInfo) {
      this.userInfo = JSON.parse(getUserInfo);
      this.apiKq.setUserInfo(JSON.parse(getUserInfo));
      this.api.getDataReg_Schedule().subscribe((res) => {
        if (res.code === 200) {
          console.log(res.data);
          this.dataSchedule = res.data.filter(
            (item: LichDK) => item.tenLop === this.userInfo.tenLop
          );
          this.dataSchedule.sort((a: LichDK, b: LichDK) => {
            const dateA = new Date(a.ngayBatDau).getTime();
            const dateB = new Date(b.ngayBatDau).getTime();
            return dateB - dateA;
          });
          console.log("sdsds", this.dataSchedule[0]);
          if (this.dataSchedule[0]) {
            if (
              this.isCurrentTimeInRange(
                this.dataSchedule[0].ngayBatDau,
                this.dataSchedule[0].ngayKetThuc
              )
            ) {
              this.getData();
            } else {
              console.log("Err: Không gọi được data");
            }
          }
        } else {
          console.log("Errorrr");
        }
      });
    }
  }

  getDataReg_Result() {
    this.apiKq.getDataReg_Result().subscribe((res) => {
      if (res.code === 200) {
        this.dataReg_Result = res.data.filter((item: any) => {
          const itemStartDate = new Date(item.ngayBd);
          const itemEndDate = new Date(item.ngayKt);

          // Kiểm tra xem item.ngayBD và ngayKT có nằm trong khoảng của displayDataTime không
          return (
            itemStartDate >= this.startDateNextSemester &&
            itemEndDate <= this.endDateNextSemester
          );
        });
      }
    });
  }

  filterDataByOption(options: any) {
    if (options === "trongKhoa") {
      this.getData();
    } else if (options === "ngoaiKhoa") {
      this.getDataAllDepartment();
    }
  }

  isChecked(item: any): boolean {
    return this.checkedItems.some(
      (checkedItem) => checkedItem.idtthdnk === item.idtthdnk
    );
  }

  toggleOptions(options: any) {
    this.filterDataByOption(options);
    //console.log(this.filteredDataHDNK)
  }

  onDelete(id: any) {
    this.api.delDataReg_ExActivity(id).subscribe((res) => {
      if (res) {
        this.toastr.success("Xoá hoạt động thành công");
        this.getDataReg_Result();
      } else {
        this.toastr.error("Đã xảy ra lỗi trong quá trình xoá !");
      }
    });
  }

  openPopupComment(data: any) {
    this.isOpenPopup = !this.isOpenPopup;
    this.detailActivity = data;
  }

  closePopupComment() {
    this.isOpenPopup = !this.isOpenPopup;
  }

  exportPDF() {
    let doc = new jsPDF();
    autoTable(doc, {
      html: "#test",
    });

    doc.save("test");
  }

  dataLichDK = {
    idlichDangKy: -1079364861,
    tenLop: "Lớp 001",
    tenNhhk: "Học kỳ 2 - Năm học 2023-2024", //khong cần
    ngayBatDau: "2024-05-01T00:00:00",
    ngayKetThuc: "2024-05-03T00:00:00",
  };
}
