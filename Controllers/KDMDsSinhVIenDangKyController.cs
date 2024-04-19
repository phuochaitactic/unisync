using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMDsSinhVIenDangKyController : ControllerBase
    {
        private readonly IKdmdssvdkService _kdmdssvdkService;
        private readonly IAuthService _auth;

        public KDMDsSinhVIenDangKyController(IKdmdssvdkService bacHeNganhService, IAuthService auth)
        {
            this._kdmdssvdkService = bacHeNganhService;
            this._auth = auth;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var data = await _kdmdssvdkService.GetAll();
                var response = (data as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        [HttpGet("TheoSinhVien")]
        public async Task<object> GetGetByIdSinhVien(long idSinhVien)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var data = await _kdmdssvdkService.GetByIdSinhVien(idSinhVien);
                var response = (data as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET: api/<KDMDsSinhVIenDangKyController>
        [HttpGet("{maSinhVien}/{maNhhk}")]
        public async Task<object> Get(string maSinhVien, string maNhhk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var data = await _kdmdssvdkService.GetDsSinhVienDangKy(maSinhVien, maNhhk);
                var response = (data as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // POST api/<KDMDsSinhVIenDangKyController>
        [HttpPost]
        public async Task<object> Post([FromBody] GiangVienComment inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateKhoa(this))
            {
                var data = await _kdmdssvdkService.CreateData(inputData);
                var response = (data as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // PUT api/<KDMDsSinhVIenDangKyController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, string loiNhan)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var data = await _kdmdssvdkService.ChangeData(id, loiNhan);
                var response = (data as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}