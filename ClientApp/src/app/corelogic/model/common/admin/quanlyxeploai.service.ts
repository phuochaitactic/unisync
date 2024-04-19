import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlyxeploaiService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getXLData(): Observable<any> {
    const apiUrl = `/api/KDMXepLoai`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any, tenvanBan: string): Observable<any> {
    return this.http.post(`/api/KDMXepLoai?tenVanBan=${tenvanBan}`, excelData);
  }

  postThemXL(addSV: any, tenvanBan: string): Observable<any> {
    return this.http.post(`/api/KDMXepLoai?tenVanBan=${tenvanBan}`, addSV);
  }

  putEditXL(editingSV: any, tenvanBan: string): Observable<any> {
    return this.http.put(
      `/api/KDMXepLoai?tenVanBan=${tenvanBan}${editingSV.idxepLoai}`,
      editingSV
    );
  }

  deleteXL(idxepLoai: number): Observable<any> {
    const url = `/api/KDMXepLoai/${idxepLoai}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
