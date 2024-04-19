import { Component, HostListener, OnInit } from "@angular/core";
import { initFlowbite } from "flowbite";
import { SinhVienCTDT } from "src/app/corelogic/interface/khoa/sinhvienctdt.model";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import { KhoaService } from "src/app/corelogic/model/common/khoa/khoa.service";
import { NamhocService } from "src/app/corelogic/model/common/khoa/namhoc.service";

@Component({
  selector: "app-sinhvienctdt-tk",
  templateUrl: "./sinhvienctdt-tk.component.html",
  styleUrls: ["./sinhvienctdt-tk.component.css"],
})
export class SinhvienctdtTkComponent implements OnInit {
  title = "Danh Sách Sinh Viên Theo CTDT";
  sinhvienData: SinhVienCTDT[] = [];
  checkAll: boolean = false;
  stt: number = 1;
  selectedItems: boolean[] = [];
  isIcon = true;
  isIcons = true;
  searchText = "";
  p: number = 1;
  bhn: string = "Lớp";
  tbhn = "";
  isButtonSelected = false;
  loading = false;
  activeButton: string | null = null;
  filter1 = false;
  user: any;
  dataSv: any;
  idKhoa: any;
  tenNhhk: any;
  dele = false;

  constructor(
    private sinhvienService: KhoaService,
    private getUser: ApifullService,
    private getNam: NamhocService
  ) {}

  ngOnInit() {
    initFlowbite();
    this.getUser.getUser().subscribe((data) => {
      this.idKhoa = data.data[0].userId;

      this.getNam.selectedYear$.subscribe((newYear: string) => {
        this.sinhvienService
          .getSVKData(newYear, this.idKhoa)
          .subscribe((data: any) => {
            this.sinhvienData = data.data;
          });
      });
    });
  }

  @HostListener("document:click", ["$event"])
  onClick(event: MouseEvent) {
    // Kiểm tra xem sự kiện click có xảy ra bên ngoài button không
    const target = event.target as HTMLElement;
    if (!target.closest("button")) {
      this.isButtonSelected = false;
    }
  }

  // tinhTong() {
  //   this.tongsinhvien = this.bachenganh.length;
  // }
  // getTong() {
  //   return this.tongsinhvien;
  // }
  filterNam(slectedNK: any) {
    this.tbhn = slectedNK;
    this.bhn = "Lớp: " + slectedNK;
  }
  resetFilterNK() {
    this.tbhn = "";
    this.bhn = "Tất cả lớp ";
  }

  toggleDropDown() {
    this.filter1 = !this.filter1;
    this.activeButton = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton = "";
    }
    localStorage.setItem("activeButton", this.activeButton);
  }
}
