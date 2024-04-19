using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KkqSvDkHdnkController : ControllerBase
    {
        protected readonly IKkqSvDkHdnkService _kqSvDkHdnkService;
        protected readonly IAuthService _auth;

        public KkqSvDkHdnkController(IKkqSvDkHdnkService kqSvDkHdnkService, IAuthService auth)
        {
            this._kqSvDkHdnkService = kqSvDkHdnkService;
            this._auth = auth;
        }

        // GET: api/<KkqSvDkHdnkController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.GetAll();
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // GET api/<KkqSvDkHdnkController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.GetById(id);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        [HttpGet("sinhVien/{id}")]
        public async Task<object> GetBySinhVienId(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.GetByIdSinhVien(id);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        [HttpGet("DsSinhVienThamGia")]
        public async Task<object> GetBySinhVienId(long IdHdnk, long? IdSinhVien)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.GetDsSinhVienThamGia(IdHdnk, IdSinhVien);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        [HttpGet("SuKienSinhVien")]
        public async Task<object> GetSuKienSinhVien(long id, string TenNhhk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.GetByIdSinhVienAndIsThamGia(id, TenNhhk);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        [HttpGet("DsSinhVienTheoHdnk")]
        public async Task<object> GetDsSinhVienTheoHdnk(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.GetSinhVienThamGiaByHdnk(id);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // POST api/<KkqSvDkHdnkController>
        [HttpPost]
        public async Task<object> Post([FromBody] KkqSvDkHdnkModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateSinhVien(this))
            {
                var bache = await _kqSvDkHdnkService.CreateKkq(inputData);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // PUT api/<KkqSvDkHdnkController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] KkqSvDkHdnkModel inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateKhoa(this))
            {
                var bache = await _kqSvDkHdnkService.ChangeData(id, inputData);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        [HttpPut("thamGia")]
        public async Task<object> PutIsThamGia([FromBody] KkqIsThamGiaModel inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateKhoa(this))
            {
                var bache = await _kqSvDkHdnkService.ChangeIsThamGia(inputData);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        // DELETE api/<KkqSvDkHdnkController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateKhoa(this) || _auth.ValidateSinhVien(this) || _auth.ValidateGiangVien(this))
            {
                var bache = await _kqSvDkHdnkService.Delete(id);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}