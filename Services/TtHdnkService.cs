using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ITtHdnkService
    {
        Task<IActionResult> GetAll();

        Task<IActionResult> GetByTenKhoa(string tenKhoa);

        Task<IActionResult> GetByTenGiangVien(string tenGiangVien);

        Task<IActionResult> GetByTenPhong(string tenPhong);

        Task<IActionResult> GetByTenDiaDiem(string tenDiaDiem);

        Task<IActionResult> GetByTenHdnk(string MaHdnk);

        Task<IActionResult> GetDsSinhVienDangKy(long idKhoa, long idBhNganh, long idNhhk);

        Task<IActionResult> CreateTtHdnk([FromBody] ThongTinHoatDongNgoaiKhoaByMaModel inputData);

        Task<IActionResult> ChangeData(long id, [FromBody] ThongTinHoatDongNgoaiKhoaModelDatum inputData);

        Task<IActionResult> Delete(long id);
    }

    public class TtHdnkService : BaseController, ITtHdnkService
    {
        public async Task<IActionResult> GetAll()
        {
            List<ThongTinHoatDongNgoaiKhoaModel2> resultList = new List<ThongTinHoatDongNgoaiKhoaModel2>();
            using (var context = new MyDBContext())
            {
                try
                {
                    var query = (from tt in context.Kdmtthdnks
                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into ttdl
                                 from dl in ttdl.DefaultIfEmpty()
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk into ttdlhd
                                 from hd in ttdlhd.DefaultIfEmpty()
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into ttdlhdnhhk
                                 from hk in ttdlhdnhhk.DefaultIfEmpty()
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into ttdlhdnhhkbhn
                                 from bhn in ttdlhdnhhkbhn.DefaultIfEmpty()
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ttdlhdnhhkbhnn
                                 from n in ttdlhdnhhkbhnn.DefaultIfEmpty()
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ttdlhdnhhkbhnnk
                                 from k in ttdlhdnhhkbhnnk.DefaultIfEmpty()
                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into ttgv
                                 from gv in ttgv.DefaultIfEmpty()
                                 join dd in context.PdmdiaDiems on tt.IddiaDiem equals dd.IddiaDiem into ttdd
                                 from dd in ttdd.DefaultIfEmpty()
                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ttp
                                 from p in ttp.DefaultIfEmpty()

                                 select new ThongTinHoatDongNgoaiKhoaModel2
                                 {
                                     Idtthdnk = tt.Idtthdnk,
                                     IdHdnk = hd.Idhdnk,
                                     IdDuLieu = dl.IdduLieuHdnk,
                                     TenNHHK = hk.TenNhhk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaKhoa = k.MaKhoa,
                                     TenKhoa = k.TenKhoa,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     DiemHdnk = hd.Diemhdnk,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHDNK = hd.KyNangHdnk,
                                     TenGiangVien = gv.HoTen,
                                     TenPhong = p.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     PhamVi = tt.PhamVi,
                                     SoLuongThucTe = tt.SoLuongThucTe,
                                     SoLuongDuKien = tt.SoLuongDuKien,
                                     GhiChu = tt.GhiChu,
                                     BuoiUuTien = tt.BuoiUuTien,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     NgayBd = tt.NgayBđ,
                                     NgayKt = tt.NgayKt,
                                     IsCanPhong = tt.IsCanPhong,
                                     HinhAnh = tt.HinhAnh,
                                     isCanMinhChung = tt.IsCanMinhChung,
                                     tinhTrangDuyet = tt.TinhTragDuyet,
                                     CreatedBy = tt.CreatedBy,
                                     CreatedTime = tt.CreatedTime
                                 }).ToList().Distinct().ToList();

                    resultList = query.Select(item => new ThongTinHoatDongNgoaiKhoaModel2
                    {
                        Idtthdnk = item.Idtthdnk,
                        IdHdnk = item.IdHdnk,
                        IdDuLieu = item.IdDuLieu,
                        TenNHHK = item.TenNHHK,
                        TenBHNgChng = item.TenBHNgChng,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        DiemHdnk = item.DiemHdnk,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHDNK = item.KyNangHDNK,
                        TenGiangVien = item.TenGiangVien,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        PhamVi = item.PhamVi,
                        HinhAnh = item.HinhAnh,
                        SoLuongThucTe = item.SoLuongThucTe,
                        SoLuongDuKien = item.SoLuongDuKien,
                        GhiChu = item.GhiChu,
                        BuoiUuTien = item.BuoiUuTien,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        IsCanPhong = item.IsCanPhong,
                        isCanMinhChung = item.isCanMinhChung,
                        tinhTrangDuyet = item.tinhTrangDuyet,
                        CreatedBy = item.CreatedBy,
                        CreatedTime = item.CreatedTime
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    // ID not found
                    Code = 404;
                    Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> GetByTenKhoa(string TenKhoa)
        {
            try
            {
                List<ThongTinHoatDongNgoaiKhoaModel2> resultList = new List<ThongTinHoatDongNgoaiKhoaModel2>();
                using (var context = new MyDBContext())
                {
                    var query = (from tt in context.Kdmtthdnks
                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into ttdl
                                 from dl in ttdl.DefaultIfEmpty()
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk into ttdlhd
                                 from hd in ttdlhd.DefaultIfEmpty()
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into ttdlhdnhhk
                                 from hk in ttdlhdnhhk.DefaultIfEmpty()
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into ttdlhdnhhkbhn
                                 from bhn in ttdlhdnhhkbhn.DefaultIfEmpty()
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ttdlhdnhhkbhnn
                                 from n in ttdlhdnhhkbhnn.DefaultIfEmpty()
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ttdlhdnhhkbhnnk
                                 from k in ttdlhdnhhkbhnnk.DefaultIfEmpty()
                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into ttgv
                                 from gv in ttgv.DefaultIfEmpty()
                                 join dd in context.PdmdiaDiems on tt.IddiaDiem equals dd.IddiaDiem into ttdd
                                 from dd in ttdd.DefaultIfEmpty()
                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ttp
                                 from p in ttp.DefaultIfEmpty()
                                 where k.TenKhoa.Contains(TenKhoa)
                                 select new ThongTinHoatDongNgoaiKhoaModel2
                                 {
                                     Idtthdnk = tt.Idtthdnk,
                                     IdHdnk = hd.Idhdnk,
                                     IdDuLieu = dl.IdduLieuHdnk,
                                     TenNHHK = hk.TenNhhk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaKhoa = k.MaKhoa,
                                     TenKhoa = k.TenKhoa,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     DiemHdnk = hd.Diemhdnk,
                                     CoVu = hd.CoVu,
                                     HinhAnh = tt.HinhAnh,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHDNK = hd.KyNangHdnk,
                                     TenGiangVien = gv.HoTen,
                                     TenPhong = p.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     PhamVi = tt.PhamVi,
                                     SoLuongThucTe = tt.SoLuongThucTe,
                                     SoLuongDuKien = tt.SoLuongDuKien,
                                     GhiChu = tt.GhiChu,
                                     BuoiUuTien = tt.BuoiUuTien,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     NgayBd = tt.NgayBđ,
                                     NgayKt = tt.NgayKt,
                                     IsCanPhong = tt.IsCanPhong,
                                     isCanMinhChung = tt.IsCanMinhChung,
                                     tinhTrangDuyet = tt.TinhTragDuyet,
                                     CreatedBy = tt.CreatedBy,
                                     CreatedTime = tt.CreatedTime
                                 }).ToList().Distinct().ToList();

                    resultList = query.Select(item => new ThongTinHoatDongNgoaiKhoaModel2
                    {
                        Idtthdnk = item.Idtthdnk,
                        IdHdnk = item.IdHdnk,
                        IdDuLieu = item.IdDuLieu,
                        TenNHHK = item.TenNHHK,
                        TenBHNgChng = item.TenBHNgChng,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        HinhAnh = item.HinhAnh,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        DiemHdnk = item.DiemHdnk,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHDNK = item.KyNangHDNK,
                        TenGiangVien = item.TenGiangVien,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        PhamVi = item.PhamVi,
                        SoLuongThucTe = item.SoLuongThucTe,
                        SoLuongDuKien = item.SoLuongDuKien,
                        GhiChu = item.GhiChu,
                        BuoiUuTien = item.BuoiUuTien,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        IsCanPhong = item.IsCanPhong,
                        isCanMinhChung = item.isCanMinhChung,
                        tinhTrangDuyet = item.tinhTrangDuyet,
                        CreatedBy = item.CreatedBy,
                        CreatedTime = item.CreatedTime
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Thong tin hoat dong not found";
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
            };
        }

        public async Task<IActionResult> GetDsSinhVienDangKy(long idKhoa, long idBhNganh, long idNhhk)
        {
            try
            {
                List<DsSinhVienDangKyModel> resultList = new List<DsSinhVienDangKyModel>();
                using (var context = new MyDBContext())
                {
                    var query = (from tt in context.Kdmtthdnks

                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into dls
                                 from dl in dls.DefaultIfEmpty()

                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into gvs
                                 from gv in gvs.DefaultIfEmpty()

                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ps
                                 from p in ps.DefaultIfEmpty()

                                 join d in context.PdmdiaDiems on tt.IddiaDiem equals d.IddiaDiem into ds
                                 from d in ds.DefaultIfEmpty()

                                 join hd in context.Kdmhdnks on tt.Idhdnk equals hd.Idhdnk into hds
                                 from hd in hds.DefaultIfEmpty()

                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into bhns
                                 from bhn in bhns.DefaultIfEmpty()

                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ns
                                 from n in ns.DefaultIfEmpty()

                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ks
                                 from k in ks.DefaultIfEmpty()

                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into hks
                                 from hk in hks.DefaultIfEmpty()
                                 join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung

                                 join dieu in context.Kdmdieus on mc.Iddieu equals dieu.Iddieu into dieus
                                 from dieu in dieus.DefaultIfEmpty()
                                 select new DsSinhVienDangKyModel
                                 {
                                     Idhdnk = hd.Idhdnk,
                                     IdminhChung = hd.IdminhChung,
                                     TenHdnk = hd.TenHdnk,
                                     PhamVi = tt.PhamVi,
                                     NgayBD = tt.NgayBđ,
                                     NgayKT = tt.NgayKt,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     TenKhoa = k.TenKhoa,
                                     maDieu = dieu.MaDieu,
                                     DiemHDNK = hd.Diemhdnk,
                                     HinhAnh = tt.HinhAnh,
                                 }).ToList();

                    resultList = query.Select(item => new DsSinhVienDangKyModel
                    {
                        Idhdnk = item.Idhdnk,
                        IdminhChung = item.IdminhChung,
                        PhamVi = item.PhamVi,
                        NgayBD = item.NgayBD,
                        NgayKT = item.NgayKT,
                        HinhAnh = item.HinhAnh,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        TenKhoa = item.TenKhoa,
                        TenHdnk = item.TenHdnk,
                        maDieu = item.maDieu,
                        DiemHDNK = item.DiemHDNK,
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
                Message = "Thong tin hoat dong not found";
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
            };
        }

        public async Task<IActionResult> GetByTenGiangVien(string TenGiangVien)
        {
            try
            {
                List<ThongTinHoatDongNgoaiKhoaModel2> resultList = new List<ThongTinHoatDongNgoaiKhoaModel2>();
                using (var context = new MyDBContext())
                {
                    long idGiangVien = context.NdmgiangViens
                     .Where(gv => gv.HoTen == TenGiangVien)
                     .Select(gv => gv.IdgiangVien)
                     .FirstOrDefault();
                    var query = (from tt in context.Kdmtthdnks
                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into ttdl
                                 from dl in ttdl.DefaultIfEmpty()
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk into ttdlhd
                                 from hd in ttdlhd.DefaultIfEmpty()
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into ttdlhdnhhk
                                 from hk in ttdlhdnhhk.DefaultIfEmpty()
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into ttdlhdnhhkbhn
                                 from bhn in ttdlhdnhhkbhn.DefaultIfEmpty()
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ttdlhdnhhkbhnn
                                 from n in ttdlhdnhhkbhnn.DefaultIfEmpty()
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ttdlhdnhhkbhnnk
                                 from k in ttdlhdnhhkbhnnk.DefaultIfEmpty()
                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into ttgv
                                 from gv in ttgv.DefaultIfEmpty()
                                 join dd in context.PdmdiaDiems on tt.IddiaDiem equals dd.IddiaDiem into ttdd
                                 from dd in ttdd.DefaultIfEmpty()
                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ttp
                                 from p in ttp.DefaultIfEmpty()
                                 where k.Idkhoa == tt.Idkhoa && tt.IdgiangVien == idGiangVien
                                 select new ThongTinHoatDongNgoaiKhoaModel2
                                 {
                                     Idtthdnk = tt.Idtthdnk,
                                     IdHdnk = hd.Idhdnk,
                                     IdDuLieu = dl.IdduLieuHdnk,
                                     TenNHHK = hk.TenNhhk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaKhoa = k.MaKhoa,
                                     HinhAnh = tt.HinhAnh,
                                     TenKhoa = k.TenKhoa,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     DiemHdnk = hd.Diemhdnk,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHDNK = hd.KyNangHdnk,
                                     TenGiangVien = gv.HoTen,
                                     TenPhong = p.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     PhamVi = tt.PhamVi,
                                     SoLuongThucTe = tt.SoLuongThucTe,
                                     SoLuongDuKien = tt.SoLuongDuKien,
                                     GhiChu = tt.GhiChu,
                                     BuoiUuTien = tt.BuoiUuTien,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     NgayBd = tt.NgayBđ,
                                     NgayKt = tt.NgayKt,
                                     IsCanPhong = tt.IsCanPhong,
                                     isCanMinhChung = tt.IsCanMinhChung,
                                     tinhTrangDuyet = tt.TinhTragDuyet,
                                     CreatedBy = tt.CreatedBy,
                                     CreatedTime = tt.CreatedTime
                                 }).ToList().Distinct().ToList();

                    resultList = query.Select(item => new ThongTinHoatDongNgoaiKhoaModel2
                    {
                        HinhAnh = item.HinhAnh,
                        Idtthdnk = item.Idtthdnk,
                        IdHdnk = item.IdHdnk,
                        IdDuLieu = item.IdDuLieu,
                        TenNHHK = item.TenNHHK,
                        TenBHNgChng = item.TenBHNgChng,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        DiemHdnk = item.DiemHdnk,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHDNK = item.KyNangHDNK,
                        TenGiangVien = item.TenGiangVien,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        PhamVi = item.PhamVi,
                        SoLuongThucTe = item.SoLuongThucTe,
                        SoLuongDuKien = item.SoLuongDuKien,
                        GhiChu = item.GhiChu,
                        BuoiUuTien = item.BuoiUuTien,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        IsCanPhong = item.IsCanPhong,
                        isCanMinhChung = item.isCanMinhChung,
                        tinhTrangDuyet = item.tinhTrangDuyet,
                        CreatedBy = item.CreatedBy,
                        CreatedTime = item.CreatedTime
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> GetByTenPhong(string TenPhong)
        {
            try
            {
                List<ThongTinHoatDongNgoaiKhoaModel2> resultList = new List<ThongTinHoatDongNgoaiKhoaModel2>();
                using (var context = new MyDBContext())
                {
                    long idPhong = context.Pdmphongs
                     .Where(phong => phong.TenPhong == TenPhong)
                     .Select(phong => phong.Idphong)
                     .FirstOrDefault();
                    var query = (from tt in context.Kdmtthdnks
                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into ttdl
                                 from dl in ttdl.DefaultIfEmpty()
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk into ttdlhd
                                 from hd in ttdlhd.DefaultIfEmpty()
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into ttdlhdnhhk
                                 from hk in ttdlhdnhhk.DefaultIfEmpty()
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into ttdlhdnhhkbhn
                                 from bhn in ttdlhdnhhkbhn.DefaultIfEmpty()
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ttdlhdnhhkbhnn
                                 from n in ttdlhdnhhkbhnn.DefaultIfEmpty()
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ttdlhdnhhkbhnnk
                                 from k in ttdlhdnhhkbhnnk.DefaultIfEmpty()
                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into ttgv
                                 from gv in ttgv.DefaultIfEmpty()
                                 join dd in context.PdmdiaDiems on tt.IddiaDiem equals dd.IddiaDiem into ttdd
                                 from dd in ttdd.DefaultIfEmpty()
                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ttp
                                 from p in ttp.DefaultIfEmpty()
                                 where k.Idkhoa == tt.Idkhoa && tt.Idphong == idPhong
                                 select new ThongTinHoatDongNgoaiKhoaModel2
                                 {

                                     Idtthdnk = tt.Idtthdnk,
                                     IdHdnk = hd.Idhdnk,
                                     IdDuLieu = dl.IdduLieuHdnk,
                                     TenNHHK = hk.TenNhhk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaKhoa = k.MaKhoa,
                                     TenKhoa = k.TenKhoa,
                                     MaHdnk = hd.MaHdnk,
                                     TenHdnk = hd.TenHdnk,
                                     DiemHdnk = hd.Diemhdnk,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     HinhAnh = tt.HinhAnh,
                                     KyNangHDNK = hd.KyNangHdnk,
                                     TenGiangVien = gv.HoTen,
                                     TenPhong = p.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     PhamVi = tt.PhamVi,
                                     SoLuongThucTe = tt.SoLuongThucTe,
                                     SoLuongDuKien = tt.SoLuongDuKien,
                                     GhiChu = tt.GhiChu,
                                     BuoiUuTien = tt.BuoiUuTien,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     NgayBd = tt.NgayBđ,
                                     NgayKt = tt.NgayKt,
                                     IsCanPhong = tt.IsCanPhong,
                                     isCanMinhChung = tt.IsCanMinhChung,
                                     tinhTrangDuyet = tt.TinhTragDuyet,
                                     CreatedBy = tt.CreatedBy,
                                     CreatedTime = tt.CreatedTime
                                 }).ToList().Distinct().ToList();

                    resultList = query.Select(item => new ThongTinHoatDongNgoaiKhoaModel2
                    {
                        Idtthdnk = item.Idtthdnk,
                        IdHdnk = item.IdHdnk,
                        IdDuLieu = item.IdDuLieu,
                        TenNHHK = item.TenNHHK,
                        TenBHNgChng = item.TenBHNgChng,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        DiemHdnk = item.DiemHdnk,
                        HinhAnh = item.HinhAnh,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHDNK = item.KyNangHDNK,
                        TenGiangVien = item.TenGiangVien,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        PhamVi = item.PhamVi,
                        SoLuongThucTe = item.SoLuongThucTe,
                        SoLuongDuKien = item.SoLuongDuKien,
                        GhiChu = item.GhiChu,
                        BuoiUuTien = item.BuoiUuTien,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        IsCanPhong = item.IsCanPhong,
                        isCanMinhChung = item.isCanMinhChung,
                        tinhTrangDuyet = item.tinhTrangDuyet,
                        CreatedBy = item.CreatedBy,
                        CreatedTime = item.CreatedTime
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> GetByTenDiaDiem(string tenDiaDiem)
        {
            try
            {
                List<ThongTinHoatDongNgoaiKhoaModel2> resultList = new List<ThongTinHoatDongNgoaiKhoaModel2>();
                using (var context = new MyDBContext())
                {
                    long idDiaDiem = context.PdmdiaDiems
                     .Where(diaDiem => diaDiem.TenDiaDiem == tenDiaDiem)
                     .Select(diaDiem => diaDiem.IddiaDiem)
                     .FirstOrDefault();
                    var query = (from tt in context.Kdmtthdnks
                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into ttdl
                                 from dl in ttdl.DefaultIfEmpty()
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk into ttdlhd
                                 from hd in ttdlhd.DefaultIfEmpty()
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into ttdlhdnhhk
                                 from hk in ttdlhdnhhk.DefaultIfEmpty()
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into ttdlhdnhhkbhn
                                 from bhn in ttdlhdnhhkbhn.DefaultIfEmpty()
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ttdlhdnhhkbhnn
                                 from n in ttdlhdnhhkbhnn.DefaultIfEmpty()
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ttdlhdnhhkbhnnk
                                 from k in ttdlhdnhhkbhnnk.DefaultIfEmpty()
                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into ttgv
                                 from gv in ttgv.DefaultIfEmpty()
                                 join dd in context.PdmdiaDiems on tt.IddiaDiem equals dd.IddiaDiem into ttdd
                                 from dd in ttdd.DefaultIfEmpty()
                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ttp
                                 from p in ttp.DefaultIfEmpty()
                                 where k.Idkhoa == tt.Idkhoa && tt.IddiaDiem == idDiaDiem

                                 select new ThongTinHoatDongNgoaiKhoaModel2
                                 {
                                     Idtthdnk = tt.Idtthdnk,
                                     IdDuLieu = dl.IdduLieuHdnk,
                                     IdHdnk = hd.Idhdnk,
                                     TenNHHK = hk.TenNhhk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaKhoa = k.MaKhoa,
                                     TenKhoa = k.TenKhoa,
                                     MaHdnk = hd.MaHdnk,
                                     HinhAnh = tt.HinhAnh,
                                     TenHdnk = hd.TenHdnk,
                                     DiemHdnk = hd.Diemhdnk,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHDNK = hd.KyNangHdnk,
                                     TenGiangVien = gv.HoTen,
                                     TenPhong = p.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     PhamVi = tt.PhamVi,
                                     SoLuongThucTe = tt.SoLuongThucTe,
                                     SoLuongDuKien = tt.SoLuongDuKien,
                                     GhiChu = tt.GhiChu,
                                     BuoiUuTien = tt.BuoiUuTien,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     NgayBd = tt.NgayBđ,
                                     NgayKt = tt.NgayKt,
                                     IsCanPhong = tt.IsCanPhong,
                                     isCanMinhChung = tt.IsCanMinhChung,
                                     tinhTrangDuyet = tt.TinhTragDuyet,
                                     CreatedBy = tt.CreatedBy,
                                     CreatedTime = tt.CreatedTime
                                 }).ToList().Distinct().ToList();

                    resultList = query.Select(item => new ThongTinHoatDongNgoaiKhoaModel2
                    {
                        Idtthdnk = item.Idtthdnk,
                        IdHdnk = item.IdHdnk,
                        IdDuLieu = item.IdDuLieu,
                        TenNHHK = item.TenNHHK,
                        TenBHNgChng = item.TenBHNgChng,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        DiemHdnk = item.DiemHdnk,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        HinhAnh = item.HinhAnh,
                        KyNangHDNK = item.KyNangHDNK,
                        TenGiangVien = item.TenGiangVien,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        PhamVi = item.PhamVi,
                        SoLuongThucTe = item.SoLuongThucTe,
                        SoLuongDuKien = item.SoLuongDuKien,
                        GhiChu = item.GhiChu,
                        BuoiUuTien = item.BuoiUuTien,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        IsCanPhong = item.IsCanPhong,
                        isCanMinhChung = item.isCanMinhChung,
                        tinhTrangDuyet = item.tinhTrangDuyet,
                        CreatedBy = item.CreatedBy,
                        CreatedTime = item.CreatedTime
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> GetByTenHdnk(string TenHdnk)
        {
            List<ThongTinHoatDongNgoaiKhoaModel2> resultList = new List<ThongTinHoatDongNgoaiKhoaModel2>();
            using (var context = new MyDBContext())
            {
                try
                {
                    long idHdnk = context.Kdmhdnks
                     .Where(hdnk => hdnk.TenHdnk == TenHdnk)
                     .Select(hdnk => hdnk.Idhdnk)
                     .FirstOrDefault();
                    var query = (from tt in context.Kdmtthdnks
                                 join dl in context.KdmduLieuHdnks on tt.Idhdnk equals dl.Idhdnk into ttdl
                                 from dl in ttdl.DefaultIfEmpty()
                                 join hd in context.Kdmhdnks on dl.Idhdnk equals hd.Idhdnk into ttdlhd
                                 from hd in ttdlhd.DefaultIfEmpty()
                                 join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into ttdlhdnhhk
                                 from hk in ttdlhdnhhk.DefaultIfEmpty()
                                 join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng into ttdlhdnhhkbhn
                                 from bhn in ttdlhdnhhkbhn.DefaultIfEmpty()
                                 join n in context.Kdmnghs on bhn.Idngh equals n.Idngh into ttdlhdnhhkbhnn
                                 from n in ttdlhdnhhkbhnn.DefaultIfEmpty()
                                 join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa into ttdlhdnhhkbhnnk
                                 from k in ttdlhdnhhkbhnnk.DefaultIfEmpty()
                                 join gv in context.NdmgiangViens on tt.IdgiangVien equals gv.IdgiangVien into ttgv
                                 from gv in ttgv.DefaultIfEmpty()
                                 join dd in context.PdmdiaDiems on tt.IddiaDiem equals dd.IddiaDiem into ttdd
                                 from dd in ttdd.DefaultIfEmpty()
                                 join p in context.Pdmphongs on tt.Idphong equals p.Idphong into ttp
                                 from p in ttp.DefaultIfEmpty()
                                 where k.Idkhoa == tt.Idkhoa && hd.Idhdnk == idHdnk
                                 select new ThongTinHoatDongNgoaiKhoaModel2
                                 {
                                     Idtthdnk = tt.Idtthdnk,
                                     IdHdnk = hd.Idhdnk,
                                     IdDuLieu = dl.IdduLieuHdnk,
                                     TenNHHK = hk.TenNhhk,
                                     TenBHNgChng = bhn.TenBhngChng,
                                     MaKhoa = k.MaKhoa,
                                     TenKhoa = k.TenKhoa,
                                     MaHdnk = hd.MaHdnk,
                                     HinhAnh = tt.HinhAnh,
                                     TenHdnk = hd.TenHdnk,
                                     DiemHdnk = hd.Diemhdnk,
                                     CoVu = hd.CoVu,
                                     BanToChuc = hd.BanToChuc,
                                     KyNangHDNK = hd.KyNangHdnk,
                                     TenGiangVien = gv.HoTen,
                                     TenPhong = p.TenPhong,
                                     TenDiaDiem = dd.TenDiaDiem,
                                     PhamVi = tt.PhamVi,
                                     SoLuongThucTe = tt.SoLuongThucTe,
                                     SoLuongDuKien = tt.SoLuongDuKien,
                                     GhiChu = tt.GhiChu,
                                     BuoiUuTien = tt.BuoiUuTien,
                                     ThoiLuongToChuc = tt.ThoiLuongToChuc,
                                     NgayBd = tt.NgayBđ,
                                     NgayKt = tt.NgayKt,
                                     IsCanPhong = tt.IsCanPhong,
                                     isCanMinhChung = tt.IsCanMinhChung,
                                     tinhTrangDuyet = tt.TinhTragDuyet,
                                     CreatedBy = tt.CreatedBy,
                                     CreatedTime = tt.CreatedTime
                                 }).ToList().Distinct().ToList();

                    resultList = query.Select(item => new ThongTinHoatDongNgoaiKhoaModel2
                    {
                        Idtthdnk = item.Idtthdnk,
                        IdHdnk = item.IdHdnk,
                        IdDuLieu = item.IdDuLieu,
                        TenNHHK = item.TenNHHK,
                        TenBHNgChng = item.TenBHNgChng,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        DiemHdnk = item.DiemHdnk,
                        HinhAnh = item.HinhAnh,
                        CoVu = item.CoVu,
                        BanToChuc = item.BanToChuc,
                        KyNangHDNK = item.KyNangHDNK,
                        TenGiangVien = item.TenGiangVien,
                        TenPhong = item.TenPhong,
                        TenDiaDiem = item.TenDiaDiem,
                        PhamVi = item.PhamVi,
                        SoLuongThucTe = item.SoLuongThucTe,
                        SoLuongDuKien = item.SoLuongDuKien,
                        GhiChu = item.GhiChu,
                        BuoiUuTien = item.BuoiUuTien,
                        ThoiLuongToChuc = item.ThoiLuongToChuc,
                        NgayBd = item.NgayBd,
                        NgayKt = item.NgayKt,
                        IsCanPhong = item.IsCanPhong,
                        isCanMinhChung = item.isCanMinhChung,
                        tinhTrangDuyet = item.tinhTrangDuyet,
                        CreatedBy = item.CreatedBy,
                        CreatedTime = item.CreatedTime
                    }).ToList().Distinct().ToList();
                    DataObject = resultList.Cast<object>().ToList();
                    Message = "Success!"; Code = 200;
                    return CreateResponse();
                }
                catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                {
                    // ID not found
                    Code = 404;
                    Message = "Thong tin hoat dong not found";
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

        public async Task<IActionResult> CreateTtHdnk([FromBody] ThongTinHoatDongNgoaiKhoaByMaModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long? idPhong = null;
                    long idKhoa = context.Ndmkhoas
                        .Where(khoa => khoa.TenKhoa == inputData.TenKhoa)
                        .Select(khoa => khoa.Idkhoa)
                        .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                        .Where(gv => gv.HoTen == inputData.TenGiangVien)
                        .Select(gv => gv.IdgiangVien)
                        .FirstOrDefault();
                    if (inputData.TenPhong != null && inputData.IsCanPhong)
                    {
                        idPhong = context.Pdmphongs
                   .Where(oh => oh.TenPhong == inputData.TenPhong)
                   .Select(oh => oh.Idphong)
                   .FirstOrDefault();
                    }
                    long idDiaDiem = context.PdmdiaDiems
                        .Where(dd => dd.TenDiaDiem == inputData.TenDiaDiem)
                        .Select(dd => dd.IddiaDiem)
                        .FirstOrDefault();
                    long idHdnk = context.Kdmhdnks
                        .Where(hd => hd.TenHdnk == inputData.TenHdnk)
                        .Select(hd => hd.Idhdnk)
                        .FirstOrDefault();
                    long idBhNgh = context.KdmbhngChngs
                       .Where(dd => dd.TenBhngChng == inputData.TenBhngChng)
                       .Select(dd => dd.IdbhngChng)
                       .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                        .Where(hd => hd.TenNhhk == inputData.TenNhhk)
                        .Select(hd => hd.Idnhhk)
                        .FirstOrDefault();
                    KdmduLieuHdnk duLieuData = new KdmduLieuHdnk()
                    {
                        IdduLieuHdnk = IdGenerator.NewUID,
                        IdbhngChng = idBhNgh,
                        Idnhhk = idNhhk,
                        Idhdnk = idHdnk,
                    };

                    Kdmtthdnk newData = new Kdmtthdnk()
                    {
                        Idtthdnk = IdGenerator.NewUID,
                        Idkhoa = idKhoa,
                        IdgiangVien = idGiangVien,
                        Idphong = idPhong,
                        IddiaDiem = idDiaDiem,
                        Idhdnk = idHdnk,
                        PhamVi = inputData.PhamVi,
                        SoLuongThucTe = inputData.SoLuongThucTe,
                        SoLuongDuKien = inputData.SoLuongDuKien,
                        GhiChu = inputData.GhiChu,
                        BuoiUuTien = inputData.BuoiUuTien,
                        ThoiLuongToChuc = inputData.ThoiLuongToChuc,
                        NgayBđ = inputData.NgayBd,
                        NgayKt = inputData.NgayKt,
                        IsCanPhong = inputData.IsCanPhong,
                        IsCanMinhChung = inputData.isCanMinhChung,
                        TinhTragDuyet = inputData.tinhTrangDuyet,
                        HinhAnh = inputData.HinhAnh,
                        LastUpdate = DateTime.Now,
                        CreatedTime = DateTime.Now,
                        CreatedBy = inputData.CreatedBy
                    };
                    await context.KdmduLieuHdnks.AddAsync(duLieuData);
                    await context.SaveChangesAsync();
                    await context.Kdmtthdnks.AddAsync(newData);
                    await context.SaveChangesAsync();
                    DataObject.Clear();
                    DataObject.Add(duLieuData);
                    DataObject.Add(newData);
                    Message = "Data created"; Code = 200;
                    return CreateResponse();
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "Thong tin hoat dong not found";
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
                Message = "An error occurred:" + ex.Message;
                return CreateResponse();
            }
        }

        public async Task<IActionResult> ChangeData(long id, [FromBody] ThongTinHoatDongNgoaiKhoaModelDatum inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    long? idPhong = null;
                    long idKhoa = context.Ndmkhoas
                          .Where(khoa => khoa.TenKhoa == inputData.TenKhoa)
                          .Select(khoa => khoa.Idkhoa)
                          .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                        .Where(gv => gv.HoTen == inputData.TenGiangVien)
                        .Select(gv => gv.IdgiangVien)
                        .FirstOrDefault();
                    if (inputData.TenPhong != null && inputData.IsCanPhong)
                    {
                        idPhong = context.Pdmphongs
                       .Where(oh => oh.TenPhong == inputData.TenPhong)
                       .Select(oh => oh.Idphong)
                       .FirstOrDefault();
                    }
                    long idDiaDiem = context.PdmdiaDiems
                        .Where(dd => dd.TenDiaDiem == inputData.TenDiaDiem)
                        .Select(dd => dd.IddiaDiem)
                        .FirstOrDefault();
                    long idHdnk = context.Kdmhdnks
                        .Where(hd => hd.TenHdnk == inputData.TenHdnk)
                        .Select(hd => hd.Idhdnk)
                        .FirstOrDefault();
                    var existing = context.Kdmtthdnks.Find(id);
                    if (existing == null)
                    {
                        return NotFound();
                    }
                    existing.Idkhoa = idKhoa;
                    existing.IdgiangVien = idGiangVien;
                    existing.Idphong = idPhong;
                    existing.IddiaDiem = idDiaDiem;
                    existing.Idhdnk = idHdnk;
                    existing.PhamVi = inputData.PhamVi;
                    existing.SoLuongThucTe = inputData.SoLuongThucTe;
                    existing.SoLuongDuKien = inputData.SoLuongDuKien;
                    existing.GhiChu = inputData.GhiChu;
                    existing.BuoiUuTien = inputData.BuoiUuTien;
                    existing.ThoiLuongToChuc = inputData.ThoiLuongToChuc;
                    existing.NgayBđ = inputData.NgayBd;
                    existing.NgayKt = inputData.NgayKt;
                    existing.IsCanMinhChung = inputData.isCanMinhChung;
                    existing.TinhTragDuyet = inputData.tinhTrangDuyet;
                    existing.IsCanPhong = inputData.IsCanPhong;
                    existing.HinhAnh = inputData.HinhAnh;
                    existing.LastUpdate = DateTime.Now;
                    context.Kdmtthdnks.Update(existing);
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
                Message = "Thong tin hoat dong not found";
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
                    var data = await context.Kdmtthdnks.FindAsync(id);
                    if (data == null)
                    {
                        return Ok("data not exist");
                    }
                    if (data.TinhTragDuyet == true)
                    {
                        return Ok("Data has been approved");
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
                Message = "Thong tin hoat dong not found";
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