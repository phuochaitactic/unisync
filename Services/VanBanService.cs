using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IVanBanService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> CreateVanBan([FromBody] KdmvanBan inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] KdmvanBan inputData);

        Task<IActionResult> Delete(long id);
    }

    public class VanBanService : BaseController, IVanBanService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    List<KdmvanBan> data = await context.KdmvanBans.ToListAsync();
                    DataObject = data.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Van ban not found";
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
                List<KdmvanBan> resultList = new List<KdmvanBan>();
                using (var context = new MyDBContext())
                {
                    var query = (from vb in context.KdmvanBans
                                 where vb.IdvanBan == id
                                 select new KdmvanBan
                                 {
                                     IdvanBan = vb.IdvanBan,
                                     MaVanBan = vb.MaVanBan,
                                     TenVanBan = vb.TenVanBan,
                                     Link = vb.Link,
                                 }).ToList();
                    resultList = query.Select(item => new KdmvanBan
                    {
                        IdvanBan = item.IdvanBan,
                        MaVanBan = item.MaVanBan,
                        TenVanBan = item.TenVanBan,
                        Link = item.Link,
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
                Message = "Van ban not found";
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

        public async Task<IActionResult> CreateVanBan([FromBody] KdmvanBan inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    KdmvanBan newData = new KdmvanBan()
                    {
                        IdvanBan = IdGenerator.NewUID,
                        MaVanBan = inputData.MaVanBan,
                        TenVanBan = inputData.TenVanBan,
                        Link = inputData.Link,
                    };
                    context.KdmvanBans.Add(newData);
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
                Message = "Van ban not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] KdmvanBan inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.KdmvanBans.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.MaVanBan = inputData.MaVanBan;
                    existing.TenVanBan = inputData.TenVanBan;
                    existing.Link = inputData.Link;
                    context.KdmvanBans.Update(existing);
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
                Message = "Van ban not found";
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
            return CreateResponse();
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var data = context.KdmvanBans.Find(id);
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
                Message = "Van ban not found";
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