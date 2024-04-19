using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IXepLoaiService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetbyTenVanBan(string tenVanBan);

        Task<IActionResult> CreateXepLoai([FromBody] XepLoaiModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] XepLoaiModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class XepLoaiService : BaseController, IXepLoaiService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<XepLoaiModel> resultList = new List<XepLoaiModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from xl in context.KdmxepLoais
                                 join vb in context.KdmvanBans on xl.IdvanBan equals vb.IdvanBan

                                 select new XepLoaiModel
                                 {
                                     IdxepLoai = xl.IdxepLoai,
                                     TenvanBan = vb.TenVanBan,
                                     MaLoaiDrl = xl.MaLoaiDrl,
                                     Diem = xl.Diem,
                                     XepLoai = xl.XepLoai,
                                 }).ToList();
                    resultList = query.Select(item => new XepLoaiModel
                    {
                        IdxepLoai = item.IdxepLoai,
                        TenvanBan = item.TenvanBan,
                        MaLoaiDrl = item.MaLoaiDrl,
                        Diem = item.Diem,
                        XepLoai = item.XepLoai,
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
                Message = "Xep loai not found";
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

        public async Task<IActionResult> GetbyTenVanBan(string tenVanBan)
        {
            try
            {
                List<XepLoaiModel> resultList = new List<XepLoaiModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from xl in context.KdmxepLoais
                                 join vb in context.KdmvanBans on xl.IdvanBan equals vb.IdvanBan
                                 where vb.TenVanBan.Contains(tenVanBan)
                                 select new XepLoaiModel
                                 {
                                     IdxepLoai = xl.IdxepLoai,
                                     TenvanBan = vb.TenVanBan,
                                     MaLoaiDrl = xl.MaLoaiDrl,
                                     Diem = xl.Diem,
                                     XepLoai = xl.XepLoai,
                                 }).ToList();
                    resultList = query.Select(item => new XepLoaiModel
                    {
                        IdxepLoai = item.IdxepLoai,
                        TenvanBan = item.TenvanBan,
                        MaLoaiDrl = item.MaLoaiDrl,
                        Diem = item.Diem,
                        XepLoai = item.XepLoai,
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
                Message = "Xep loai not found";
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

        public async Task<IActionResult> CreateXepLoai([FromBody] XepLoaiModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idVanBan = context.KdmvanBans
                             .Where(vb => vb.TenVanBan == inputData.TenvanBan)
                             .Select(vb => vb.IdvanBan)
                             .FirstOrDefault();
                    KdmxepLoai newData = new KdmxepLoai()
                    {
                        IdxepLoai = IdGenerator.NewUID,
                        IdvanBan = idVanBan,
                        MaLoaiDrl = inputData.MaLoaiDrl,
                        Diem = inputData.Diem,
                        XepLoai = inputData.XepLoai,
                    };
                    context.KdmxepLoais.Add(newData);
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
                Message = "Xep loai not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] XepLoaiModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idVanBan = context.KdmvanBans
                            .Where(vb => vb.TenVanBan == inputData.TenvanBan)
                            .Select(vb => vb.IdvanBan)
                            .FirstOrDefault();

                    var existing = context.KdmxepLoais.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.IdvanBan = idVanBan;
                    existing.MaLoaiDrl = inputData.MaLoaiDrl;
                    existing.Diem = inputData.Diem;
                    existing.XepLoai = inputData.XepLoai;
                    context.KdmxepLoais.Update(existing);
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
                Message = "Xep loai not found";
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
                    var data = context.KdmxepLoais.Find(id);
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
                Message = "Xep loai not found";
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