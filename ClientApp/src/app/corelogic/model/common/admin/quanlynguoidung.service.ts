import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlynguoidungService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getNDData(): Observable<any> {
    const apiUrl = `/api/Account`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/Account`, excelData);
  }

  postThemND(addSV: any): Observable<any> {
    return this.http.post(`/api/KDMAdmin`, addSV);
  }

  putEditND(editingSV: any): Observable<any> {
    return this.http.put(`/api/KDMAdmin/${editingSV.id}`, editingSV);
  }

  deleteND(idbh: number): Observable<any> {
    const url = `/api/KDMAdmin/${idbh}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
