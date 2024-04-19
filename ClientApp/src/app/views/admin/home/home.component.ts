import { Component, OnInit, HostListener } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthenticalService } from "src/app/corelogic/model/authentical.service";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent implements OnInit {
  sidebarVisible: boolean = false;
  activeButton: string | null = null;
  userName!: string;
  toggleSidebar = false;
  toggleLogout = false;
  activeButton2: string | null = null;
  filter1 = false;
  filter2 = false;
  filter3 = false;
  isSidebarVisible = false;

  constructor(
    private router: Router,
    private api: AuthenticalService,
    private toast: ToastrService
  ) {}

  ngOnInit() {
    if (!sessionStorage.getItem("username")) {
      this.router.navigate(["/quantri"]);
    }
    this.activeButton = localStorage.getItem("activeButton");

    if (this.activeButton) {
      localStorage.removeItem("activeButton");
    }
    this.ButtonNHHK();
    this.logWindowSize();
  }
  logout() {
    this.api.logout().subscribe(() => {
      localStorage.removeItem("activeButton");
      this.router.navigate(["/quantri"]);
      this.toast.success("Đăng xuất thành công");
    });
  }

  togglelogout() {
    this.toggleLogout = !this.toggleLogout;
  }
  @HostListener("window:resize", ["$event"])
  onResize(event: any) {
    this.logWindowSize();
  }

  logWindowSize() {
    const sidebar = document.getElementById("sidebar");
    if (sidebar) {
      if (window.innerWidth <= 1300) {
        sidebar.style.transform = "translateX(-100%)";
        this.isSidebarVisible = false;
      } else {
        sidebar.style.transform = "translateX(0)";
        this.hideGrayOverlay();
      }
    }
  }

  showGrayOverlay() {
    const grayOverlay = document.getElementById("grayOverlay");
    if (grayOverlay !== null) {
      grayOverlay.style.display = "block";
    }
  }

  hideGrayOverlay() {
    const grayOverlay = document.getElementById("grayOverlay");
    if (grayOverlay !== null) {
      grayOverlay.style.display = "none";
    }
  }

  handleButtonClick() {
    const sidebar = document.getElementById("sidebar");
    if (sidebar) {
      if (this.isSidebarVisible) {
        sidebar.style.transform = "translateX(-100%)";
        this.hideGrayOverlay();
        this.toggleSidebar = true;
      } else {
        sidebar.style.transform = "translateX(0)";
        this.showGrayOverlay();
        this.toggleSidebar = false;
      }
      this.isSidebarVisible = !this.isSidebarVisible;
    }
  }

  ButtonNHHK() {
    this.activeButton = "Quản lý năm học";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlynamhoc"]);
  }
  ButtonBacHe() {
    this.activeButton = "Quản lý bậc hệ";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlybache"]);
  }
  ButtonNganh() {
    this.activeButton = "Quản lý ngành";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlyngang"]);
  }
  ButtonBHNganh() {
    this.activeButton = "Quản lý bậc hệ ngành";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlybachenganh"]);
  }
  ButtonKhoa() {
    this.activeButton = "Quản lý khoa";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlykhoa"]);
  }
  ButtonLop() {
    this.activeButton = "Quản lý lớp";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlylop"]);
  }
  ButtonGV() {
    this.activeButton = "Quản lý giảng viên";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlygiangvien"]);
  }
  ButtonSinhVien() {
    this.activeButton = "Quản lý sinh viên";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlysinhvien"]);
  }
  ButtonDiaDiem() {
    this.activeButton = "Quản lý địa điểm";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlydiadiem"]);
  }
  ButtonPhong() {
    this.activeButton = "Quản lý phòng";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlyphong"]);
  }
  ButtonHDNK() {
    this.activeButton = "Quản lý hoạt động ngoại khoá";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlyhdnk"]);
  }
  ButtonXL() {
    this.activeButton = "Quản lý xếp loại";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlyxeploai"]);
  }
  ButtonDieu() {
    this.activeButton = "Quản lý điều";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlydieu"]);
  }
  ButtonND() {
    this.activeButton = "Quản lý người dùng";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlynguoidung"]);
  }
  ButtonVBTL() {
    this.activeButton = "Quản lý văn bản";
    localStorage.setItem("activeButton", this.activeButton);
    this.router.navigate(["home/quanlykho"]);
  }

  showDropdown = false;
  showDropdown2 = true;
  showDropdown3 = true;

  toogleClick() {
    this.showDropdown = !this.showDropdown;
  }

  toogleClick2() {
    this.showDropdown2 = !this.showDropdown2;
  }

  toogleClick3() {
    this.showDropdown3 = !this.showDropdown3;
  }
  toggleButtons(idContent: any) {
    switch (idContent) {
      case "systemContent":
        this.showDropdown = !this.showDropdown;
        break;
      case "activityContent":
        this.showDropdown2 = !this.showDropdown2;
        break;
      case "documentContent":
        this.showDropdown3 = !this.showDropdown3;
        break;
      default:
        console.log("error");
        break;
    }
  }

  toggleDropDown() {
    this.filter1 = !this.filter1;
    this.activeButton2 = "FT Tuần";
    if (this.filter1 === false) {
      this.activeButton2 = "";
    }
    localStorage.setItem("activeButton", this.activeButton2);
  }
  toggleDropDown2() {
    this.filter2 = !this.filter2;
    this.activeButton2 = "FT Tên";
    if (this.filter2 === false) {
      this.activeButton2 = "";
    }
    localStorage.setItem("activeButton", this.activeButton2);
  }
  toggleDropDown3() {
    this.filter2 = !this.filter2;
    this.activeButton2 = "FT Thử";
    if (this.filter2 === false) {
      this.activeButton2 = "";
    }
    localStorage.setItem("activeButton", this.activeButton2);
  }
}
