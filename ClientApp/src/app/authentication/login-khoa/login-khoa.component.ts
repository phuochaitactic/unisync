import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthenticalService } from "src/app/corelogic/model/authentical.service";
import { ApifullService } from "src/app/corelogic/model/common/khoa/apifull.service";
import { KhoaService } from "src/app/corelogic/model/common/khoa/khoa.service";

@Component({
  selector: "app-login-khoa",
  templateUrl: "./login-khoa.component.html",
  styleUrls: ["./login-khoa.component.css"],
})
export class LoginKhoaComponent {
  username!: string;
  password!: string;
  error!: string;
  loading: boolean = false;
  showpassword: boolean = false;

  constructor(
    private router: Router,
    private api: AuthenticalService,
    private toast: ToastrService,
    private tenKhoa: KhoaService
  ) {
    sessionStorage.clear();
  }

  toogleClick() {
    this.showpassword = !this.showpassword;
  }

  login() {
    if (!this.username || !this.password) {
      this.toast.warning("Không để trống thông tin");
    } else {
      this.loading = true;
      this.api.loginKhoa(this.username, this.password).subscribe(
        (response) => {
          if (response.code === 200) {
            const role = response.data[0].role;
            this.loading = false;
            this.toast.success("Đăng nhập thành công");
            if (role === "Khoa") {
              sessionStorage.setItem("username", response.data[0].username);
              this.router.navigate(["dashboard/quanlykhoa"]);
              sessionStorage.setItem("qlKhoa", role);
            } else if (role === "Thu Ky Khoa") {
              sessionStorage.setItem("username", response.data[0].username);
              this.router.navigate(["dashboard/thukykhoa"]);
              sessionStorage.setItem("tkKhoa", role);
            } else if (role === "Giang Vien") {
              sessionStorage.setItem("username", response.data[0].username);
              this.router.navigate(["dashboard/giangvien"]);
            } else if (role === "Sinh Vien") {
              sessionStorage.setItem("username", response.data[0].username);
              this.router.navigate(["dashboard/sinhvien"]);
            }
          } else {
            this.loading = false;
          }
        },
        (error) => {
          this.loading = false;
          this.toast.error(
            "Vui lòng kiểm tra lại thông tin tài khoản",
            "Đăng nhập không thành công"
          );
        }
      );
    }
  }
}
