using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ILichTaoHdnkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByKhoa(string TenKhoa, string TenBHN, string TenNHHK);

        Task<IActionResult> CreateData([FromBody] KDMLichTaoHDNKModel InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] KDMLichTaoHDNKModel InputData);

        Task<IActionResult> Delete(long id);
    }

    public class LichTaoHdnkService : BaseController, ILichTaoHdnkService
    {
        public async Task<IActionResult> GetAll()
        {
            List<KDMLichTaoHDNKModel> resultList = new List<KDMLichTaoHDNKModel>();
            using (var context = new MyDBContext())
            {
                try
                {
                    var query = (from lich in context.KdmlichTaoHdnks
                                 join khoa in context.Ndmkhoas on lich.Idkhoa equals khoa.Idkhoa
                                 join nhhk in context.Kdmnhhks on lich.Idnhhk equals nhhk.Idnhhk

                                 select new KDMLichTaoHDNKModel
                                 {
                                     IdlichTaoHdnk = lich.IdlichTaoHdnk,
                                     Tenkhoa = khoa.TenKhoa,
                                     TenNHHK = nhhk.TenNhhk,
                                     NgayBatDau = lich.NgayBatDau,
                                     NgayKetThuc = lich.NgayKetThuc
                                 }).ToList();

                    resultList = query.Select(item => new KDMLichTaoHDNKModel
                    {
                        IdlichTaoHdnk = item.IdlichTaoHdnk,
                        Tenkhoa = item.Tenkhoa,
                        TenNHHK = item.TenNHHK,
                        NgayBatDau = item.NgayBatDau,
                        NgayKetThuc = item.NgayKetThuc
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    // ID not found
                    Code = 404;
                    Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> GetByKhoa(string TenKhoa, string TenNHHK, string TenBHN)
        {
            List<KDMLichTaoHDNKModel> resultList = new List<KDMLichTaoHDNKModel>();
            using (var context = new MyDBContext())
            {
                try
                {
                    var query = (from lich in context.KdmlichTaoHdnks
                                 join khoa in context.Ndmkhoas on lich.Idkhoa equals khoa.Idkhoa
                                 join ngh in context.Kdmnghs on khoa.Idkhoa equals ngh.Idkhoa
                                 join bhn in context.KdmbhngChngs on ngh.Idngh equals bhn.Idngh
                                 join nhhk in context.Kdmnhhks on lich.Idnhhk equals nhhk.Idnhhk
                                 where nhhk.TenNhhk.Contains(TenNHHK) && khoa.TenKhoa.Contains(TenKhoa) && bhn.TenBhngChng.Contains(TenBHN)
                                 select new KDMLichTaoHDNKModel
                                 {
                                     IdlichTaoHdnk = lich.IdlichTaoHdnk,
                                     Tenkhoa = khoa.TenKhoa,
                                     TenNHHK = nhhk.TenNhhk,
                                     NgayBatDau = lich.NgayBatDau,
                                     NgayKetThuc = lich.NgayKetThuc
                                 }).ToList();

                    resultList = query.Select(item => new KDMLichTaoHDNKModel
                    {
                        IdlichTaoHdnk = item.IdlichTaoHdnk,
                        Tenkhoa = item.Tenkhoa,
                        TenNHHK = item.TenNHHK,
                        NgayBatDau = item.NgayBatDau,
                        NgayKetThuc = item.NgayKetThuc
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    // ID not found
                    Code = 404;
                    Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> CreateData([FromBody] KDMLichTaoHDNKModel InputData)
        {
            using (var context = new MyDBContext())

            {
                try
                {
                    long idKhoa = context.Ndmkhoas
                        .Where(khoa => khoa.TenKhoa == InputData.Tenkhoa)
                        .Select(khoa => khoa.Idkhoa)
                        .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                       .Where(NHHK => NHHK.TenNhhk == InputData.TenNHHK)
                       .Select(NHHK => NHHK.Idnhhk)
                       .FirstOrDefault();
                    KdmlichTaoHdnk newData = new KdmlichTaoHdnk()
                    {
                        IdlichTaoHdnk = IdGenerator.NewUID,
                        Idkhoa = idKhoa,
                        Idnhhk = idNhhk,
                        NgayBatDau = InputData.NgayBatDau,
                        NgayKetThuc = InputData.NgayKetThuc
                    };
                    context.KdmlichTaoHdnks.Add(newData);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(newData);
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    // ID not found
                    Code = 404;
                    Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] KDMLichTaoHDNKModel InputData)
        {
            using (var context = new MyDBContext())

            {
                try
                {
                    long idKhoa = context.Ndmkhoas
                        .Where(khoa => khoa.TenKhoa == InputData.Tenkhoa)
                        .Select(khoa => khoa.Idkhoa)
                        .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                       .Where(NHHK => NHHK.TenNhhk == InputData.TenNHHK)
                       .Select(NHHK => NHHK.Idnhhk)
                       .FirstOrDefault();
                    var existing = context.KdmlichTaoHdnks.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.Idkhoa = idKhoa;
                    existing.Idnhhk = idNhhk;
                    existing.NgayBatDau = InputData.NgayBatDau;
                    existing.NgayKetThuc = InputData.NgayKetThuc;

                    context.KdmlichTaoHdnks.Update(existing);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(existing);
                    Message = "Data Changed"; Code = 200;
                    return CreateResponse();
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    // ID not found
                    Code = 404;
                    Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var data = await context.KdmlichTaoHdnks.FindAsync(id);
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
                Message = "Thong tin hoat dong not found";
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