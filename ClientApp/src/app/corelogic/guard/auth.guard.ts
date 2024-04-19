import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
} from "@angular/router";
import { AuthenticalService } from "../model/authentical.service";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(
    private service: AuthenticalService,
    private toast: ToastrService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (this.service.IsloggedIn()) {
      return true;
    } else {
      this.router.navigate(["/"]);
      this.toast.warning("Vui lòng đăng nhập để sử dụng hệ thống");
      return false;
    }
  }
}
