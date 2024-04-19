using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BangDiemController : ControllerBase
    {
        private IBangDiemService _bangDiemService;
        private IAuthService _auth;

        public BangDiemController(IBangDiemService bangDiemService, IAuthService auth)
        {
            this._bangDiemService = bangDiemService;
            this._auth = auth;
        }

        // GET: api/<BangDiemController>
        [HttpGet]
        public async Task<object> GetSinhVienByLopAndHocKy(string MaLop, string NamHoc)
        {
            if (_auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateKhoa(this))
            {
                var bangDiem = await _bangDiemService.GetSinhVienByLopAndHocKy(MaLop, NamHoc);
                var response = (bangDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        // GET api/<BangDiemController>/5
        [HttpGet("HdnkTheoCtdtCuaSv")]
        public async Task<object> GetHdnkTheoCtdtCuaSv(string MaSinhVien)
        {
            if (_auth.ValidateSinhVien(this))
            {
                var bangDiem = await _bangDiemService.GetHdnkTheoCtdtCuaSv(MaSinhVien);
                var response = (bangDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        [HttpGet("TongDiemSinhVienTrongHocKy")]
        public async Task<object> GetTongDiemSinhVienTrongHocKy(string MaNhanVien)
        {
            if (_auth.ValidateSinhVien(this))
            {
                var bangDiem = await _bangDiemService.GetTongDiemSinhVienTrongHocKy(MaNhanVien);
                var response = (bangDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}