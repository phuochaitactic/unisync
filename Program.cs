using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllersWithViews();


builder.Services.AddCors(options =>
{

    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin();
                      });
});

// Register Service
builder.Services.AddSingleton<IAuthService, Authentication>();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<IBacHeService, BacHeService>();
builder.Services.AddSingleton<IBacHeNganhService, BacHeNganhService>();
builder.Services.AddSingleton<IHoatDongNgoaiKhoaService, HoatDongNgoaiKhoaService>();
builder.Services.AddSingleton<ISinhVienService, SinhVienService>();
builder.Services.AddSingleton<ILichDangKyService, LichDangKyService>();
builder.Services.AddSingleton<IKhoaService, KhoaService>();
builder.Services.AddSingleton<INganhService, NganhService>();
builder.Services.AddSingleton<ITtHdnkService, TtHdnkService>();
builder.Services.AddSingleton<IVanBanService, VanBanService>();
builder.Services.AddSingleton<IXepLoaiService, XepLoaiService>();
builder.Services.AddSingleton<INhhkService, NhhkService>();
builder.Services.AddSingleton<IDiaDiemService, DiaDiemService>();
builder.Services.AddSingleton<IGiangVienService, GiangVienService>();
builder.Services.AddSingleton<IKkqSvDkHdnkService, KkqSvDkHdnkService>();
builder.Services.AddSingleton<IDieuService, DieuService>();
builder.Services.AddSingleton<ILopService, LopService>();
builder.Services.AddSingleton<ILoaiHdnkService, LoaiHdnkService>();
builder.Services.AddSingleton<IDuLieuHdnkService, DuLieuHdnkService>();
builder.Services.AddSingleton<IMinhChungService, MinhChungService>();
builder.Services.AddSingleton<IPhongService, PhongService>();
builder.Services.AddSingleton<IViewSlsinhVienThamGiaHdtheoHKService, ViewSlsinhVienThamGiaHdtheoHKService>();
builder.Services.AddSingleton<IViewSvchuaThamGiaHdService, ViewSvchuaThamGiaHdService>();
builder.Services.AddSingleton<ITaoHdnkService, TaoHdnkService>();
builder.Services.AddSingleton<IBangDiemService, BangDiemService>();
builder.Services.AddSingleton<IQrService, QrService>();
builder.Services.AddSingleton<ILichDuyetSVService, LichDuyetSVService>();
builder.Services.AddSingleton<IKdmdssvdkService, KdmdssvdkService>();
builder.Services.AddSingleton<ILichTaoHdnkService, LichTaoHdnkService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "YourCookieName"; // Match this with your authentication middleware
    // Other cookie configuration...
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(15);
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseAuthentication(); // Make sure this comes before UseAuthorization
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
