import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class QuanlybhnganhService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getBHNData(): Observable<any> {
    const apiUrl = `/api/KDMBacHeNganh`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(
    excelData: any,
    tenBh: string,
    tenNganh: string
  ): Observable<any> {
    let params = new HttpParams()
      .set("TenBacHe", tenBh)
      .set("TenNganh", tenNganh);
    return this.http.post(`/api/KDMBacHeNganh`, excelData, { params });
  }

  postThemBHN(addSV: any, tenBh: string, tenNganh: string): Observable<any> {
    let params = new HttpParams()
      .set("TenBacHe", tenBh)
      .set("TenNganh", tenNganh);
    return this.http.post(`/api/KDMBacHeNganh`, addSV, { params });
  }

  putEditBHN(editingSV: any, tenBh: string, tenNganh: string): Observable<any> {
    const url = `/api/KDMBacHeNganh/${editingSV.idbhngChng}`;
    let params = new HttpParams()
      .set("TenBacHe", tenBh)
      .set("TenNganh", tenNganh);
    return this.http.put(url, editingSV, { params });
  }

  deleteBHN(idbhngChng: number): Observable<any> {
    const url = `/api/KDMBacHeNganh/${idbhngChng}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
