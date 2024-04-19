using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KDMVanBanController : ControllerBase
    {
        protected readonly IVanBanService _vanBanService;
        protected readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDMVanBanController"/> class.
        /// </summary>
        /// <param name="VanBanService">The service for managing academic semesters.</param>
        /// <param name="auth">The authentication service.</param>
        public KDMVanBanController(IVanBanService vanBanService, IAuthService auth)
        {
            this._vanBanService = vanBanService;
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
                var properties = typeof(KdmvanBan).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "IdvanBan")
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

        // GET: api/<KDMVanBanController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _vanBanService.GetAll();
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("Unauthorized");
        }

        // GET api/<KDMVanBanController>/5
        [HttpGet("{id}")]
        public async Task<object> GetById(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var ttHdnk = await _vanBanService.GetById(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // POST api/<KDMVanBanController>
        [HttpPost]
        public async Task<object> Post([FromBody] KdmvanBan inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var ttHdnk = await _vanBanService.CreateVanBan(inputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // PUT api/<KDMVanBanController>/5
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] KdmvanBan inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var ttHdnk = await _vanBanService.ChangeData(id, inputData);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }

        // DELETE api/<KDMVanBanController>/5
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var ttHdnk = await _vanBanService.Delete(id);
                var response = (ttHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("Unauthorized");
        }
    }
}