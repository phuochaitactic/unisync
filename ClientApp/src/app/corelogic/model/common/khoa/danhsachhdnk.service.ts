import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class DanhsachhdnkService {
  constructor(private http: HttpClient) {}

  getLopData(): Observable<any> {
    const apiUrl = `/api/KDMTTHDNK`;
    return this.http.get<any>(apiUrl);
  }
  getHDbyKhoa(tenKhoa: any): Observable<any> {
    const apiUrl = `/api/KDMTTHDNK/tenKhoa/${tenKhoa}`;
    return this.http.get<any>(apiUrl);
  }
  getHdnkbyId(idduLieuHdnk: any): Observable<any> {
    const apiUrl = `/api/KDMDuLieuHdnk/${idduLieuHdnk}`;
    return this.http.get(apiUrl);
  }

  postAddHDNK(addHDNK: any): Observable<any> {
    return this.http.post(`/api/KDMHoatDongNgoaiKhoa`, addHDNK);
  }
  postAddHDNK2(addHDNK: any): Observable<any> {
    return this.http.post(`/api/KDMDuLieuHdnk`, addHDNK);
  }
  postAddHDNK3(addHDNK: any): Observable<any> {
    return this.http.post(`/api/KDMTTHDNK`, addHDNK);
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/KDMTTHDNK`, excelData);
  }

  putEditHdnk(editingHD: any): Observable<any> {
    return this.http.put(`/api/KDMTTHDNK/${editingHD.idtthdnk}`, editingHD);
  }

  deleteHD(idtthdnk: number): Observable<any> {
    const url = `/api/KDMTTHDNK/${idtthdnk}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
  deleteHDNK(idhdnk: number): Observable<any> {
    const url = `/api/KDMHoatDongNgoaiKhoa/${idhdnk}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
  deleteDLhd(idduLieuHdnk: number): Observable<any> {
    const url = `/api/KDMDuLieuHdnk/${idduLieuHdnk}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
