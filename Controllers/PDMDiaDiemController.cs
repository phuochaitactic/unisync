using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDMDiaDiemController : ControllerBase
    {
        protected readonly IDiaDiemService _diaDiemService;
        protected readonly IAuthService _auth;

        public PDMDiaDiemController(IDiaDiemService diaDiemService, IAuthService auth)
        {
            this._diaDiemService = diaDiemService;
            this._auth = auth;
        }

        /// <summary>
        /// Exports the columns of the SinhVienTableModel to Excel file
        /// </summary>
        /// <returns>Excel file of column metadata</returns>
        [HttpGet("ToExcel/")]
        public IActionResult GetToExcel()
        {
            var properties = typeof(PdmdiaDiem).GetProperties();

            try
            {
                // Get column metadata
                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "IddiaDiem")
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

        // GET: api/<PDMDiaDiemController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var diaDiem = await _diaDiemService.GetAll();
                var response = (diaDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        // GET api/<PDMDiaDiemController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(long id)
        {
            if (!_auth.ValidateAdmin(this) || !_auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var diaDiem = await _diaDiemService.GetById(id);
                var response = (diaDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// POST api/<PDMDiaDiemController>
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public async Task<object> Post([FromBody] PdmdiaDiem InputData)
        {
            if (_auth.ValidateAdmin(this))
            {
                var diaDiem = await _diaDiemService.CreateDiaDiem(InputData);
                var response = (diaDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// PUT api/<PDMDiaDiemController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] PdmdiaDiem InputData)
        {
            if (_auth.ValidateAdmin(this))
            {
                var diaDiem = await _diaDiemService.ChangeData(id, InputData);
                var response = (diaDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// DELETE api/<PDMDiaDiemController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this))
            {
                var diaDiem = await _diaDiemService.Delete(id);
                var response = (diaDiem as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}