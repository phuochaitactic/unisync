import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class ApiService {
  apiURL = "http://localhost:3000";
  api = "http://aqtech.vn:5564/CongRenLuyen/api/KDMAdmin";

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    const apiUrl = `${this.apiURL}/login`;
    const params = { username, password };
    return this.http.get(apiUrl, { params });
  }
}
