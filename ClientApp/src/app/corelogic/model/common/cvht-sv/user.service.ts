import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class UserService {
  apiURL = "http://localhost:3000";

  constructor(private http: HttpClient) {}

  private proxyUrl = "/api";
  private userInfo: any;

  userName!: string;
  public setUserName(name: string) {
    this.userName = name;
  }

  login(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.http.post(`/api/Account/login`, body);
  }

  setUserInfo(data: any[]): void {
    this.userInfo = data;
  }

  getYearSemester(): Observable<any> {
    return this.http.get(`api/KDMNHHK`);
  }

  getDataCurrentUser(): Observable<any> {
    return this.http.get(`/api/Sdmsvs/userinfo`);
  }

  getDataCurrentAdvisor(): Observable<any> {
    return this.http.get(`/api/NDMGiangVien/userinfo`);
  }

  logout(body?: any): Observable<any> {
    return this.http.post(`/api/KDMAdmin/logout`, body);
  }
}
