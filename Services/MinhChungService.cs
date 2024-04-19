using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IMinhChungService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByMaDieu(string maDieu);

        Task<IActionResult> GetByMaLoaiHdnk(string maLoaiHdnk);

        Task<IActionResult> CreateMinhChung([FromBody] MinhChungModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] MinhChungModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class MinhChungService : BaseController, IMinhChungService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<MinhChungModel> resultList = new List<MinhChungModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from mc in context.KdmndminhChungs
                                 join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu
                                 join lhd in context.KdmloaiHdnks on mc.IdloaiHdnk equals lhd.IdloaiHdnk
                                 select new MinhChungModel
                                 {
                                     IdminhChung = mc.IdminhChung,
                                     MaLoaiHdnk = lhd.MaLoaiHdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     MaDieu = dieu.MaDieu
                                 }).ToList();
                    resultList = query.Select(item => new MinhChungModel
                    {
                        IdminhChung = item.IdminhChung,
                        MaLoaiHdnk = item.MaLoaiHdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        MaDieu = item.MaDieu
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
                Message = "Minh chung not found";
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

        public async Task<IActionResult> GetByMaDieu(string maDieu)
        {
            try
            {
                List<MinhChungModel> resultList = new List<MinhChungModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from mc in context.KdmndminhChungs
                                 join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu
                                 join lhd in context.KdmloaiHdnks on mc.IdloaiHdnk equals lhd.IdloaiHdnk
                                 where dieu.MaDieu == maDieu
                                 select new MinhChungModel
                                 {
                                     IdminhChung = mc.IdminhChung,
                                     MaLoaiHdnk = lhd.MaLoaiHdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     MaDieu = dieu.MaDieu
                                 }).ToList();
                    resultList = query.Select(item => new MinhChungModel
                    {
                        IdminhChung = item.IdminhChung,
                        MaLoaiHdnk = item.MaLoaiHdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        MaDieu = item.MaDieu
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
                Message = "Minh chung not found";
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

        public async Task<IActionResult> GetByMaLoaiHdnk(string maLoaiHdnk)
        {
            try
            {
                List<MinhChungModel> resultList = new List<MinhChungModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from mc in context.KdmndminhChungs
                                 join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu
                                 join lhd in context.KdmloaiHdnks on mc.IdloaiHdnk equals lhd.IdloaiHdnk
                                 where lhd.MaLoaiHdnk == maLoaiHdnk
                                 select new MinhChungModel
                                 {
                                     IdminhChung = mc.IdminhChung,
                                     MaLoaiHdnk = lhd.MaLoaiHdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     MaDieu = dieu.MaDieu
                                 }).ToList();
                    resultList = query.Select(item => new MinhChungModel
                    {
                        IdminhChung = item.IdminhChung,
                        MaLoaiHdnk = item.MaLoaiHdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        MaDieu = item.MaDieu
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
                Message = "Minh chung not found";
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

        public async Task<IActionResult> CreateMinhChung([FromBody] MinhChungModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idDieu = context.Kdmdieus
                             .Where(dieu => dieu.MaDieu == inputData.MaDieu)
                             .Select(dieu => dieu.Iddieu)
                             .FirstOrDefault();
                    long idLoaiHdnk = context.KdmloaiHdnks
                        .Where(lhd => lhd.MaLoaiHdnk == inputData.MaLoaiHdnk)
                        .Select(lhd => lhd.IdloaiHdnk)
                        .FirstOrDefault();
                    KdmndminhChung newData = new KdmndminhChung()
                    {
                        IdminhChung = IdGenerator.NewUID,
                        Iddieu = idDieu,
                        IdloaiHdnk = idLoaiHdnk,
                        NoiDungMinhChung = inputData.NoiDungMinhChung,
                    };
                    context.KdmndminhChungs.Add(newData);
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
                Message = "Minh chung not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] MinhChungModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idDieu = context.Kdmdieus
                             .Where(dieu => dieu.MaDieu == inputData.MaDieu)
                             .Select(dieu => dieu.Iddieu)
                             .FirstOrDefault();
                    long idLoaiHdnk = context.KdmloaiHdnks
                        .Where(lhd => lhd.MaLoaiHdnk == inputData.MaLoaiHdnk)
                        .Select(lhd => lhd.IdloaiHdnk)
                        .FirstOrDefault();
                    var existing = context.KdmndminhChungs.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.Iddieu = idDieu;
                    existing.IdloaiHdnk = idLoaiHdnk;
                    existing.NoiDungMinhChung = inputData.NoiDungMinhChung;
                    context.KdmndminhChungs.Update(existing);
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
                Message = "Minh chung not found";
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
                    var data = await context.KdmndminhChungs.FindAsync(id);
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
                Message = "Minh chung not found";
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