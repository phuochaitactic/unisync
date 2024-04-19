using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewSlsinhVienThamGiaHdtheoHKController : ControllerBase
    {
        protected readonly IViewSlsinhVienThamGiaHdtheoHKService _service;
        protected readonly IAuthService _auth;

        public ViewSlsinhVienThamGiaHdtheoHKController(IViewSlsinhVienThamGiaHdtheoHKService service, IAuthService auth)
        {
            this._service = service;
            this._auth = auth;
        }

        // GET: api/<ViewSlsinhVienThamGiaHdtheoHKController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var hdNK = await _service.GetAll();
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}