using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IAdminService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> CreateAdmin([FromBody] Kdmadmin InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] Kdmadmin InputData);

        Task<IActionResult> Delete(long id);
    }

    public class AdminService : BaseController, IAdminService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    List<Kdmadmin> resultList = await context.Kdmadmins.ToListAsync();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Admin not found";
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
                List<Kdmadmin> resultList = new List<Kdmadmin>();
                using (var context = new MyDBContext())
                {
                    var query = await (from ac in context.Kdmadmins
                                       where ac.Idadmin == id
                                       select new Kdmadmin
                                       {
                                           Idadmin = ac.Idadmin,
                                           Username = ac.Username,
                                           AdminPassword = ac.AdminPassword,
                                       }).ToListAsync();
                    resultList = query.Select(item => new Kdmadmin
                    {
                        Idadmin = item.Idadmin,
                        Username = item.Username,
                        AdminPassword = item.AdminPassword,
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
                Message = "Admin not found";

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

        public async Task<IActionResult> CreateAdmin([FromBody] Kdmadmin InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    Kdmadmin newData = new Kdmadmin()
                    {
                        Idadmin = IdGenerator.NewUID,
                        Username = InputData.Username,
                        AdminPassword = PasswordGenerator.HashPassword(InputData.AdminPassword),
                    };
                    await context.Kdmadmins.AddAsync(newData);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(newData);
                    Message = "Admin created";
                    Code = 200;

                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Admin not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] Kdmadmin InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = await context.Kdmadmins.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.Username = InputData.Username;
                    existing.AdminPassword = PasswordGenerator.HashPassword(InputData.AdminPassword);
                    context.Kdmadmins.Update(existing);
                    await context.SaveChangesAsync();
                    DataObject.Clear();
                    DataObject.Add(existing);
                    Message = "Data Changed";
                    Code = 200;

                    return Ok(existing);
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Admin not found";
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
                    var data = await context.Kdmadmins.FindAsync(id);
                    if (data == null)
                    {
                        return Ok("data not exist");
                    }
                    context.Remove(data);
                    await context.SaveChangesAsync();
                    DataObject.Clear();
                    DataObject.Add(data);
                    Message = "Data deleted";
                    Code = 200;

                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Admin not found";
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