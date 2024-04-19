import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable, BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class BachenganhkService {
  constructor(private http: HttpClient) {}

  private tenKhoaSource = new BehaviorSubject<string>("");
  tenKhoa$ = this.tenKhoaSource.asObservable();

  private idNhhkSource = new BehaviorSubject<string>("");
  idNhhk$ = this.idNhhkSource.asObservable();

  setTenKhoa(tenKhoa: string) {
    this.tenKhoaSource.next(tenKhoa);
  }

  setIdNhhk(idnhhk: string) {
    this.idNhhkSource.next(idnhhk);
  }
}
