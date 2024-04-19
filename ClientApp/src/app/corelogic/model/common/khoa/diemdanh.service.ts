import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class DiemdanhService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  getLopData(): Observable<any> {
    const apiUrl = `/api/KDMTTHDNK`;
    return this.http.get<any>(apiUrl);
  }
  getDataDD(idHdnk: any): Observable<any> {
    const apiUrl = `/api/KkqSvDkHdnk/DsSinhVienThamGia?IdHdnk=${idHdnk}`;
    return this.http.get<any>(apiUrl);
  }
  getDataRE(idHdnk: any): Observable<any> {
    const apiUrl = `/api/KkqSvDkHdnk/DsSinhVienThamGia?IdHdnk=${idHdnk}`;
    return this.http.get<any>(apiUrl);
  }

  getBHNData(tenKhoa: string): Observable<any> {
    const apiUrl = `/api/KDMBacHeNganh/BacHeNganhTheoKhoa`;
    const params = new HttpParams().set("TenKhoa", tenKhoa);
    return this.http.get<any>(apiUrl, { params });
  }

  putDDData(idHdnk: any, idSinhVien: any, ngayThamGia: string) {
    const apiUrl = `/api/Qr`;
    const body = { idHdnk, idSinhVien, ngayThamGia };
    return this.http.put<any>(apiUrl, body);
  }
  putTGData(editingLop: any): Observable<any> {
    return this.http.put(
      `/api/KkqSvDkHdnk/thamGia/${editingLop.idSinhVien}`,
      editingLop
    );
  }
  deleteDD(
    idHdnk: number,
    idSinhVien: any,
    trangthai: boolean
  ): Observable<any> {
    const url = `/api/KkqSvDkHdnk/thamGia`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" }),
    };

    const data = {
      idHdnk: idHdnk,
      idSinhVien: idSinhVien,
      isThamGia: false,
    };

    return this.http.put(url, data, httpOptions);
  }
}
