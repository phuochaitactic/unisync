using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMLichDuyetSVController : ControllerBase
    {
        private readonly ILichDuyetSVService _LichDuyetSVService;
        private readonly IAuthService _auth;

        public KDMLichDuyetSVController(ILichDuyetSVService lichDuyetSVService, IAuthService auth)
        {
            this._LichDuyetSVService = lichDuyetSVService;
            this._auth = auth;
        }

        // GET: api/<KDMLichDuyetSVController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var lichDuyetSV = await _LichDuyetSVService.GetAll();
                var response = (lichDuyetSV as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        [HttpGet("TheoKhoa")]
        public async Task<object> GetByTenKhoa(string TenKhoa, string TenNhhk, string TenBHN)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var lich = await _LichDuyetSVService.GetTheoKhoa(TenKhoa, TenNhhk, TenBHN);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        // POST api/<KDMLichDuyetSVController>
        [HttpPost]
        public async Task<object> Post([FromBody] LichDuyetSVModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var lichDuyetSV = await _LichDuyetSVService.CreateLichDuyetSV(inputData);
                var response = (lichDuyetSV as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // PUT api/<KDMLichDuyetSVController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] LichDuyetSVModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var lichDuyetSV = await _LichDuyetSVService.ChangeData(id, inputData);
                var response = (lichDuyetSV as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // DELETE api/<KDMLichDuyetSVController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var lichDuyetSV = await _LichDuyetSVService.Delete(id);
                var response = (lichDuyetSV as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}