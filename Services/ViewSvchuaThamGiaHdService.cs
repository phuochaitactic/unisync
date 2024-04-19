using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IViewSvchuaThamGiaHdService
    {
        Task<IActionResult> GetAll();
    }

    public class ViewSvchuaThamGiaHdService : BaseController, IViewSvchuaThamGiaHdService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ViewSvchuaThamGiaHd> resultList = new List<ViewSvchuaThamGiaHd>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.ViewSvchuaThamGiaHds
                                 select new ViewSvchuaThamGiaHd
                                 {
                                     Idnhhk = sv.Idnhhk,
                                     MaNhhk = sv.MaNhhk,
                                     Idkhoa = sv.Idkhoa,
                                     MaKhoa = sv.MaKhoa,
                                     TenKhoa = sv.TenKhoa,
                                     SlsvtheoCtdt = sv.SlsvtheoCtdt,
                                     Idhdnk = sv.Idhdnk,
                                     MaHdnk = sv.MaHdnk,
                                     TenHdnk = sv.TenHdnk,
                                     Diemhdnk = sv.Diemhdnk,
                                     SlsvdaTungThamGia = sv.SlsvdaTungThamGia,
                                     SlsvchuaThamGia = sv.SlsvchuaThamGia
                                 }).ToList();
                    resultList = query.Select(item => new ViewSvchuaThamGiaHd()
                    {
                        Idnhhk = item.Idnhhk,
                        MaNhhk = item.MaNhhk,
                        Idkhoa = item.Idkhoa,
                        MaKhoa = item.MaKhoa,
                        TenKhoa = item.TenKhoa,
                        SlsvtheoCtdt = item.SlsvtheoCtdt,
                        Idhdnk = item.Idhdnk,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        SlsvdaTungThamGia = item.SlsvdaTungThamGia,
                        SlsvchuaThamGia = item.SlsvchuaThamGia
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