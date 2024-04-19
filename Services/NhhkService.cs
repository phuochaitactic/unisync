using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface INhhkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetByMa(long MaNHHK);

        Task<IActionResult> GetByTen(string TenNhhk);

        Task<IActionResult> CreateNganh([FromBody] Kdmnhhk InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] Kdmnhhk InputData);

        Task<IActionResult> Delete(long id);
    }

    public class NhhkService : BaseController, INhhkService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var resultList = await context.Kdmnhhks.ToListAsync();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Nam hoc hoc ky not found";
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
                List<Kdmnhhk> resultList = new List<Kdmnhhk>();
                using (var context = new MyDBContext())
                {
                    var query = (from nhhk in context.Kdmnhhks
                                 where nhhk.Idnhhk == id
                                 select new Kdmnhhk
                                 {
                                     Idnhhk = nhhk.Idnhhk,
                                     MaNhhk = nhhk.MaNhhk,
                                     TenNhhk = nhhk.TenNhhk,
                                     NgayBatDau = nhhk.NgayBatDau,
                                     TuanBatDau = nhhk.TuanBatDau,
                                     SoTuanHk = nhhk.SoTuanHk,
                                     NgayKetThuc = nhhk.NgayKetThuc
                                 }).ToList();
                    resultList = (List<Kdmnhhk>)query.Select(item => new Kdmnhhk
                    {
                        Idnhhk = item.Idnhhk,
                        MaNhhk = item.MaNhhk,
                        TenNhhk = item.TenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        TuanBatDau = item.TuanBatDau,
                        SoTuanHk = item.SoTuanHk,
                        NgayKetThuc = item.NgayKetThuc,
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
                Message = "Nam hoc hoc ky not found";
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

        public async Task<IActionResult> GetByMa(long MaNHHK)
        {
            try
            {
                List<Kdmnhhk> resultList = new List<Kdmnhhk>();
                using (var context = new MyDBContext())
                {
                    var query = (from nhhk in context.Kdmnhhks
                                 where nhhk.MaNhhk == MaNHHK
                                 select new Kdmnhhk
                                 {
                                     Idnhhk = nhhk.Idnhhk,
                                     MaNhhk = nhhk.MaNhhk,
                                     TenNhhk = nhhk.TenNhhk,
                                     NgayBatDau = nhhk.NgayBatDau,
                                     TuanBatDau = nhhk.TuanBatDau,
                                     SoTuanHk = nhhk.SoTuanHk,
                                     NgayKetThuc = nhhk.NgayKetThuc,
                                 }).ToList();
                    resultList = (List<Kdmnhhk>)query.Select(item => new Kdmnhhk
                    {
                        Idnhhk = item.Idnhhk,
                        MaNhhk = item.MaNhhk,
                        TenNhhk = item.TenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        TuanBatDau = item.TuanBatDau,
                        SoTuanHk = item.SoTuanHk,
                        NgayKetThuc = item.NgayKetThuc,
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
                Message = "Nam hoc hoc ky not found";
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

        public async Task<IActionResult> GetByTen(string TenNhhk)
        {
            try
            {
                List<Kdmnhhk> resultList = new List<Kdmnhhk>();
                using (var context = new MyDBContext())
                {
                    var query = (from nhhk in context.Kdmnhhks
                                 where nhhk.TenNhhk == TenNhhk
                                 select new Kdmnhhk
                                 {
                                     Idnhhk = nhhk.Idnhhk,
                                     MaNhhk = nhhk.MaNhhk,
                                     TenNhhk = nhhk.TenNhhk,
                                     NgayBatDau = nhhk.NgayBatDau,
                                     TuanBatDau = nhhk.TuanBatDau,
                                     SoTuanHk = nhhk.SoTuanHk,
                                     NgayKetThuc = nhhk.NgayKetThuc
                                 }).ToList();
                    resultList = (List<Kdmnhhk>)query.Select(item => new Kdmnhhk
                    {
                        Idnhhk = item.Idnhhk,
                        MaNhhk = item.MaNhhk,
                        TenNhhk = item.TenNhhk,
                        NgayBatDau = item.NgayBatDau,
                        TuanBatDau = item.TuanBatDau,
                        SoTuanHk = item.SoTuanHk,
                        NgayKetThuc = item.NgayKetThuc,
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
                Message = "Nam hoc hoc ky not found";
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

        public async Task<IActionResult> CreateNganh([FromBody] Kdmnhhk InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    Kdmnhhk newData = new Kdmnhhk()
                    {
                        Idnhhk = IdGenerator.NewUID,
                        MaNhhk = InputData.MaNhhk,
                        TenNhhk = InputData.TenNhhk,
                        NgayBatDau = InputData.NgayBatDau,
                        TuanBatDau = InputData.TuanBatDau,
                        SoTuanHk = InputData.SoTuanHk,
                        NgayKetThuc = InputData.NgayKetThuc,
                    };
                    context.Kdmnhhks.Add(newData);
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
                Message = "Nam hoc hoc ky not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] Kdmnhhk InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.Kdmnhhks.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.MaNhhk = InputData.MaNhhk;
                    existing.TenNhhk = InputData.TenNhhk;
                    existing.NgayBatDau = InputData.NgayBatDau;
                    existing.TuanBatDau = InputData.TuanBatDau;
                    existing.SoTuanHk = InputData.SoTuanHk;
                    existing.NgayKetThuc = InputData.NgayKetThuc;

                    context.Kdmnhhks.Update(existing);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(existing);
                    Message = "Data Changed"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Nam hoc hoc ky not found";
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
                    var data = context.Kdmnhhks.Find(id);
                    if (data == null)
                    {
                        return Ok("data not exist");
                    }
                    context.Remove(data);
                    context.SaveChanges();
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
                Message = "Nam hoc hoc ky not found";
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