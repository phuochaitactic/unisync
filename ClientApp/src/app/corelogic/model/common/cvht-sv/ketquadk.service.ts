import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class KetquadkService {
  apiURL = "http://localhost:3000";

  private userInfo: any;
  private activitySource = new BehaviorSubject<string>("");
  currentActivity = this.activitySource.asObservable();

  constructor(private http: HttpClient) {}

  setUserInfo(userInfo: any) {
    this.userInfo = userInfo;
  }

  setActivityId(activity: any) {
    this.activitySource.next(activity);
  }

  getYearSemester(): Observable<any> {
    return this.http.get(`/api/KDMNHHK`);
  }

  getDataReg_Result(): Observable<any> {
    const param = this.userInfo.userId;
    return this.http.get(`/api/KkqSvDkHdnk/sinhVien/${param}`);
  }

  getDataReg_ResultFromAdviser(id: any): Observable<any> {
    return this.http.get(`/api/KkqSvDkHdnk/sinhVien/${id}`);
  }

  putDataReg_ResultEachActivityFromAdviser(
    option: boolean,
    data: any
  ): Observable<any> {
    const activity = data;
    const id = activity.iddangKy;
    console.log("option ", option);
    console.log(id);
    const apiUrl = `/api/KkqSvDkHdnk/${id}`;
    const body = {
      tensinhVien: activity.tensinhVien,
      tenhdnk: activity.tenhdnk,
      tengiangVien: activity.tengiangVien,
      ngayLap: activity.ngayLap,
      ngayDuyet: new Date().toISOString(),
      tinhTrangDuyet: option,
      ghiChu: "chua",
      ngayThamGia: activity.ngayThamGia,
      isThamGia: activity.isThamGia,
      soDiem: activity.soDiem,
      minhChungThamGia: activity.minhChungThamGia,
      vaiTroTg: activity.vaiTroTg,
      ngayBd: activity.ngayBd,
      ngayKt: activity.ngayKt,
      maDieu: activity.maDieu,
    };

    return this.http.put<any>(apiUrl, body);
  }

  putDataReg_ResultFromAdviser(option: boolean): Observable<any> {
    const activity = this.activitySource.getValue() as any;
    const id = activity.iddangKy;
    console.log("option ", option);
    console.log(id);
    const apiUrl = `/api/KkqSvDkHdnk/${id}`;
    const body = {
      tensinhVien: activity.tensinhVien,
      tenhdnk: activity.tenhdnk,
      tengiangVien: activity.tengiangVien,
      ngayLap: activity.ngayLap,
      ngayDuyet: new Date().toISOString(),
      tinhTrangDuyet: option,
      ghiChu: "chua",
      ngayThamGia: activity.ngayThamGia,
      isThamGia: activity.isThamGia,
      minhChungThamGia: activity.minhChungThamGia,
      vaiTroTg: activity.vaiTroTg,
    };

    return this.http.put<any>(apiUrl, body);
  }

  getComment(): Observable<any> {
    const param = this.userInfo.userId;
    return this.http.get(
      `/api/KDMDsSinhVIenDangKy/TheoSinhVien?idSinhVien=${param}`
    );
  }
}
