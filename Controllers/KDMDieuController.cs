using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMDieuController : ControllerBase
    {
        private readonly IDieuService _dieuService;
        private readonly IAuthService _auth;

        public KDMDieuController(IDieuService dieuService, IAuthService auth)
        {
            this._dieuService = dieuService;
            this._auth = auth;
        }

        // GET: api/<KDMDieuController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bacHeNganh = await _dieuService.GetAll();
                var response = (bacHeNganh as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<KDMDieuController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(long id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var bacHeNganh = await _dieuService.GetById(id);
                var response = (bacHeNganh as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // POST api/<KDMDieuController>
        [HttpPost]
        public async Task<object> Post([FromBody] DieuModel inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var bacHeNganh = await _dieuService.CreateDieu(inputData);
                var response = (bacHeNganh as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // PUT api/<KDMDieuController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] DieuModel inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var bacHeNganh = await _dieuService.ChangeData(id, inputData);
                var response = (bacHeNganh as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // DELETE api/<KDMDieuController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this))
            {
                var bacHeNganh = await _dieuService.Delete(id);
                var response = (bacHeNganh as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}