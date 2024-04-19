import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthenticalService } from "src/app/corelogic/model/authentical.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent {
  username!: string;
  password!: string;
  error!: string;
  loading: boolean = false;
  showpassword: boolean = false;

  constructor(
    private router: Router,
    private api: AuthenticalService,
    private toast: ToastrService
  ) {
    sessionStorage.clear();
  }

  toogleClick() {
    this.showpassword = !this.showpassword;
  }
  login() {
    if (!this.username || !this.password) {
      this.error = "Không để trống thông tin";
    } else {
      this.loading = true;
      this.api.login(this.username, this.password).subscribe(
        (response) => {
          if (response.code === 200) {
            this.toast.success("Đăng nhập thành công");
            sessionStorage.setItem("username", response.data[0].username);
            sessionStorage.setItem("admin", JSON.stringify(response.data[0]));
            this.loading = false;
            this.router.navigate(["home"]);
          } else {
            this.toast.error(
              "Vui lòng kiểm tra lại thông tin tài khoản",
              "Đăng nhập không thành công"
            );
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
