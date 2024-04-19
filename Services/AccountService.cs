using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IAccountService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByUsername(string username);

        Task<IActionResult> LoginAdmin(HttpContext HttpContext, [FromBody] LoginModel model);

        Task<IActionResult> Login(HttpContext HttpContext, [FromBody] LoginModel model);

        IActionResult Logout(HttpContext HttpContextl);
    }

    public class AccountService : BaseController, IAccountService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var admin = (from ad in context.Kdmadmins

                                 select new DsAccountModel
                                 {
                                     Id = ad.Idadmin,
                                     Username = ad.Username,
                                     Password = ad.AdminPassword,
                                     Role = "admin"
                                 }).ToList();
                    var sinhVien = (from sv in context.Sdmsvs
                                    select new DsAccountModel
                                    {
                                        Id = sv.IdsinhVien,
                                        Username = sv.MaSinhVien,
                                        Password = sv.MatKhau,
                                        Role = "sinh vien",
                                        name = sv.HoTenSinhVien
                                    }).ToList();
                    var giangVien = (from gv in context.NdmgiangViens
                                     select new DsAccountModel
                                     {
                                         Id = gv.IdgiangVien,
                                         Username = gv.MaNv,
                                         Password = gv.MatKhau,
                                         Role = gv.VaiTro == "Giảng Viên" ? "Giang Vien" : "Thu Ky Khoa",
                                         name = gv.HoTen
                                     }).ToList();
                    var khoa = (from k in context.Ndmkhoas
                                select new DsAccountModel
                                {
                                    Id = k.Idkhoa,
                                    Username = k.MaKhoa,
                                    Password = k.MatKhau,
                                    Role = "khoa",
                                    name = k.TenKhoa
                                }).ToList();
                    var accounts = admin
                                 .Concat(sinhVien)
                                 .Concat(giangVien)
                                 .Concat(khoa)
                                 .ToList();
                    DataObject = accounts.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Admin not found";
                return CreateResponse();
            }
            catch (ValidationException ex)
            {
                // Invalid input data
                Code = 400;
                Message = ex.Message;
                return CreateResponse();
            }
            catch (UnauthorizedAccessException ex)
            {
                // User does not have access
                Code = 401;
                Message = "Unauthorized";
                return CreateResponse();
            }
            catch (Exception ex)
            {
                // Unhandled error
                Code = 500;
                Message = ex.Message;
                return CreateResponse();
            }
        }

        public async Task<IActionResult> GetByUsername(string username)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var admin = (from ad in context.Kdmadmins

                                 select new DsAccountModel
                                 {
                                     Id = ad.Idadmin,
                                     Username = ad.Username,
                                     Password = ad.AdminPassword
                                 }).ToList();
                    var sinhVien = (from sv in context.Sdmsvs
                                    select new DsAccountModel
                                    {
                                        Id = sv.IdsinhVien,
                                        Username = sv.MaSinhVien,
                                        Password = sv.MatKhau
                                    }).ToList();
                    var giangVien = (from gv in context.NdmgiangViens
                                     select new DsAccountModel
                                     {
                                         Id = gv.IdgiangVien,
                                         Username = gv.MaNv,
                                         Password = gv.MatKhau
                                     }).ToList();
                    var khoa = (from k in context.Ndmkhoas
                                select new DsAccountModel
                                {
                                    Id = k.Idkhoa,
                                    Username = k.MaKhoa,
                                    Password = k.MatKhau,
                                    Role = "giang vien",
                                    name = k.TenKhoa
                                }).ToList();
                    if (!string.IsNullOrEmpty(username))
                    {
                        admin = admin.Where(ad => ad.Username.Contains(username)).ToList();

                        sinhVien = sinhVien.Where(sv => sv.Username.Contains(username)).ToList();

                        giangVien = giangVien.Where(gv => gv.Username.Contains(username)).ToList();
                        khoa = khoa.Where(k => k.Username.Contains(username)).ToList();
                    }
                    var accounts = admin
                                 .Concat(sinhVien)
                                 .Concat(giangVien)
                                 .Concat(khoa)
                                 .ToList();
                    DataObject = accounts.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Admin not found";
                return CreateResponse();
            }
            catch (ValidationException ex)
            {
                // Invalid input data
                Code = 400;
                Message = ex.Message;
                return CreateResponse();
            }
            catch (UnauthorizedAccessException ex)
            {
                // User does not have access
                Code = 401;
                Message = "Unauthorized";
                return CreateResponse();
            }
            catch (Exception ex)
            {
                // Unhandled error
                Code = 500;
                Message = ex.Message;
                return CreateResponse();
            }
        }

        public async Task<IActionResult> LoginAdmin(HttpContext HttpContext, [FromBody] LoginModel model)
        {
            try
            {
                // login code
                using (var context = new MyDBContext())
                {
                    var user = await context.Kdmadmins.FirstOrDefaultAsync(u => u.Username == model.Username);
                    if (user == null)
                        return BadRequest("Invalid username");
                    if (!PasswordGenerator.VerifyPassword(model.Password, user.AdminPassword))
                        return BadRequest("Invalid password");
                    // Store user information in session

                    HttpContext.Session.SetString("UserId", user.Idadmin.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", "admin");

                    // Return OK if credentials are valid
                    var dataObject = new List<object>();
                    DataObject.Clear();
                    DataObject.Add(user);
                    Message = "Logged in as Admin";
                    Code = 200;
                    return CreateResponse();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Login(HttpContext HttpContext, [FromBody] LoginModel model)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    AccountModel user = await (from sv in context.Sdmsvs
                                               where model.Username == sv.MaSinhVien
                                               select new AccountModel
                                               {
                                                   Id = sv.IdsinhVien,
                                                   Username = sv.MaSinhVien,
                                                   Password = sv.MatKhau,
                                                   Role = "Sinh Vien"
                                               }).FirstOrDefaultAsync()
                    ??
                    await (from k in context.Ndmkhoas
                           where model.Username == k.MaKhoa
                           select new AccountModel
                           {
                               Id = k.Idkhoa,
                               Username = k.MaKhoa,
                               Password = k.MatKhau,
                               Role = "Khoa"
                           }).FirstOrDefaultAsync()
                    ??
                    await (from gv in context.NdmgiangViens
                           where model.Username == gv.MaNv
                           select new AccountModel
                           {
                               Id = gv.IdgiangVien,
                               Username = gv.MaNv,
                               Password = gv.MatKhau,
                               Role = gv.VaiTro == "Giảng Viên" ? "Giang Vien" : "Thu Ky Khoa"
                           }).FirstOrDefaultAsync() ??
                    new AccountModel
                    {
                        Username = null,
                        Password = null,
                        Role = null
                    };
                    if (user.Username == null)
                        return BadRequest("Invalid username");
                    if (!PasswordGenerator.VerifyPassword(model.Password, user.Password))
                        return BadRequest("Invalid password");

                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    if (user.Role.ToLower() == "khoa")
                    {
                        SetKhoaSession(HttpContext, user);
                    }
                    else if (user.Role.ToLower() == "sinh vien")
                    {
                        Console.WriteLine(HttpContext.Features.Get<ISessionFeature>()?.Session != null);
                        if (HttpContext.Session.GetString("isLoggedIn") == "true")
                        {
                            HttpContext.Session.Clear();
                        }
                        SetSinhVienSession(HttpContext, user);
                    }
                    else if (user.Role.ToLower() == "giảng viên" || user.Role.ToLower() == "giang vien" || user.Role.ToLower() == "thu ky khoa")
                    {
                        SetGiangVienVaThuKySession(HttpContext, user);
                    }
                    var dataObject = new List<object>();
                    DataObject.Clear();
                    DataObject.Add(user);
                    Message = "Logged in as " + user.Username;
                    Code = 200;
                    return CreateResponse();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Logout(HttpContext HttpContext)
        {
            try
            {
                HttpContext.Session.Clear();

                return Ok("Logged out");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void SetKhoaSession(HttpContext HttpContext, AccountModel user)
        {
            using (var context = new MyDBContext())
            {
                var khoaData = context.Ndmkhoas.Where(k => k.Idkhoa == user.Id)
                        .Select(k => k)
                        .FirstOrDefault();

                HttpContext.Session.SetString("maKhoa", khoaData.MaKhoa);
                HttpContext.Session.SetString("tenKhoa", khoaData.TenKhoa);
            }
        }

        private void SetGiangVienVaThuKySession(HttpContext HttpContext, AccountModel user)
        {
            using (var context = new MyDBContext())
            {
                var giangVienData = context.NdmgiangViens
                             .Where(gv => gv.IdgiangVien == user.Id)
                          .Select(gv => gv)
                          .FirstOrDefault();
                var tenKhoa = (from gv in context.NdmgiangViens
                               join kh in context.Ndmkhoas on gv.Idkhoa equals kh.Idkhoa
                               where gv.IdgiangVien == user.Id
                               select kh.TenKhoa).FirstOrDefault();
                HttpContext.Session.SetString("hoTen", giangVienData.HoTen);
                HttpContext.Session.SetString("phai", giangVienData.Phai.ToString());
                HttpContext.Session.SetString("ngaySinh", giangVienData.NgaySinh.ToString());
                HttpContext.Session.SetString("tenKhoa", tenKhoa);
                HttpContext.Session.SetString("vaiTro", giangVienData.VaiTro);
                HttpContext.Session.SetString("thongTinLienHe", giangVienData.ThongTinLienHe);
            }
        }

        private void SetSinhVienSession(HttpContext HttpContext, AccountModel user)
        {

            using (var context = new MyDBContext())
            {
                var sinhVienData = context.Sdmsvs.Where(k => k.IdsinhVien == user.Id)
                          .Select(k => k)
                          .FirstOrDefault();
                var tenGiangVien = context.Sdmsvs
                .Where(sv => sv.IdsinhVien == user.Id)
                .Join(context.NdmgiangViens,
                      sv => sv.IdgiangVien,
                      gv => gv.IdgiangVien,
                      (sv, gv) => new { TenGv = gv.HoTen })
                .Select(x => x.TenGv)
                .FirstOrDefault();
                var tenLop = context.Sdmsvs
                .Where(sv => sv.IdsinhVien == user.Id)
                .Join(context.Sdmlops,
                      sv => sv.Idlop,
                      lop => lop.Idlop,
                      (sv, lop) => new { tenLop = lop.TenLop })
                .Select(x => x.tenLop)
                .FirstOrDefault();
                var tenNhhk = context.Sdmsvs
               .Where(sv => sv.IdsinhVien == user.Id)
               .Join(context.Kdmnhhks,
                     sv => sv.Idnhhk,
                     lop => lop.Idnhhk,
                     (sv, lop) => new { tenNhhk = lop.TenNhhk })
               .Select(x => x.tenNhhk)
               .FirstOrDefault();
                var DiaChiLienHe = (from sv in context.Sdmsvs
                                    select sv.DiaChiLienHe).FirstOrDefault();
                var tenNganh = (from sv in context.Sdmsvs
                                join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                join bhn in context.KdmbhngChngs on l.IdbhngChng equals bhn.IdbhngChng
                                join n in context.Kdmnghs on bhn.Idngh equals n.Idngh
                                where sv.IdsinhVien == user.Id
                                select n.TenNgh).FirstOrDefault();
                var tenKhoa = (from sv in context.Sdmsvs
                               join l in context.Sdmlops on sv.Idlop equals l.Idlop
                               join kh in context.Ndmkhoas on l.Idkhoa equals kh.Idkhoa
                               where sv.IdsinhVien == user.Id
                               select kh.TenKhoa).FirstOrDefault();
                var maKhoa = (from sv in context.Sdmsvs
                              join l in context.Sdmlops on sv.Idlop equals l.Idlop
                              join kh in context.Ndmkhoas on l.Idkhoa equals kh.Idkhoa
                              where sv.IdsinhVien == user.Id
                              select kh.MaKhoa).FirstOrDefault();
                var tenBhNgh = (from sv in context.Sdmsvs
                                join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                join bhn in context.KdmbhngChngs on l.IdbhngChng equals bhn.IdbhngChng
                                where sv.IdsinhVien == user.Id
                                select bhn.TenBhngChng).FirstOrDefault();
                var maLop = (from sv in context.Sdmsvs
                             join l in context.Sdmlops on sv.Idlop equals l.Idlop
                             where sv.IdsinhVien == user.Id
                             select l.MaLop).FirstOrDefault();

                HttpContext.Session.SetString("hoTenSinhVien", sinhVienData.HoTenSinhVien);
                HttpContext.Session.SetString("phai", sinhVienData.Phai.ToString());
                HttpContext.Session.SetString("ngaySinh", sinhVienData.NgaySinh.ToString());
                HttpContext.Session.SetString("trangThaiSinhVien", sinhVienData.TrangThaiSinhVien);
                HttpContext.Session.SetString("isBanCanSu", sinhVienData.IsBanCanSu.ToString());
                HttpContext.Session.SetString("tenGv", tenGiangVien);
                HttpContext.Session.SetString("tenLop", tenLop);
                HttpContext.Session.SetString("tenNhhk", tenNhhk);
                HttpContext.Session.SetString("tenNganh", tenNganh);
                HttpContext.Session.SetString("tenKhoa", tenKhoa);
                HttpContext.Session.SetString("tenBhNgh", tenBhNgh);
                HttpContext.Session.SetString("maLop", maLop);
                HttpContext.Session.SetString("maKhoa", maKhoa);
                HttpContext.Session.SetString("sLoiggedIn", "true");
                if (DiaChiLienHe != null)
                {
                    HttpContext.Session.SetString("diaChiLienHe", DiaChiLienHe);
                }
            }
        }

    }
}