using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers
{
    /// <summary>
    /// Controller for managing academic majors.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KDMNganhController : Controller
    {
        private readonly INganhService _nganhService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDMNganhController"/> class.
        /// </summary>
        /// <param name="nganhService">The service for managing academic majors.</param>
        /// <param name="auth">The authentication service.</param>
        public KDMNganhController(INganhService nganhService, IAuthService auth)
        {
            this._nganhService = nganhService;
            this._auth = auth;
        }

        /// <summary>
        /// Exports the columns of the NganhModel to an Excel file.
        /// </summary>
        /// <returns>Excel file containing column metadata.</returns>
        [HttpGet("ToExcel/")]
        public IActionResult GetToExcel()
        {
            try
            {
                // Get column metadata
                var properties = typeof(NganhModel).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "Idngh")
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
        /// Gets all academic majors.
        /// </summary>
        /// <returns>List of academic majors.</returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var nganh = await _nganhService.GetAll();
                var response = (nganh as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Gets an academic major by ID.
        /// </summary>
        /// <param name="id">The ID of the academic major.</param>
        /// <returns>The academic major with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<object> GetById(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var nganh = await _nganhService.GetById(id);
                var response = (nganh as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Gets academic majors by name.
        /// </summary>
        /// <param name="tenNganh">The name of the academic major.</param>
        /// <returns>List of academic majors with the specified name.</returns>
        [HttpGet("TenNganh/{tenNganh}")]
        public async Task<object> GetByTenNganh(string tenNganh)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var nganh = await _nganhService.GetByTenNganh(tenNganh);
                var response = (nganh as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Creates a new academic major.
        /// </summary>
        /// <param name="inputData">The data for creating the academic major.</param>
        /// <returns>The created academic major.</returns>
        [HttpPost]
        public async Task<object> Post([FromBody] NganhModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var nganh = await _nganhService.CreateNganh(inputData);
                var response = (nganh as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Updates an existing academic major.
        /// </summary>
        /// <param name="id">The ID of the academic major.</param>
        /// <param name="inputData">The updated data for the academic major.</param>
        /// <returns>The updated academic major.</returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] NganhModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var nganh = await _nganhService.ChangeData(id, inputData);
                var response = (nganh as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        /// <summary>
        /// Deletes an academic major by ID.
        /// </summary>
        /// <param name="id">The ID of the academic major to delete.</param>
        /// <returns>The deleted academic major.</returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var nganh = await _nganhService.Delete(id);
                var response = (nganh as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }
    }
}