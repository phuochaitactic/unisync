using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface ITaoHdnkService
    {
        Task<IActionResult> CreateHdnk([FromBody] TaoHdnkModel inputData);
    }

    public class TaoHdnkService : BaseController, ITaoHdnkService
    {
        public async Task<IActionResult> CreateHdnk([FromBody] TaoHdnkModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    TaoHdnkModelResult result = new TaoHdnkModelResult();
                    long? idPhong = null;
                    long idKhoa = context.Ndmkhoas
                        .Where(khoa => khoa.TenKhoa == inputData.tthdnk.TenKhoa)
                        .Select(khoa => khoa.Idkhoa)
                        .FirstOrDefault();
                    long idGiangVien = context.NdmgiangViens
                        .Where(gv => gv.HoTen == inputData.tthdnk.TenGiangVien)
                        .Select(gv => gv.IdgiangVien)
                        .FirstOrDefault();
                    if (inputData.tthdnk.TenPhong != null && inputData.tthdnk.IsCanPhong)
                    {
                        idPhong = context.Pdmphongs
                   .Where(oh => oh.TenPhong == inputData.tthdnk.TenPhong)
                   .Select(oh => oh.Idphong)
                   .FirstOrDefault();
                    }
                    long idDiaDiem = context.PdmdiaDiems
                        .Where(dd => dd.TenDiaDiem == inputData.tthdnk.TenDiaDiem)
                        .Select(dd => dd.IddiaDiem)
                        .FirstOrDefault();
                    long idHdnk = context.Kdmhdnks
                        .Where(hd => hd.TenHdnk == inputData.tthdnk.TenHdnk)
                        .Select(hd => hd.Idhdnk)
                        .FirstOrDefault();
                    long idBhNgh = context.KdmbhngChngs
                       .Where(dd => dd.TenBhngChng == inputData.tthdnk.TenBHNgChng)
                       .Select(dd => dd.IdbhngChng)
                       .FirstOrDefault();
                    long idNhhk = context.Kdmnhhks
                        .Where(hd => hd.TenNhhk == inputData.tthdnk.TenNHHK)
                        .Select(hd => hd.Idnhhk)
                        .FirstOrDefault();
                    long IdMinhChung = context.KdmndminhChungs
                                .Where(mc => mc.NoiDungMinhChung == inputData.hoatDongNgoaiKhoa.NoiDungMinhChung)
                                .Select(mc => mc.IdminhChung)
                                .FirstOrDefault();
                    var isHdnk = (context.Kdmhdnks)
                        .Where(hd => hd.MaHdnk == inputData.hoatDongNgoaiKhoa.MaHdnk)
                        .Where(hd => hd.TenHdnk == inputData.hoatDongNgoaiKhoa.TenHdnk)
                        .Select(hd => hd.MaHdnk);
                    if (isHdnk != null)
                    {
                        Code = 500;
                        Message = "hoạt động đã tồn tại";
                        return CreateResponse();
                    }
                    Kdmhdnk newHdnk = new Kdmhdnk()
                    {
                        Idhdnk = IdGenerator.NewUID,
                        IdminhChung = IdMinhChung,
                        MaHdnk = inputData.hoatDongNgoaiKhoa.MaHdnk,
                        TenHdnk = inputData.hoatDongNgoaiKhoa.TenHdnk,
                        Diemhdnk = inputData.hoatDongNgoaiKhoa.Diemhdnk,
                        CoVu = inputData.hoatDongNgoaiKhoa.CoVu,
                        BanToChuc = inputData.hoatDongNgoaiKhoa.BanToChuc,
                        KyNangHdnk = inputData.hoatDongNgoaiKhoa.KyNangHdnk,
                    };
                    KdmduLieuHdnk duLieuData = new KdmduLieuHdnk()
                    {
                        IdduLieuHdnk = IdGenerator.NewUID,
                        IdbhngChng = idBhNgh,
                        Idnhhk = idNhhk,
                        Idhdnk = idHdnk,
                    };

                    Kdmtthdnk newTthdnk = new Kdmtthdnk()
                    {
                        Idtthdnk = IdGenerator.NewUID,
                        Idkhoa = idKhoa,
                        IdgiangVien = idGiangVien,
                        Idphong = idPhong,
                        IddiaDiem = idDiaDiem,
                        Idhdnk = idHdnk,
                        PhamVi = inputData.tthdnk.PhamVi,
                        SoLuongThucTe = inputData.tthdnk.SoLuongThucTe,
                        SoLuongDuKien = inputData.tthdnk.SoLuongDuKien,
                        GhiChu = inputData.tthdnk.GhiChu,
                        BuoiUuTien = inputData.tthdnk.BuoiUuTien,
                        ThoiLuongToChuc = inputData.tthdnk.ThoiLuongToChuc,
                        NgayBđ = inputData.tthdnk.NgayBd,
                        NgayKt = inputData.tthdnk.NgayKt,
                        IsCanPhong = inputData.tthdnk.IsCanPhong,
                        IsCanMinhChung = inputData.tthdnk.isCanMinhChung,
                        TinhTragDuyet = inputData.tthdnk.tinhTrangDuyet,
                        LastUpdate = DateTime.Now,
                        CreatedTime = DateTime.Now,
                        CreatedBy = inputData.tthdnk.CreatedBy
                    };
                    result.hoatDongNgoaiKhoa = newHdnk;
                    result.duLieuHdnk = duLieuData;
                    result.tthdnk = newTthdnk;
                    await context.Kdmhdnks.AddAsync(newHdnk);
                    await context.SaveChangesAsync();
                    await context.KdmduLieuHdnks.AddAsync(duLieuData);
                    await context.SaveChangesAsync();
                    await context.Kdmtthdnks.AddAsync(newTthdnk);
                    await context.SaveChangesAsync();
                    DataObject.Clear();
                    DataObject.Add(result);
                    Message = "Data created"; Code = 200;
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