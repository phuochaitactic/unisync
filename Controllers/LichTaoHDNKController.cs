using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichTaoHDNKController : ControllerBase
    {
        private readonly ILichTaoHdnkService _lichTaoHdnkService;
        private readonly IAuthService _auth;

        public LichTaoHDNKController(ILichTaoHdnkService _lichTaoHdnkService, IAuthService _auth)
        {
            this._lichTaoHdnkService = _lichTaoHdnkService;
            this._auth = _auth;
        }

        // GET: api/<LichTaoHDNKController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _lichTaoHdnkService.GetAll();
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("TheoKhoa")]
        public async Task<object> GetByTenKhoa(string TenKhoa, string TenNhhk, string TenBHN)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var lich = await _lichTaoHdnkService.GetByKhoa(TenKhoa, TenNhhk, TenBHN);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        // POST api/<LichTaoHDNKController>
        [HttpPost]
        public async Task<object> Post([FromBody] KDMLichTaoHDNKModel InputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _lichTaoHdnkService.CreateData(InputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // PUT api/<LichTaoHDNKController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] KDMLichTaoHDNKModel InputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _lichTaoHdnkService.ChangeData(id, InputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // DELETE api/<LichTaoHDNKController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _lichTaoHdnkService.Delete(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}