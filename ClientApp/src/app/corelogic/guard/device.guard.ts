import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { DeviceDetectorService } from "ngx-device-detector";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class DeviceGuard implements CanActivate {
  constructor(
    private deviceService: DeviceDetectorService,
    private router: Router,
    private toast: ToastrService
  ) {}

  canActivate(): boolean {
    const isMobile = this.deviceService.isMobile();

    if (isMobile) {
      this.toast.warning(
        "Vui lòng truy cập bằng máy tính để có trải nghiệm tốt nhất"
      );
      this.router.navigate(["/error"]);
      return false;
    }

    return true;
  }
}
