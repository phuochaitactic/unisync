using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IKkqSvDkHdnkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetById(long id);

        Task<IActionResult> GetByIdSinhVien(long id);

        Task<IActionResult> GetByIdSinhVienAndIsThamGia(long id, string TenNhhk);

        //Task<IActionResult> GetKQDKCuaSinhVienTrongHk(string MaSinhVien, string MaHocKy);
        Task<IActionResult> GetSinhVienThamGiaByHdnk(long id);

        Task<IActionResult> GetDsSinhVienThamGia(long IdHdnk, long? IdSinhVien);

        Task<IActionResult> CreateKkq([FromBody] KkqSvDkHdnkModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] KkqSvDkHdnkModel inputData);

        Task<IActionResult> ChangeIsThamGia([FromBody] KkqIsThamGiaModel inputData);

        Task<IActionResult> Delete(long id);
    }

    public class KkqSvDkHdnkService : BaseController, IKkqSvDkHdnkService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<KkqSvDkHdnkModel> resultList = new List<KkqSvDkHdnkModel>();
                using (var context = new MyDBContext())
                {
                    var query = await (from kkq in context.Kkqsvdkhdnks
                                       join gv in context.NdmgiangViens on kkq.IdgiangVien equals gv.IdgiangVien
                                       join sv in context.Sdmsvs on kkq.IdsinhVien equals sv.IdsinhVien
                                       join hd in context.Kdmhdnks on kkq.Idhdnk equals hd.Idhdnk
                                       join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                       join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung

                                       join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu into dieus
                                       from dieu in dieus.DefaultIfEmpty()
                                       select new KkqSvDkHdnkModel
                                       {
                                           IddangKy = kkq.IddangKy,
                                           TengiangVien = gv.HoTen,
                                           Tenhdnk = hd.TenHdnk,
                                           TensinhVien = sv.HoTenSinhVien,
                                           NgayLap = kkq.NgayLap,
                                           NgayDuyet = kkq.NgayDuyet,
                                           TinhTrangDuyet = kkq.TinhTrangDuyet,
                                           GhiChu = kkq.GhiChu,
                                           NgayThamGia = kkq.NgayThamGia,
                                           IsThamGia = kkq.IsThamGia,
                                           SoDiem = kkq.SoDiem,
                                           MinhChungThamGia = kkq.MinhChungThamGia,
                                           VaiTroTg = kkq.VaiTroTg,
                                           NgayBd = tt.NgayBđ,
                                           NgayKt = tt.NgayKt,
                                       }).ToListAsync();
                    resultList = query.Select(item => new KkqSvDkHdnkModel
                    {
                        IddangKy = item.IddangKy,
                        TengiangVien = item.TengiangVien,
                        Tenhdnk = item.Tenhdnk,
                        TensinhVien = item.TensinhVien,
                        NgayLap = item.NgayLap,
                        NgayDuyet = item.NgayDuyet,
                        TinhTrangDuyet = item.TinhTrangDuyet,
                        GhiChu = item.GhiChu,
                        NgayThamGia = item.NgayThamGia,
                        IsThamGia = item.IsThamGia,
                        SoDiem = item.SoDiem,
                        MinhChungThamGia = item.MinhChungThamGia,
                        VaiTroTg = item.VaiTroTg,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Code = 200;
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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
                Message = ex.ToString();
                return CreateResponse();
            }
        }

        public async Task<IActionResult> GetDsSinhVienThamGia(long IdHdnk, long? IdSinhVien)
        {
            try
            {
                List<ThongTinSinhVienThamGia> resultList = new List<ThongTinSinhVienThamGia>
                ();
                List<ThongTinSinhVienThamGia> query = new List<ThongTinSinhVienThamGia>();
                using (var context = new MyDBContext())
                {
                    if (IdSinhVien != null)
                    {
                        query = await (from sv in context.Sdmsvs
                                       join kq in context.Kkqsvdkhdnks on sv.IdsinhVien equals kq.IdsinhVien
                                       join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                       join bhn in context.KdmbhngChngs on l.IdbhngChng equals bhn.IdbhngChng
                                       join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                       join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk
                                       join nhhk in context.Kdmnhhks on dl.Idnhhk equals nhhk.Idnhhk
                                       where kq.TinhTrangDuyet == true
                                       && kq.IsThamGia == true
                                       && kq.Idhdnk == IdHdnk
                                       && sv.IdsinhVien == IdSinhVien
                                       select new ThongTinSinhVienThamGia
                                       {
                                           HoTenSV = sv.HoTenSinhVien,
                                           MSSV = sv.MaSinhVien,
                                           TenLop = l.TenLop,
                                           TenBacHeNganh = bhn.TenBhngChng,
                                           NienKhoa = l.NienKhoa,
                                           VaiTroThamGia = kq.VaiTroTg,
                                           HoTenGV = gv.HoTen
                                       }).Distinct().ToListAsync();
                        resultList = query.Select(item => new ThongTinSinhVienThamGia
                        {
                            HoTenSV = item.HoTenSV,
                            MSSV = item.MSSV,
                            TenLop = item.TenLop,
                            TenBacHeNganh = item.TenBacHeNganh,
                            NienKhoa = item.NienKhoa,
                            VaiTroThamGia = item.VaiTroThamGia,
                            HoTenGV = item.HoTenGV
                        }).ToList().Distinct().ToList();
                        DataObject = resultList.Cast<object>().ToList();
                        Code = 200;
                        Message = "Success!"; Code = 200;
                        return CreateResponse();
                    }
                    query = await (from sv in context.Sdmsvs
                                   join kq in context.Kkqsvdkhdnks on sv.IdsinhVien equals kq.IdsinhVien
                                   join l in context.Sdmlops on sv.Idlop equals l.Idlop
                                   join bhn in context.KdmbhngChngs on l.IdbhngChng equals bhn.IdbhngChng
                                   join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                                   join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk
                                   join nhhk in context.Kdmnhhks on dl.Idnhhk equals nhhk.Idnhhk
                                   where kq.TinhTrangDuyet == true
                                   && kq.IsThamGia == true
                                   && kq.Idhdnk == IdHdnk
                                   select new ThongTinSinhVienThamGia
                                   {
                                       idSinhVien = sv.IdsinhVien,
                                       HoTenSV = sv.HoTenSinhVien,
                                       MSSV = sv.MaSinhVien,
                                       TenLop = l.TenLop,
                                       TenBacHeNganh = bhn.TenBhngChng,
                                       NienKhoa = l.NienKhoa,
                                       VaiTroThamGia = kq.VaiTroTg,
                                       HoTenGV = gv.HoTen
                                   }).Distinct().ToListAsync();
                    resultList = query.Select(item => new ThongTinSinhVienThamGia
                    {
                        idSinhVien = item.idSinhVien,
                        HoTenSV = item.HoTenSV,
                        MSSV = item.MSSV,
                        TenLop = item.TenLop,
                        TenBacHeNganh = item.TenBacHeNganh,
                        NienKhoa = item.NienKhoa,
                        VaiTroThamGia = item.VaiTroThamGia,
                        HoTenGV = item.HoTenGV
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Code = 200;
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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
                List<KkqSvDkHdnkModel> resultList = new List<KkqSvDkHdnkModel>();
                using (var context = new MyDBContext())
                {
                    var query = await (from kkq in context.Kkqsvdkhdnks
                                       join gv in context.NdmgiangViens on kkq.IdgiangVien equals gv.IdgiangVien
                                       join sv in context.Sdmsvs on kkq.IdsinhVien equals sv.IdsinhVien
                                       join hd in context.Kdmhdnks on kkq.Idhdnk equals hd.Idhdnk
                                       join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                       where kkq.IddangKy == id
                                       select new KkqSvDkHdnkModel
                                       {
                                           IddangKy = kkq.IddangKy,
                                           TengiangVien = gv.HoTen,
                                           Tenhdnk = hd.TenHdnk,
                                           TensinhVien = sv.HoTenSinhVien,
                                           NgayLap = kkq.NgayLap,
                                           NgayDuyet = kkq.NgayDuyet,
                                           TinhTrangDuyet = kkq.TinhTrangDuyet,
                                           GhiChu = kkq.GhiChu,
                                           NgayThamGia = kkq.NgayThamGia,
                                           IsThamGia = kkq.IsThamGia,
                                           SoDiem = kkq.SoDiem,
                                           MinhChungThamGia = kkq.MinhChungThamGia,
                                           VaiTroTg = kkq.VaiTroTg,
                                           NgayBd = tt.NgayBđ,
                                           NgayKt = tt.NgayKt
                                       }).ToListAsync();
                    resultList = query.Select(item => new KkqSvDkHdnkModel
                    {
                        IddangKy = item.IddangKy,
                        TengiangVien = item.TengiangVien,
                        Tenhdnk = item.Tenhdnk,
                        TensinhVien = item.TensinhVien,
                        NgayLap = item.NgayLap,
                        NgayDuyet = item.NgayDuyet,
                        TinhTrangDuyet = item.TinhTrangDuyet,
                        GhiChu = item.GhiChu,
                        NgayThamGia = item.NgayThamGia,
                        IsThamGia = item.IsThamGia,
                        SoDiem = item.SoDiem,
                        MinhChungThamGia = item.MinhChungThamGia,
                        VaiTroTg = item.VaiTroTg,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt
                    }).ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Code = 200;
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> GetByIdSinhVien(long id)
        {
            try
            {
                List<KkqSvDkHdnkModel2> resultList = new List<KkqSvDkHdnkModel2>();
                using (var context = new MyDBContext())
                {
                    var query = await (from kkq in context.Kkqsvdkhdnks
                                       join gv in context.NdmgiangViens on kkq.IdgiangVien equals gv.IdgiangVien
                                       join sv in context.Sdmsvs on kkq.IdsinhVien equals sv.IdsinhVien
                                       join hd in context.Kdmhdnks on kkq.Idhdnk equals hd.Idhdnk
                                       join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                                       join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung

                                       join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu into dieus
                                       from dieu in dieus.DefaultIfEmpty()
                                       where sv.IdsinhVien == id
                                       select new KkqSvDkHdnkModel2
                                       {
                                           IddangKy = kkq.IddangKy,
                                           TengiangVien = gv.HoTen,
                                           Tenhdnk = hd.TenHdnk,
                                           TensinhVien = sv.HoTenSinhVien,
                                           NgayLap = kkq.NgayLap,
                                           NgayDuyet = kkq.NgayDuyet,
                                           TinhTrangDuyet = kkq.TinhTrangDuyet,
                                           GhiChu = kkq.GhiChu,
                                           NgayThamGia = kkq.NgayThamGia,
                                           IsThamGia = kkq.IsThamGia,
                                           SoDiem = kkq.SoDiem,
                                           MinhChungThamGia = kkq.MinhChungThamGia,
                                           VaiTroTg = kkq.VaiTroTg,
                                           NgayBd = tt.NgayBđ,
                                           NgayKt = tt.NgayKt,
                                           KyNangHDNK = hd.KyNangHdnk,
                                           MaDieu = dieu.MaDieu,
                                           NoiDungMinhChung = mc.NoiDungMinhChung
                                       }).Distinct().ToListAsync();
                    resultList = query.Select(item => new KkqSvDkHdnkModel2
                    {
                        IddangKy = item.IddangKy,
                        TengiangVien = item.TengiangVien,
                        Tenhdnk = item.Tenhdnk,
                        TensinhVien = item.TensinhVien,
                        NgayLap = item.NgayLap,
                        NgayDuyet = item.NgayDuyet,
                        TinhTrangDuyet = item.TinhTrangDuyet,
                        GhiChu = item.GhiChu,
                        NgayThamGia = item.NgayThamGia,
                        IsThamGia = item.IsThamGia,
                        SoDiem = item.SoDiem,
                        MinhChungThamGia = item.MinhChungThamGia,
                        VaiTroTg = item.VaiTroTg,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        KyNangHDNK = item.KyNangHDNK,
                        MaDieu = item.MaDieu,
                        NoiDungMinhChung = item.NoiDungMinhChung
                    }).Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> GetByIdSinhVienAndIsThamGia(long id, string TenNhhk)
        {
            try
            {
                List<KkqSvDkHdnkModel> resultList = new List<KkqSvDkHdnkModel>();
                using (var context = new MyDBContext())
                {
                    var query = await (from kq in context.Kkqsvdkhdnks
                                       join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
                                       join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk into dlj
                                       from dlr in dlj.DefaultIfEmpty()
                                       join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk into hdj
                                       from hdr in hdj.DefaultIfEmpty()
                                       join hk in context.Kdmnhhks on dlr.Idnhhk equals hk.Idnhhk into hkj
                                       from hkr in hkj.DefaultIfEmpty()
                                       join mc in context.KdmndminhChungs on hdr.IdminhChung equals mc.IdminhChung into mcj
                                       from mcr in mcj.DefaultIfEmpty()
                                       join d in context.Kdmdieus on mcr.Iddieu equals d.Iddieu into dj
                                       from dr in dj.DefaultIfEmpty()
                                       where sv.IdsinhVien == id && kq.IsThamGia == true && hkr.TenNhhk == TenNhhk
                                       select new
                                       {
                                           TenNHHK = hkr.TenNhhk,
                                           HoTenSinhVien = sv.HoTenSinhVien,
                                           MaHDNK = hdr.MaHdnk,
                                           TenHDNK = hdr.TenHdnk,
                                           KyNangHDNK = hdr.KyNangHdnk,
                                           MaDieu = dr.MaDieu,
                                           NoiDungMinhChung = mcr.NoiDungMinhChung,
                                           IsThamGia = kq.IsThamGia,
                                           SoDiem = kq.SoDiem,
                                           VaiTroTG = kq.VaiTroTg
                                       }).Distinct().ToListAsync();

                    DataObject = query.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> GetKQDKCuaSinhVienTrongHk(string MaSinhVien, long MaNhhk)
        {
            try
            {
                List<KkqCuaSinhVienTheoNhhkModel> resultList = new List<KkqCuaSinhVienTheoNhhkModel>();
                using (var context = new MyDBContext())
                {
                    var query = await (from kq in context.Kkqsvdkhdnks
                                       join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
                                       join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk into dlj
                                       from dlr in dlj.DefaultIfEmpty()
                                       join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk into hdj
                                       from hdr in hdj.DefaultIfEmpty()
                                       join hk in context.Kdmnhhks on dlr.Idnhhk equals hk.Idnhhk into hkj
                                       from hkr in hkj.DefaultIfEmpty()
                                       join mc in context.KdmndminhChungs on hdr.IdminhChung equals mc.IdminhChung into mcj
                                       from mcr in mcj.DefaultIfEmpty()
                                       join d in context.Kdmdieus on mcr.Iddieu equals d.Iddieu into dj
                                       from dr in dj.DefaultIfEmpty()
                                       where sv.MaSinhVien == MaSinhVien && hkr.MaNhhk == MaNhhk
                                       select new KkqCuaSinhVienTheoNhhkModel
                                       {
                                           MaHDNK = hdr.MaHdnk,
                                           TenHDNK = hdr.TenHdnk,
                                           KyNangHDNK = hdr.KyNangHdnk,
                                           MaDieu = dr.MaDieu,
                                           NoiDungMinhChung = mcr.NoiDungMinhChung,
                                           IsThamGia = kq.IsThamGia,
                                           SoDiem = kq.SoDiem,
                                           VaiTroTG = kq.VaiTroTg
                                       }).Distinct().ToListAsync();

                    DataObject = query.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> GetSinhVienThamGiaByHdnk(long id)
        {
            try
            {
                List<KkqSvDkHdnkModel2> resultList = new List<KkqSvDkHdnkModel2>();
                using (var context = new MyDBContext())
                {
                    var query = await (from kq in context.Kkqsvdkhdnks
                                       join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
                                       join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk into dlj
                                       from dlr in dlj.DefaultIfEmpty()
                                       join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk into hdj
                                       from hdr in hdj.DefaultIfEmpty()
                                       join hk in context.Kdmnhhks on dlr.Idnhhk equals hk.Idnhhk into hkj
                                       from hkr in hkj.DefaultIfEmpty()
                                       join mc in context.KdmndminhChungs on hdr.IdminhChung equals mc.IdminhChung into mcj
                                       from mcr in mcj.DefaultIfEmpty()
                                       join d in context.Kdmdieus on mcr.Iddieu equals d.Iddieu into dj
                                       from dr in dj.DefaultIfEmpty()
                                       where hdr.Idhdnk == id && kq.IsThamGia == true
                                       select new
                                       {
                                           Idsinhvien = sv.IdsinhVien,
                                           TenNHHK = hkr.TenNhhk,
                                           HoTenSinhVien = sv.HoTenSinhVien,
                                           MaHDNK = hdr.MaHdnk,
                                           TenHDNK = hdr.TenHdnk,
                                           KyNangHDNK = hdr.KyNangHdnk,
                                           MaDieu = dr.MaDieu,
                                           NoiDungMinhChung = mcr.NoiDungMinhChung,
                                           IsThamGia = kq.IsThamGia,
                                           SoDiem = kq.SoDiem,
                                           VaiTroTG = kq.VaiTroTg
                                       }).Distinct().ToListAsync();

                    DataObject = query.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> CreateKkq([FromBody] KkqSvDkHdnkModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idSinhVien = context.Sdmsvs
                             .Where(sinhVien => sinhVien.HoTenSinhVien == inputData.TensinhVien)
                             .Select(sinhVien => sinhVien.IdsinhVien)
                             .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                             .Where(giangvien => giangvien.HoTen == inputData.TengiangVien)
                             .Select(giangvien => giangvien.IdgiangVien)
                             .FirstOrDefault();
                    long idHdnk = context.Kdmhdnks
                             .Where(hd => hd.TenHdnk == inputData.Tenhdnk)
                             .Select(hd => hd.Idhdnk)
                             .FirstOrDefault();
                    Kkqsvdkhdnk newData = new Kkqsvdkhdnk()
                    {
                        IddangKy = IdGenerator.NewUID,
                        IdgiangVien = idGiangVien,
                        Idhdnk = idHdnk,
                        IdsinhVien = idSinhVien,
                        NgayLap = inputData.NgayLap,
                        NgayDuyet = inputData.NgayDuyet,
                        TinhTrangDuyet = inputData.TinhTrangDuyet,
                        SoDiem = inputData.SoDiem,
                        GhiChu = inputData.GhiChu,
                        NgayThamGia = inputData.NgayThamGia,
                        IsThamGia = inputData.IsThamGia,
                        MinhChungThamGia = inputData.MinhChungThamGia,
                        VaiTroTg = inputData.VaiTroTg,
                    };
                    context.Kkqsvdkhdnks.Add(newData);
                    context.SaveChanges();
                    DataObject.Clear();
                    DataObject.Add(newData);
                    Code = 200;
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> ChangeData(long id, [FromBody] KkqSvDkHdnkModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long idSinhVien = context.Sdmsvs
                             .Where(sinhVien => sinhVien.HoTenSinhVien == inputData.TensinhVien)
                             .Select(sinhVien => sinhVien.IdsinhVien)
                             .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                             .Where(giangvien => giangvien.HoTen == inputData.TengiangVien)
                             .Select(giangvien => giangvien.IdgiangVien)
                             .FirstOrDefault();
                    long idHdnk = context.Kdmhdnks
                             .Where(hd => hd.TenHdnk == inputData.Tenhdnk)
                             .Select(hd => hd.Idhdnk)
                             .FirstOrDefault();
                    var existing = context.Kkqsvdkhdnks.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.IdgiangVien = idGiangVien;
                    existing.Idhdnk = idHdnk;
                    existing.IdsinhVien = idSinhVien;
                    existing.NgayLap = inputData.NgayLap;
                    existing.NgayDuyet = inputData.NgayDuyet;
                    existing.TinhTrangDuyet = inputData.TinhTrangDuyet;
                    existing.GhiChu = inputData.GhiChu;
                    existing.NgayThamGia = inputData.NgayThamGia;
                    existing.IsThamGia = inputData.IsThamGia;
                    existing.SoDiem = inputData.SoDiem;
                    existing.MinhChungThamGia = inputData.MinhChungThamGia;
                    existing.VaiTroTg = inputData.VaiTroTg;
                    context.Kkqsvdkhdnks.Update(existing);
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
                Message = "Ket qua dang ky not found";
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

        public async Task<IActionResult> ChangeIsThamGia([FromBody] KkqIsThamGiaModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    var existing = context.Kkqsvdkhdnks
       .FirstOrDefault(x => x.IdsinhVien == inputData.IdSinhVien && x.Idhdnk == inputData.IdHdnk);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.IsThamGia = inputData.IsThamGia;

                    context.Kkqsvdkhdnks.Update(existing);
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
                Message = "Ket qua dang ky not found";
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
                    var data = context.Kkqsvdkhdnks.Find(id);
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
                Message = "Ket qua dang ky not found";
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