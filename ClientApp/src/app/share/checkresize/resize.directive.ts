import { Directive, HostListener } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthGuard } from "src/app/corelogic/guard/auth.guard";
import { AuthenticalService } from "src/app/corelogic/model/authentical.service";

@Directive({
  selector: "[appResize]",
})
export class ResizeDirective {
  constructor(
    private router: Router,
    private toast: ToastrService,
    private authService: AuthenticalService
  ) {}

  @HostListener("window:resize", ["$event"])
  onResize(event: Event): void {
    if (this.authService.isAdmin() || this.authService.isKhoa()) {
      this.checkWindowSize();
    }
  }

  ngOnInit(): void {
    if (this.authService.isAdmin() || this.authService.isKhoa()) {
      this.checkWindowSize();
    }
  }

  private checkWindowSize(): void {
    const windowWidth = window.innerWidth;

    if (windowWidth < 1250) {
      this.router.navigate(["error"]);
      this.toast.warning("Vui lòng sử dụng desktop để có trải nghiệm tốt nhất");
      this.router.navigate(["/"]);
    }
  }
}
