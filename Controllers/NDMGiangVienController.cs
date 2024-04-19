using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NDMGiangVienController : ControllerBase
    {
        protected readonly IGiangVienService _giangVienService;
        protected readonly IAuthService _auth;

        public NDMGiangVienController(IGiangVienService giangVienService, IAuthService auth)
        {
            this._giangVienService = giangVienService;
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
                var properties = typeof(GiangVienTableModel).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "IdgiangVien")
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

        /// <summary>
        /// GET: api/<NDMGiangVienController>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _giangVienService.GetAll();
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/<NDMGiangVienController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<object> GetById(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _giangVienService.GetById(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/NDMGiangVien/MaNv/{MaNV}
        /// </summary>
        /// <param name="MaNV"></param>
        /// <returns></returns>
        [HttpGet("api/NDMGiangVien/MaNv/{MaNV}")]
        public async Task<object> GetGVByMaNV(string MaNV)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _giangVienService.GetGVByMaNV(MaNV);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/NDMGiangVien/HoTen/{HoTen}
        /// </summary>
        /// <param name="HoTen"></param>
        /// <returns></returns>
        [HttpGet("api/NDMGiangVien/HoTen/{HoTen}")]
        public async Task<object> GetGVbyHoTen(string HoTen)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _giangVienService.GetGVbyHoTen(HoTen);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/NDMGiangVien/TenKhoa/{TenKhoa}
        /// </summary>
        /// <param name="TenKhoa"></param>
        /// <returns></returns>
        [HttpGet("api/NDMGiangVien/TenKhoa/{TenKhoa}")]
        public async Task<object> GetGVbyTenKhoa(string TenKhoa)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _giangVienService.GetGVbyTenKhoa(TenKhoa);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/NDMGiangVien/VaiTro/{VaiTro}
        /// </summary>
        /// <param name="VaiTro"></param>
        /// <returns></returns>
        [HttpGet("api/NDMGiangVien/VaiTro/{VaiTro}")]
        public async Task<object> GetGVbyVaiTro(string VaiTro)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var ttHdnk = await _giangVienService.GetGVbyVaiTro(VaiTro);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// POST api/<NDMGiangVienController>
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task<object> Post([FromBody] GiangVienTableModel InputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var ttHdnk = await _giangVienService.CreateGiangVien(InputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// PUT api/<NDMGiangVienController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, string tenKhoa, [FromBody] GiangVienTableModel InputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateKhoa(this))
            {
                var ttHdnk = await _giangVienService.ChangeData(id, InputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// DELETE api/<NDMGiangVienController>/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateKhoa(this))
            {
                var ttHdnk = await _giangVienService.Delete(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("userinfo")]
        public async Task<object> GetUserInfo()
        {
            var giangVien = await _giangVienService.GetUserInfo(HttpContext);
            var response = (giangVien as ObjectResult)?.Value;
            return response;
        }
    }
}