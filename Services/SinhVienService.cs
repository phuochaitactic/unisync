using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ISinhVienService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByGiangVien(string TenNhhk, long IdGiangVien);

        Task<IActionResult> GetByKhoa(string TenNhhk, long IdKhoa);

        Task<IActionResult> GetByHdnk(string TenNhhk, long IdHdnk);

        Task<IActionResult> GetSdmsvByTen(string HoTenSinhVien);

        Task<IActionResult> GetSdmsvByMa(string MaSV);

        Task<IActionResult> GetUserInfo(HttpContext HttpContext);

        /// <summary>
        /// thuoc khoa
        /// </summary>
        /// <param name="idNhhk"></param>
        /// <param name="idKhoa"></param>
        /// <returns></returns>

        Task<IActionResult> GetSdmsvTheoLop(string maLop);

        Task<IActionResult> GetSdmsvTheoBHNG(string TenBacHeNganh);

        Task<IActionResult> CreateSinhVien([FromBody] SinhVienTableModel InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] SinhVienTableModel InputData);

        Task<IActionResult> Delete(long id);
    }

    public class SinhVienService : BaseController, ISinhVienService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<SinhVienTableModel> resultList = new List<SinhVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.Sdmsvs
                                 join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                 join nh in context.Kdmnhhks on sv.Idnhhk equals nh.Idnhhk
                                 join lop in context.Sdmlops on sv.Idlop equals lop.Idlop
                                 select new SinhVienTableModel
                                 {
                                     IdsinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     MatKhau = sv.MatKhau,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     TenGv = gv.HoTen,
                                     TenLop = lop.TenLop,
                                     TenNhhk = nh.TenNhhk,
                                     DiaChiLienHe = sv.DiaChiLienHe
                                 }).ToList();
                    resultList = (List<SinhVienTableModel>)query.Select(item => new SinhVienTableModel
                    {
                        IdsinhVien = item.IdsinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        MatKhau = item.MatKhau,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        IsBanCanSu = item.IsBanCanSu,
                        TenGv = item.TenGv,
                        TenLop = item.TenLop,
                        TenNhhk = item.TenNhhk,
                        DiaChiLienHe = item.DiaChiLienHe
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetByGiangVien(string TenNhhk, long IdGiangVien)
        {
            try
            {
                List<SinhVienTheoGiangVien> resultList = new List<SinhVienTheoGiangVien>();
                using (var context = new MyDBContext())
                {
                    long? MaNhhk = context.Kdmnhhks
                         .Where(NHHK => NHHK.TenNhhk == TenNhhk)
                         .Select(NHHK => NHHK.MaNhhk)
                         .FirstOrDefault();
                    int NHHKYear;
                    if (MaNhhk.HasValue)
                    {
                        string maNhhkStr = MaNhhk.Value.ToString();
                        NHHKYear = int.Parse(maNhhkStr.Substring(0, 4));
                    }
                    else { NHHKYear = 0; }

                    var query = (from sv in context.Sdmsvs
                                 join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                 join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                 where gv.IdgiangVien == IdGiangVien
                                 select new SinhVienTheoGiangVien
                                 {
                                     IdSinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     MatKhau = sv.MatKhau,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     IdGiangVien = sv.IdgiangVien,
                                     TenGv = gv.HoTen,
                                     TenLop = l.TenLop,
                                     IdLop = sv.Idlop,
                                     NienKhoa = l.NienKhoa,
                                     DiaChiLienHe = sv.DiaChiLienHe,
                                     AcademicYear = SinhVienService.CalculateAcademicYear(NHHKYear, l.NienKhoa)
                                 }).OrderBy(x => x.MaSinhVien).ToList();
                    resultList = (List<SinhVienTheoGiangVien>)query.Select(item => new SinhVienTheoGiangVien
                    {
                        IdSinhVien = item.IdSinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        MatKhau = item.MatKhau,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        IsBanCanSu = item.IsBanCanSu,
                        IdGiangVien = item.IdGiangVien,
                        TenLop = item.TenLop,
                        TenGv = item.TenGv,
                        IdLop = item.IdLop,
                        DiaChiLienHe = item.DiaChiLienHe,
                        NienKhoa = item.NienKhoa,
                        AcademicYear = item.AcademicYear
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetByKhoa(string TenNhhk, long IdKhoa)
        {
            try
            {
                List<SinhVienTheoGiangVien> resultList = new List<SinhVienTheoGiangVien>();
                using (var context = new MyDBContext())
                {
                    long? MaNhhk = context.Kdmnhhks
                         .Where(NHHK => NHHK.TenNhhk == TenNhhk)
                         .Select(NHHK => NHHK.MaNhhk)
                         .FirstOrDefault();
                    int NHHKYear;
                    if (MaNhhk.HasValue)
                    {
                        string maNhhkStr = MaNhhk.Value.ToString();
                        NHHKYear = int.Parse(maNhhkStr.Substring(0, 4));
                    }
                    else { NHHKYear = 0; }

                    var query = (from sv in context.Sdmsvs
                                 join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                 join khoa in context.Ndmkhoas on l.Idkhoa equals khoa.Idkhoa
                                 join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                 where khoa.Idkhoa == IdKhoa
                                 select new SinhVienTheoGiangVien
                                 {
                                     IdSinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     MatKhau = sv.MatKhau,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     IdGiangVien = sv.IdgiangVien,
                                     TenGv = gv.HoTen,
                                     TenLop = l.TenLop,
                                     IdLop = sv.Idlop,
                                     NienKhoa = l.NienKhoa,
                                     DiaChiLienHe = sv.DiaChiLienHe,
                                     AcademicYear = SinhVienService.CalculateAcademicYear(NHHKYear, l.NienKhoa)
                                 }).OrderBy(x => x.MaSinhVien).ToList();
                    resultList = (List<SinhVienTheoGiangVien>)query.Select(item => new SinhVienTheoGiangVien
                    {
                        IdSinhVien = item.IdSinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        MatKhau = item.MatKhau,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        IsBanCanSu = item.IsBanCanSu,
                        IdGiangVien = item.IdGiangVien,
                        TenLop = item.TenLop,
                        TenGv = item.TenGv,
                        IdLop = item.IdLop,
                        DiaChiLienHe = item.DiaChiLienHe,
                        NienKhoa = item.NienKhoa,
                        AcademicYear = item.AcademicYear
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetByHdnk(string TenNhhk, long IdHdnk)
        {
            try
            {
                List<SinhVienTheoHdnk> resultList = new List<SinhVienTheoHdnk>();
                using (var context = new MyDBContext())
                {
                    long? MaNhhk = context.Kdmnhhks
                         .Where(NHHK => NHHK.TenNhhk == TenNhhk)
                         .Select(NHHK => NHHK.MaNhhk)
                         .FirstOrDefault();
                    int NHHKYear;
                    if (MaNhhk.HasValue)
                    {
                        string maNhhkStr = MaNhhk.Value.ToString();
                        NHHKYear = int.Parse(maNhhkStr.Substring(0, 4));
                    }
                    else { NHHKYear = 0; }

                    var query = (from sv in context.Sdmsvs
                                 join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                 join kkq in context.Kkqsvdkhdnks on sv.IdsinhVien equals kkq.IdsinhVien
                                 join hd in context.Kdmhdnks on kkq.Idhdnk equals hd.Idhdnk
                                 where hd.Idhdnk == IdHdnk
                                 select new SinhVienTheoHdnk
                                 {
                                     IdSinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     MatKhau = sv.MatKhau,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     TenLop = l.TenLop,
                                     IdLop = sv.Idlop,
                                     DiaChiLienHe = sv.DiaChiLienHe,
                                     NienKhoa = l.NienKhoa,
                                     AcademicYear = SinhVienService.CalculateAcademicYear(NHHKYear, l.NienKhoa)
                                 }).OrderBy(x => x.MaSinhVien).ToList();
                    resultList = (List<SinhVienTheoHdnk>)query.Select(item => new SinhVienTheoHdnk
                    {
                        IdSinhVien = item.IdSinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        MatKhau = item.MatKhau,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        IsBanCanSu = item.IsBanCanSu,
                        TenLop = item.TenLop,
                        IdLop = item.IdLop,
                        DiaChiLienHe = item.DiaChiLienHe,
                        NienKhoa = item.NienKhoa,
                        AcademicYear = item.AcademicYear
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetSdmsvByTen(string HoTenSinhVien)
        {
            try
            {
                List<SinhVienTableModel> resultList = new List<SinhVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.Sdmsvs
                                 join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                 join nh in context.Kdmnhhks on sv.Idnhhk equals nh.Idnhhk
                                 join lop in context.Sdmlops on sv.Idlop equals lop.Idlop
                                 where sv.HoTenSinhVien.Contains(HoTenSinhVien)
                                 select new SinhVienTableModel
                                 {
                                     IdsinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     MatKhau = sv.MatKhau,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     TenGv = gv.HoTen,
                                     TenLop = lop.TenLop,
                                     TenNhhk = nh.TenNhhk,
                                     DiaChiLienHe = sv.DiaChiLienHe
                                 }).ToList();
                    resultList = (List<SinhVienTableModel>)query.Select(item => new SinhVienTableModel
                    {
                        IdsinhVien = item.IdsinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        MatKhau = item.MatKhau,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        IsBanCanSu = item.IsBanCanSu,
                        TenGv = item.TenGv,
                        TenLop = item.TenLop,
                        TenNhhk = item.TenNhhk,
                        DiaChiLienHe = item.DiaChiLienHe
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetUserInfo(HttpContext HttpContext)
        {
            try
            {
                // Retrieve user information from the session
                var userId = HttpContext.Session.GetString("UserId");
                var username = HttpContext.Session.GetString("Username");
                var role = HttpContext.Session.GetString("Role");
                var hoTenSinhVien = HttpContext.Session.GetString("hoTenSinhVien");
                var phai = HttpContext.Session.GetString("phai");
                var ngaySinh = HttpContext.Session.GetString("ngaySinh");
                var trangThaiSinhVien = HttpContext.Session.GetString("trangThaiSinhVien");
                var isBanCanSu = HttpContext.Session.GetString("isBanCanSu");
                var tenGv = HttpContext.Session.GetString("tenGv");
                var tenLop = HttpContext.Session.GetString("tenLop");
                var tenNhhk = HttpContext.Session.GetString("tenNhhk");
                var tenNganh = HttpContext.Session.GetString("tenNganh");
                var tenKhoa = HttpContext.Session.GetString("tenKhoa");
                var tenBhNgh = HttpContext.Session.GetString("tenBhNgh");
                var maLop = HttpContext.Session.GetString("maLop");
                var maKhoa = HttpContext.Session.GetString("maKhoa");
                var DiaChiLienHe = HttpContext.Session.GetString("DiaChiLienHe");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                {
                    return BadRequest("Session data not found. User may not BuildCongRenLuyen logged in.");
                }

                // You can use the user information as needed
                var userInfo = new { UserId = userId, Username = username, role = role, hoTenSinhVien = hoTenSinhVien, phai = phai, ngaySinh = ngaySinh, trangThaiSinhVien = trangThaiSinhVien, isBanCanSu = isBanCanSu, tenGv = tenGv, tenLop = tenLop, tenNhhk = tenNhhk, tenNganh = tenNganh, tenKhoa = tenKhoa, tenBhNgh = tenBhNgh, maLop = maLop, maKhoa = maKhoa, DiaChiLienHe = DiaChiLienHe };
                DataObject.Clear();
                DataObject.Add(userInfo);

                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetSdmsvByMa(string MaSV)
        {
            try
            {
                List<SinhVienTableModel> resultList = new List<SinhVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.Sdmsvs
                                 join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                 join nh in context.Kdmnhhks on sv.Idnhhk equals nh.Idnhhk
                                 join lop in context.Sdmlops on sv.Idlop equals lop.Idlop
                                 where sv.MaSinhVien.Contains(MaSV)
                                 select new SinhVienTableModel
                                 {
                                     IdsinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     MatKhau = sv.MatKhau,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     TenGv = gv.HoTen,
                                     TenLop = lop.TenLop,
                                     TenNhhk = nh.TenNhhk,
                                     DiaChiLienHe = sv.DiaChiLienHe
                                 }).ToList();
                    resultList = (List<SinhVienTableModel>)query.Select(item => new SinhVienTableModel
                    {
                        IdsinhVien = item.IdsinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        MatKhau = item.MatKhau,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        IsBanCanSu = item.IsBanCanSu,
                        TenGv = item.TenGv,
                        TenLop = item.TenLop,
                        TenNhhk = item.TenNhhk,
                        DiaChiLienHe = item.DiaChiLienHe
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetSdmsvTheoLop(string maLop)
        {
            try
            {
                List<SinhVienTableModel> resultList = new List<SinhVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.Sdmsvs
                                 join lop in context.Sdmlops on sv.Idlop equals lop.Idlop
                                 where lop.MaLop == maLop
                                 //where sv.MaSinhVien.Contains(MaSV)
                                 select new SinhVienTableModel
                                 {
                                     IdsinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     TenLop = lop.TenLop,
                                     DiaChiLienHe = sv.DiaChiLienHe
                                 }).ToList();
                    resultList = query.Select(item => new SinhVienTableModel
                    {
                        IdsinhVien = item.IdsinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        IsBanCanSu = item.IsBanCanSu,
                        TenLop = item.TenLop,
                        DiaChiLienHe = item.DiaChiLienHe
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> GetSdmsvTheoBHNG(string TenBacHeNganh)
        {
            try
            {
                List<SinhVienTableModel> resultList = new List<SinhVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.Sdmsvs
                                 join lop in context.Sdmlops on sv.Idlop equals lop.Idlop
                                 join bhn in context.KdmbhngChngs on lop.IdbhngChng equals bhn.IdbhngChng
                                 join nhhk in context.Kdmnhhks on sv.Idnhhk equals nhhk.Idnhhk
                                 join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                 where bhn.TenBhngChng.Contains(TenBacHeNganh)
                                 //where sv.MaSinhVien.Contains(MaSV)
                                 select new SinhVienTableModel
                                 {
                                     IdsinhVien = sv.IdsinhVien,
                                     MaSinhVien = sv.MaSinhVien,
                                     HoTenSinhVien = sv.HoTenSinhVien,
                                     Phai = sv.Phai,
                                     NgaySinh = sv.NgaySinh,
                                     TrangThaiSinhVien = sv.TrangThaiSinhVien,
                                     IsBanCanSu = sv.IsBanCanSu,
                                     TenLop = lop.TenLop,
                                     TenGv = gv.HoTen,
                                     TenNhhk = nhhk.TenNhhk,
                                     DiaChiLienHe = sv.DiaChiLienHe
                                 }).ToList();
                    resultList = query.Select(item => new SinhVienTableModel
                    {
                        IdsinhVien = item.IdsinhVien,
                        MaSinhVien = item.MaSinhVien,
                        HoTenSinhVien = item.HoTenSinhVien,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        IsBanCanSu = item.IsBanCanSu,
                        TenLop = item.TenLop,
                        TrangThaiSinhVien = item.TrangThaiSinhVien,
                        TenGv = item.TenGv,
                        TenNhhk = item.TenNhhk,
                        DiaChiLienHe = item.DiaChiLienHe
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> CreateSinhVien([FromBody] SinhVienTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idLop = context.Sdmlops
                          .Where(lop => lop.TenLop == InputData.TenLop)
                          .Select(lop => lop.Idlop)
                          .FirstOrDefault();
                    long idNHHK = context.Kdmnhhks
                        .Where(nhhk => nhhk.TenNhhk == InputData.TenNhhk)
                        .Select(nhhk => nhhk.Idnhhk)
                        .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                          .Where(gv => gv.HoTen == InputData.TenGv)
                        .Select(gv => gv.IdgiangVien)
                        .FirstOrDefault();
                    Sdmsv newData = new Sdmsv()
                    {
                        IdsinhVien = IdGenerator.NewUID,
                        MaSinhVien = InputData.MaSinhVien,
                        HoTenSinhVien = InputData.HoTenSinhVien,
                        MatKhau = PasswordGenerator.HashPassword(InputData.MatKhau),
                        Phai = InputData.Phai,
                        NgaySinh = InputData.NgaySinh,
                        TrangThaiSinhVien = InputData.TrangThaiSinhVien,
                        IsBanCanSu = InputData.IsBanCanSu,
                        DiaChiLienHe = InputData.DiaChiLienHe,
                        IdgiangVien = idGiangVien,
                        Idlop = idLop,
                        Idnhhk = idNHHK
                    };
                    context.Sdmsvs.Add(newData);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(newData);
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] SinhVienTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idLop = context.Sdmlops
                        .Where(lop => lop.TenLop == InputData.TenLop)
                        .Select(lop => lop.Idlop)
                        .FirstOrDefault();
                    long idNHHK = context.Kdmnhhks
                        .Where(nhhk => nhhk.TenNhhk == InputData.TenNhhk)
                        .Select(nhhk => nhhk.Idnhhk)
                        .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                          .Where(gv => gv.HoTen == InputData.TenGv)
                        .Select(gv => gv.IdgiangVien)
                        .FirstOrDefault();
                    var existing = context.Sdmsvs.Find(id);
                    if (existing == null)
                    {
                        return null;
                    }
                    existing.MaSinhVien = InputData.MaSinhVien;
                    existing.HoTenSinhVien = InputData.HoTenSinhVien;
                    existing.MatKhau = PasswordGenerator.HashPassword(InputData.MatKhau);
                    existing.Phai = InputData.Phai;
                    existing.NgaySinh = InputData.NgaySinh;
                    existing.TrangThaiSinhVien = InputData.TrangThaiSinhVien;
                    existing.IsBanCanSu = InputData.IsBanCanSu;
                    existing.DiaChiLienHe = InputData.DiaChiLienHe;
                    existing.IdgiangVien = idGiangVien;
                    existing.Idlop = idLop;
                    existing.Idnhhk = idNHHK;
                    context.Sdmsvs.Update(existing);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(existing);
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var data = context.Sdmsvs.Find(id);
                    if (data == null)
                    {
                        return null;
                    }
                    context.Remove(data);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(data);
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Sinh vien not found";
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

        private static string CalculateAcademicYear(int nhhkYear, string nienKhoa)
        {
            int yearDiff = nhhkYear - int.Parse(nienKhoa.Substring(0, 4));

            return yearDiff switch
            {
                0 => "Sinh Viên Năm 1",
                1 => "Sinh Viên Năm 2",
                2 => "Sinh Viên Năm 3",
                3 => "Sinh Viên Năm 4",
                < 0 => "Chưa Đến Niên Khóa",
                _ => "Hết Niên Khóa"
            };
        }
    }
}