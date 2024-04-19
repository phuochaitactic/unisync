using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ILichDangKyService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByTenLop(string tenLop);

        Task<IActionResult> GetByKhoa(string TenKhoa, string TenNhhk, string TenBHN);

        Task<IActionResult> GetByTenNHHK(string tenNHHK);

        Task<IActionResult> CreateLich([FromBody] LichDangKyModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] LichDangKyModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class LichDangKyService : BaseController, ILichDangKyService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<LichDangKyModel> resultList = new List<LichDangKyModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ldk in context.KdmlichDangKies
                                 join lop in context.Sdmlops on ldk.Idlop equals lop.Idlop
                                 join nh in context.Kdmnhhks on ldk.Idnhhk equals nh.Idnhhk
                                 select new LichDangKyModel
                                 {
                                     IdlichDangKy = ldk.IdlichDangKy,
                                     TenLop = lop.TenLop,
                                     tenNhhk = nh.TenNhhk,
                                     NgayBatDau = ldk.NgayBatDau,
                                     NgayKetThuc = ldk.NgayKetThuc,
                                 }).ToList();
                    resultList = query.Select(item => new LichDangKyModel()
                    {
                        IdlichDangKy = item.IdlichDangKy,
                        TenLop = item.TenLop,
                        tenNhhk = item.tenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        NgayKetThuc = item.NgayKetThuc,
                    }).ToList();
                }
                DataObject = resultList.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lich dang ky not found";
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

        public async Task<IActionResult> GetByKhoa(string TenKhoa, string TenNhhk, string TenBHN)
        {
            try
            {
                List<LichDangKyModel> resultList = new List<LichDangKyModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ldk in context.KdmlichDangKies
                                 join lop in context.Sdmlops on ldk.Idlop equals lop.Idlop
                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa
                                 join bhn in context.KdmbhngChngs on lop.IdbhngChng equals bhn.IdbhngChng
                                 join nhhk in context.Kdmnhhks on ldk.Idnhhk equals nhhk.Idnhhk
                                 where nhhk.TenNhhk.Contains(TenNhhk) && khoa.TenKhoa.Contains(TenKhoa) && bhn.TenBhngChng.Contains(TenBHN)
                                 select new LichDangKyModel
                                 {
                                     IdlichDangKy = ldk.IdlichDangKy,
                                     TenLop = lop.TenLop,
                                     tenNhhk = nhhk.TenNhhk,
                                     NgayBatDau = ldk.NgayBatDau,
                                     NgayKetThuc = ldk.NgayKetThuc,
                                 }).ToList();
                    resultList = query.Select(item => new LichDangKyModel()
                    {
                        IdlichDangKy = item.IdlichDangKy,
                        TenLop = item.TenLop,
                        tenNhhk = item.tenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        NgayKetThuc = item.NgayKetThuc,
                    }).ToList();
                }
                DataObject = resultList.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lich dang ky not found";
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

        public async Task<IActionResult> GetByTenLop(string tenLop)
        {
            try
            {
                List<LichDangKyModel> resultList = new List<LichDangKyModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ldk in context.KdmlichDangKies
                                 join lop in context.Sdmlops on ldk.Idlop equals lop.Idlop
                                 join nh in context.Kdmnhhks on ldk.Idnhhk equals nh.Idnhhk
                                 where lop.TenLop.Contains(tenLop)
                                 select new LichDangKyModel
                                 {
                                     IdlichDangKy = ldk.IdlichDangKy,
                                     TenLop = lop.TenLop,
                                     tenNhhk = nh.TenNhhk,
                                     NgayBatDau = ldk.NgayBatDau,
                                     NgayKetThuc = ldk.NgayKetThuc,
                                 }).ToList();
                    resultList = query.Select(item => new LichDangKyModel()
                    {
                        IdlichDangKy = item.IdlichDangKy,
                        TenLop = item.TenLop,
                        tenNhhk = item.tenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        NgayKetThuc = item.NgayKetThuc,
                    }).ToList();
                }
                DataObject = resultList.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lich dang ky not found";
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

        public async Task<IActionResult> GetByTenNHHK(string tenNHHK)
        {
            try
            {
                List<LichDangKyModel> resultList = new List<LichDangKyModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ldk in context.KdmlichDangKies
                                 join lop in context.Sdmlops on ldk.Idlop equals lop.Idlop
                                 join nh in context.Kdmnhhks on ldk.Idnhhk equals nh.Idnhhk
                                 where nh.TenNhhk.Contains(tenNHHK)
                                 select new LichDangKyModel
                                 {
                                     IdlichDangKy = ldk.IdlichDangKy,
                                     TenLop = lop.TenLop,
                                     tenNhhk = nh.TenNhhk,
                                     NgayBatDau = ldk.NgayBatDau,
                                     NgayKetThuc = ldk.NgayKetThuc,
                                 }).ToList();
                    resultList = query.Select(item => new LichDangKyModel()
                    {
                        IdlichDangKy = item.IdlichDangKy,
                        TenLop = item.TenLop,
                        tenNhhk = item.tenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        NgayKetThuc = item.NgayKetThuc,
                    }).ToList();
                }
                DataObject = resultList.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lich dang ky not found";
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

        public async Task<IActionResult> CreateLich([FromBody] LichDangKyModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idLop = context.Sdmlops
                          .Where(lop => lop.TenLop == inputData.TenLop)
                          .Select(lop => lop.Idlop)
                          .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                                              .Where(nh => nh.TenNhhk == inputData.tenNhhk)
                                              .Select(nh => nh.Idnhhk)
                                              .FirstOrDefault();

                    KdmlichDangKy newData = new KdmlichDangKy()
                    {
                        IdlichDangKy = IdGenerator.NewUID,
                        Idlop = idLop,
                        Idnhhk = idNhhk,
                        NgayBatDau = inputData.NgayBatDau,
                        NgayKetThuc = inputData.NgayKetThuc,
                    };
                    context.KdmlichDangKies.Add(newData);
                    context.SaveChanges();

                    DataObject.Clear();
                    DataObject.Add(newData);
                    Message = "Data created"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lich dang ky not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] LichDangKyModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idLop = context.Sdmlops
                         .Where(lop => lop.TenLop == inputData.TenLop)
                         .Select(lop => lop.Idlop)
                         .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                                              .Where(nh => nh.TenNhhk == inputData.tenNhhk)
                                              .Select(nh => nh.Idnhhk)
                                              .FirstOrDefault();
                    var existing = context.KdmlichDangKies.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.Idlop = idLop;
                    existing.Idnhhk = idNhhk;
                    existing.NgayBatDau = inputData.NgayBatDau;
                    existing.NgayKetThuc = inputData.NgayKetThuc;
                    context.KdmlichDangKies.Update(existing);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(existing);
                    Message = "Data Changed"; Code = 200;
                    Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lich dang ky not found";
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
                    var data = await context.KdmlichDangKies.FindAsync(id);
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
                Message = "Lich dang ky not found";
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