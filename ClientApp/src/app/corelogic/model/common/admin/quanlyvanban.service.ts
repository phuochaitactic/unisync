import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlyvanbanService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getVanBanData(): Observable<any> {
    const apiUrl = `/api/KDMVanBan`;
    return this.http.get<any>(apiUrl);
  }
  readPdfAsBase64(pdfLink: string): Observable<any> {
    return this.http.get(pdfLink, { responseType: "arraybuffer" as "json" });
  }
  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/KDMVanBan`, excelData);
  }

  postThemVB(addKhoa: any): Observable<any> {
    return this.http.post(`/api/KDMVanBan`, addKhoa);
  }

  putEditVB(editingkhoa: any): Observable<any> {
    return this.http.put(`/api/KDMVanBan/${editingkhoa.idvanBan}`, editingkhoa);
  }

  deleteVB(idvanBan: number): Observable<any> {
    const url = `/api/KDMVanBan/${idvanBan}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
