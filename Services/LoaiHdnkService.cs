using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ILoaiHdnkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByMa(string maHdnk);

        Task<IActionResult> CreateLoaiHdnk([FromBody] KdmloaiHdnk inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] KdmloaiHdnk inputData);

        Task<IActionResult> Delete(long id);
    }

    public class LoaiHdnkService : BaseController, ILoaiHdnkService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<KdmloaiHdnk> resultList = new List<KdmloaiHdnk>();
                using (var context = new MyDBContext())
                {
                    var query = (from lhd in context.KdmloaiHdnks
                                 select new KdmloaiHdnk
                                 {
                                     IdloaiHdnk = lhd.IdloaiHdnk,
                                     MaLoaiHdnk = lhd.MaLoaiHdnk,
                                     NoiDungLoaiHdnk = lhd.NoiDungLoaiHdnk,
                                     DiemCong = lhd.DiemCong,
                                     DiemTru = lhd.DiemTru,
                                     DonViThucHien = lhd.DonViThucHien,
                                 }).ToList();
                    resultList = query.Select(item => new KdmloaiHdnk
                    {
                        IdloaiHdnk = item.IdloaiHdnk,
                        MaLoaiHdnk = item.MaLoaiHdnk,
                        NoiDungLoaiHdnk = item.NoiDungLoaiHdnk,
                        DiemCong = item.DiemCong,
                        DiemTru = item.DiemTru,
                        DonViThucHien = item.DonViThucHien,
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
                Message = "Loai hoat dong not found";
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

        public async Task<IActionResult> GetByMa(string maHdnk)
        {
            try
            {
                List<KdmloaiHdnk> resultList = new List<KdmloaiHdnk>();
                using (var context = new MyDBContext())
                {
                    var query = (from lhd in context.KdmloaiHdnks
                                 where lhd.MaLoaiHdnk == maHdnk
                                 select new KdmloaiHdnk
                                 {
                                     IdloaiHdnk = lhd.IdloaiHdnk,
                                     MaLoaiHdnk = lhd.MaLoaiHdnk,
                                     NoiDungLoaiHdnk = lhd.NoiDungLoaiHdnk,
                                     DiemCong = lhd.DiemCong,
                                     DiemTru = lhd.DiemTru,
                                     DonViThucHien = lhd.DonViThucHien,
                                 }).ToList();
                    resultList = query.Select(item => new KdmloaiHdnk
                    {
                        IdloaiHdnk = item.IdloaiHdnk,
                        MaLoaiHdnk = item.MaLoaiHdnk,
                        NoiDungLoaiHdnk = item.NoiDungLoaiHdnk,
                        DiemCong = item.DiemCong,
                        DiemTru = item.DiemTru,
                        DonViThucHien = item.DonViThucHien,
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
                Message = "Loai hoat dong not found";
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

        public async Task<IActionResult> CreateLoaiHdnk([FromBody] KdmloaiHdnk inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    KdmloaiHdnk newData = new KdmloaiHdnk()
                    {
                        IdloaiHdnk = IdGenerator.NewUID,
                        MaLoaiHdnk = inputData.MaLoaiHdnk,
                        NoiDungLoaiHdnk = inputData.NoiDungLoaiHdnk,
                        DiemCong = inputData.DiemCong,
                        DiemTru = inputData.DiemTru,
                        DonViThucHien = inputData.DonViThucHien,
                    };
                    context.KdmloaiHdnks.Add(newData);
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
                Message = "Loai hoat dong not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] KdmloaiHdnk inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.KdmloaiHdnks.Find(id);
                    if (existing == null)
                    {
                        return null;
                    }
                    existing.MaLoaiHdnk = inputData.MaLoaiHdnk;
                    existing.NoiDungLoaiHdnk = inputData.NoiDungLoaiHdnk;
                    existing.DiemCong = inputData.DiemCong;
                    existing.DiemTru = inputData.DiemTru;
                    existing.DonViThucHien = inputData.DonViThucHien;
                    context.KdmloaiHdnks.Update(existing);
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
                Message = "Loai hoat dong not found";
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
                    var data = context.KdmloaiHdnks.Find(id);
                    if (data == null)
                    {
                        return null;
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
                Message = "Loai hoat dong not found";
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