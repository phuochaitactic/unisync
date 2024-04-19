using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers
{
    /// <summary>
    /// Controller for managing SinhVien (Student) data through API endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SdmsvsController : ControllerBase
    {
        private readonly ISinhVienService _sinhVienService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmsvsController"/> class.
        /// </summary>
        /// <param name="sinhVienService">The service for managing SinhVien data.</param>
        /// <param name="auth">The authentication service.</param>
        public SdmsvsController(ISinhVienService sinhVienService, IAuthService auth)
        {
            this._sinhVienService = sinhVienService;
            this._auth = auth;
        }

        // Declare columns variable
        private List<Dictionary<string, string>> columns = new List<Dictionary<string, string>>();

        /// <summary>
        /// Exports the columns of the SinhVienTableModel to an Excel file.
        /// </summary>
        /// <returns>Excel file of column metadata.</returns>
        [HttpGet("ToExcel/")]
        public IActionResult GetToExcel()
        {
            try
            {
                // Get column metadata
                var properties = typeof(SinhVienTableModel).GetProperties();
                var columnMetadata = new List<Dictionary<string, string>>();
                foreach (var property in properties)
                {
                    if (property.Name != "IdsinhVien")
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
        /// GET: api/Sdmsvs
        /// </summary>
        /// <returns>List of SinhVienTableModel.</returns>
        [HttpGet]
        public async Task<object> GetSdmsvs()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetAll();
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// GET: api/Sdmsvs
        /// </summary>
        /// <returns>List of SinhVienTableModel.</returns>
        [HttpGet("ByBacHeNganh")]
        public async Task<object> GetSdmsvTheoBHNG(string TenBacHeNganh)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetSdmsvTheoBHNG(TenBacHeNganh);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// GET: api/Sdmsvs
        /// </summary>
        /// <returns>List of SinhVienTableModel.</returns>
        [HttpGet("ByGiangVien")]
        public async Task<object> GetByGiangVien(string TenNhhk, long IdGiangVien)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetByGiangVien(TenNhhk, IdGiangVien);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        [HttpGet("ByKhoa")]
        public async Task<object> GetByKhoa(string TenNhhk, long IdKhoa)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetByKhoa(TenNhhk, IdKhoa);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        [HttpGet("ByHdnk")]
        public async Task<object> GetByHdnk(string TenNhhk, long IdHdnk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetByHdnk(TenNhhk, IdHdnk);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// This C# function retrieves a student's information by their full name if the user has the
        /// necessary authorization.
        /// </summary>
        /// <param name="HoTenSinhVien">The code you provided is a C# method that handles a GET request
        /// to retrieve a student's information by their full name (HoTenSinhVien).</param>
        /// <returns>
        /// The code is returning the result of calling the
        /// `_sinhVienService.GetSdmsvByTen(HoTenSinhVien)` method as an object. If the user is validated
        /// as a member of certain roles (khoa, admin, thukykhoa, giangvien), then the response from the
        /// method call is returned. Otherwise, it returns an "Unauthorized" message indicating
        /// </returns>
        [HttpGet("api/Sdmsvs/HoTen/{HoTenSinhVien}")]
        public async Task<object> GetSdmsvByTen(string HoTenSinhVien)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetSdmsvByTen(HoTenSinhVien);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// This C# function retrieves student information based on a specified class code, with
        /// authentication checks included.
        /// </summary>
        /// <param name="maLop">The `GetSdmsvTheoLop` method is a GET endpoint that retrieves student
        /// information based on the provided class code (`maLop`).</param>
        /// <returns>
        /// The `GetSdmsvTheoLop` method returns an object that contains information about students
        /// (`sinhVien`) based on the provided class code (`maLop`). If the user is not authenticated as
        /// a Khoa or Thu Ky Khoa, an "Unauthorized" message is returned.
        /// </returns>
        [HttpGet("api/Sdmsvs/theoLop/{maLop}")]
        public async Task<object> GetSdmsvTheoLop(string maLop)
        {
            if (!_auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this))
            {
                var sinhVien = await _sinhVienService.GetSdmsvTheoLop(maLop);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// GET: api/Sdmsvs/MaSV/KT001
        /// </summary>
        /// <param name="MaSV">The student code.</param>
        /// <returns>List of SinhVienTableModel.</returns>
        [HttpGet("api/Sdmsvs/MaSV/{MaSV}")]
        public async Task<object> GetSdmsvByMa(string MaSV)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var sinhVien = await _sinhVienService.GetSdmsvByMa(MaSV);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// POST: api/Sdmsvs
        /// </summary>
        /// <param name="tenLop">The class name.</param>
        /// <param name="tenNHHK">The academic year name.</param>
        /// <param name="tenGiangVien">The lecturer's name.</param>
        /// <param name="inputData">The student data to BuildCongRenLuyen created.</param>
        /// <returns>ActionResult with the created SinhVien.</returns>
        [HttpPost]
        public async Task<object> PostSdmsv([FromBody] SinhVienTableModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var sinhVien = await _sinhVienService.CreateSinhVien(inputData);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// PUT: api/Sdmsvs
        /// </summary>
        /// <param name="id">The student id.</param>
        /// <param name="tenLop">The class name.</param>
        /// <param name="tenNHHK">The academic year name.</param>
        /// <param name="tenGiangVien">The lecturer's name.</param>
        /// <param name="inputData">The updated student data.</param>
        /// <returns>ActionResult with the updated SinhVien.</returns>
        [HttpPut]
        public async Task<object> PutSdmsv(long id, string tenLop, string tenNHHK, string tenGiangVien, [FromBody] SinhVienTableModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var sinhVien = await _sinhVienService.ChangeData(id, inputData);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// DELETE: api/Sdmsvs/5
        /// </summary>
        /// <param name="id">The student id to BuildCongRenLuyen deleted.</param>
        /// <returns>ActionResult indicating the result of the deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<object> DeleteSdmsv(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var sinhVien = await _sinhVienService.Delete(id);
                var response = (sinhVien as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        [HttpGet("userinfo")]
        public async Task<object> GetUserInfo()
        {
            var sinhVien = await _sinhVienService.GetUserInfo(HttpContext);
            var response = (sinhVien as ObjectResult)?.Value;
            return response;
        }
    }
}