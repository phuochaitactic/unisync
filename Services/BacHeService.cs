using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IBacHeService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetByTenBacHe(string TenBH);

        Task<IActionResult> CreateBacHe([FromBody] Kdmbh InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] Kdmbh InputData);

        Task<IActionResult> Delete(long id);
    }
}

public class BacHeService : BaseController, IBacHeService
{
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<Kdmbh> resultList = new List<Kdmbh>();
            using (var context = new MyDBContext())
            {
                resultList = await context.Kdmbhs.ToListAsync();
                DataObject = resultList.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
        }
        catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
        {
            // ID not found
            Code = 404;
            Message = "bac he not found";
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
            List<Kdmbh> resultList = new List<Kdmbh>();
            using (var context = new MyDBContext())
            {
                var query = await (from ac in context.Kdmbhs
                                   where ac.Idbh == id
                                   select new Kdmbh
                                   {
                                       Idbh = ac.Idbh,
                                       MaBh = ac.MaBh,
                                       TenBh = ac.TenBh,
                                   }).ToListAsync();
                resultList = query.Select(item => new Kdmbh
                {
                    Idbh = item.Idbh,
                    MaBh = item.MaBh,
                    TenBh = item.TenBh,
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
            Message = "bac he not found";
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

    public async Task<IActionResult> GetByTenBacHe(string TenBH)
    {
        try
        {
            List<Kdmbh> resultList = new List<Kdmbh>();
            using (var context = new MyDBContext())
            {
                var query = (from bh in context.Kdmbhs
                             where bh.TenBh.Contains(TenBH)
                             select new Kdmbh
                             {
                                 Idbh = bh.Idbh,
                                 MaBh = bh.MaBh,
                                 TenBh = bh.TenBh,
                             }).ToList();
                resultList = query.Select(item => new Kdmbh()
                {
                    Idbh = item.Idbh,
                    MaBh = item.MaBh,
                    TenBh = item.TenBh,
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
            Message = "bac he not found";
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

    public async Task<IActionResult> CreateBacHe([FromBody] Kdmbh inputData)
    {
        try
        {
            using (var context = new MyDBContext())
            {
                Kdmbh newData = new Kdmbh()
                {
                    Idbh = IdGenerator.NewUID,
                    MaBh = inputData.MaBh,
                    TenBh = inputData.TenBh,
                };
                await context.Kdmbhs.AddAsync(newData);
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
            Message = "bac he not found";
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

    public async Task<IActionResult> ChangeData(long id, [FromBody] Kdmbh inputData)
    {
        try
        {
            using (var context = new MyDBContext())
            {
                var existing = await context.Kdmbhs.FindAsync(id);
                if (existing == null)
                {
                    return NotFound();
                }
                existing.TenBh = inputData.TenBh;
                existing.MaBh = inputData.MaBh;
                context.Kdmbhs.Update(existing);
                await context.SaveChangesAsync();
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
            Message = "bac he not found";
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
                var data = await context.Kdmbhs.FindAsync(id);
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
            Message = "bac he not found";
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