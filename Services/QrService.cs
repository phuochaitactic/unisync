using BuildCongRenLuyen.Controllers.Base;
using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BuildCongRenLuyen.Services
{
    public interface IQrService
    {
        Task<IActionResult> UpdateSinhVienDiemDanh([FromBody] QrCodeModel inputData);

        Task<IActionResult> GuiLinkDiemDanh();
    }

    public class QrService : BaseController, IQrService
    {
        public async Task<IActionResult> GuiLinkDiemDanh()
        {
            try
            {
                string data = "/login/";
                DataObject.Add(data);
                Message = "Success!"; Code = 200;
                return CreateResponse();
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // ID not found
                Code = 404;
                Message = "data not found";
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

        public async Task<IActionResult> UpdateSinhVienDiemDanh([FromBody] QrCodeModel inputData)
        {
            try
            {
                using (var context = new MyDBContext())
                {
                    Sdmsv SinhVien = context.Sdmsvs
                             .Where(sinhVien => sinhVien.IdsinhVien == inputData.IdSinhVien)
                             .Select(sinhVien => sinhVien)
                             .FirstOrDefault();
                    if (SinhVien == null)
                    {
                        Message = "sinh viên không tồn tại";
                        Code = 500;
                        return CreateResponse();
                    }
                    long idKkqSv = context.Kkqsvdkhdnks
                             .Where(sinhVien => sinhVien.IdsinhVien == inputData.IdSinhVien)
                             .Where(sinhVien => sinhVien.Idhdnk == inputData.idHdnk)
                             .Select(sinhVien => sinhVien.IddangKy)
                             .FirstOrDefault();
                    var existing = context.Kkqsvdkhdnks.Find(idKkqSv);
                    if (existing == null)
                    {
                        Message = "Sinh viên chưa đăng kí hoạt động ngoại khóa"; Code = 500;
                        return CreateResponse();
                    }
                    if (existing.TinhTrangDuyet != true)
                    {
                        Message = "Hoạt động của bạn chưa được duyệt"; Code = 500;
                        return CreateResponse();
                    }
                    existing.IsThamGia = true;
                    existing.NgayThamGia = inputData.NgayThamGia;
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