import { Component, OnInit } from "@angular/core";
import { Chart, registerables } from "node_modules/chart.js";
Chart.register(...registerables);

@Component({
  selector: "app-thongke",
  templateUrl: "./thongke.component.html",
  styleUrls: ["./thongke.component.css"],
})
export class ThongkeComponent {
  constructor() {}

  ngOnInit(): void {
    this.renderChart();
  }

  renderChart() {
    const myChart = new Chart("myChart", {
      type: "bar",
      data: {
        labels: ["Điều 1", "Điều 2", "Điều 3", "Điều 4", "Điều 5"],
        datasets: [
          {
            label: "Điểm của điều cần đạt được",
            data: [25, 35, 25, 10, 5],
            borderWidth: 1,
            borderColor: "#ffc000",
            backgroundColor: "#ffc000",
          },
          {
            label: "Điểm sinh viên đã đăng ký",
            data: [15, 20, 40, 10, 10],
            borderWidth: 1,
            borderColor: "#4472c4",
            backgroundColor: "#4472c4",
          },
        ],
      },
      options: {
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });
  }
}
