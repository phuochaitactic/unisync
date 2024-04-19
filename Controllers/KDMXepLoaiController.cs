using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMXepLoaiController : ControllerBase
    {
        protected readonly IXepLoaiService _xepLoaiService;
        protected readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDMXepLoaiController"/> class.
        /// </summary>
        /// <param name="XepLoaiService">The service for managing academic semesters.</param>
        /// <param name="auth">The authentication service.</param>
        public KDMXepLoaiController(IXepLoaiService xepLoaiService, IAuthService auth)
        {
            this._xepLoaiService = xepLoaiService;
            this._auth = auth;
        }

        // GET: api/<KDMXepLoaiController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _xepLoaiService.GetAll();
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // GET api/<KDMXepLoaiController>/5
        [HttpGet("{tenVanBan}")]
        public async Task<object> Get(string tenVanBan)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _xepLoaiService.GetbyTenVanBan(tenVanBan);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // POST api/<KDMXepLoaiController>
        [HttpPost]
        public async Task<object> Post([FromBody] XepLoaiModel inputData)
        {
            if (_auth.ValidateAdmin(this))
            {
                var ttHdnk = await _xepLoaiService.CreateXepLoai(inputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // PUT api/<KDMXepLoaiController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] XepLoaiModel inputData)
        {
            if (_auth.ValidateAdmin(this))
            {
                var ttHdnk = await _xepLoaiService.ChangeData(id, inputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // DELETE api/<KDMXepLoaiController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this))
            {
                var ttHdnk = await _xepLoaiService.Delete(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }
    }
}