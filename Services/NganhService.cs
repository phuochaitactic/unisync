using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface INganhService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetByTenNganh(string TenNgh);

        Task<IActionResult> CreateNganh([FromBody] NganhModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] NganhModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class NganhService : BaseController, INganhService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<NganhModel> resultList = new List<NganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ngh in context.Kdmnghs
                                 join khoa in context.Ndmkhoas on ngh.Idkhoa equals khoa.Idkhoa
                                 select new NganhModel()
                                 {
                                     Idngh = ngh.Idngh,
                                     MaNgh = ngh.MaNgh,
                                     TenNgh = ngh.TenNgh,
                                     TenKhoa = khoa.TenKhoa,
                                 }).ToList();
                    resultList = query.Select(item => new NganhModel()
                    {
                        Idngh = item.Idngh,
                        MaNgh = item.MaNgh,
                        TenNgh = item.TenNgh,
                        TenKhoa = item.TenKhoa
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
                Message = "Nganh not found";
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
                List<NganhModel> resultList = new List<NganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ngh in context.Kdmnghs
                                 join khoa in context.Ndmkhoas on ngh.Idkhoa equals khoa.Idkhoa
                                 where ngh.Idngh == id
                                 select new NganhModel()
                                 {
                                     Idngh = ngh.Idngh,
                                     MaNgh = ngh.MaNgh,
                                     TenNgh = ngh.TenNgh,
                                     TenKhoa = khoa.TenKhoa
                                 }).ToList();
                    resultList = query.Select(item => new NganhModel()
                    {
                        Idngh = item.Idngh,
                        MaNgh = item.MaNgh,
                        TenNgh = item.TenNgh,
                        TenKhoa = item.TenKhoa
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
                Message = "Nganh not found";
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

        public async Task<IActionResult> GetByTenNganh(string TenNgh)
        {
            try
            {
                List<NganhModel> resultList = new List<NganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from ngh in context.Kdmnghs
                                 join khoa in context.Ndmkhoas on ngh.Idkhoa equals khoa.Idkhoa
                                 where ngh.TenNgh.Contains(TenNgh)
                                 select new NganhModel()
                                 {
                                     Idngh = ngh.Idngh,
                                     MaNgh = ngh.MaNgh,
                                     TenNgh = ngh.TenNgh,
                                     TenKhoa = khoa.TenKhoa,
                                 }).ToList();
                    resultList = query.Select(item => new NganhModel()
                    {
                        Idngh = item.Idngh,
                        MaNgh = item.MaNgh,
                        TenNgh = item.TenNgh,
                        TenKhoa = item.TenKhoa
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
                Message = "Nganh not found";
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

        public async Task<IActionResult> CreateNganh([FromBody] NganhModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idKhoa = context.Ndmkhoas
                               .Where(khoa => khoa.TenKhoa == inputData.TenKhoa)
                               .Select(khoa => khoa.Idkhoa)
                               .FirstOrDefault();

                    Kdmngh newData = new Kdmngh()
                    {
                        Idngh = IdGenerator.NewUID,
                        MaNgh = inputData.MaNgh,
                        TenNgh = inputData.TenNgh,
                        Idkhoa = idKhoa
                    };
                    context.Kdmnghs.Add(newData);
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
                Message = "Nganh not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] NganhModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idKhoa = context.Ndmkhoas
                          .Where(khoa => khoa.TenKhoa == inputData.TenKhoa)
                          .Select(khoa => khoa.Idkhoa)
                          .FirstOrDefault();
                    var existing = context.Kdmnghs.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.MaNgh = inputData.MaNgh;
                    existing.TenNgh = inputData.TenNgh;
                    existing.Idkhoa = idKhoa;
                    context.Kdmnghs.Update(existing);
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
                Message = "Nganh not found";
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
                    var data = context.Kdmnghs.Find(id);
                    if (data == null)
                    {
                        return Ok("data not exist");
                    }
                    context.Remove(data);
                    context.SaveChangesAsync();
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
                Message = "Nganh not found";
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