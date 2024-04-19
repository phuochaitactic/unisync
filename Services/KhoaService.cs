using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IKhoaService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetByMaKhoa(string MaKhoa);

        Task<IActionResult> GetUserInfo(HttpContext HttpContext);

        Task<IActionResult> GetByTenKhoa(string TenKhoa);

        Task<IActionResult> CreateKhoa([FromBody] Ndmkhoa InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] Ndmkhoa InputData);

        Task<IActionResult> Delete(long id);
    }

    public class KhoaService : BaseController, IKhoaService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var resultList = await context.Ndmkhoas.ToListAsync();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Khoa not found";
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
                var maKhoa = HttpContext.Session.GetString("maKhoa");
                var tenKhoa = HttpContext.Session.GetString("tenKhoa");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                {
                    return BadRequest("Session data not found. User may not BuildCongRenLuyen logged in.");
                }

                // You can use the user information as needed
                var userInfo = new { UserId = userId, Username = username, role = role, tenKhoa = tenKhoa, maKhoa = maKhoa };
                DataObject.Clear();
                DataObject.Add(userInfo);

                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Khoa not found";
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
                List<Ndmkhoa> resultList = new List<Ndmkhoa>();
                using (var context = new MyDBContext())
                {
                    var query = (from khoa in context.Ndmkhoas
                                 where khoa.Idkhoa == id
                                 select new Ndmkhoa
                                 {
                                     Idkhoa = khoa.Idkhoa,
                                     MaKhoa = khoa.MaKhoa,
                                     MatKhau = khoa.MatKhau,
                                     TenKhoa = khoa.TenKhoa,
                                 }).ToList();
                    resultList = (List<Ndmkhoa>)query.Select(item => new Ndmkhoa
                    {
                        Idkhoa = item.Idkhoa,
                        MaKhoa = item.MaKhoa,
                        MatKhau = item.MatKhau,
                        TenKhoa = item.TenKhoa,
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
                Message = "Khoa not found";
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

        public async Task<IActionResult> GetByMaKhoa(string MaKhoa)
        {
            try
            {
                List<Ndmkhoa> resultList = new List<Ndmkhoa>();
                using (var context = new MyDBContext())
                {
                    var query = (from khoa in context.Ndmkhoas
                                 where khoa.MaKhoa.Contains(MaKhoa)
                                 select new Ndmkhoa
                                 {
                                     Idkhoa = khoa.Idkhoa,
                                     MaKhoa = khoa.MaKhoa,
                                     MatKhau = khoa.MatKhau,
                                     TenKhoa = khoa.TenKhoa,
                                 }).ToList();
                    resultList = (List<Ndmkhoa>)query.Select(item => new Ndmkhoa
                    {
                        Idkhoa = item.Idkhoa,
                        MaKhoa = item.MaKhoa,
                        MatKhau = item.MatKhau,
                        TenKhoa = item.TenKhoa,
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
                Message = "Khoa not found";
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

        public async Task<IActionResult> GetByTenKhoa(string TenKhoa)
        {
            try
            {
                List<Ndmkhoa> resultList = new List<Ndmkhoa>();
                using (var context = new MyDBContext())
                {
                    var query = (from khoa in context.Ndmkhoas
                                 where khoa.TenKhoa.Contains(TenKhoa)
                                 select new Ndmkhoa
                                 {
                                     Idkhoa = khoa.Idkhoa,
                                     MaKhoa = khoa.MaKhoa,
                                     MatKhau = khoa.MatKhau,
                                     TenKhoa = khoa.TenKhoa,
                                 }).ToList();
                    resultList = (List<Ndmkhoa>)query.Select(item => new Ndmkhoa
                    {
                        Idkhoa = item.Idkhoa,
                        MaKhoa = item.MaKhoa,
                        MatKhau = item.MatKhau,
                        TenKhoa = item.TenKhoa,
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
                Message = "Khoa not found";
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

        public async Task<IActionResult> CreateKhoa([FromBody] Ndmkhoa InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var iskhoaTheoMa = context.Ndmkhoas
                     .Where(gv => gv.MaKhoa == InputData.MaKhoa)
                     .Select(gv => gv)
                     .FirstOrDefault();
                    var iskhoaTheoTen = context.Ndmkhoas
                   .Where(gv => gv.TenKhoa == InputData.TenKhoa)
                   .Select(gv => gv)
                   .FirstOrDefault();
                    if (iskhoaTheoMa != null || iskhoaTheoTen != null)
                    {
                        Code = 500;
                        Message = "Khoa đã tồn tại";
                        return CreateResponse();
                    }
                    Ndmkhoa newData = new Ndmkhoa()
                    {
                        Idkhoa = IdGenerator.NewUID,
                        MaKhoa = InputData.MaKhoa,
                        TenKhoa = InputData.TenKhoa,
                        MatKhau = PasswordGenerator.HashPassword(InputData.MatKhau),
                    };
                    await context.Ndmkhoas.AddAsync(newData);
                    await context.SaveChangesAsync();
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
                Message = "Khoa not found";
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
                //Code = 401;
                Message = "Unauthorized";
                return CreateResponse();
            }
            catch (Exception ex)
            {
                // Unhandled error
                Code = 500;
                Message = "error" + ex.Message;
                return CreateResponse();
            }
        }

        public async Task<IActionResult> ChangeData(long id, [FromBody] Ndmkhoa InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.Ndmkhoas.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.MaKhoa = InputData.MaKhoa;
                    existing.TenKhoa = InputData.TenKhoa;
                    existing.MatKhau = PasswordGenerator.HashPassword(InputData.MatKhau);
                    context.Ndmkhoas.Update(existing);
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
                Message = "Khoa not found";
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
                    var data = context.Ndmkhoas.Find(id);
                    if (data == null)
                    {
                        return Ok("data not exist");
                    }
                    context.Remove(data);
                    await context.SaveChangesAsync();
                    DataObject.Clear();
                    DataObject.Add(data);
                    Message = "Data deleted"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Khoa not found";
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