using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ILichDuyetSVService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetTheoKhoa(string TenKhoa, string TenNHHK, string TenBHN);

        Task<IActionResult> CreateLichDuyetSV([FromBody] LichDuyetSVModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] LichDuyetSVModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class LichDuyetSVService : BaseController, ILichDuyetSVService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<LichDuyetSVModel> resultList = new List<LichDuyetSVModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ldk in context.KdmlichDuyetSvs
                                 join khoa in context.Ndmkhoas on ldk.Idkhoa equals khoa.Idkhoa
                                 join nh in context.Kdmnhhks on ldk.Idnhhk equals nh.Idnhhk

                                 select new LichDuyetSVModel
                                 {
                                     IdlichDuyet = ldk.IdlichDuyet,
                                     TenKhoa = khoa.TenKhoa,
                                     MaKhoa = khoa.MaKhoa,
                                     TenNhhk = nh.TenNhhk,
                                     NgayBatDau = ldk.NgayBatDau,
                                     NgayKetThuc = ldk.NgayKetThuc,
                                 }).ToList();
                    resultList = query.Select(item => new LichDuyetSVModel()
                    {
                        IdlichDuyet = item.IdlichDuyet,
                        TenKhoa = item.TenKhoa,
                        MaKhoa = item.MaKhoa,
                        TenNhhk = item.TenNhhk,
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
                Message = "Lich duyet sinh vien not found";
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

        public async Task<IActionResult> GetTheoKhoa(string TenKhoa, string TenNHHK, string TenBHN)
        {
            try
            {
                List<LichDuyetSVModel> resultList = new List<LichDuyetSVModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ldk in context.KdmlichDuyetSvs
                                 join khoa in context.Ndmkhoas on ldk.Idkhoa equals khoa.Idkhoa
                                 join ngh in context.Kdmnghs on khoa.Idkhoa equals ngh.Idkhoa
                                 join bhn in context.KdmbhngChngs on ngh.Idngh equals bhn.IdbhngChng
                                 join nh in context.Kdmnhhks on ldk.Idnhhk equals nh.Idnhhk
                                 where nh.TenNhhk.Contains(TenNHHK) && khoa.TenKhoa.Contains(TenKhoa) && bhn.TenBhngChng.Contains(TenBHN)
                                 select new LichDuyetSVModel
                                 {
                                     IdlichDuyet = ldk.IdlichDuyet,
                                     TenKhoa = khoa.TenKhoa,
                                     MaKhoa = khoa.MaKhoa,
                                     TenNhhk = nh.TenNhhk,
                                     NgayBatDau = ldk.NgayBatDau,
                                     NgayKetThuc = ldk.NgayKetThuc,
                                 }).ToList();
                    resultList = query.Select(item => new LichDuyetSVModel()
                    {
                        IdlichDuyet = item.IdlichDuyet,
                        TenKhoa = item.TenKhoa,
                        MaKhoa = item.MaKhoa,
                        TenNhhk = item.TenNhhk,
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
                Message = "Lich duyet sinh vien not found";
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

        public async Task<IActionResult> CreateLichDuyetSV([FromBody] LichDuyetSVModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idkhoa = context.Ndmkhoas
                          .Where(khoa => khoa.TenKhoa == inputData.TenKhoa)
                          .Select(khoa => khoa.Idkhoa)
                          .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                                              .Where(nh => nh.TenNhhk == inputData.TenNhhk)
                                              .Select(nh => nh.Idnhhk)
                                              .FirstOrDefault();

                    KdmlichDuyetSv newData = new KdmlichDuyetSv()
                    {
                        IdlichDuyet = IdGenerator.NewUID,
                        Idkhoa = idkhoa,
                        Idnhhk = idNhhk,
                        NgayBatDau = inputData.NgayBatDau,
                        NgayKetThuc = inputData.NgayKetThuc,
                    };
                    context.KdmlichDuyetSvs.Add(newData);
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
                Message = "Lich duyet sinh vien not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] LichDuyetSVModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idLop = context.Ndmkhoas
                         .Where(lop => lop.TenKhoa == inputData.TenKhoa)
                         .Select(lop => lop.Idkhoa)
                         .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                                              .Where(nh => nh.TenNhhk == inputData.TenNhhk)
                                              .Select(nh => nh.Idnhhk)
                                              .FirstOrDefault();
                    var existing = context.KdmlichDuyetSvs.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.Idkhoa = idLop;
                    existing.Idnhhk = idNhhk;
                    existing.NgayBatDau = inputData.NgayBatDau;
                    existing.NgayKetThuc = inputData.NgayKetThuc;
                    context.KdmlichDuyetSvs.Update(existing);
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
                Message = "Lich duyet sinh vien not found";
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
                    var data = await context.KdmlichDuyetSvs.FindAsync(id);
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
                Message = "Lich duyet sinh vien not found";
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