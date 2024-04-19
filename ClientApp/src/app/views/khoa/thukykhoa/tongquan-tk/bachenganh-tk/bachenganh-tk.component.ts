import { Component, HostListener } from "@angular/core";
import { initFlowbite } from "flowbite";
import { BacHeNganhK } from "src/app/corelogic/interface/khoa/bachenganhk.model";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import { BachenganhkService } from "src/app/corelogic/model/common/khoa/bachenganhk.service";
import { KhoaService } from "src/app/corelogic/model/common/khoa/khoa.service";

@Component({
  selector: "app-bachenganh-tk",
  templateUrl: "./bachenganh-tk.component.html",
  styleUrls: ["./bachenganh-tk.component.css"],
})
export class BachenganhTkComponent {
  title = "Danh Sách Bậc Hệ Ngành";
  bachenganh: BacHeNganhK[] = [];
  checkAll: boolean = false;
  stt: number = 1;
  selectedItems: boolean[] = [];
  isIcon = true;
  isIcons = true;
  searchText = "";
  p: number = 1;
  bhn: string = "";
  tbhn = "";
  isButtonSelected = false;
  loading = false;
  activeButton: string | null = null;
  filter1 = false;
  namebhn = "Tên Bậc Hệ Ngành";
  tenKhoa: string = "";
  idNhhk: string = "";
  showDeleteAllButton = false;
  dataBHN: any;
  slsv: any;

  constructor(
    private bachenganhService: BachenganhkService,
    private khoaService: KhoaService,
    private apiFull: ApifullService
  ) {}

  ngOnInit(): void {
    initFlowbite();
    this.loading = true;
    this.apiFull.getUser().subscribe((data) => {
      this.tenKhoa = data.data[0].tenKhoa;
      this.khoaService.getBHNData(this.tenKhoa).subscribe((data) => {
        this.bachenganh = data.data;
        const dataBHN = this.bachenganh.map((item) => item.tenBh);

        dataBHN.forEach((tenBh) => {
          this.apiFull.getSVBHN(tenBh).subscribe((svData: any) => {
            this.slsv = svData.data.length;
          });
        });
      });

      this.loading = false;
    });
  }

  @HostListener("document:click", ["$event"])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest("button")) {
      this.isButtonSelected = false;
    }
  }

  filterNam(slectedNK: any) {
    this.tbhn = slectedNK;
    this.namebhn = "Bậc Hệ Ngành: " + slectedNK;
  }
  resetFilterNK() {
    this.tbhn = "";
    this.namebhn = "Tất Cả Bậc Hệ Ngành";
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
