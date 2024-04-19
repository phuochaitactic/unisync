import { Component } from "@angular/core";
import { Router } from "@angular/router";
import {
  trigger,
  state,
  transition,
  style,
  animate,
} from "@angular/animations";
import { ToastrService } from "ngx-toastr";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import { KhoaService } from "src/app/corelogic/model/common/khoa/khoa.service";
import { AuthenticalService } from "src/app/corelogic/model/authentical.service";
import { NamhocService } from "src/app/corelogic/model/common/khoa/namhoc.service";

@Component({
  selector: "app-homeql",
  templateUrl: "./homeql.component.html",
  styleUrls: ["./homeql.component.css"],

  animations: [
    trigger("accordionAnimation", [
      state(
        "closed",
        style({
          height: 0,
        })
      ),
      state(
        "open",
        style({
          height: "auto",
        })
      ),
      transition("closed => open", animate("0.3s ease-in")),
    ]),
  ],
})
export class HomeqlComponent {
  activeButton: string | null = null;
  semester = false;
  toggleLogout = false;
  dropNHHK: any;
  user: any;
  showOptionsNH: boolean = false;
  selectedOptionNH: string = "";
  selectedOptionNH2: any;
  years: string[] = [];
  selectedYear: string = "";
  selectedSemester = "";
  filteredNhhkData: any;

  constructor(
    private router: Router,
    private api: ApifullService,
    private toast: ToastrService,
    private addId: KhoaService,
    private auth: AuthenticalService,
    private getNam: NamhocService
  ) {}

  ngOnInit(): void {
    this.api.getNHData().subscribe((data) => {
      this.dropNHHK = data.data;
      this.years = Array.from(
        new Set(
          this.dropNHHK.map((item: any) => this.extractYear(item.tenNhhk))
        )
      );

      this.years.sort((a, b) => parseInt(b) - parseInt(a));
    });
    this.api.getUser().subscribe((data) => {
      this.user = data.data[0].tenKhoa;
    });
    this.buttonBHNganh();
  }
  onSemesterChange(event: any): void {
    this.selectedSemester = event.target.value;
    this.selectedOptionNH = this.selectedSemester;
    this.addId.setTenKhoa(this.user);
    if (this.selectedOptionNH !== undefined) {
      this.getNam.setSelectedYear(this.selectedOptionNH);
    } else {
      console.error("Giá trị năm học không được xác định.");
    }
  }
  extractYear(tenNhhk: string): string {
    const yearMatch = tenNhhk.match(/\d{4}-\d{4}/);
    return yearMatch ? yearMatch[0] : "";
  }

  filterNhhkData(): void {
    this.filteredNhhkData = this.dropNHHK.filter(
      (item: any) => this.extractYear(item.tenNhhk) === this.selectedYear
    );
    this.filteredNhhkData.sort((a: any, b: any) => {
      const semesterA = parseInt(a.tenNhhk.match(/\d+/)[0]);
      const semesterB = parseInt(b.tenNhhk.match(/\d+/)[0]);

      return semesterB - semesterA;
    });
  }

  onYearChange(event: any): void {
    this.selectedYear = event.target.value;
    this.addId.setTenNhhk(this.selectedYear);
    this.filterNhhkData();
  }
  showNH() {
    this.showOptionsNH = !this.showOptionsNH;
  }
  // selectedNH(lop: any) {
  //   this.selectedOptionNH = lop.tenNhhk;
  //   const idnhhk = lop.idnhhk;
  //   this.addId.setTenKhoa(this.user);
  //   this.addId.setIdNhhk(idnhhk);
  //   if (this.selectedOptionNH !== undefined) {
  //     this.getNam.setSelectedYear(this.selectedOptionNH);
  //   } else {
  //     console.error("Giá trị năm học không được xác định.");
  //   }
  //   this.showOptionsNH = false;
  // }
  togglelogout() {
    this.toggleLogout = !this.toggleLogout;
  }
  logout() {
    this.auth.logout().subscribe(() => {
      localStorage.removeItem("activeButton");
      this.router.navigate(["/"]);
      this.toast.success("Đăng xuất thành công");
    });
  }
  buttonNext() {
    if (this.selectedOptionNH === "") {
      this.toast.warning("Vui lòng chọn năm học-học kỳ");
      return;
    } else {
      this.selectedOptionNH2 = this.selectedOptionNH;
      this.semester = true;
    }
  }

  buttonBHNganh() {
    this.activeButton = "Bậc hệ ngành";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/bachenganh"]);
  }

  buttonSVCTDT() {
    this.activeButton = "Sinh viên theo CTDT";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/sinhvienctdt"]);
  }

  buttonDSVBQD() {
    this.activeButton = "Danh sách VB-QD";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/danhsachvbqd"]);
  }

  buttonDSTHK() {
    this.activeButton = "Danh sách tuần học kì";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/danhsachthk"]);
  }

  buttonLKHDK() {
    this.activeButton = "Lập kế hoạch dự kiến";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/lapkehoachdukien"]);
  }
  buttonDSHDNK() {
    this.activeButton = "Danh sách HDNK";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/danhsachhdnk"]);
  }
  buttonLKHHD() {
    this.activeButton = "Lập kế hoạch HD";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/lichdk"]);
  }
  buttonXLHDNK() {
    this.activeButton = "Tạo lịch Duyệt";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["dashboard/quanlykhoa/lichduyet"]);
  }

  showDropdown = true;
  showDropdown2 = true;
  showDropdown3 = false;

  toggleButtons(idContent: any) {
    switch (idContent) {
      // case "systemContent":
      //   this.showDropdown = !this.showDropdown;
      //   break;
      case "systemContent":
        this.showDropdown2 = !this.showDropdown2;
        break;
      case "otherContent":
        this.showDropdown3 = !this.showDropdown3;
        break;
      default:
        console.log("error");
        break;
    }
  }

  selectSemester() {
    if (this.selectedOptionNH === "") {
      this.toast.warning("Vui lòng chọn năm học-học kỳ");
      return;
    }
    this.semester = !this.semester;
    this.showOptionsNH = false;
  }
}
