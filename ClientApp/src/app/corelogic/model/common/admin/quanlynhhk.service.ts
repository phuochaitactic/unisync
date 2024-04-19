import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlynhhkService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/KDMNHHK";

  constructor(private http: HttpClient) {}

  getNHHKData(): Observable<any> {
    return this.http.get<any>("/api/KDMNHHK");
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/KDMNHHK`, excelData);
  }

  postThemNH(addSV: any): Observable<any> {
    return this.http.post("/api/KDMNHHK", addSV);
  }

  putEditNH(editingSV: any): Observable<any> {
    const url = `/api/KDMNHHK/${editingSV.idnhhk}`;
    return this.http.put(url, editingSV);
  }

  deleteNH(idnhhk: number): Observable<any> {
    const url = `/api/KDMNHHK/${idnhhk}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
