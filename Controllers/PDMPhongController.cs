using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDMPhongController : ControllerBase
    {
        protected readonly IPhongService _phongService;
        protected readonly IAuthService _auth;

        public PDMPhongController(IPhongService phongService, IAuthService auth)
        {
            this._phongService = phongService;
            this._auth = auth;
        }

        /// <summary>
        /// Exports the columns of the SinhVienTableModel to Excel file
        /// </summary>
        /// <returns>Excel file of column metadata</returns>
        [HttpGet("ToExcel/")]
        public IActionResult GetToExcel()
        {
            try
            {
                // Get column metadata
                var properties = typeof(PhongTableModel).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "Idphong")
                    {
                        var column = new Dictionary<string, string>();
                        column.Add("ColumnName", property.Name);
                        column.Add("DataType", property.PropertyType.Name);

                        columnMetadata.Add(column);
                    }
                }
                // Export to Excel
                return ExcelExporter.ExportToExcel(columnMetadata);
            }
            catch (Exception ex)
            {
                return BadRequest($"ERROR: {ex.Message}");
            }
        }

        // GET: api/<PDMPhongController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetAll();
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/MaPhong/{MaPhong}")]
        public async Task<object> GetPhongByMaPhong(string MaPhong)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByMaPhong(MaPhong);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpPost("TheoNgay")]
        public async Task<object> GetPhongTheoNgay(NgayBatDauNgayKetThucModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongTheoNgay(inputData);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/TenPhong/{TenPhong}")]
        public async Task<object> GetPhongByTenPhong(string TenPhong)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByTenPhong(TenPhong);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/TenDiaDiem/{TenDiaDiem}")]
        public async Task<object> GetPhongByDiaDiem(string TenDiaDiem)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByDiaDiem(TenDiaDiem);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/SucChua/{SucChua}")]
        public async Task<object> GetPhongBySucChua(int SucChua)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongBySucChua(SucChua);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/DayPhong/{DayPhong}")]
        public async Task<object> GetPhongByDayPhong(string DayPhong)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByDayPhong(DayPhong);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/CoSo/{CoSo}")]
        public async Task<object> GetPhongByCoSo(string CoSo)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByCoSo(CoSo);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/DienTich/{DienTich}")]
        public async Task<object> GetPhongByDienTich(int DienTich)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByDienTich(DienTich);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<PDMPhongController>/5
        [HttpGet("api/PDMPhong/TinhChat/{TinhChat}")]
        public async Task<object> GetPhongByTinhChat(string TinhChat)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var hdNK = await _phongService.GetPhongByTinhChat(TinhChat);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // POST api/<PDMPhongController>
        [HttpPost]
        public async Task<object> Post([FromBody] PhongTableModel InputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var hdNK = await _phongService.CreateBacHeNganh(InputData);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // PUT api/<PDMPhongController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] PhongTableModel InputData)
        {
            if (_auth.ValidateAdmin(this))
            {
                var hdNK = await _phongService.ChangeData(id, InputData);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // DELETE api/<PDMPhongController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this))
            {
                var hdNK = await _phongService.Delete(id);
                var response = (hdNK as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}