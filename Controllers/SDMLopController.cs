using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SDMLopController : ControllerBase
    {
        protected readonly ILopService _lopService;
        protected readonly IAuthService _auth;

        public SDMLopController(ILopService lopService, IAuthService auth)
        {
            this._lopService = lopService;
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
                var properties = typeof(LopTableModel).GetProperties();

                var columnMetadata = new List<Dictionary<string, string>>();

                foreach (var property in properties)
                {
                    if (property.Name != "Idlop")
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
        /// GET: api/<SDMLopController>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var bache = await _lopService.GetAll();
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// GET api/SDMLop/MaLop/Lop001
        /// </summary>
        /// <param name="MaLop"></param>
        /// <returns></returns>
        [HttpGet("api/SDMLop/MaNv/{MaNhanVien}")]
        public async Task<object> GetLopByMaNhanVien(string MaNhanVien)
        {
            //if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            //{
            var bache = await _lopService.GetLopByMaNhanVien(MaNhanVien);
            var response = (bache as ObjectResult)?.Value;
            return response;
            //}
            //return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// GET api/SDMLop/MaLop/Lop001
        /// </summary>
        /// <param name="MaLop"></param>
        /// <returns></returns>
        [HttpGet("api/SDMLop/MaLop/{MaLop}")]
        public async Task<object> GetLopByMaLop(string MaLop)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var bache = await _lopService.GetLopByMaLop(MaLop);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// api/SDMLop/TenLop/lop 1
        /// </summary>
        /// <param name="TenLop"></param>
        /// <returns></returns>
        [HttpGet("api/SDMLop/TenLop/{TenLop}")]
        public async Task<object> GetLopByTenLop(string TenLop)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var bache = await _lopService.GetLopByTenLop(TenLop);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// api/SDMLop/NienKhoa/nienkhoa1
        /// </summary>
        /// <param name="NienKhoa"></param>
        /// <returns></returns>
        [HttpGet("api/SDMLop/NienKhoa/{NienKhoa}")]
        public async Task<object> GetLopByNamVao(string NienKhoa)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var bache = await _lopService.GetLopByNamVao(NienKhoa);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// api/SDMLop/Khoa/khoa 1
        /// </summary>
        /// <param name="TenKhoa"></param>
        /// <returns></returns>
        [HttpGet("api/SDMLop/Khoa/{TenKhoa}")]
        public async Task<object> GetLopByKhoa(string TenKhoa)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this))
            {
                var bache = await _lopService.GetLopByKhoa(TenKhoa);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// api/SDMLop/BacHeNganh/bh001
        /// </summary>
        /// <param name="MaBacHeNganh"></param>
        /// <returns></returns>
        [HttpGet("api/SDMLop/BacHeNganh/{MaBacHeNganh}")]
        public async Task<object> GetLopByBacHeNganh(string TenBacHeNganh)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this))
            {
                var bache = await _lopService.GetLopByBacHeNganh(TenBacHeNganh);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// POST api/<SDMLopController>
        /// </summary>
        /// <param name="tenKhoa"></param>
        /// <param name="TenBHNganh"></param>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Post([FromBody] LopTableModel InputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var bache = await _lopService.CreateLop(InputData);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// PUT api/<SDMLopController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tenKhoa"></param>
        /// <param name="MaBHNganh"></param>
        /// <param name="InputData"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] LopTableModel InputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var bache = await _lopService.ChangeData(id, InputData);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// DELETE api/<SDMLopController>/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var bache = await _lopService.Delete(id);
                var response = (bache as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}