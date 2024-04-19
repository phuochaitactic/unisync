using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers
{
    /// <summary>
    /// Controller for managing academic semesters.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KDMNHHKController : ControllerBase
    {
        private readonly INhhkService _nhhkService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDMNHHKController"/> class.
        /// </summary>
        /// <param name="nhhkService">The service for managing academic semesters.</param>
        /// <param name="auth">The authentication service.</param>
        public KDMNHHKController(INhhkService nhhkService, IAuthService auth)
        {
            this._nhhkService = nhhkService;
            this._auth = auth;
        }

        /// <summary>
        /// Exports the columns of the Kdmnhhk to an Excel file.
        /// </summary>
        /// <returns>Excel file containing column metadata.</returns>
        [HttpGet("ToExcel/")]
        public IActionResult GetToExcel()
        {
            try
            {
                // Get column metadata
                var properties = typeof(Kdmnhhk).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "Idnhhk")
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
        /// Gets all academic semesters.
        /// </summary>
        /// <returns>List of academic semesters.</returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var academicSemesters = await _nhhkService.GetAll();
                var response = (academicSemesters as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Gets an academic semester by ID.
        /// </summary>
        /// <param name="id">The ID of the academic semester.</param>
        /// <returns>The academic semester with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<object> Get(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var academicSemesters = await _nhhkService.GetById(id);
                var response = (academicSemesters as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Gets an academic semester by semester code.
        /// </summary>
        /// <param name="MaNHHK">The semester code of the academic semester.</param>
        /// <returns>The academic semester with the specified code.</returns>
        [HttpGet("MaNHHK/{MaNHHK}")]
        public async Task<object> GetByMaNHHK(long MaNHHK)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var academicSemester = await _nhhkService.GetByMa(MaNHHK);
                var response = (academicSemester as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Gets academic semesters by name.
        /// </summary>
        /// <param name="TenNHHK">The name of the academic semester.</param>
        /// <returns>List of academic semesters with the specified name.</returns>
        [HttpGet("TenNHHK/{TenNHHK}")]
        public async Task<object> GetByTenNHHK(string TenNHHK)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var academicSemesters = await _nhhkService.GetByTen(TenNHHK);
                var response = (academicSemesters as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Creates a new academic semester.
        /// </summary>
        /// <param name="inputData">The data for creating the academic semester.</param>
        /// <returns>The created academic semester.</returns>
        [HttpPost]
        public async Task<object> Post([FromBody] Kdmnhhk inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var academicSemester = await _nhhkService.CreateNganh(inputData);
                var response = (academicSemester as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Updates an existing academic semester.
        /// </summary>
        /// <param name="id">The ID of the academic semester.</param>
        /// <param name="InputData">The updated data for the academic semester.</param>
        /// <returns>The updated academic semester.</returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] Kdmnhhk InputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var academicSemester = await _nhhkService.ChangeData(id, InputData);
                var response = (academicSemester as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Deletes an academic semester by ID.
        /// </summary>
        /// <param name="id">The ID of the academic semester to delete.</param>
        /// <returns>The deleted academic semester.</returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this))
            {
                var academicSemester = await _nhhkService.Delete(id);
                var response = (academicSemester as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}