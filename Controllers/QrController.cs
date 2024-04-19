using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        private readonly IQrService _qrService;
        private readonly IAuthService _auth;

        public QrController(IQrService qrService, IAuthService auth)
        {
            this._qrService = qrService;
            this._auth = auth;
        }

        // GET: api/<QrController>
        [HttpGet("LayPathLogin")]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var qrcode = await _qrService.GuiLinkDiemDanh();
                var response = (qrcode as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        // PUT api/<QrController>/5
        [HttpPut]
        public async Task<object> Put([FromBody] QrCodeModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateAdmin(this))
            {
                var qrcode = await _qrService.UpdateSinhVienDiemDanh(inputData);
                var response = (qrcode as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}