import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlysinhvienService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/Sdmsvs";

  constructor(private http: HttpClient) {}

  getSVData(): Observable<any> {
    const apiUrl = `/api/Sdmsvs`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(
    excelData: any,
    tenLop: string,
    tenNHHK: string,
    tenGiangVien: string
  ): Observable<any> {
    const url = `/api/Sdmsvs?tenLop=${tenLop}&tenNHHK=${tenNHHK}&tenGiangVien=${tenGiangVien}`;
    return this.http.post(url, excelData);
  }
  postThemSV(
    addSV: any,
    tenLop: string,
    tenNHHK: string,
    tenGiangVien: string
  ): Observable<any> {
    let params = new HttpParams()
      .set("tenLop", tenLop)
      .set("tenNHHK", tenNHHK)
      .set("tenGiangVien", tenGiangVien);
    return this.http.post(`/api/Sdmsvs`, addSV, { params });
  }

  putEditSV(
    editingSV: any,
    tenLop: string,
    tenNHHK: string,
    tenGiangVien: string
  ): Observable<any> {
    const url = `/api/Sdmsvs?id=${editingSV.idsinhVien}&tenLop=${tenLop}&tenNHHK=${tenNHHK}&tenGiangVien=${tenGiangVien}`;
    return this.http.put(url, editingSV);
  }

  deleteSV(idsinhVien: number): Observable<any> {
    const url = `/api/Sdmsvs/${idsinhVien}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
