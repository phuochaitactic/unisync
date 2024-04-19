import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlybacheService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getBacHeData(): Observable<any> {
    const apiUrl = `/api/KDMBacHe`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/KDMBacHe`, excelData);
  }

  postThemBH(addSV: any): Observable<any> {
    return this.http.post(`/api/KDMBacHe`, addSV);
  }

  putEditBH(editingSV: any): Observable<any> {
    return this.http.put(`/api/KDMBacHe/${editingSV.idbh}`, editingSV);
  }

  deleteBH(idbh: number): Observable<any> {
    const url = `/api/KDMBacHe/${idbh}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
