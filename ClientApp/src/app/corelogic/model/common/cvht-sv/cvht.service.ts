import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { KetquaDK } from "src/app/corelogic/interface/cvht-sv/ketquadk.model";

@Injectable({
  providedIn: "root",
})
export class CvhtService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getDataReg_ResultFromAdviser(): Observable<any> {
    return this.http.get(`/api/KkqSvDkHdnk/sinhVien/-2128302847`);
  }

  getDataStudent(): Observable<any> {
    return this.http.get(`/api/Sdmsvs`);
  }

  getDataStudentByAdvisor(tenNhhk: string, id: string): Observable<any> {
    return this.http.get(
      `/api/Sdmsvs/ByGiangVien?TenNhhk=${tenNhhk}&IdGiangVien=${id}`
    );
  }

  getYearSemester(): Observable<any> {
    return this.http.get(`/api/KDMNHHK`);
  }

  getDataReviewSchedule(): Observable<any> {
    return this.http.get(`/api/KDMLichDuyetSV`);
  }

  postCommentActivities(
    tenSinhVien: string,
    tenNhhk: string,
    loiNhan: string
  ): Observable<any> {
    const body = {
      tenSinhVien: tenSinhVien,
      tenNhhk: tenNhhk,
      loiNhan: loiNhan,
    };
    return this.http.post(`/api/KDMDsSinhVIenDangKy`, body);
  }
}
