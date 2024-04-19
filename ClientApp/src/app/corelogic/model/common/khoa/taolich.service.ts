import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class TaolichService {
  constructor(private http: HttpClient) {}

  getLichData(): Observable<any> {
    const apiUrl = `/api/KdmlichDangKy`;
    return this.http.get<any>(apiUrl);
  }
  postThemLich(addHDNK: any): Observable<any> {
    return this.http.post(`/api/KdmlichDangKy`, addHDNK);
  }
  postDataToDatabases(excelData: any): Observable<any> {
    return this.http.post(`/api/KdmlichDangKy`, excelData);
  }
  putEditLich(editing: any): Observable<any> {
    return this.http.put(`/api/KdmlichDangKy/${editing.idlichDangKy}`, editing);
  }
  deleteLich(idlichDangKy: number): Observable<any> {
    const url = `/api/KdmlichDangKy/${idlichDangKy}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }

  //duyêt
  getLichDuyetData(): Observable<any> {
    const apiUrl = `/api/KDMLichDuyetSV`;
    return this.http.get<any>(apiUrl);
  }
  postLichDuyetD(addHDNK: any): Observable<any> {
    return this.http.post(`/api/KDMLichDuyetSV`, addHDNK);
  }
  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/KDMLichDuyetSV`, excelData);
  }
  putEditLichD(editing: any): Observable<any> {
    return this.http.put(`/api/KDMLichDuyetSV/${editing.idlichDuyet}`, editing);
  }

  deleteLichD(idlichDuyet: number): Observable<any> {
    const url = `/api/KDMLichDuyetSV/${idlichDuyet}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
