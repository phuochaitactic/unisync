import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlydiadiemService {
  api = "http://aqtech.vn:5564/CongRenLuyen/api/PDMDiaDiem";

  constructor(private http: HttpClient) {}

  getDiadiemData(): Observable<any> {
    return this.http.get<any>("/api/PDMDiaDiem");
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/PDMDiaDiem`, excelData);
  }

  postThemDiadiem(addKhoa: any): Observable<any> {
    return this.http.post("/api/PDMDiaDiem", addKhoa);
  }

  putEditDiadiem(editingkhoa: any): Observable<any> {
    const url = `/api/PDMDiaDiem/${editingkhoa.iddiaDiem}`;

    return this.http.put(url, editingkhoa);
  }
  deleteDiadiem(iddiaDiem: number): Observable<any> {
    const url = `/api/PDMDiaDiem/${iddiaDiem}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
