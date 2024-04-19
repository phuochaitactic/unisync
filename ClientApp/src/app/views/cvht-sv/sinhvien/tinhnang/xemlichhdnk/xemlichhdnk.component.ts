import { Component } from "@angular/core";
import {
  Appointment,
  Service,
} from "src/app/corelogic/model/common/cvht-sv/schedule.service";
import { KetquadkService } from "src/app/corelogic/model/common/cvht-sv/ketquadk.service";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";

@Component({
  selector: "app-xemlichhdnk",
  templateUrl: "./xemlichhdnk.component.html",
  styleUrls: ["./xemlichhdnk.component.css"],
  providers: [Service],
})
export class XemlichhdnkComponent {
  dataReg_Result: KetquaDK[] = [];
  appointmentsData: Appointment[] = [];

  currentDate: Date = new Date();

  constructor(
    service: Service,
    private api: KetquadkService,
    private services: Service
  ) {}

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    const a = localStorage.getItem("userInfo");
    if (a) {
      const userInfoObject = JSON.parse(a);
      console.log(userInfoObject);
      this.api.setUserInfo(userInfoObject);
      this.getDataReg_Result();
    } else {
      console.log("No userInfo found in localStorage");
    }
  }

  getDataReg_Result() {
    this.api.getDataReg_Result().subscribe((res) => {
      console.log("sdsdsd", res);
      if (res.code === 200) {
        this.dataReg_Result = res.data.filter(
          (item: { tinhTrangDuyet: boolean }) => item.tinhTrangDuyet === true
        );
        this.appointmentsData = this.services.convertToAppointments(
          this.dataReg_Result
        );
      }
    });
  }
}
