using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IBacHeNganhService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetByMaBacHeNganh(string MaBhNgh);

        Task<IActionResult> GetBacHeNganhTheoKhoa(string TenKhoa);

        Task<IActionResult> CreateBacHeNganh([FromBody] BacHeNganhModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] BacHeNganhModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class BacHeNganhService : BaseController, IBacHeNganhService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<BacHeNganhModel> resultList = new List<BacHeNganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from bhngh in context.KdmbhngChngs
                                 join bh in context.Kdmbhs on bhngh.Idbh equals bh.Idbh
                                 join ngh in context.Kdmnghs on bhngh.Idngh equals ngh.Idngh
                                 select new BacHeNganhModel
                                 {
                                     IdbhngChng = bhngh.IdbhngChng,
                                     MaBhngChng = bhngh.MaBhngChng,
                                     TenBh = bh.TenBh,
                                     TenNganh = ngh.TenNgh,
                                     TenBhngChng = bhngh.TenBhngChng,
                                 }).ToList();
                    resultList = query.Select(item => new BacHeNganhModel()
                    {
                        IdbhngChng = item.IdbhngChng,
                        MaBhngChng = item.MaBhngChng,
                        TenBh = item.TenBh,
                        TenNganh = item.TenNganh,
                        TenBhngChng = item.TenBhngChng,
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
                Message = "bac he nganh not found";
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
                List<BacHeNganhModel> resultList = new List<BacHeNganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from bhngh in context.KdmbhngChngs
                                 join bh in context.Kdmbhs on bhngh.Idbh equals bh.Idbh
                                 join ngh in context.Kdmnghs on bhngh.Idngh equals ngh.Idngh
                                 where bhngh.IdbhngChng == id
                                 select new BacHeNganhModel
                                 {
                                     IdbhngChng = bhngh.IdbhngChng,
                                     MaBhngChng = bhngh.MaBhngChng,
                                     TenBh = bh.TenBh,
                                     TenNganh = ngh.TenNgh,
                                     TenBhngChng = bhngh.TenBhngChng,
                                 }).ToList();
                    resultList = query.Select(item => new BacHeNganhModel()
                    {
                        IdbhngChng = item.IdbhngChng,
                        MaBhngChng = item.MaBhngChng,
                        TenBh = item.TenBh,
                        TenNganh = item.TenNganh,
                        TenBhngChng = item.TenBhngChng,
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
                Message = "bac he nganh not found";
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

        public async Task<IActionResult> GetBacHeNganhTheoKhoa(string TenKhoa)
        {
            try
            {
                List<BacHeNganhTheoKhoaModel> resultList = new List<BacHeNganhTheoKhoaModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from bhn in context.KdmbhngChngs
                                 join ng in context.Kdmnghs on bhn.Idngh equals ng.Idngh
                                 join khoa in context.Ndmkhoas on ng.Idkhoa equals khoa.Idkhoa
                                 where khoa.TenKhoa.Contains(TenKhoa)
                                 group new { bhn.MaBhngChng, bhn.TenBhngChng } by new { bhn.MaBhngChng, bhn.TenBhngChng } into g
                                 select new BacHeNganhTheoKhoaModel
                                 {
                                     MaBhngChng = g.Key.MaBhngChng,
                                     TenBh = g.Key.TenBhngChng,
                                 }).ToList();

                    resultList = query.Select(item => new BacHeNganhTheoKhoaModel
                    {
                        MaBhngChng = item.MaBhngChng,
                        TenBh = item.TenBh,
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
                Message = "bac he nganh not found";
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

        public async Task<IActionResult> GetByMaBacHeNganh(string MaBhNgh)
        {
            try
            {
                List<BacHeNganhModel> resultList = new List<BacHeNganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from bhngh in context.KdmbhngChngs
                                 join bh in context.Kdmbhs on bhngh.Idbh equals bh.Idbh
                                 join ngh in context.Kdmnghs on bhngh.Idngh equals ngh.Idngh
                                 where bhngh.MaBhngChng.Contains(MaBhNgh)
                                 select new BacHeNganhModel
                                 {
                                     IdbhngChng = bhngh.IdbhngChng,
                                     MaBhngChng = bhngh.MaBhngChng,
                                     TenBh = bh.TenBh,
                                     TenNganh = ngh.TenNgh,
                                     TenBhngChng = bhngh.TenBhngChng,
                                 }).ToList();
                    resultList = query.Select(item => new BacHeNganhModel()
                    {
                        IdbhngChng = item.IdbhngChng,
                        MaBhngChng = item.MaBhngChng,
                        TenBh = item.TenBh,
                        TenNganh = item.TenNganh,
                        TenBhngChng = item.TenBhngChng,
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
                Message = "bac he nganh not found";
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

        public async Task<IActionResult> CreateBacHeNganh([FromBody] BacHeNganhModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idBacHe = context.Kdmbhs
                             .Where(bh => bh.TenBh == inputData.TenBh)
                             .Select(bh => bh.Idbh)
                             .FirstOrDefault();
                    long idNganh = context.Kdmnghs
                        .Where(bh => bh.TenNgh == inputData.TenNganh)
                        .Select(bh => bh.Idngh)
                        .FirstOrDefault();
                    if (idNganh == null || idBacHe == null)
                    {
                        Code = 500;
                        Message = "invalid Input Nganh or BacHe";
                        return CreateResponse();
                    }
                    KdmbhngChng newData = new KdmbhngChng()
                    {
                        IdbhngChng = IdGenerator.NewUID,
                        MaBhngChng = inputData.MaBhngChng,
                        Idbh = idBacHe,
                        Idngh = idNganh,
                        TenBhngChng = inputData.TenBh + " " + inputData.TenNganh
                    };
                    context.KdmbhngChngs.Add(newData);
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
                Message = "bac he nganh not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] BacHeNganhModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idBacHe = context.Kdmbhs
                            .Where(bh => bh.TenBh == inputData.TenBh)
                            .Select(bh => bh.Idbh)
                            .FirstOrDefault();
                    long idNganh = context.Kdmnghs
                        .Where(bh => bh.TenNgh == inputData.TenNganh)
                        .Select(bh => bh.Idngh)
                        .FirstOrDefault();
                    var existing = context.KdmbhngChngs.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    if (idNganh == 0 || idBacHe == 0)
                    {
                        Code = 500;
                        Message = "invalid Input Nganh or BacHe";
                        return CreateResponse();
                    }
                    existing.MaBhngChng = inputData.MaBhngChng;
                    existing.Idngh = idNganh;
                    existing.Idbh = idBacHe;
                    context.KdmbhngChngs.Update(existing);
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
                Message = "bac he nganh not found";
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
                    var data = context.KdmbhngChngs.Find(id);
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
                Message = "bac he nganh not found";
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