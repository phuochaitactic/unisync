using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IGiangVienService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetGVByMaNV(string MaNV);

        Task<IActionResult> GetGVbyHoTen(string HoTen);

        Task<IActionResult> GetUserInfo(HttpContext HttpContext);

        Task<IActionResult> GetGVbyTenKhoa(string TenKhoa);

        Task<IActionResult> GetGVbyVaiTro(string VaiTro);

        Task<IActionResult> CreateGiangVien([FromBody] GiangVienTableModel InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] GiangVienTableModel InputData);

        Task<IActionResult> Delete(long id);
    }

    public class GiangVienService : BaseController, IGiangVienService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<GiangVienTableModel> resultList = new List<GiangVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 select new GiangVienTableModel
                                 {
                                     IdgiangVien = gv.IdgiangVien,
                                     MaNv = gv.MaNv,
                                     MatKhau = gv.MatKhau,
                                     HoTen = gv.HoTen,
                                     Phai = gv.Phai,
                                     NgaySinh = gv.NgaySinh,
                                     TenKhoa = khoa.TenKhoa,
                                     IdKhoa = khoa.Idkhoa,
                                     VaiTro = gv.VaiTro,
                                     ThongTinLienHe = gv.ThongTinLienHe,
                                 }).ToList();
                    resultList = (List<GiangVienTableModel>)query.Select(item => new GiangVienTableModel
                    {
                        IdgiangVien = item.IdgiangVien,
                        MaNv = item.MaNv,
                        MatKhau = item.MatKhau,
                        HoTen = item.HoTen,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TenKhoa = item.TenKhoa,
                        IdKhoa = item.IdKhoa,
                        VaiTro = item.VaiTro,
                        ThongTinLienHe = item.ThongTinLienHe,
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
                Message = "Giang vien not found";
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
                var hoTen = HttpContext.Session.GetString("hoTen");
                var phai = HttpContext.Session.GetString("phai");
                var ngaySinh = HttpContext.Session.GetString("ngaySinh");
                var tenKhoa = HttpContext.Session.GetString("tenKhoa");
                var vaiTro = HttpContext.Session.GetString("vaiTro");
                var thongTinLienHe = HttpContext.Session.GetString("thongTinLienHe");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                {
                    return BadRequest("Session data not found. User may not BuildCongRenLuyen logged in.");
                }

                // You can use the user information as needed
                var userInfo = new { UserId = userId, Username = username, role = role, hoTen = hoTen, phai = phai, ngaySinh = ngaySinh, tenKhoa = tenKhoa, vaiTro = vaiTro, thongTinLienHe = thongTinLienHe };
                DataObject.Clear();
                DataObject.Add(userInfo);

                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Giang vien not found";
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

        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                List<GiangVienTableModel> resultList = new List<GiangVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 where gv.IdgiangVien == id
                                 select new GiangVienTableModel
                                 {
                                     IdgiangVien = gv.IdgiangVien,
                                     MaNv = gv.MaNv,
                                     MatKhau = gv.MatKhau,
                                     HoTen = gv.HoTen,
                                     Phai = gv.Phai,
                                     NgaySinh = gv.NgaySinh,
                                     TenKhoa = khoa.TenKhoa,
                                     IdKhoa = khoa.Idkhoa,
                                     VaiTro = gv.VaiTro,
                                     ThongTinLienHe = gv.ThongTinLienHe,
                                 }).ToList();
                    resultList = query.Select(item => new GiangVienTableModel
                    {
                        IdgiangVien = item.IdgiangVien,
                        MaNv = item.MaNv,
                        MatKhau = item.MatKhau,
                        HoTen = item.HoTen,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TenKhoa = item.TenKhoa,
                        IdKhoa = item.IdKhoa,
                        VaiTro = item.VaiTro,
                        ThongTinLienHe = item.ThongTinLienHe,
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
                Message = "Giang vien not found";
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

        public async Task<IActionResult> GetGVByMaNV(string MaNV)
        {
            try
            {
                List<GiangVienTableModel> resultList = new List<GiangVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 where gv.MaNv.Contains(MaNV)
                                 select new GiangVienTableModel
                                 {
                                     IdgiangVien = gv.IdgiangVien,
                                     MaNv = gv.MaNv,
                                     MatKhau = gv.MatKhau,
                                     HoTen = gv.HoTen,
                                     Phai = gv.Phai,
                                     NgaySinh = gv.NgaySinh,
                                     TenKhoa = khoa.TenKhoa,
                                     IdKhoa = khoa.Idkhoa,
                                     VaiTro = gv.VaiTro,
                                     ThongTinLienHe = gv.ThongTinLienHe,
                                 }).ToList();
                    resultList = (List<GiangVienTableModel>)query.Select(item => new GiangVienTableModel
                    {
                        IdgiangVien = item.IdgiangVien,
                        MaNv = item.MaNv,
                        MatKhau = item.MatKhau,
                        HoTen = item.HoTen,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TenKhoa = item.TenKhoa,
                        IdKhoa = item.IdKhoa,
                        VaiTro = item.VaiTro,
                        ThongTinLienHe = item.ThongTinLienHe,
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
                Message = "Giang vien not found";
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

        public async Task<IActionResult> GetGVbyHoTen(string HoTen)
        {
            try
            {
                List<GiangVienTableModel> resultList = new List<GiangVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 where gv.HoTen.Contains(HoTen)
                                 select new GiangVienTableModel
                                 {
                                     IdgiangVien = gv.IdgiangVien,
                                     MaNv = gv.MaNv,
                                     MatKhau = gv.MatKhau,
                                     HoTen = gv.HoTen,
                                     Phai = gv.Phai,
                                     NgaySinh = gv.NgaySinh,
                                     TenKhoa = khoa.TenKhoa,
                                     IdKhoa = khoa.Idkhoa,
                                     VaiTro = gv.VaiTro,
                                     ThongTinLienHe = gv.ThongTinLienHe,
                                 }).ToList();
                    resultList = query.Select(item => new GiangVienTableModel
                    {
                        IdgiangVien = item.IdgiangVien,
                        MaNv = item.MaNv,
                        MatKhau = item.MatKhau,
                        HoTen = item.HoTen,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TenKhoa = item.TenKhoa,
                        IdKhoa = item.IdKhoa,
                        VaiTro = item.VaiTro,
                        ThongTinLienHe = item.ThongTinLienHe,
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
                Message = "Giang vien not found";
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

        public async Task<IActionResult> GetGVbyTenKhoa(string TenKhoa)
        {
            try
            {
                List<GiangVienTableModel> resultList = new List<GiangVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 where khoa.TenKhoa.Contains(TenKhoa)
                                 select new GiangVienTableModel
                                 {
                                     IdgiangVien = gv.IdgiangVien,
                                     MaNv = gv.MaNv,
                                     MatKhau = gv.MatKhau,
                                     HoTen = gv.HoTen,
                                     Phai = gv.Phai,
                                     NgaySinh = gv.NgaySinh,
                                     TenKhoa = khoa.TenKhoa,
                                     IdKhoa = khoa.Idkhoa,
                                     VaiTro = gv.VaiTro,
                                     ThongTinLienHe = gv.ThongTinLienHe,
                                 }).ToList();
                    resultList = (List<GiangVienTableModel>)query.Select(item => new GiangVienTableModel
                    {
                        IdgiangVien = item.IdgiangVien,
                        MaNv = item.MaNv,
                        MatKhau = item.MatKhau,
                        HoTen = item.HoTen,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TenKhoa = item.TenKhoa,
                        IdKhoa = item.IdKhoa,
                        VaiTro = item.VaiTro,
                        ThongTinLienHe = item.ThongTinLienHe,
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
                Message = "Giang vien not found";
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

        public async Task<IActionResult> GetGVbyVaiTro(string VaiTro)
        {
            try
            {
                List<GiangVienTableModel> resultList = new List<GiangVienTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 where gv.VaiTro.Contains(VaiTro)
                                 select new GiangVienTableModel
                                 {
                                     IdgiangVien = gv.IdgiangVien,
                                     MaNv = gv.MaNv,
                                     MatKhau = gv.MatKhau,
                                     HoTen = gv.HoTen,
                                     Phai = gv.Phai,
                                     NgaySinh = gv.NgaySinh,
                                     IdKhoa = khoa.Idkhoa,
                                     TenKhoa = khoa.TenKhoa,
                                     VaiTro = gv.VaiTro,
                                     ThongTinLienHe = gv.ThongTinLienHe,
                                 }).ToList();
                    resultList = (List<GiangVienTableModel>)query.Select(item => new GiangVienTableModel
                    {
                        IdgiangVien = item.IdgiangVien,
                        MaNv = item.MaNv,
                        MatKhau = item.MatKhau,
                        HoTen = item.HoTen,
                        Phai = item.Phai,
                        NgaySinh = item.NgaySinh,
                        TenKhoa = item.TenKhoa,
                        IdKhoa = item.IdKhoa,
                        VaiTro = item.VaiTro,
                        ThongTinLienHe = item.ThongTinLienHe,
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
                Message = "Giang vien not found";
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

        public async Task<IActionResult> CreateGiangVien([FromBody] GiangVienTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idKhoa = context.Ndmkhoas
                          .Where(khoa => khoa.TenKhoa == InputData.TenKhoa)
                          .Select(khoa => khoa.Idkhoa)
                          .FirstOrDefault();
                    var isGiangVienTheoMa = context.NdmgiangViens
                     .Where(gv => gv.MaNv == InputData.MaNv)
                     .Select(gv => gv)
                     .FirstOrDefault();
                    var isGiangVienTheoTen = context.NdmgiangViens
                   .Where(gv => gv.HoTen == InputData.HoTen)
                   .Select(gv => gv)
                   .FirstOrDefault();
                    if (isGiangVienTheoMa != null || isGiangVienTheoTen != null)
                    {
                        Code = 500;
                        Message = "giảng viên đã tồn tại";
                        return CreateResponse();
                    }
                    NdmgiangVien newData = new NdmgiangVien()
                    {
                        IdgiangVien = IdGenerator.NewUID,
                        MaNv = InputData.MaNv,
                        MatKhau = PasswordGenerator.HashPassword(InputData.MatKhau),
                        Phai = InputData.Phai,
                        NgaySinh = InputData.NgaySinh,
                        HoTen = InputData.HoTen,
                        VaiTro = InputData.VaiTro,
                        ThongTinLienHe = InputData.ThongTinLienHe,
                        Idkhoa = idKhoa
                    };
                    context.NdmgiangViens.Add(newData);
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
                Message = "Giang vien not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] GiangVienTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idKhoa = context.Ndmkhoas
                          .Where(khoa => khoa.TenKhoa == InputData.TenKhoa)
                          .Select(khoa => khoa.Idkhoa)
                          .FirstOrDefault();
                    var existing = context.NdmgiangViens.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.MaNv = InputData.MaNv;
                    existing.MatKhau = InputData.MatKhau;
                    existing.Phai = InputData.Phai;
                    existing.NgaySinh = InputData.NgaySinh;
                    existing.HoTen = InputData.HoTen;
                    existing.VaiTro = InputData.VaiTro;
                    existing.ThongTinLienHe = InputData.ThongTinLienHe;
                    existing.Idkhoa = idKhoa;

                    context.NdmgiangViens.Update(existing);
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
                Message = "Giang vien not found";
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
                    var data = context.NdmgiangViens.Find(id);
                    if (data == null)
                    {
                        return Ok("data not exist");
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
                Message = "Giang vien not found";
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
    }
}