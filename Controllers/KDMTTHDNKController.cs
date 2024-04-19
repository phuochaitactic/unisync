using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMTTHDNKController : ControllerBase
    {
        private readonly ITtHdnkService _ttHdnkService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDMTTHDNKController"/> class.
        /// </summary>
        /// <param name="TtHdnkService">The service for managing academic semesters.</param>
        /// <param name="auth">The authentication service.</param>
        public KDMTTHDNKController(ITtHdnkService _ttHdnkService, IAuthService auth)
        {
            this._ttHdnkService = _ttHdnkService;
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
                var properties = typeof(ThongTinHoatDongNgoaiKhoaModel).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "Idtthdnk")
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
        /// GET: api/<KDMTTHDNKController>
        /// </summary>
        /// <returns> List<ThongTinHoatDongNgoaiKhoaModel> </returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetAll();
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("DsSinhVienDangKy/")]
        public async Task<object> GetByTenKhoa(long idKhoa, long idBhNganh, long idNhhk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetDsSinhVienDangKy(idKhoa, idBhNganh, idNhhk);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/TenKhoa/tenKhoa
        /// </summary>
        /// <param name="tenKhoa"></param>
        /// <returns> List<ThongTinHoatDongNgoaiKhoaModel> </returns>
        [HttpGet("tenKhoa/{tenKhoa}")]
        public async Task<object> GetByTenKhoa(string tenKhoa)
        {
            Console.WriteLine(tenKhoa);
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetByTenKhoa(tenKhoa);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/TenGiangVien/TenGiangVien
        /// </summary>
        /// <param name="tenKhoa"></param>
        /// <returns> List<ThongTinHoatDongNgoaiKhoaModel> </returns>
        [HttpGet("TenGiangVien/{TenGiangVien}")]
        public async Task<object> GetByTenGiangVien(string TenGiangVien)
        {
            Console.WriteLine(TenGiangVien);
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetByTenGiangVien(TenGiangVien);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/TenPhong/TenPhong
        /// </summary>
        /// <param name="tenKhoa"></param>
        /// <returns> List<ThongTinHoatDongNgoaiKhoaModel> </returns>
        [HttpGet("TenPhong/{TenPhong}")]
        public async Task<object> GetByTenPhong(string TenPhong)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetByTenPhong(TenPhong);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/TenDiaDiem/TenDiaDiem
        /// </summary>
        /// <param name="tenKhoa"></param>
        /// <returns> List<ThongTinHoatDongNgoaiKhoaModel> </returns>
        [HttpGet("TenDiaDiem/{tenDiaDiem}")]
        public async Task<object> GetByTenDiaDiem(string tenDiaDiem)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetByTenDiaDiem(tenDiaDiem);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// GET api/TenHdnk/TenHdnk
        /// </summary>
        /// <param name="tenKhoa"></param>
        /// <returns> List<ThongTinHoatDongNgoaiKhoaModel> </returns>
        [HttpGet("TenHdnk/{tenHdnk}")]
        public async Task<object> GetByTenHdnk(string tenHdnk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _ttHdnkService.GetByTenHdnk(tenHdnk);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// POST api/<KDMTTHDNKController>
        /// </summary>
        /// <param name="tenHdnk"></param>
        /// <param name="tenDiaDiem"></param>
        /// <param name="tenPhong"></param>
        /// <param name="tenKhoa"></param>
        /// <param name="hoTen"></param>
        /// <param name="inputData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Post([FromBody] ThongTinHoatDongNgoaiKhoaByMaModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var ttHdnk = await _ttHdnkService.CreateTtHdnk(inputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// PUT api/<KDMTTHDNKController>/5
        /// </summary>
        /// <param name="tenHdnk"></param>
        /// <param name="tenDiaDiem"></param>
        /// <param name="tenPhong"></param>
        /// <param name="tenKhoa"></param>
        /// <param name="hoTen"></param>
        /// <param name="id"></param>
        /// <param name="inputData"></param>
        /// <returns> <IActionResult> </returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] ThongTinHoatDongNgoaiKhoaModelDatum inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var ttHdnk = await _ttHdnkService.ChangeData(id, inputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// DELETE api/<KDMTTHDNKController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns> <IActionResult> </returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var ttHdnk = await _ttHdnkService.Delete(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}