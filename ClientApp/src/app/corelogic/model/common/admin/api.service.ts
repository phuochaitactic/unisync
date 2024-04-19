import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class ApiService {
  constructor(private http: HttpClient) {}

  getKhoaData(): Observable<any> {
    return this.http.get<any>("/api/NDMKhoa");
  }

  getLopData(): Observable<any> {
    const apiUrl = `/api/SDMLop`;
    return this.http.get<any>(apiUrl);
  }

  getGVData(): Observable<any> {
    return this.http.get<any>("/api/NDMGiangVien");
  }

  getNHData(): Observable<any> {
    return this.http.get<any>("/api/KDMNHHK");
  }
  getBHNData(): Observable<any> {
    const apiUrl = `/api/KDMBacHeNganh`;
    return this.http.get<any>(apiUrl);
  }
  getBHNKData(TenKhoa: string): Observable<any> {
    const apiUrl = `/api/KDMBacHeNganh/BacHeNganhTheoKhoa?TenKhoa=${TenKhoa}`;
    return this.http.get<any>(apiUrl);
  }
  getBHData(): Observable<any> {
    const apiUrl = `/api/KDMBacHe`;
    return this.http.get<any>(apiUrl);
  }
  getNganhData(): Observable<any> {
    const apiUrl = `/api/KDMNganh`;
    return this.http.get<any>(apiUrl);
  }

  getPhongData(): Observable<any> {
    const apiUrl = `/api/PDMPhong`;
    return this.http.get<any>(apiUrl);
  }

  getDiadiemData(): Observable<any> {
    return this.http.get<any>("/api/PDMDiaDiem");
  }
}
