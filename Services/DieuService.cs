using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IDieuService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> CreateDieu([FromBody] DieuModel InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] DieuModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class DieuService : BaseController, IDieuService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<DieuModel> resultList = new List<DieuModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from dd in context.Kdmdieus
                                 join vb in context.KdmvanBans on dd.IdvanBan equals vb.IdvanBan
                                 select new DieuModel
                                 {
                                     Iddieu = dd.Iddieu,
                                     TenVanBan = vb.TenVanBan,
                                     MaDieu = dd.MaDieu,
                                     NoiDung = dd.NoiDung,
                                     DiemCoBan = dd.DiemCoBan,
                                     DiemToiDa = dd.DiemToiDa,
                                 }).ToList();
                    resultList = (List<DieuModel>)query.Select(item => new DieuModel
                    {
                        Iddieu = item.Iddieu,
                        TenVanBan = item.TenVanBan,
                        MaDieu = item.MaDieu,
                        NoiDung = item.NoiDung,
                        DiemCoBan = item.DiemCoBan,
                        DiemToiDa = item.DiemToiDa,
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
                Message = ex.Message;
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
                List<DieuModel> resultList = new List<DieuModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from dd in context.Kdmdieus
                                 join vb in context.KdmvanBans on dd.IdvanBan equals vb.IdvanBan
                                 where dd.Iddieu == id
                                 select new DieuModel
                                 {
                                     Iddieu = dd.Iddieu,
                                     TenVanBan = vb.TenVanBan,
                                     MaDieu = dd.MaDieu,
                                     NoiDung = dd.NoiDung,
                                     DiemCoBan = dd.DiemCoBan,
                                     DiemToiDa = dd.DiemToiDa,
                                 }).ToList();
                    resultList = (List<DieuModel>)query.Select(item => new DieuModel
                    {
                        Iddieu = item.Iddieu,
                        TenVanBan = item.TenVanBan,
                        MaDieu = item.MaDieu,
                        NoiDung = item.NoiDung,
                        DiemCoBan = item.DiemCoBan,
                        DiemToiDa = item.DiemToiDa,
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
                Message = ex.Message;
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

        public async Task<IActionResult> CreateDieu([FromBody] DieuModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idVanBan = context.KdmvanBans
                             .Where(vb => vb.TenVanBan == inputData
                             .TenVanBan)
                             .Select(vb => vb.IdvanBan)
                             .FirstOrDefault();
                    var isDieu = context.Kdmdieus
                      .Where(dieu => dieu.MaDieu == inputData.MaDieu)
                      .Select(dieu => dieu)
                      .FirstOrDefault();
                    if (isDieu != null)
                    {
                        Code = 500;
                        Message = "Điều đã tồn tại";
                        return CreateResponse();
                    }
                    Kdmdieu newData = new Kdmdieu()
                    {
                        Iddieu = IdGenerator.NewUID,
                        IdvanBan = idVanBan,
                        MaDieu = inputData.MaDieu,
                        NoiDung = inputData.NoiDung,
                        DiemCoBan = inputData.DiemCoBan,
                        DiemToiDa = inputData.DiemToiDa,
                    };
                    context.Kdmdieus.Add(newData);
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
                Message = ex.Message;
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] DieuModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idVanBan = context.KdmvanBans
                             .Where(vb => vb.TenVanBan == inputData.TenVanBan
                             )
                             .Select(vb => vb.IdvanBan)
                             .FirstOrDefault();
                    var existing = context.Kdmdieus.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.IdvanBan = idVanBan;
                    existing.MaDieu = inputData.MaDieu;
                    existing.NoiDung = inputData.NoiDung;
                    existing.DiemCoBan = inputData.DiemCoBan;
                    existing.DiemToiDa = inputData.DiemToiDa;
                    context.Kdmdieus.Update(existing);
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
                Message = ex.Message;
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
                    var data = context.Kdmdieus.Find(id);
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
                Message = ex.Message;
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