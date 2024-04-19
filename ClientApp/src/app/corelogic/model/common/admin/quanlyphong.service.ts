import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlyphongService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/PDMPhong";

  constructor(private http: HttpClient) {}

  getPhongData(): Observable<any> {
    const apiUrl = `/api/PDMPhong`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any, tenDiaDiem: string): Observable<any> {
    let params = new HttpParams().set("tenDiaDiem", tenDiaDiem);
    return this.http.post(`/api/PDMPhong`, excelData, { params });
  }

  postThemPhong(addSV: any, tenDiaDiem: string): Observable<any> {
    let params = new HttpParams().set("tenDiaDiem", tenDiaDiem);
    return this.http.post(`/api/PDMPhong`, addSV, { params });
  }

  putEditPhong(editingSV: any, tenDiaDiem: string): Observable<any> {
    const url = `/api/PDMPhong/${editingSV.idphong}`;
    let params = new HttpParams().set("tenDiaDiem", tenDiaDiem);
    return this.http.put(url, editingSV, { params });
  }

  deletePhong(idphong: number): Observable<any> {
    const url = `/api/PDMPhong/${idphong}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
