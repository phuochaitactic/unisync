import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, map, forkJoin, concatMap, from } from "rxjs";
import { DangkyHDNK } from "src/app/corelogic/interface/cvht-sv/dangkyhdnk.model";
import { UserService } from "./user.service";

@Injectable({
  providedIn: "root",
})
export class DangkyhdnkService {
  apiURL = "http://localhost:3000";

  private userInfoJSON: any;
  private dataExActivity: any;

  constructor(private http: HttpClient, private userService: UserService) {}

  setDataExActivity(data: any) {
    this.dataExActivity = data;
    const userInfo = localStorage.getItem("userInfo");
    if (userInfo) {
      this.userInfoJSON = JSON.parse(userInfo);
    }
  }

  getDataReg_Schedule(): Observable<any> {
    return this.http.get(`/api/KdmlichDangKy`);
  }

  getDataReg_ExActivityByDepartment(
    tenKhoa: string,
    maKhoa: string
  ): Observable<any> {
    return this.http.get(`api/KDMTTHDNK/tenKhoa/${tenKhoa}?MaKhoa=${maKhoa}`);
  }

  getDataReg_ExActivity(): Observable<any> {
    return this.http.get(`api/KDMTTHDNK`);
  }

  postDataReg_ExActivity(): Observable<any> {
    return (from(this.dataExActivity) as Observable<DangkyHDNK>).pipe(
      concatMap((item: DangkyHDNK) => {
        const body = {
          tensinhVien: this.userInfoJSON.hoTenSinhVien,
          tenhdnk: item.tenHdnk,
          tengiangVien: this.userInfoJSON.tenGv,
          ngayLap: new Date().toISOString(),
          ngayDuyet: new Date().toISOString(),
          tinhTrangDuyet: false,
          ghiChu: "Không có",
          ngayThamGia: new Date().toISOString(),
          isThamGia: true,
          soDiem: item.diemHdnk,
          minhChungThamGia: "string",
          vaiTroTg: "Tham Gia",
          ngayBd: item.ngayBd,
          ngayKt: item.ngayKt,
          maDieu: item.maDieu,
        };

        console.log("Request Body:", body);

        return this.http.post(`/api/KkqSvDkHdnk`, body);
      })
    );
  }

  delDataReg_ExActivity(id: string): Observable<any> {
    return this.http.delete(`/api/KkqSvDkHdnk/${id}`);
  }
}
