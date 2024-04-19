import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable, BehaviorSubject, Subject } from "rxjs";
@Injectable({
  providedIn: "root",
})
export class KhoaService {
  constructor(private http: HttpClient) {}

  private tenKhoaSource = new BehaviorSubject<string>("");
  tenKhoa$ = this.tenKhoaSource.asObservable();

  private tenNhhkSource = new BehaviorSubject<string>("");
  tenNhhk$ = this.tenNhhkSource.asObservable();

  private idKhoaSource = new BehaviorSubject<string>("");
  idKhoa$ = this.idKhoaSource.asObservable();

  private idNhhkSource = new BehaviorSubject<string>("");
  idNhhk$ = this.idNhhkSource.asObservable();

  setTenKhoa(tenKhoa: string) {
    this.tenKhoaSource.next(tenKhoa);
  }
  getTenKhoa(): Observable<string> {
    return this.tenKhoa$;
  }
  setIdNhhk(idnhhk: string) {
    this.idNhhkSource.next(idnhhk);
  }
  setTenNhhk(tenNhhk: string) {
    this.tenNhhkSource.next(tenNhhk);
  }

  getTenNhhk(): Observable<string> {
    return this.tenNhhk$;
  }
  setidKhoa(idKhoa: string) {
    this.idKhoaSource.next(idKhoa);
  }
  // setTenKhoaAndIdNhhk(tenKhoa: string, idNhhk: string) {
  //   this.getBHNData(tenKhoa, idNhhk).subscribe((data) => {});
  // }

  getBHNData(tenKhoa: string): Observable<any> {
    const apiUrl = `/api/KDMBacHeNganh/BacHeNganhTheoKhoa`;
    const params = new HttpParams().set("TenKhoa", tenKhoa);
    return this.http.get<any>(apiUrl, { params });
  }
  getSVKData(tenNhhk: string, idKhoa: number): Observable<any> {
    const apiUrl = `/api/Sdmsvs/ByKhoa?TenNhhk=${tenNhhk}&IdKhoa=${idKhoa}`;
    return this.http.get<any>(apiUrl);
  }
}
