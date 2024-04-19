using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ILopService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetLopByMaLop(string MaLop);

        Task<IActionResult> GetLopByMaNhanVien(string MaNhanVien);

        Task<IActionResult> GetLopByTenLop(string TenLop);

        Task<IActionResult> GetLopByNamVao(string NienKhoa);

        Task<IActionResult> GetLopByKhoa(string TenKhoa);

        Task<IActionResult> GetLopByBacHeNganh(string TenBacHeNganh);

        Task<IActionResult> CreateLop([FromBody] LopTableModel InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] LopTableModel InputData);

        Task<IActionResult> Delete(long id);
    }

    public class LopService : BaseController, ILopService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from lop in context.Sdmlops
                                 join bhng in context.KdmbhngChngs on lop.IdbhngChng equals bhng.IdbhngChng

                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa
                                 select new LopTableModel
                                 {
                                     Idlop = lop.Idlop,
                                     MaLop = lop.MaLop,
                                     TenLop = lop.TenLop,
                                     NamVao = lop.NamVao,
                                     TenKhoa = khoa.TenKhoa,
                                     TenBhngChng = bhng.TenBhngChng,
                                     NienKhoa = lop.NienKhoa,
                                 }).ToList();
                    resultList = (List<LopTableModel>)query.Select(item => new LopTableModel
                    {
                        Idlop = item.Idlop,
                        MaLop = item.MaLop,
                        TenLop = item.TenLop,
                        NamVao = item.NamVao,
                        TenKhoa = item.TenKhoa,
                        TenBhngChng = item.TenBhngChng,
                        NienKhoa = item.NienKhoa,
                        TenNhhk = item.TenNhhk
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
                Message = "Lop not found";
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

        public async Task<IActionResult> GetLopByMaLop(string MaLop)
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from lop in context.Sdmlops
                                 join bhng in context.KdmbhngChngs on lop.IdbhngChng equals bhng.IdbhngChng
                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa
                                 where lop.MaLop.Contains(MaLop)
                                 select new LopTableModel
                                 {
                                     Idlop = lop.Idlop,
                                     MaLop = lop.MaLop,
                                     TenLop = lop.TenLop,
                                     NamVao = lop.NamVao,
                                     TenKhoa = khoa.TenKhoa,
                                     TenBhngChng = bhng.TenBhngChng,
                                     NienKhoa = lop.NienKhoa,
                                 }).ToList();
                    resultList = (List<LopTableModel>)query.Select(item => new LopTableModel
                    {
                        Idlop = item.Idlop,
                        MaLop = item.MaLop,
                        TenLop = item.TenLop,
                        NamVao = item.NamVao,
                        TenKhoa = item.TenKhoa,
                        TenBhngChng = item.TenBhngChng,
                        NienKhoa = item.NienKhoa,
                        TenNhhk = item.TenNhhk
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
                Message = "Lop not found";
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

        public async Task<IActionResult> GetLopByMaNhanVien(string MaNhanVien)
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from gv in context.NdmgiangViens
                                 join khoa in context.Ndmkhoas on gv.Idkhoa equals khoa.Idkhoa
                                 join lop in context.Sdmlops on khoa.Idkhoa equals lop.Idkhoa
                                 join sv in context.Sdmsvs on lop.Idlop equals sv.Idlop
                                 where gv.MaNv.Contains(MaNhanVien)
                                 group lop by new { lop.Idlop, lop.MaLop, lop.TenLop } into g
                                 select new LopTheoMaNvModel
                                 {
                                     IDLop = g.Key.Idlop,
                                     MaLop = g.Key.MaLop,
                                     TenLop = g.Key.TenLop,
                                     SLSV = g.Count()
                                 }).ToList();

                    DataObject = query.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Lop not found";
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

        public async Task<IActionResult> GetLopByTenLop(string TenLop)
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from lop in context.Sdmlops
                                 join bhng in context.KdmbhngChngs on lop.IdbhngChng equals bhng.IdbhngChng
                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa
                                 where lop.TenLop.Contains(TenLop)
                                 select new LopTableModel
                                 {
                                     Idlop = lop.Idlop,
                                     MaLop = lop.MaLop,
                                     TenLop = lop.TenLop,
                                     NamVao = lop.NamVao,
                                     TenKhoa = khoa.TenKhoa,
                                     TenBhngChng = bhng.TenBhngChng,
                                     NienKhoa = lop.NienKhoa,
                                 }).ToList();
                    resultList = (List<LopTableModel>)query.Select(item => new LopTableModel
                    {
                        Idlop = item.Idlop,
                        MaLop = item.MaLop,
                        TenLop = item.TenLop,
                        NamVao = item.NamVao,
                        TenKhoa = item.TenKhoa,
                        TenBhngChng = item.TenBhngChng,
                        NienKhoa = item.NienKhoa,
                        TenNhhk = item.TenNhhk
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
                Message = "Lop not found";
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

        public async Task<IActionResult> GetLopByNamVao(string NienKhoa)
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from lop in context.Sdmlops
                                 join bhng in context.KdmbhngChngs on lop.IdbhngChng equals bhng.IdbhngChng
                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa
                                 where lop.NienKhoa.Contains(NienKhoa)
                                 select new LopTableModel
                                 {
                                     Idlop = lop.Idlop,
                                     MaLop = lop.MaLop,
                                     TenLop = lop.TenLop,
                                     NamVao = lop.NamVao,
                                     TenKhoa = khoa.TenKhoa,
                                     TenBhngChng = bhng.TenBhngChng,
                                     NienKhoa = lop.NienKhoa,
                                 }).ToList();
                    resultList = (List<LopTableModel>)query.Select(item => new LopTableModel
                    {
                        Idlop = item.Idlop,
                        MaLop = item.MaLop,
                        TenLop = item.TenLop,
                        NamVao = item.NamVao,
                        TenKhoa = item.TenKhoa,
                        TenBhngChng = item.TenBhngChng,
                        NienKhoa = item.NienKhoa,
                        TenNhhk = item.TenNhhk
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
                Message = "Lop not found";
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

        public async Task<IActionResult> GetLopByKhoa(string TenKhoa)
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from lop in context.Sdmlops
                                 join bhng in context.KdmbhngChngs on lop.IdbhngChng equals bhng.IdbhngChng
                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa
                                 where khoa.TenKhoa.Contains(TenKhoa)
                                 select new LopTableModel
                                 {
                                     Idlop = lop.Idlop,
                                     MaLop = lop.MaLop,
                                     TenLop = lop.TenLop,
                                     NamVao = lop.NamVao,
                                     TenKhoa = khoa.TenKhoa,
                                     TenBhngChng = bhng.TenBhngChng,
                                     NienKhoa = lop.NienKhoa,
                                 }).ToList();
                    resultList = (List<LopTableModel>)query.Select(item => new LopTableModel
                    {
                        Idlop = item.Idlop,
                        MaLop = item.MaLop,
                        TenLop = item.TenLop,
                        NamVao = item.NamVao,
                        TenKhoa = item.TenKhoa,
                        TenBhngChng = item.TenBhngChng,
                        NienKhoa = item.NienKhoa,
                        TenNhhk = item.TenNhhk
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
                Message = "Lop not found";
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

        public async Task<IActionResult> GetLopByBacHeNganh(string TenBacHeNganh)
        {
            try
            {
                List<LopTableModel> resultList = new List<LopTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from lop in context.Sdmlops
                                 join bhng in context.KdmbhngChngs on lop.IdbhngChng equals bhng.IdbhngChng
                                 join khoa in context.Ndmkhoas on lop.Idkhoa equals khoa.Idkhoa

                                 where bhng.TenBhngChng.Contains(TenBacHeNganh)
                                 select new LopTableModel
                                 {
                                     Idlop = lop.Idlop,
                                     MaLop = lop.MaLop,
                                     TenLop = lop.TenLop,
                                     NamVao = lop.NamVao,
                                     TenKhoa = khoa.TenKhoa,
                                     TenBhngChng = bhng.TenBhngChng,
                                     NienKhoa = lop.NienKhoa,
                                 }).ToList();
                    resultList = query.Select(item => new LopTableModel
                    {
                        Idlop = item.Idlop,
                        MaLop = item.MaLop,
                        TenLop = item.TenLop,
                        NamVao = item.NamVao,
                        TenKhoa = item.TenKhoa,
                        TenBhngChng = item.TenBhngChng,
                        NienKhoa = item.NienKhoa,
                        TenNhhk = item.TenNhhk
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
                Message = "Lop not found";
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

        public async Task<IActionResult> CreateLop([FromBody] LopTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idKhoa = context.Ndmkhoas
                          .Where(khoa => khoa.TenKhoa == InputData.TenKhoa)
        .Select(khoa => khoa.Idkhoa)
                          .FirstOrDefault();
                    long idBHNganh = context.KdmbhngChngs
                        .Where(BHN => BHN.TenBhngChng == InputData.TenBhngChng)
                    .Select(BHN => BHN.IdbhngChng)
                        .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                      .Where(NH => NH.TenNhhk == InputData.TenNhhk)
                      .Select(NH => NH.Idnhhk)
                      .FirstOrDefault();
                    var isLopTheoMa = context.Sdmlops
                   .Where(gv => gv.MaLop == InputData.MaLop)
                   .Select(gv => gv)
                   .FirstOrDefault();
                    var isLopTheoTen = context.Sdmlops
                   .Where(gv => gv.TenLop == InputData.TenLop)
                   .Select(gv => gv)
                   .FirstOrDefault();
                    if (isLopTheoMa != null || isLopTheoTen != null)
                    {
                        Code = 500;
                        Message = "Lớp đã tồn tại";
                        return CreateResponse();
                    }
                    Sdmlop newData = new Sdmlop()
                    {
                        Idlop = IdGenerator.NewUID,
                        MaLop = InputData.MaLop,
                        TenLop = InputData.TenLop,
                        NamVao = InputData.NamVao,
                        Idkhoa = idKhoa,
                        IdbhngChng = idBHNganh,
                    };
                    context.Sdmlops.Add(newData);
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
                Message = "Lop not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] LopTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idKhoa = context.Ndmkhoas
                           .Where(khoa => khoa.TenKhoa == InputData.TenKhoa)
                           .Select(khoa => khoa.Idkhoa)
                           .FirstOrDefault();
                    long idBHNganh = context.KdmbhngChngs
                        .Where(BHN => BHN.TenBhngChng == InputData.TenBhngChng)
                        .Select(BHN => BHN.IdbhngChng)
                        .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                          .Where(NH => NH.TenNhhk == InputData.TenNhhk)
                          .Select(NH => NH.Idnhhk)
                          .FirstOrDefault();

                    var existing = context.Sdmlops.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.MaLop = InputData.MaLop;
                    existing.TenLop = InputData.TenLop;
                    existing.NamVao = InputData.NamVao;
                    existing.Idkhoa = idKhoa;
                    existing.IdbhngChng = idBHNganh;
                    context.Sdmlops.Update(existing);
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
                Message = "Lop not found";
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
                    var data = context.Sdmlops.Find(id);
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
                Message = "Lop not found";
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