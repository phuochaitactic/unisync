using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IPhongService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetPhongByMaPhong(string MaPhong);

        Task<IActionResult> GetPhongTheoNgay(NgayBatDauNgayKetThucModel inputData);

        Task<IActionResult> GetPhongByTenPhong(string TenPhong);

        Task<IActionResult> GetPhongByDiaDiem(string TenDiaDiem);

        Task<IActionResult> GetPhongBySucChua(int SucChua);

        Task<IActionResult> GetPhongByDayPhong(string DayPhong);

        Task<IActionResult> GetPhongByCoSo(string CoSo);

        Task<IActionResult> GetPhongByDienTich(int DienTich);

        Task<IActionResult> GetPhongByTinhChat(string TinhChat);

        Task<IActionResult> CreateBacHeNganh([FromBody] PhongTableModel InputData);

        Task<IActionResult> ChangeData(long id, [FromBody] PhongTableModel InputData);

        Task<IActionResult> Delete(long id);
    }

    public class PhongService : BaseController, IPhongService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem

                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByMaPhong(string MaPhong)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.MaPhong.Contains(MaPhong)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongTheoNgay(NgayBatDauNgayKetThucModel inputData)
        {
            try
            {
                List<PhongTrongModel> resultList = new List<PhongTrongModel>();
                using (var context = new MyDBContext())
                {
                    //variable
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                 }).ToList();
                    var bookedRooms = context.Kdmtthdnks
                     .Where(r =>
                       (r.NgayBđ <= inputData.NgayBđ && inputData.NgayKt
                       <= r.NgayKt) ||
                       (inputData.NgayBđ <= r.NgayBđ && r.NgayBđ <= inputData.NgayKt) ||
                       (inputData.NgayBđ <= r.NgayKt && r.NgayKt <= inputData.NgayKt));
                    List<PhongTrongModel> availableRooms = new List<PhongTrongModel>();

                    if (bookedRooms.Select(r => r.Idphong).Distinct().Count() == 0)
                    {
                        availableRooms = query.Select(item => new PhongTrongModel
                        {
                            Idphong = item.Idphong,
                            MaPhong = item.MaPhong,
                            TenPhong = item.TenPhong,
                        }).ToList();
                    }
                    else
                    {
                        availableRooms = context.Pdmphongs
                            .Where(p => !bookedRooms.Select(r => r.Idphong)
                                .Contains(p.Idphong))
                            .Select(room => new PhongTrongModel
                            {
                                Idphong = room.Idphong,
                                MaPhong = room.MaPhong,
                                TenPhong = room.TenPhong
                            }).ToList();
                    }

                    var result = availableRooms.Distinct().ToList();
                    DataObject = result.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByTenPhong(string TenPhong)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.TenPhong.Contains(TenPhong)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByDiaDiem(string TenDiaDiem)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where dd.TenDiaDiem.Contains(TenDiaDiem)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongBySucChua(int SucChua)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.SucChua == SucChua
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByDayPhong(string DayPhong)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.DayPhong.Contains(DayPhong)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByCoSo(string CoSo)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.CoSo.Contains(CoSo)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByDienTich(int DienTich)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.DienTichSuDung == (DienTich)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> GetPhongByTinhChat(string TinhChat)
        {
            try
            {
                List<PhongTableModel> resultList = new List<PhongTableModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from phong in context.Pdmphongs
                                 join dd in context.PdmdiaDiems on phong.IddiaDiem equals dd.IddiaDiem
                                 where phong.TinhChatPhong.Contains(TinhChat)
                                 select new PhongTableModel
                                 {
                                     Idphong = phong.Idphong,
                                     MaPhong = phong.MaPhong,
                                     TenPhong = phong.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     SucChua = phong.SucChua,
                                     DayPhong = phong.DayPhong,
                                     CoSo = phong.CoSo,
                                     TinhChatPhong = phong.TinhChatPhong,
                                     DienTichSuDung = phong.DienTichSuDung,
                                 }).ToList();
                    resultList = (List<PhongTableModel>)query.Select(item => new PhongTableModel
                    {
                        Idphong = item.Idphong,
                        MaPhong = item.MaPhong,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        SucChua = item.SucChua,
                        DayPhong = item.DayPhong,
                        CoSo = item.CoSo,
                        TinhChatPhong = item.TinhChatPhong,
                        DienTichSuDung = item.DienTichSuDung,
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
                Message = "Phong not found";
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

        public async Task<IActionResult> CreateBacHeNganh([FromBody] PhongTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idDiaDiem = context.PdmdiaDiems
                          .Where(dd => dd.TenDiaDiem == InputData.TenDiaDiem)
                          .Select(dd => dd.IddiaDiem)
                          .FirstOrDefault();

                    Pdmphong newData = new Pdmphong()
                    {
                        Idphong = IdGenerator.NewUID,
                        MaPhong = InputData.MaPhong,
                        TenPhong = InputData.TenPhong,
                        SucChua = InputData.SucChua,
                        DayPhong = InputData.DayPhong,
                        CoSo = InputData.CoSo,
                        TinhChatPhong = InputData.TinhChatPhong,
                        DienTichSuDung = InputData.DienTichSuDung,
                        IddiaDiem = idDiaDiem
                    };
                    context.Pdmphongs.Add(newData);
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
                Message = "Phong not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] PhongTableModel InputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idDiaDiem = context.PdmdiaDiems
                         .Where(dd => dd.TenDiaDiem == InputData.TenDiaDiem)
                         .Select(dd => dd.IddiaDiem)
                         .FirstOrDefault();
                    var existing = context.Pdmphongs.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.MaPhong = InputData.MaPhong;
                    existing.TenPhong = InputData.TenPhong;
                    existing.SucChua = InputData.SucChua;
                    existing.DayPhong = InputData.DayPhong;
                    existing.CoSo = InputData.CoSo;
                    existing.TinhChatPhong = InputData.TinhChatPhong;
                    existing.DienTichSuDung = InputData.DienTichSuDung;
                    existing.IddiaDiem = idDiaDiem;
                    context.Pdmphongs.Update(existing);
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
                Message = "Phong not found";
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
                    var data = context.Pdmphongs.Find(id);
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
                Message = "Phong not found";
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