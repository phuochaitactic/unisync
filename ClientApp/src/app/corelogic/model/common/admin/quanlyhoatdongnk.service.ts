import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlyhoatdongnkService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/KDMHoatDongNgoaiKhoa";

  constructor(private http: HttpClient) {}

  getHDNKData(): Observable<any> {
    const apiUrl = `/api/KDMHoatDongNgoaiKhoa`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any, maDieu: string): Observable<any> {
    let params = new HttpParams().set("maDieu", maDieu);
    return this.http.post(`/api/KDMHoatDongNgoaiKhoa`, excelData, { params });
  }

  postThemHDNK(addSV: any, maDieu: string): Observable<any> {
    let params = new HttpParams().set("maDieu", maDieu);
    return this.http.post(`/api/KDMHoatDongNgoaiKhoa`, addSV, { params });
  }
  putEditHDNK(editingSV: any, maDieu: string): Observable<any> {
    const url = `/api/KDMHoatDongNgoaiKhoa/${editingSV.idhdnk}`;
    let params = new HttpParams().set("maDieu", maDieu);
    return this.http.put(url, editingSV, { params });
  }

  deleteHDNK(idhdnk: number): Observable<any> {
    const url = `/api/KDMHoatDongNgoaiKhoa/${idhdnk}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
