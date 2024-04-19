import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
@Injectable({
  providedIn: "root",
})
export class QuanlygiangvienService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/NDMGiangVien";

  constructor(private http: HttpClient) {}

  getGVData(): Observable<any> {
    return this.http.get<any>("/api/NDMGiangVien");
  }

  postDataToDatabase(
    excelData: any,
    tenKhoa: string,
    matKhau: string
  ): Observable<any> {
    let params = new HttpParams()
      .set("TenKhoa", tenKhoa)
      .set("MatKhau", matKhau);
    return this.http.post(`/api/NDMGiangVien`, excelData, { params });
  }

  postThemGV(addSV: any, tenKhoa: string): Observable<any> {
    let params = new HttpParams().set("TenKhoa", tenKhoa);
    return this.http.post(`/api/NDMGiangVien`, addSV, { params });
  }

  putEditGV(editingSV: any, tenKhoa: string): Observable<any> {
    const url = `/api/NDMGiangVien/${editingSV.idgiangVien}`;
    let params = new HttpParams().set("TenKhoa", tenKhoa);
    return this.http.put(url, editingSV, { params });
  }

  deleteGV(idgiangVien: number): Observable<any> {
    const url = `/api/NDMGiangVien/${idgiangVien}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
