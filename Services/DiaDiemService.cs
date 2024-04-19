using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IDiaDiemService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> CreateDiaDiem([FromBody] PdmdiaDiem InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] PdmdiaDiem InputData);

        Task<IActionResult> Delete(long id);
    }

    public class DiaDiemService : BaseController, IDiaDiemService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    List<PdmdiaDiem> resultList = await context.PdmdiaDiems.ToListAsync();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Dia diem not found";
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
                List<PdmdiaDiem> resultList = new List<PdmdiaDiem>();
                using (var context = new MyDBContext())
                {
                    var query = (from dd in context.PdmdiaDiems
                                 where dd.IddiaDiem == id
                                 select new PdmdiaDiem
                                 {
                                     IddiaDiem = dd.IddiaDiem,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     DiaChi = dd.DiaChi,
                                 }).ToList();
                    resultList = (List<PdmdiaDiem>)query.Select(item => new PdmdiaDiem
                    {
                        IddiaDiem = item.IddiaDiem,
                        TenDiaDiem = item.TenDiaDiem,
                        DiaChi = item.DiaChi,
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
                Message = "Dia diem not found";
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

        public async Task<IActionResult> CreateDiaDiem([FromBody] PdmdiaDiem InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    PdmdiaDiem newData = new PdmdiaDiem()
                    {
                        IddiaDiem = IdGenerator.NewUID,
                        TenDiaDiem = InputData.TenDiaDiem,
                        DiaChi = InputData.DiaChi,
                    };
                    var isDiaDiem = context.PdmdiaDiems
                        .Where(dd => dd.TenDiaDiem == InputData.TenDiaDiem)
                        .Select(dd => dd)
                        .FirstOrDefault();
                    if (isDiaDiem != null)
                    {
                        Code = 500;
                        Message = "địa điểm đã tồn tại";
                        return CreateResponse();
                    }
                    context.PdmdiaDiems.Add(newData);
                    await context.SaveChangesAsync();
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
                Message = "Dia diem not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] PdmdiaDiem InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.PdmdiaDiems.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.TenDiaDiem = InputData.TenDiaDiem;
                    existing.DiaChi = InputData.DiaChi;

                    context.PdmdiaDiems.Update(existing);
                    await context.SaveChangesAsync();
                    DataObject.Clear();
                    DataObject.Add(existing);
                    Message = "Data Changed"; Code = 200;

                    return Ok(existing);
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Dia diem not found";
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
                    var data = context.PdmdiaDiems.Find(id);
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
                Message = "Dia diem not found";
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