import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class NamhocService {
  private selectedYear: string = "";

  private selectedYearSubject: BehaviorSubject<string> =
    new BehaviorSubject<string>(this.selectedYear);

  selectedYear$ = this.selectedYearSubject.asObservable();

  getSelectedYear(): string {
    return this.selectedYear;
  }

  setSelectedYear(year: string): void {
    this.selectedYear = year;
    this.selectedYearSubject.next(year);
  }
}
