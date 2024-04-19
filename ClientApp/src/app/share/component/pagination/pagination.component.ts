import { Component } from "@angular/core";

@Component({
  selector: "app-pagination",
  templateUrl: "./pagination.component.html",
  styleUrls: ["./pagination.component.css"],
})
export class PaginationComponent {
  totalPage: number = 10;
  pages: number[] = [];

  constructor() {
    for (let i = 1; i <= this.totalPage; i++) {
      this.pages.push(i);
    }
  }

  selectPage(index: number) {
    console.log(index);
  }
}
