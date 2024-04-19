using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Services
{
    public interface IBangDiemService
    {
        Task<IActionResult> GetSinhVienByLopAndHocKy(string MaLop, string NamHoc);

        Task<IActionResult> GetHdnkTheoCtdtCuaSv(string MaSinhVien);

        Task<IActionResult> GetTongDiemSinhVienTrongHocKy(string MaNhanVien);
    }

    public class BangDiemService : BaseController, IBangDiemService
    {
        public async Task<IActionResult> GetSinhVienByLopAndHocKy(string MaLop, string NamHoc)
        {
            using (var context = new MyDBContext())
            {
                var query = (from kq in context.Kkqsvdkhdnks
                             join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
                             join l in context.Sdmlops on sv.Idlop equals l.Idlop
                             join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk
                             join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                             join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                             join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk
                             join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk
                             where l.MaLop == MaLop && hk.TenNhhk.Contains(NamHoc)
                             group new
                             {
                                 sv,
                                 l,
                                 d,
                                 hk,
                                 kq
                             } by new
                             {
                                 sv.HoTenSinhVien,
                                 hk.TenNhhk,
                                 l.MaLop,
                                 d.MaDieu,
                                 d.DiemToiDa,
                                 d.DiemCoBan
                             } into g
                             select new BangDiemModel
                             {
                                 HoTenSinhVien = g.Key.HoTenSinhVien,
                                 MaLop = g.Key.MaLop,
                                 MaDieu = g.Key.MaDieu,
                                 HOCKY = g.Key.TenNhhk.Substring(0, 8),
                                 TONGDRL = g.Sum(x => x.kq.SoDiem),
                                 DANHGIA = g.Key.DiemCoBan <= g.Sum(x => x.kq.SoDiem) && g.Sum(x => x.kq.SoDiem) <= g.Key.DiemToiDa ? "Đạt" : "Không Đạt"
                             }).ToList();
                DataObject = query.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
        }

        public async Task<IActionResult> GetHdnkTheoCtdtCuaSv(string MaSinhVien)
        {
            using (var context = new MyDBContext())
            {
                var query = (from hd in context.Kdmhdnks
                             join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                             join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                             join tt in context.Kdmtthdnks on hd.Idhdnk equals tt.Idhdnk
                             join dl in context.KdmduLieuHdnks on hd.Idhdnk equals dl.Idhdnk
                             join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk
                             join bhn in context.KdmbhngChngs on dl.IdbhngChng equals bhn.IdbhngChng
                             join l in context.Sdmlops on bhn.IdbhngChng equals l.IdbhngChng
                             join sv in context.Sdmsvs on l.Idlop equals sv.Idlop
                             join n in context.Kdmnghs on bhn.Idngh equals n.Idngh
                             join k in context.Ndmkhoas on n.Idkhoa equals k.Idkhoa
                             join kkq in context.Kkqsvdkhdnks on new { sv.IdsinhVien, hd.Idhdnk }
                                equals new { kkq.IdsinhVien, kkq.Idhdnk } into kj
                             from kkq in kj.DefaultIfEmpty()

                             where sv.MaSinhVien == MaSinhVien
                             select new
                             {
                                 hd.MaHdnk,
                                 hd.TenHdnk,
                                 d.MaDieu,
                                 DIEM = kkq != null ? kkq.SoDiem : 0,
                                 ISTHAMGIA = kkq != null ? "THAM GIA" : "KHÔNG THAM GIA",
                                 VAITROTG = kkq != null ? kkq.VaiTroTg : "NULL"
                             }).ToList();
                DataObject = query.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
        }

        public async Task<IActionResult> GetTongDiemSinhVienTrongHocKy(string MaNhanVien)
        {
            using (var context = new MyDBContext())
            {
                var query = (from kq in context.Kkqsvdkhdnks
                             join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
                             join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk into svdl
                             from dl in svdl.DefaultIfEmpty()
                             join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into dlhk
                             from hk in dlhk.DefaultIfEmpty()
                             join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
                             join l in context.Sdmlops on sv.Idlop equals l.Idlop
                             join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk
                             join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
                             join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
                             where kq.IsThamGia == true && gv.MaNv == MaNhanVien
                             group new { sv, l, d, kq } by new { sv.IdsinhVien, sv.HoTenSinhVien, l.MaLop, d.MaDieu } into g
                             join d in context.Kdmdieus on g.Key.MaDieu equals d.MaDieu into gd
                             from d in gd.DefaultIfEmpty()
                             select new
                             {
                                 IDSinhVien = g.Key.IdsinhVien,
                                 HoTenSinhVien = g.Key.HoTenSinhVien,
                                 MaLop = g.Key.MaLop,
                                 MaDieu = g.Key.MaDieu,
                                 TONGDRL = g.Sum(x => x.kq.SoDiem),
                                 DANHGIA = d.DiemCoBan <= g.Sum(x => x.kq.SoDiem) && g.Sum(x => x.kq.SoDiem) <= d.DiemToiDa ? "Đạt" : "Không Đạt"
                             }).ToList();
                DataObject = query.Cast<object>().ToList();
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
        }

        //public async Task<IActionResult> GetTongDiemSinhVienTrongHocKy(string MaNhanVien)
        //{
        //    using (var context = new MyDBContext())
        //    {
        //        var query = (from kq in context.Kkqsvdkhdnks
        //                     join sv in context.Sdmsvs on kq.IdsinhVien equals sv.IdsinhVien
        //                     join dl in context.KdmduLieuHdnks on kq.Idhdnk equals dl.Idhdnk into svdl
        //                     from dl in svdl.DefaultIfEmpty()
        //                     join hk in context.Kdmnhhks on dl.Idnhhk equals hk.Idnhhk into dlhk
        //                     from hk in dlhk.DefaultIfEmpty()
        //                     join gv in context.NdmgiangViens on sv.IdgiangVien equals gv.IdgiangVien
        //                     join l in context.Sdmlops on sv.Idlop equals l.Idlop
        //                     join hd in context.Kdmhdnks on kq.Idhdnk equals hd.Idhdnk
        //                     join mc in context.KdmndminhChungs on hd.IdminhChung equals mc.IdminhChung
        //                     join d in context.Kdmdieus on mc.Iddieu equals d.Iddieu
        //                     where kq.IsThamGia == true && gv.MaNv == MaNhanVien
        //                     group new { sv, l, d, kq } by new { sv.IdsinhVien, sv.HoTenSinhVien, l.MaLop, d.MaDieu } into g
        //                     join d in context.Kdmdieus on g.Key.MaDieu equals d.MaDieu into gd
        //                     from d in gd.DefaultIfEmpty()
        //                     select new
        //                     {
        //                         IDSinhVien = g.Key.IdsinhVien,
        //                         HoTenSinhVien = g.Key.HoTenSinhVien,
        //                         MaLop = g.Key.MaLop,
        //                         MaDieu = g.Key.MaDieu,
        //                         TONGDRL = g.Sum(x => x.kq.SoDiem),
        //                         DANHGIA = d.DiemCoBan <= g.Sum(x => x.kq.SoDiem) && g.Sum(x => x.kq.SoDiem) <= d.DiemToiDa ? "Đạt" : "Không Đạt"
        //                     }).ToList();
        //        DataObject = query.Cast<object>().ToList();
        //        Message = "Success!"; Code = 200;
        //        return CreateResponse();
        //    }
        //}
    }
}