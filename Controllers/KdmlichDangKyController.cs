using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers
{
    /// <summary>
    /// Controller for managing registration schedules.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KdmlichDangKyController : ControllerBase
    {
        private readonly ILichDangKyService _lichDangKyService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KdmlichDangKyController"/> class.
        /// </summary>
        /// <param name="lichDangKyService">The registration schedule service.</param>
        /// <param name="auth">The authentication service.</param>
        public KdmlichDangKyController(ILichDangKyService lichDangKyService, IAuthService auth)
        {
            this._lichDangKyService = lichDangKyService;
            this._auth = auth;
        }

        /// <summary>
        /// Gets all registration schedules.
        /// </summary>
        /// <returns>List of registration schedules.</returns>
        [HttpGet]
        public async Task<object> Get()
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var lich = await _lichDangKyService.GetAll();
                // do something with lich
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// Gets registration schedules by class name.
        /// </summary>
        /// <param name="tenLop">The class name.</param>
        /// <returns>List of registration schedules for the specified class.</returns>
        [HttpGet("tenlop/{tenLop}")]
        public async Task<object> GetByTenLop(string tenLop)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var lich = await _lichDangKyService.GetByTenLop(tenLop);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        [HttpGet("TheoKhoa")]
        public async Task<object> GetByTenKhoa(string TenKhoa, string TenNhhk, string TenBHN)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var lich = await _lichDangKyService.GetByKhoa(TenKhoa, TenNhhk, TenBHN);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// Gets registration schedules by semester name.
        /// </summary>
        /// <param name="tenNHHK">The semester name.</param>
        /// <returns>List of registration schedules for the specified semester.</returns>
        [HttpGet("{tenNHHK}")]
        public async Task<object> GetByTenNHHK(string tenNHHK)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var lich = await _lichDangKyService.GetByTenNHHK(tenNHHK);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// Creates a new registration schedule.
        /// </summary>
        /// <param name="tenNhhk">The semester name.</param>
        /// <param name="tenLop">The class name.</param>
        /// <param name="inputData">The registration schedule data.</param>
        /// <returns>The created registration schedule.</returns>
        [HttpPost]
        public async Task<object> Post([FromBody] LichDangKyModel inputData)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this))
            {
                var lich = await _lichDangKyService.CreateLich(inputData);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// Updates an existing registration schedule.
        /// </summary>
        /// <param name="tenNhhk">The semester name.</param>
        /// <param name="tenLop">The class name.</param>
        /// <param name="id">The registration schedule ID.</param>
        /// <param name="inputData">The updated registration schedule data.</param>
        /// <returns>The updated registration schedule.</returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] LichDangKyModel inputData)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this))
            {
                var lich = await _lichDangKyService.ChangeData(id, inputData);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not _authenticated.");
        }

        /// <summary>
        /// Deletes a registration schedule by ID.
        /// </summary>
        /// <param name="id">The ID of the registration schedule to delete.</param>
        /// <returns>The deleted registration schedule.</returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this) || _auth.ValidateThuKyKhoa(this))
            {
                var lich = await _lichDangKyService.Delete(id);
                var response = (lich as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not _authenticated.");
        }
    }
}