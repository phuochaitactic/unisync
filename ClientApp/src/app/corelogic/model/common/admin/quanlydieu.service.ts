import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
@Injectable({
  providedIn: "root",
})
export class QuanlydieuService {
  apiURL = "http://localhost:3000";
  fileExtension = ".xlsx";

  constructor(private http: HttpClient) {}

  getDieuData(): Observable<any> {
    const apiUrl = `/api/KDMDieu`;
    return this.http.get<any>(apiUrl);
  }

  postDataToDatabase(excelData: any): Observable<any> {
    return this.http.post(`/api/KDMDieu`, excelData);
  }

  // exportExcel({
  //   data,
  //   fileName,
  //   sheetName = "Data",
  //   header = [],
  //   table,
  // }: {
  //   data: any[];
  //   fileName: string;
  //   sheetName?: string;
  //   header?: any[];
  //   table?: any;
  // }): void {
  //   const dataWithoutId = data.map((item) => {
  //     const { iddieu, ...rest } = item;
  //     return rest;
  //   });

  //   let wb: WorkBook;
  //   if (table) {
  //     wb = XLSXUtils.table_to_book(table);
  //   } else {
  //     const ws: WorkSheet = XLSXUtils.json_to_sheet(dataWithoutId, { header });
  //     wb = XLSXUtils.book_new();
  //     XLSXUtils.book_append_sheet(wb, ws, sheetName);
  //   }
  //   writeFile(wb, `${fileName}${this.fileExtension}`);
  // }

  postThemDieu(addSV: any): Observable<any> {
    return this.http.post(`/api/KDMDieu`, addSV);
  }

  putEditDieu(editingSV: any): Observable<any> {
    return this.http.put(`/api/KDMDieu/${editingSV.iddieu}`, editingSV);
  }

  deleteDieu(iddieu: number): Observable<any> {
    const url = `/api/KDMDieu/${iddieu}`;
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "text/plain" }),
      responseType: "text" as "json",
    };
    return this.http.delete(url, httpOptions);
  }
}
