using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMDuLieuHdnkController : ControllerBase
    {
        protected readonly IDuLieuHdnkService _duLieuHdnkService;
        protected readonly IAuthService _auth;

        public KDMDuLieuHdnkController(IDuLieuHdnkService duLieuHdnkService, IAuthService auth)
        {
            this._duLieuHdnkService = duLieuHdnkService;
            this._auth = auth;
        }

        // GET: api/<KDMDuLieuHdnk>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var duLieu = await _duLieuHdnkService.GetAll();
                var response = (duLieu as ObjectResult)?.Value;

                return response;
            }
            return Unauthorized("Unauthorized");
        }

        [HttpGet("{idDuLieu}")]
        public async Task<object> Get(long idDuLieu)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var duLieu = await _duLieuHdnkService.GetById(idDuLieu);
                var response = (duLieu as ObjectResult)?.Value;

                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // POST api/<KDMDuLieuHdnk>
        [HttpPost]
        public async Task<object> Post([FromBody] DuLieuHdnkModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var bache = await _duLieuHdnkService.CreateDuLieu(inputData);

                var response = (bache as ObjectResult)?.Value;

                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // PUT api/<KDMDuLieuHdnk>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] KdmduLieuHdnk inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var bache = await _duLieuHdnkService.ChangeData(id, inputData);

                var response = (bache as ObjectResult)?.Value;

                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // DELETE api/<KDMDuLieuHdnk>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(int id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateKhoa(this))
            {
                var bache = await _duLieuHdnkService.Delete(id);

                var response = (bache as ObjectResult)?.Value;

                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}