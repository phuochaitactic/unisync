import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class AuthenticalService {
  isLoggedIn: boolean = false;
  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.http.post(`/api/Account/quantri`, body);
  }

  logout(): Observable<any> {
    return this.http.post(`/api/KDMAdmin/logout`, {}).pipe(
      tap(() => {
        sessionStorage.removeItem("username");
      })
    );
  }

  loginKhoa(username: string, password: string): Observable<any> {
    const body = { username, password };
    return this.http.post(`/api/Account/login`, body);
  }

  logoutKhoa(): Observable<any> {
    return this.http.post(`/api/Account/logout`, {}).pipe(
      tap(() => {
        sessionStorage.removeItem("username");
      })
    );
  }

  IsloggedIn(): boolean {
    return sessionStorage.getItem("username") !== null;
  }

  isAdmin(): boolean {
    const userData = sessionStorage.getItem("admin");
    if (userData) {
      try {
        const user = JSON.parse(userData);
        return !!user.adminPassword;
      } catch (error) {
        console.error("Error parsing user data:", error);
        return false;
      }
    }
    return false;
  }

  isKhoa(): boolean {
    const qlKhoaData = sessionStorage.getItem("qlKhoa");
    const tkKhoaData = sessionStorage.getItem("tkKhoa");
    if (qlKhoaData || tkKhoaData) {
      return true;
    }
    return false;
  }
}
