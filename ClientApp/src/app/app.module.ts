import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { AppRoutingModule } from "./app-routing.module";

import { AuthGuard } from "./corelogic/guard/auth.guard";
import { DeviceGuard } from "./corelogic/guard/device.guard";

import { AppComponent } from "./app.component";
import { LoginComponent } from "./authentication/login/login.component";
import { HomeComponent } from "./views/admin/home/home.component";
import { QuanlykhoaComponent } from "./views/admin/quanlykhoa/quanlykhoa.component";
import { QuanlysinhvienComponent } from "./views/admin/quanlysinhvien/quanlysinhvien.component";
import { QuanlylopComponent } from "./views/admin/quanlylop/quanlylop.component";
import { QuanlydiadiemComponent } from "./views/admin/quanlydiadiem/quanlydiadiem.component";
import { QuanlygiangvienComponent } from "./views/admin/quanlygiangvien/quanlygiangvien.component";
import { QuanlyvbtlComponent } from "./views/admin/quanlyvbtl/quanlyvbtl.component";
import { QuanlynhhkComponent } from "./views/admin/quanlynhhk/quanlynhhk.component";
import { QuanlybacheComponent } from "./views/admin/quanlybache/quanlybache.component";
import { QuanlynganhComponent } from "./views/admin/quanlynganh/quanlynganh.component";
import { QuanlybhnganhComponent } from "./views/admin/quanlybhnganh/quanlybhnganh.component";
import { QuanlyhoatdongnkComponent } from "./views/admin/quanlyhoatdongnk/quanlyhoatdongnk.component";
import { QuanlyxeploaiComponent } from "./views/admin/quanlyxeploai/quanlyxeploai.component";
import { QuanlynguoidungComponent } from "./views/admin/quanlynguoidung/quanlynguoidung.component";
import { QuanlyphongComponent } from "./views/admin/quanlyphong/quanlyphong.component";
import { QuanlydieuComponent } from "./views/admin/quanlydieu/quanlydieu.component";

import { LoginKhoaComponent } from "./authentication/login-khoa/login-khoa.component";
import { HomeqlComponent } from "./views/khoa/quanlykhoa/homeql/homeql.component";
import { HometkComponent } from "./views/khoa/thukykhoa/hometk/hometk.component";
import { DanhsachhdnkComponent } from "./views/khoa/quanlykhoa/chucnang/danhsachhdnk/danhsachhdnk.component";
import { TaolichdkComponent } from "./views/khoa/quanlykhoa/chucnang/taolichdk/taolichdk.component";
import { BachenganhkComponent } from "./views/khoa/quanlykhoa/tongquan/bachenganhk/bachenganhk.component";
import { DanhsachvbQdComponent } from "./views/khoa/quanlykhoa/tongquan/danhsachvb-qd/danhsachvb-qd.component";
import { SinhvienctdtComponent } from "./views/khoa/quanlykhoa/tongquan/sinhvienctdt/sinhvienctdt.component";
import { DanhsachhdnkTkComponent } from "./views/khoa/thukykhoa/chucnang-thuky/danhsachhdnk-tk/danhsachhdnk-tk.component";
import { TaolichdkTkComponent } from "./views/khoa/thukykhoa/chucnang-thuky/taolichdk-tk/taolichdk-tk.component";
import { DiemdanhComponent } from "./views/khoa/thukykhoa/chucnang-thuky/diemdanh/diemdanh.component";

import { NgxPaginationModule } from "ngx-pagination";
import { NgxExtendedPdfViewerModule } from "ngx-extended-pdf-viewer";
import { ToastrModule } from "ngx-toastr";
import { NgxSkeletonLoaderModule } from "ngx-skeleton-loader";
import { NgxScannerQrcodeModule } from "ngx-scanner-qrcode";
import { DxSchedulerModule } from "devextreme-angular";
import { QRCodeModule } from "angularx-qrcode";

import { SearchPipe } from "./share/filter/search.pipe";
import { FilterPipe } from "./share/filter/filter.pipe";
import { DateFormatPipe } from "./share/filter/filter-date.pipe";
import { FilterGenderPipe } from "./share/filter/filter-gender.pipe";
import { FilterBcansuPipe } from "./share/filter/filter-bcansu.pipe";
import { UniqueFilterPipe } from "./share/filter/uniquefilterpipe.pipe";
import { ErrorComponent } from "./views/admin/error/error.component";
import { ResizeDirective } from "./share/checkresize/resize.directive";
import { StatusCheckPipe } from "./share/filter/filter-checktt.pipe";
import { TaolichduyetComponent } from "./views/khoa/quanlykhoa/chucnang/taolichduyet/taolichduyet.component";
import { TaolichduyetTkComponent } from "./views/khoa/thukykhoa/chucnang-thuky/taolichduyet-tk/taolichduyet-tk.component";
import { PaginationComponent } from "./share/component/pagination/pagination.component";
import { HomeGiangvienComponent } from "./views/cvht-sv/cvht/home-giangvien/home-giangvien.component";
import { DanhsachSinhvienComponent } from "./views/cvht-sv/cvht/tinhnang/danhsach-sinhvien/danhsach-sinhvien.component";
import { DanhsachlichsuComponent } from "./views/cvht-sv/cvht/tinhnang/danhsach-lichsu/danhsach-lichsu.component";
import { DuyethdnkGiangvienComponent } from "./views/cvht-sv/cvht/tinhnang/duyethdnk-giangvien/duyethdnk-giangvien.component";
import { LichsuduyetComponent } from "./views/cvht-sv/cvht/tinhnang/lichsuduyet/lichsuduyet.component";
import { ThongkeComponent } from "./views/cvht-sv/sinhvien/thongbao/thongke/thongke.component";
import { DangkyhdnkComponent } from "./views/cvht-sv/sinhvien/tinhnang/dangkyhdnk/dangkyhdnk.component";
import { KetquadkhdnkComponent } from "./views/cvht-sv/sinhvien/tinhnang/ketquadkhdnk/ketquadkhdnk.component";
import { NopminhchungComponent } from "./views/cvht-sv/sinhvien/tinhnang/nopminhchung/nopminhchung.component";
import { XemlichhdnkComponent } from "./views/cvht-sv/sinhvien/tinhnang/xemlichhdnk/xemlichhdnk.component";
import { HomeComponentSV } from "./views/cvht-sv/sinhvien/home/home.component";
import { BachenganhTkComponent } from "./views/khoa/thukykhoa/tongquan-tk/bachenganh-tk/bachenganh-tk.component";
import { SinhvienctdtTkComponent } from "./views/khoa/thukykhoa/tongquan-tk/sinhvienctdt-tk/sinhvienctdt-tk.component";
import { VanbanTkComponent } from "./views/khoa/thukykhoa/tongquan-tk/vanban-tk/vanban-tk.component";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    QuanlykhoaComponent,
    QuanlysinhvienComponent,
    QuanlylopComponent,
    QuanlydiadiemComponent,
    QuanlygiangvienComponent,
    QuanlyvbtlComponent,
    QuanlynhhkComponent,
    QuanlybacheComponent,
    QuanlynganhComponent,
    QuanlybhnganhComponent,
    QuanlyhoatdongnkComponent,
    QuanlyxeploaiComponent,
    QuanlynguoidungComponent,
    SearchPipe,
    FilterPipe,
    UniqueFilterPipe,
    FilterGenderPipe,
    FilterBcansuPipe,
    DateFormatPipe,
    QuanlyphongComponent,
    QuanlydieuComponent,
    ErrorComponent,
    ResizeDirective,
    LoginKhoaComponent,
    HomeqlComponent,
    HometkComponent,
    DanhsachhdnkComponent,
    TaolichdkComponent,
    BachenganhkComponent,
    DanhsachvbQdComponent,
    SinhvienctdtComponent,
    DanhsachhdnkTkComponent,
    TaolichdkTkComponent,
    DiemdanhComponent,
    StatusCheckPipe,
    TaolichduyetComponent,
    TaolichduyetTkComponent,
    PaginationComponent,
    HomeGiangvienComponent,
    DanhsachSinhvienComponent,
    DanhsachlichsuComponent,
    DuyethdnkGiangvienComponent,
    LichsuduyetComponent,
    ThongkeComponent,
    DangkyhdnkComponent,
    KetquadkhdnkComponent,
    NopminhchungComponent,
    XemlichhdnkComponent,
    HomeComponentSV,
    BachenganhTkComponent,
    SinhvienctdtTkComponent,
    VanbanTkComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgxPaginationModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    NgxExtendedPdfViewerModule,
    ToastrModule.forRoot({ timeOut: 7000, progressBar: true }),
    NgxSkeletonLoaderModule,
    NgxScannerQrcodeModule,
    DxSchedulerModule,
    QRCodeModule,
  ],
  providers: [AuthGuard, DeviceGuard],

  bootstrap: [AppComponent],
})
export class AppModule {}
