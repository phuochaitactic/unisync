using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IViewSlsinhVienThamGiaHdtheoHKService
    {
        Task<IActionResult> GetAll();
    }

    public class ViewSlsinhVienThamGiaHdtheoHKService : BaseController, IViewSlsinhVienThamGiaHdtheoHKService
    {
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ViewSlsinhVienThamGiaHdtheoHk> resultList = new List<ViewSlsinhVienThamGiaHdtheoHk>();
                using (var context = new MyDBContext())
                {
                    var query = (from sv in context.ViewSlsinhVienThamGiaHdtheoHks
                                 select new ViewSlsinhVienThamGiaHdtheoHk
                                 {
                                     Idnhhk = sv.Idnhhk,
                                     MaNhhk = sv.MaNhhk,
                                     TenNhhk = sv.TenNhhk,
                                     Idkhoa = sv.Idkhoa,
                                     TenKhoa = sv.TenKhoa,
                                     Idhdnk = sv.Idhdnk,
                                     MaHdnk = sv.MaHdnk,
                                     TenHdnk = sv.TenHdnk,
                                     Diemhdnk = sv.Diemhdnk,
                                     Slsv = sv.Slsv,
                                 }).ToList();
                    resultList = query.Select(item => new ViewSlsinhVienThamGiaHdtheoHk()
                    {
                        Idnhhk = item.Idnhhk,
                        MaNhhk = item.MaNhhk,
                        Idkhoa = item.Idkhoa,
                        TenKhoa = item.TenKhoa,
                        Idhdnk = item.Idhdnk,
                        MaHdnk = item.MaHdnk,
                        TenHdnk = item.TenHdnk,
                        Diemhdnk = item.Diemhdnk,
                        Slsv = item.Slsv,
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