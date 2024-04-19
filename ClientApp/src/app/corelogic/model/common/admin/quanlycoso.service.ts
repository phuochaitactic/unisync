import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlycosoService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getCosoData(): Observable<any> {
    const apiUrl = `${this.apiURL}/quanlycoso`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`${this.apiURL}/quanlycoso`, excelData);
  }

  postThemCoso(addKhoa: any): Observable<any> {
    return this.http.post(`${this.apiURL}/quanlycoso`, addKhoa);
  }

  putEditCoso(editingkhoa: any): Observable<any> {
    return this.http.put(
      `${this.apiURL}/quanlycoso/${editingkhoa.id}`,
      editingkhoa
    );
  }

  deleteCoso(id: number): Observable<any> {
    const url = `${this.apiURL}/quanlycoso/${id}`;
    return this.http.delete(url);
  }
}
