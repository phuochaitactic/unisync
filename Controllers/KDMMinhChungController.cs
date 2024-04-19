using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMMinhChungController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IMinhChungService _minhChungService;
        private readonly IAccountService _accountService;

        public KDMMinhChungController(IAuthService auth, IMinhChungService _minhChungService, IAccountService accountService
            )
        {
            this._minhChungService = _minhChungService;
            this._auth = auth;
            this._accountService = accountService;
        }

        // GET: api/<KDMMinhChungController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var minhChung = await _minhChungService.GetAll();
                var response = (minhChung as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // GET api/<KDMMinhChungController>/5
        [HttpGet("maDieu/{maDieu}")]
        public async Task<object> GetByMaDieu(string maDieu)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var minhChung = await _minhChungService.GetByMaDieu(maDieu);
                var response = (minhChung as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // GET api/<KDMMinhChungController>/5
        [HttpGet("MaLoaiHdnk/{MaLoaiHdnk}")]
        public async Task<object> GetByMaLoaiHdnk(string maLoaiHdnk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var minhChung = await _minhChungService.GetByMaLoaiHdnk(maLoaiHdnk);
                var response = (minhChung as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // POST api/<KDMMinhChungController>
        [HttpPost]
        public async Task<object> Post(string maDieu, string MaLoaiHdnk, [FromBody] MinhChungModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var minhChung = await _minhChungService.CreateMinhChung(inputData);
                var response = (minhChung as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        // PUT api/<KDMMinhChungController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, string maDieu, string MaLoaiHdnk, [FromBody] MinhChungModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var minhChung = await _minhChungService.ChangeData(id, inputData);
                var response = (minhChung as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        // DELETE api/<KDMMinhChungController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this))
            {
                var minhChung = await _minhChungService.Delete(id);
                var response = (minhChung as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }
    }
}