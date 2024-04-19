import { NgModule } from "@angular/core";
import { AuthGuard } from "./corelogic/guard/auth.guard";
import { RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./authentication/login/login.component";
import { HomeComponent } from "./views/admin/home/home.component";
import { QuanlynhhkComponent } from "./views/admin/quanlynhhk/quanlynhhk.component";
import { QuanlybacheComponent } from "./views/admin/quanlybache/quanlybache.component";
import { QuanlynganhComponent } from "./views/admin/quanlynganh/quanlynganh.component";
import { QuanlybhnganhComponent } from "./views/admin/quanlybhnganh/quanlybhnganh.component";
import { QuanlykhoaComponent } from "./views/admin/quanlykhoa/quanlykhoa.component";
import { QuanlysinhvienComponent } from "./views/admin/quanlysinhvien/quanlysinhvien.component";
import { QuanlylopComponent } from "./views/admin/quanlylop/quanlylop.component";
import { QuanlyhoatdongnkComponent } from "./views/admin/quanlyhoatdongnk/quanlyhoatdongnk.component";
import { QuanlydiadiemComponent } from "./views/admin/quanlydiadiem/quanlydiadiem.component";
import { QuanlygiangvienComponent } from "./views/admin/quanlygiangvien/quanlygiangvien.component";
import { QuanlyvbtlComponent } from "./views/admin/quanlyvbtl/quanlyvbtl.component";
import { QuanlyxeploaiComponent } from "./views/admin/quanlyxeploai/quanlyxeploai.component";
import { QuanlynguoidungComponent } from "./views/admin/quanlynguoidung/quanlynguoidung.component";
import { QuanlydieuComponent } from "./views/admin/quanlydieu/quanlydieu.component";
import { QuanlyphongComponent } from "./views/admin/quanlyphong/quanlyphong.component";
import { DeviceGuard } from "./corelogic/guard/device.guard";
import { ErrorComponent } from "./views/admin/error/error.component";
import { LoginKhoaComponent } from "./authentication/login-khoa/login-khoa.component";
import { HometkComponent } from "./views/khoa/thukykhoa/hometk/hometk.component";
import { HomeqlComponent } from "./views/khoa/quanlykhoa/homeql/homeql.component";
import { DanhsachhdnkComponent } from "./views/khoa/quanlykhoa/chucnang/danhsachhdnk/danhsachhdnk.component";
import { TaolichdkComponent } from "./views/khoa/quanlykhoa/chucnang/taolichdk/taolichdk.component";
import { DanhsachhdnkTkComponent } from "./views/khoa/thukykhoa/chucnang-thuky/danhsachhdnk-tk/danhsachhdnk-tk.component";
import { TaolichdkTkComponent } from "./views/khoa/thukykhoa/chucnang-thuky/taolichdk-tk/taolichdk-tk.component";
import { BachenganhkComponent } from "./views/khoa/quanlykhoa/tongquan/bachenganhk/bachenganhk.component";
import { SinhvienctdtComponent } from "./views/khoa/quanlykhoa/tongquan/sinhvienctdt/sinhvienctdt.component";
import { DanhsachvbQdComponent } from "./views/khoa/quanlykhoa/tongquan/danhsachvb-qd/danhsachvb-qd.component";
import { DiemdanhComponent } from "./views/khoa/thukykhoa/chucnang-thuky/diemdanh/diemdanh.component";
import { TaolichduyetComponent } from "./views/khoa/quanlykhoa/chucnang/taolichduyet/taolichduyet.component";
import { TaolichduyetTkComponent } from "./views/khoa/thukykhoa/chucnang-thuky/taolichduyet-tk/taolichduyet-tk.component";
import { HomeGiangvienComponent } from "./views/cvht-sv/cvht/home-giangvien/home-giangvien.component";
import { DuyethdnkGiangvienComponent } from "./views/cvht-sv/cvht/tinhnang/duyethdnk-giangvien/duyethdnk-giangvien.component";
import { DanhsachSinhvienComponent } from "./views/cvht-sv/cvht/tinhnang/danhsach-sinhvien/danhsach-sinhvien.component";
import { LichsuduyetComponent } from "./views/cvht-sv/cvht/tinhnang/lichsuduyet/lichsuduyet.component";
import { DanhsachlichsuComponent } from "./views/cvht-sv/cvht/tinhnang/danhsach-lichsu/danhsach-lichsu.component";
import { DangkyhdnkComponent } from "./views/cvht-sv/sinhvien/tinhnang/dangkyhdnk/dangkyhdnk.component";
import { KetquadkhdnkComponent } from "./views/cvht-sv/sinhvien/tinhnang/ketquadkhdnk/ketquadkhdnk.component";
import { XemlichhdnkComponent } from "./views/cvht-sv/sinhvien/tinhnang/xemlichhdnk/xemlichhdnk.component";
import { NopminhchungComponent } from "./views/cvht-sv/sinhvien/tinhnang/nopminhchung/nopminhchung.component";
import { ThongkeComponent } from "./views/cvht-sv/sinhvien/thongbao/thongke/thongke.component";
import { HomeComponentSV } from "./views/cvht-sv/sinhvien/home/home.component";
import { BachenganhTkComponent } from "./views/khoa/thukykhoa/tongquan-tk/bachenganh-tk/bachenganh-tk.component";
import { SinhvienctdtTkComponent } from "./views/khoa/thukykhoa/tongquan-tk/sinhvienctdt-tk/sinhvienctdt-tk.component";
import { VanbanTkComponent } from "./views/khoa/thukykhoa/tongquan-tk/vanban-tk/vanban-tk.component";

const routes: Routes = [
  { path: "quantri", component: LoginComponent, pathMatch: "full" },
  { path: "", component: LoginKhoaComponent, pathMatch: "full" },
  //role admin
  {
    path: "home",
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: "quanlynamhoc",
        component: QuanlynhhkComponent,
      },
      {
        path: "quanlybache",
        component: QuanlybacheComponent,
      },
      {
        path: "quanlyngang",
        component: QuanlynganhComponent,
      },
      {
        path: "quanlybachenganh",
        component: QuanlybhnganhComponent,
      },
      {
        path: "quanlykhoa",
        component: QuanlykhoaComponent,
      },
      {
        path: "quanlylop",
        component: QuanlylopComponent,
      },
      {
        path: "quanlygiangvien",
        component: QuanlygiangvienComponent,
      },
      {
        path: "quanlysinhvien",
        component: QuanlysinhvienComponent,
      },
      {
        path: "quanlydiadiem",
        component: QuanlydiadiemComponent,
      },
      {
        path: "quanlyphong",
        component: QuanlyphongComponent,
      },
      {
        path: "quanlyhdnk",
        component: QuanlyhoatdongnkComponent,
      },
      {
        path: "quanlyxeploai",
        component: QuanlyxeploaiComponent,
      },
      {
        path: "quanlydieu",
        component: QuanlydieuComponent,
      },
      {
        path: "quanlynguoidung",
        component: QuanlynguoidungComponent,
      },
      {
        path: "quanlykho",
        component: QuanlyvbtlComponent,
      },
    ],
  },
  //role qlkhoa
  {
    path: "dashboard/quanlykhoa",
    component: HomeqlComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: "bachenganh",
        component: BachenganhkComponent,
      },
      {
        path: "sinhvienctdt",
        component: SinhvienctdtComponent,
      },
      {
        path: "danhsachvbqd",
        component: DanhsachvbQdComponent,
      },
      {
        path: "danhsachhdnk",
        component: DanhsachhdnkComponent,
      },
      {
        path: "lichdk",
        component: TaolichdkComponent,
      },
      {
        path: "lichduyet",
        component: TaolichduyetComponent,
      },
    ],
  },
  //role tkkhoa
  {
    path: "dashboard/thukykhoa",
    component: HometkComponent,
    children: [
      {
        path: "bachenganh-tk",
        component: BachenganhTkComponent,
      },
      {
        path: "sinhvienctdt-tk",
        component: SinhvienctdtTkComponent,
      },
      {
        path: "vanban-tk",
        component: VanbanTkComponent,
      },
      {
        path: "tochuchoatdong",
        component: DiemdanhComponent,
      },
      {
        path: "danhsachhdnk-tk",
        component: DanhsachhdnkTkComponent,
      },
      {
        path: "taolich-tk",
        component: TaolichdkTkComponent,
      },
      {
        path: "lichduyet-tk",
        component: TaolichduyetTkComponent,
      },
    ],
  },
  //role giangvien
  {
    path: "dashboard/giangvien",
    component: HomeGiangvienComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: "duyethdnk",
        component: DuyethdnkGiangvienComponent,
      },
      {
        path: "danhsachsv",
        component: DanhsachSinhvienComponent,
      },
      {
        path: "lichsuduyet",
        component: LichsuduyetComponent,
      },
      {
        path: "danhsachlichsu",
        component: DanhsachlichsuComponent,
      },
    ],
  },
  //role sinhvien
  {
    path: "dashboard/sinhvien",
    component: HomeComponentSV,
    canActivate: [AuthGuard],
    children: [
      {
        path: "dangkyhdnk",
        component: DangkyhdnkComponent,
      },
      {
        path: "ketquadkhdnk",
        component: KetquadkhdnkComponent,
      },
      {
        path: "xemlichhdnk",
        component: XemlichhdnkComponent,
      },
      {
        path: "nopminhchung",
        component: NopminhchungComponent,
      },
      {
        path: "thongke",
        component: ThongkeComponent,
      },
    ],
  },
  { path: "", component: LoginComponent },
  { path: "error", component: ErrorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
