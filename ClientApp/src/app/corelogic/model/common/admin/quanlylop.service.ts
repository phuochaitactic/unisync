import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlylopService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/SDMLop";

  constructor(private http: HttpClient) {}

  getLopData(): Observable<any> {
    const apiUrl = `/api/SDMLop`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(
    excelData: any,
    tenKhoa: string,
    TenBHNganh: string
  ): Observable<any> {
    const url = `/api/SDMLop?tenKhoa=${tenKhoa}&TenBHNganh=${TenBHNganh}`;
    return this.http.post(url, excelData);
  }

  postThemLop(
    addSV: any,
    tenKhoa: string,
    TenBHNganh: string
  ): Observable<any> {
    const url = `/api/SDMLop?tenKhoa=${tenKhoa}&TenBHNganh=${TenBHNganh}`;
    return this.http.post(url, addSV);
  }

  putEditLop(
    editingSV: any,
    tenKhoa: string,
    TenBHNganh: string
  ): Observable<any> {
    const url = `/api/SDMLop/${editingSV.idlop}`;
    let params = new HttpParams()
      .set("tenKhoa", tenKhoa)
      .set("TenBHNganh", TenBHNganh);
    return this.http.put(url, editingSV, { params });
  }

  deleteLop(idlop: number): Observable<any> {
    const url = `/api/SDMLop/${idlop}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
