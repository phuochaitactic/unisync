using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NDMKhoaController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IAccountService _accountService;
        private readonly IKhoaService _khoaService;

        public NDMKhoaController(IAuthService auth, IAccountService accountService, IKhoaService khoaService)
        {
            this._auth = auth;
            this._accountService = accountService;
            this._khoaService = khoaService;
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
                var properties = typeof(Ndmkhoa).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "Idkhoa")
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
        /// GET: api/<NDMKhoaController>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.GetAll();
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// GET api/<NDMKhoaController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<object> Get(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.GetById(id);
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// GET api/<NDMKhoaController>/MaKhoa/Khoa001
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MaKhoa/{MaKhoa}")]
        public async Task<object> GetByMaKhoa(string MaKhoa)
        {
            Console.WriteLine(MaKhoa);
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.GetByMaKhoa(MaKhoa);
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// GET api/<NDMKhoaController>/tenkhoa/CongNgheThongTin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("TenKhoa/{TenKhoa}")]
        public async Task<object> GetByTenKhoa(string TenKhoa)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.GetByTenKhoa(TenKhoa);
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// POST api/<NDMKhoaController>
        /// </summary>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Post([FromBody] Ndmkhoa InputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.CreateKhoa(InputData);
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        [HttpGet("userinfo")]
        public async Task<object> GetUserInfo()
        {
            var khoa = await _khoaService.GetUserInfo(HttpContext);
            var response = (khoa as ObjectResult)?.Value;
            return response;
        }

        /// <summary>
        /// PUT api/<NDMKhoaController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="InputData"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] Ndmkhoa InputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.ChangeData(id, InputData);
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// DELETE api/<NDMKhoaController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var khoa = await _khoaService.Delete(id);
                var response = (khoa as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}