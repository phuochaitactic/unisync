using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IDuLieuHdnkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long idDuLieu);

        Task<IActionResult> CreateDuLieu([FromBody] DuLieuHdnkModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] KdmduLieuHdnk inputData);

        Task<IActionResult> Delete(long id);
    }

    public class DuLieuHdnkService : BaseController, IDuLieuHdnkService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<DuLieuHdnkModel> resultList = new List<DuLieuHdnkModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from dl in context.KdmduLieuHdnks
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk
                                 join nh in context.Kdmnhhks on dl.Idnhhk equals nh.Idnhhk
                                 select new DuLieuHdnkModel
                                 {
                                     IdduLieuHdnk = dl.IdduLieuHdnk,
                                     tenBhngh = bhn.TenBhngChng,
                                     tenHdnk = hd.TenHdnk,
                                     tenNhhk = nh.TenNhhk,
                                 }).ToList();
                    resultList = query.Select(item => new DuLieuHdnkModel()
                    {
                        IdduLieuHdnk = item.IdduLieuHdnk,
                        tenBhngh = item.tenBhngh,
                        tenHdnk = item.tenHdnk,
                        tenNhhk = item.tenNhhk,
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
                Message = "du lieu not found";
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

        public async Task<IActionResult> GetById(long idDuLieu)
        {
            try
            {
                List<DuLieuHdnkModel> resultList = new List<DuLieuHdnkModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from dl in context.KdmduLieuHdnks
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk
                                 join nh in context.Kdmnhhks on dl.Idnhhk equals nh.Idnhhk
                                 where dl.IdduLieuHdnk == idDuLieu
                                 select new DuLieuHdnkModel
                                 {
                                     IdduLieuHdnk = dl.IdduLieuHdnk,
                                     tenBhngh = bhn.TenBhngChng,
                                     tenHdnk = hd.TenHdnk,
                                     tenNhhk = nh.TenNhhk,
                                 }).ToList();
                    resultList = query.Select(item => new DuLieuHdnkModel()
                    {
                        IdduLieuHdnk = item.IdduLieuHdnk,
                        tenBhngh = item.tenBhngh,
                        tenHdnk = item.tenHdnk,
                        tenNhhk = item.tenNhhk,
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
                Message = "du lieu not found";
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

        public async Task<IActionResult> CreateDuLieu([FromBody] DuLieuHdnkModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idBhngh = context.KdmbhngChngs
                          .Where(bhng => bhng.TenBhngChng == inputData.tenBhngh)
                          .Select(bhng => bhng.IdbhngChng)
                          .FirstOrDefault();
                    long Idhdnk = context.Kdmhdnks
                   .Where(hd => hd.TenHdnk == inputData.tenHdnk)
                   .Select(hd => hd.Idhdnk)
                   .FirstOrDefault();
                    long Idnhhk = context.Kdmnhhks
                   .Where(bhng => bhng.TenNhhk == inputData.tenNhhk)
                   .Select(bhng => bhng.Idnhhk)
                   .FirstOrDefault();
                    KdmduLieuHdnk newData = new KdmduLieuHdnk()
                    {
                        IdduLieuHdnk = IdGenerator.NewUID,
                        IdbhngChng = idBhngh,
                        Idhdnk = Idhdnk,
                        Idnhhk = Idnhhk,
                    };
                    context.KdmduLieuHdnks.Add(newData);
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
                Message = "du lieu not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] KdmduLieuHdnk inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.KdmduLieuHdnks.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.IdbhngChng = inputData.IdbhngChng;
                    existing.Idhdnk = inputData.Idhdnk;
                    existing.Idnhhk = inputData.Idnhhk;
                    context.KdmduLieuHdnks.Update(existing);
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
                Message = "du lieu not found";
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
                    var data = context.KdmduLieuHdnks.Find(id);
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
                Message = "du lieu not found";
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