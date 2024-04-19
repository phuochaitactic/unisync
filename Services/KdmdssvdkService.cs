using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IKdmdssvdkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetDsSinhVienDangKy(string maSinhVien, string maNhhk);

        Task<IActionResult> GetByIdSinhVien(long idSinhVien);

        Task<IActionResult> CreateData([FromBody] GiangVienComment inputData);

        Task<IActionResult> ChangeData(long id, string loiNhan);
    }

    public class KdmdssvdkService : BaseController, IKdmdssvdkService
    {
        public async Task<IActionResult> GetDsSinhVienDangKy(string maSinhVien, string maNhhk)
        {
            try
            {
                List<DsSinhVienDangKyHdnkModel> resultList = new List<DsSinhVienDangKyHdnkModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from kq in context.Kkqsvdkhdnks
                                 join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk
                                 join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
                                 join ap in context.Kdmdssvdks on sv.IdsinhVien equals ap.IdsinhVien
                                 join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                 join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk
                                 join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                                 join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                                 where sv.MaSinhVien == "207KT30141" && hk.Idnhhk == 2
                                 group new { ap.Loinhan, sv.MaSinhVien, sv.HoTenSinhVien, hk.TenNhhk, l.MaLop, kq.TinhTrangDuyet, kq.IdsinhVien, kq.IddangKy, ap.Idnhhk } by new
                                 {
                                     ap.Loinhan,
                                     sv.MaSinhVien,
                                     sv.HoTenSinhVien,
                                     hk.TenNhhk,
                                     l.MaLop,
                                     kq.TinhTrangDuyet
                                 } into g

                                 select new DsSinhVienDangKyHdnkModel
                                 {
                                     MaSinhVIen = g.Key.MaSinhVien,
                                     HoTenSinhVien = g.Key.HoTenSinhVien,
                                     TenNHHK = g.Key.TenNhhk,
                                     MaLop = g.Key.MaLop,
                                     TinhTrangDuyet = g.Key.TinhTrangDuyet,
                                     LoiNhan = g.Key.Loinhan,
                                 }).ToList();
                    resultList = query.Select(item => new DsSinhVienDangKyHdnkModel()
                    {
                        MaSinhVIen = item.MaSinhVIen,
                        HoTenSinhVien = item.HoTenSinhVien,
                        TenNHHK = item.TenNHHK,
                        MaLop = item.MaLop,
                        TinhTrangDuyet = item.TinhTrangDuyet,
                        LoiNhan = item.LoiNhan,
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

        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<GiangVienCommentResult> resultList = new List<GiangVienCommentResult>();
                using (var context = new MyDBContext())
                {
                    var query = (from cmt in context.Kdmdssvdks
                                 join nhhk in context.Kdmnhhks on cmt.Idnhhk equals nhhk.Idnhhk
                                 select new GiangVienCommentResult
                                 {
                                     IdAP = cmt.Idap,
                                     IdNHHK = cmt.Idnhhk,
                                     TenNHHK = nhhk.TenNhhk,
                                     IdSinhVien = cmt.IdsinhVien,
                                     LoiNhan = cmt.Loinhan,
                                     TongDiem = cmt.TongDiem
                                 }).ToList();
                    resultList = query.Select(item => new GiangVienCommentResult()
                    {
                        IdAP = item.IdAP,
                        IdNHHK = item.IdNHHK,
                        TenNHHK = item.TenNHHK,
                        IdSinhVien = item.IdSinhVien,
                        LoiNhan = item.LoiNhan,
                        TongDiem = item.TongDiem
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
                Message = "du lieu not found";
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

        public async Task<IActionResult> GetByIdSinhVien(long idSinhVien)
        {
            try
            {
                List<GiangVienCommentResult> resultList = new List<GiangVienCommentResult>();
                using (var context = new MyDBContext())
                {
                    var query = (from cmt in context.Kdmdssvdks
                                 join nhhk in context.Kdmnhhks on cmt.Idnhhk equals nhhk.Idnhhk
                                 where cmt.IdsinhVien == idSinhVien
                                 select new GiangVienCommentResult
                                 {
                                     IdAP = cmt.Idap,
                                     IdNHHK = cmt.Idnhhk,
                                     IdSinhVien = cmt.IdsinhVien,
                                     TenNHHK = nhhk.TenNhhk,
                                     LoiNhan = cmt.Loinhan,
                                     TongDiem = cmt.TongDiem
                                 }).ToList();
                    resultList = query.Select(item => new GiangVienCommentResult()
                    {
                        IdAP = item.IdAP,
                        IdNHHK = item.IdNHHK,
                        IdSinhVien = item.IdSinhVien,
                        TenNHHK = item.TenNHHK,
                        LoiNhan = item.LoiNhan,
                        TongDiem = item.TongDiem
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
                Message = "du lieu not found";
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

        public async Task<IActionResult> CreateData(GiangVienComment inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idSinhVien = context.Sdmsvs
                        .Where(sinhVien => sinhVien.HoTenSinhVien == inputData.TenSinhVien)
                        .Select(sinhVien => sinhVien.IdsinhVien)
                        .FirstOrDefault();
                    long IdNHHK = context.Kdmnhhks
                    .Where(nhhk => nhhk.TenNhhk == inputData.TenNHHK)
                    .Select(nhhk => nhhk.Idnhhk)
                    .FirstOrDefault();

                    Kdmdssvdk newData = new Kdmdssvdk()
                    {
                        Idap = IdGenerator.NewUID,
                        IdsinhVien = idSinhVien,
                        Idnhhk = IdNHHK,
                        Loinhan = inputData.LoiNhan
                    };
                    context.Kdmdssvdks.Add(newData);
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
                Message = "du lieu not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] string LoiNhan)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.Kdmdssvdks.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }

                    existing.Loinhan = LoiNhan;
                    context.Kdmdssvdks.Update(existing);
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
                Message = "du lieu not found";
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