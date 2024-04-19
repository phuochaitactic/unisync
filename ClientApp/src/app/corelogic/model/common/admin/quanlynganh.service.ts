import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
@Injectable({
  providedIn: "root",
})
export class QuanlynganhService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/KDMNganh";

  constructor(private http: HttpClient) {}

  getNganhData(): Observable<any> {
    const apiUrl = `/api/KDMNganh`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any, tenKhoa: string): Observable<any> {
    let params = new HttpParams().set("tenKhoa", tenKhoa);
    return this.http.post(`/api/KDMNganh`, excelData, { params });
  }
  postThemNganh(addSV: any, tenKhoa: string): Observable<any> {
    let params = new HttpParams().set("tenKhoa", tenKhoa);
    return this.http.post(`/api/KDMNganh`, addSV, { params });
  }

  putEditNganh(editingSV: any, tenKhoa: string): Observable<any> {
    let params = new HttpParams().set("tenKhoa", tenKhoa);
    return this.http.put(`/api/KDMNganh/${editingSV.idngh}`, editingSV, {
      params,
    });
  }

  deleteNganh(idngh: number): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    const url = `/api/KDMNganh/${idngh}`;
    return this.http.delete(url, httpOptions);
  }
}
