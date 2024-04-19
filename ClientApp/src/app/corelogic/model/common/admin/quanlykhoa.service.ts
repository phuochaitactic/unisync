import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlykhoaService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/NDMKhoa";

  constructor(private http: HttpClient) {}

  getKhoaData(): Observable<any> {
    return this.http.get<any>("/api/NDMKhoa");
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/NDMKhoa`, excelData);
  }

  postThemKhoa(addKhoa: any): Observable<any> {
    return this.http.post(`/api/NDMKhoa`, addKhoa);
  }

  putEditKhoa(editingkhoa: any): Observable<any> {
    return this.http.put(`/api/NDMKhoa/${editingkhoa.idkhoa}`, editingkhoa);
  }

  deleteKhoa(idkhoa: number): Observable<any> {
    const url = `/api/NDMKhoa/${idkhoa}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
