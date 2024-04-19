import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class ApifullService {
  constructor(private http: HttpClient) {}

  getUser() {
    const apiUrl = `/api/NDMGiangVien/userinfo`;
    return this.http.get<any>(apiUrl);
  }

  getHDNKData(): Observable<any> {
    const apiUrl = `/api/KDMHoatDongNgoaiKhoa`;
    return this.http.get<any>(apiUrl);
  }

  getDLHDData(): Observable<any> {
    const apiUrl = `/api/KDMDuLieuHdnk`;
    return this.http.get<any>(apiUrl);
  }

  getBHNData(): Observable<any> {
    const apiUrl = `/api/KDMBacHeNganh`;
    return this.http.get<any>(apiUrl);
  }
  getDieuData(): Observable<any> {
    const apiUrl = `/api/KDMDieu`;
    return this.http.get<any>(apiUrl);
  }
  getNHData(): Observable<any> {
    return this.http.get<any>("/api/KDMNHHK");
  }
  getGVData(): Observable<any> {
    return this.http.get<any>("/api/NDMGiangVien");
  }
  getSVData(): Observable<any> {
    return this.http.get<any>("/api/Sdmsvs");
  }
  getDiadiemData(): Observable<any> {
    return this.http.get<any>("/api/PDMDiaDiem");
  }
  getKhoaData(): Observable<any> {
    return this.http.get<any>("/api/NDMKhoa");
  }
  getPhongData(): Observable<any> {
    const apiUrl = `/api/PDMPhong`;
    return this.http.get<any>(apiUrl);
  }
  getLopData(): Observable<any> {
    const apiUrl = `/api/SDMLop`;
    return this.http.get<any>(apiUrl);
  }
  getUserKData(): Observable<any> {
    const apiUrl = `/api/NDMKhoa/userinfo`;
    return this.http.get<any>(apiUrl);
  }
  getNganhData(): Observable<any> {
    const apiUrl = ` /api/KDMNganh`;
    return this.http.get<any>(apiUrl);
  }
  getNDMC(): Observable<any> {
    const apifull = `/api/KDMMinhChung`;
    return this.http.get<any>(apifull);
  }
  getSVBHN(TenBacHeNganh: string): Observable<any> {
    const apiUrl = `/api/Sdmsvs/ByBacHeNganh?TenBacHeNganh=${TenBacHeNganh}`;
    return this.http.get<any>(apiUrl);
  }
}
