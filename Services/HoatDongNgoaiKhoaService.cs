using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IHoatDongNgoaiKhoaService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByTenHoatDongNgoaiKhoa(string tenHoatDongNgoaiKhoa);

        Task<IActionResult> GetByNganh(long idKhoa, string TenBhngChng);

        Task<IActionResult> GetByHocKy(long idKhoa, long IdNhhk);

        Task<IActionResult> GetbySinhVienTheoHocKy(string MaSinhVien, long MaNhhk);

        Task<IActionResult> GetbyGiangVienTheoHocKy(string MaGiangVien, long MaNhhk);

        Task<IActionResult> CreateHDNK([FromBody] HdnkTheoHocKyModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] HdnkTheoHocKyModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class HoatDongNgoaiKhoaService : BaseController, IHoatDongNgoaiKhoaService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<HoatDongNgoaiKhoaModel> resultList = new List<HoatDongNgoaiKhoaModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from hdnk in context.Kdmhdnks
                                 join mc in context.KdmndminhChungs on hdnk.IdminhChung equals mc.IdminhChung
                                 join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu
                                 select new HoatDongNgoaiKhoaModel
                                 {
                                     Idhdnk = hdnk.Idhdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     MaHdnk = hdnk.MaHdnk,
                                     TenHdnk = hdnk.TenHdnk,
                                     Diemhdnk = hdnk.Diemhdnk,
                                     CoVu = hdnk.CoVu,
                                     BanToChuc = hdnk.BanToChuc,
                                     KyNangHdnk = hdnk.KyNangHdnk,
                                     MaDieu = dieu.MaDieu
                                 }).ToList();
                    resultList = query.Select(item => new HoatDongNgoaiKhoaModel
                    {
                        Idhdnk = item.Idhdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHdnk = item.KyNangHdnk,
                        MaDieu = item.MaDieu
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> GetByTenHoatDongNgoaiKhoa(string tenHoatDongNgoaiKhoa)
        {
            try
            {
                List<HoatDongNgoaiKhoaModel> resultList = new List<HoatDongNgoaiKhoaModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from hdnk in context.Kdmhdnks
                                 join mc in context.KdmndminhChungs on hdnk.IdminhChung equals mc.IdminhChung
                                 join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu
                                 where hdnk.TenHdnk.Contains(tenHoatDongNgoaiKhoa)
                                 select new HoatDongNgoaiKhoaModel
                                 {
                                     Idhdnk = hdnk.Idhdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     MaHdnk = hdnk.MaHdnk,
                                     TenHdnk = hdnk.TenHdnk,
                                     Diemhdnk = hdnk.Diemhdnk,
                                     CoVu = hdnk.CoVu,
                                     BanToChuc = hdnk.BanToChuc,
                                     KyNangHdnk = hdnk.KyNangHdnk,
                                     MaDieu = dieu.MaDieu
                                 }).ToList();
                    resultList = query.Select(item => new HoatDongNgoaiKhoaModel
                    {
                        Idhdnk = item.Idhdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHdnk = item.KyNangHdnk,
                        MaDieu = item.MaDieu
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> GetByNganh(long idKhoa, string TenBhngChng)
        {
            try
            {
                List<HdnkTheoNganhModel> resultList = new List<HdnkTheoNganhModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from hd in context.Kdmhdnks
                                 join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                                 join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                                 join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                 join dl in context.KdmduLieuHdnks on hd.Idhdnk equals dl.Idhdnk
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa
                                 where k.Idkhoa == idKhoa &&
                                       bhn.TenBhngChng == TenBhngChng
                                 select new HdnkTheoNganhModel
                                 {
                                     Idhdnk = dl.Idhdnk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaHDNK = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     Diemhdnk = hd.Diemhdnk,
                                     MaDieu = d.MaDieu,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHdnk = hd.KyNangHdnk,
                                 }).ToList();
                    resultList = query.Select(item => new HdnkTheoNganhModel
                    {
                        Idhdnk = item.Idhdnk,
                        TenBHNgChng = item.TenBHNgChng,
                        MaHDNK = item.MaHDNK,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        MaDieu = item.MaDieu,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHdnk = item.KyNangHdnk
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> GetByHocKy(long idKhoa, long IdNhhk)
        {
            try
            {
                List<HdnkTheoHocKyModel> resultList = new List<HdnkTheoHocKyModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from hd in context.Kdmhdnks
                                 join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                                 join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                                 join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                 join dl in context.KdmduLieuHdnks on hd.Idhdnk equals dl.Idhdnk
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa
                                 where k.Idkhoa == idKhoa &&
                                       dl.Idnhhk == IdNhhk
                                 select new HdnkTheoHocKyModel
                                 {
                                     Idhdnk = dl.Idhdnk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     Diemhdnk = hd.Diemhdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHdnk = hd.KyNangHdnk,
                                     MaDieu = d.MaDieu
                                 }).ToList();
                    resultList = query.Select(item => new HdnkTheoHocKyModel
                    {
                        Idhdnk = item.Idhdnk,
                        TenBHNgChng = item.TenBHNgChng,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHdnk = item.KyNangHdnk,
                        MaDieu = item.MaDieu
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> GetbySinhVienTheoHocKy(string MaSinhVien, long MaNhhk)
        {
            try
            {
                List<HdnkTheoSinhVienModel> resultList = new List<HdnkTheoSinhVienModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from hd in context.Kdmhdnks
                                 join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                                 join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                                 join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                 join dl in context.KdmduLieuHdnks on hd.Idhdnk equals dl.Idhdnk
                                 join nh in context.Kdmnhhks on dl.Idnhhk equals nh.Idnhhk
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                                 join l in context.Sdmlops on bhn.Idngh equals l.IdbhngChng
                                 join sv in context.Sdmsvs on l.Idlop equals sv.Idlop
                                 where sv.MaSinhVien == MaSinhVien &&
                                       nh.MaNhhk == MaNhhk
                                 select new HdnkTheoSinhVienModel
                                 {
                                     Idhdnk = dl.Idhdnk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     Diemhdnk = hd.Diemhdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHdnk = hd.KyNangHdnk,
                                     MaDieu = d.MaDieu,
                                     TinhTrangDuyet = tt.TinhTragDuyet
                                 }).ToList();
                    resultList = query.Select(item => new HdnkTheoSinhVienModel
                    {
                        Idhdnk = item.Idhdnk,
                        TenBHNgChng = item.TenBHNgChng,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHdnk = item.KyNangHdnk,
                        MaDieu = item.MaDieu,
                        TinhTrangDuyet = item.TinhTrangDuyet
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> GetbyGiangVienTheoHocKy(string MaGiangVien, long MaNhhk)
        {
            try
            {
                List<HdnkTheoGiangVienModel> resultList = new List<HdnkTheoGiangVienModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from hd in context.Kdmhdnks
                                 join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                                 join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                                 join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                 join dl in context.KdmduLieuHdnks on hd.Idhdnk equals dl.Idhdnk
                                 join nh in context.Kdmnhhks on dl.Idnhhk equals nh.Idnhhk
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa
                                 join gv in context.NdmgiangViens on k.Idkhoa equals gv.Idkhoa
                                 where gv.MaNv == MaGiangVien &&
                                       nh.MaNhhk == MaNhhk
                                 select new HdnkTheoGiangVienModel
                                 {
                                     Idhdnk = dl.Idhdnk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     Diemhdnk = hd.Diemhdnk,
                                     NoiDungMinhChung = mc.NoiDungMinhChung,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHdnk = hd.KyNangHdnk,
                                     MaDieu = d.MaDieu,
                                     TinhTrangDuyet = tt.TinhTragDuyet
                                 }).ToList();
                    resultList = query.Select(item => new HdnkTheoGiangVienModel
                    {
                        Idhdnk = item.Idhdnk,
                        TenBHNgChng = item.TenBHNgChng,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        NoiDungMinhChung = item.NoiDungMinhChung,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHdnk = item.KyNangHdnk,
                        MaDieu = item.MaDieu,
                        TinhTrangDuyet = item.TinhTrangDuyet
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> CreateHDNK([FromBody] HdnkTheoHocKyModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long IdMinhChung = context.KdmndminhChungs
                                .Where(mc => mc.NoiDungMinhChung == inputData.NoiDungMinhChung)
                                .Select(mc => mc.IdminhChung)
                                .FirstOrDefault();
                    var isHoatDongTheoMa = context.Kdmhdnks
                     .Where(gv => gv.MaHdnk == inputData.MaHdnk)
                     .Select(gv => gv)
                     .FirstOrDefault();
                    var isHoatDongTheoTen = context.Kdmhdnks
                   .Where(gv => gv.TenHdnk == inputData.TenHdnk)
                   .Select(gv => gv)
                   .FirstOrDefault();
                    if (isHoatDongTheoMa != null || isHoatDongTheoTen != null)
                    {
                        Code = 500;
                        Message = "hoạt dộng đã tồn tại";
                        return CreateResponse();
                    }
                    Kdmhdnk newData = new Kdmhdnk()
                    {
                        Idhdnk = IdGenerator.NewUID,
                        IdminhChung = IdMinhChung,
                        MaHdnk = inputData.MaHdnk,
                        TenHdnk = inputData.TenHdnk,
                        Diemhdnk = inputData.Diemhdnk,
                        CoVu = inputData.CoVu,
                        BanToChuc = inputData.BanToChuc,
                        KyNangHdnk = inputData.KyNangHdnk,
                    };
                    await context.Kdmhdnks.AddAsync(newData);
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
                Message = "Hoat dong not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] HdnkTheoHocKyModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long IdMinhChung = context.KdmndminhChungs
                                 .Where(mc => mc.NoiDungMinhChung == inputData.NoiDungMinhChung)
                                 .Select(mc => mc.IdminhChung)
                                 .FirstOrDefault();
                    var existing = await context.Kdmhdnks.FindAsync(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.IdminhChung = IdMinhChung;
                    existing.MaHdnk = inputData.MaHdnk;
                    existing.TenHdnk = inputData.TenHdnk;
                    existing.Diemhdnk = inputData.Diemhdnk;
                    existing.CoVu = inputData.CoVu;
                    existing.BanToChuc = inputData.BanToChuc;
                    existing.KyNangHdnk = inputData.KyNangHdnk;
                    context.Kdmhdnks.Update(existing);
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
                Message = "Hoat dong not found";
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
                    var data = await context.Kdmhdnks.FindAsync(id);
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
                Message = "Hoat dong not found";
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