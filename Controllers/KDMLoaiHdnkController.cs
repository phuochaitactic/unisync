using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers
{
    /// <summary>
    /// Controller for managing types of external activities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KDMLoaiHdnkController : ControllerBase
    {
        private readonly ILoaiHdnkService _loaiHdnkService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDMLoaiHdnkController"/> class.
        /// </summary>
        /// <param name="loaiHdnkService">The service for managing types of external activities.</param>
        /// <param name="authService">The authentication service.</param>
        public KDMLoaiHdnkController(ILoaiHdnkService loaiHdnkService, IAuthService authService)
        {
            _loaiHdnkService = loaiHdnkService;
            _auth = authService;
        }

        /// <summary>
        /// Gets all types of external activities.
        /// </summary>
        /// <returns>List of types of external activities.</returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var loaiHdnk = await _loaiHdnkService.GetAll();
                var response = (loaiHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
            // do something with loaiHdnk
        }

        /// <summary>
        /// Gets a type of external activity by its ID.
        /// </summary>
        /// <param name="maHdnk">The ID of the type of external activity.</param>
        /// <returns>The type of external activity with the specified ID.</returns>
        [HttpGet("{maHdnk}")]
        public async Task<object> Get(string maHdnk)
        {
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this) || _auth.ValidateThuKyKhoa(this) || _auth.ValidateGiangVien(this) || _auth.ValidateSinhVien(this))
            {
                var loaiHdnk = await _loaiHdnkService.GetByMa(maHdnk);
                var response = (loaiHdnk as ObjectResult)?.Value;
                return response;
            }

            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// Creates a new type of external activity.
        /// </summary>
        /// <param name="inputData">The data for creating the type of external activity.</param>
        /// <returns>The created type of external activity.</returns>
        [HttpPost]
        public async Task<object> Post([FromBody] KdmloaiHdnk inputData)
        {
            //if (!this.ValidateAdmin())
            if (_auth.ValidateKhoa(this) || _auth.ValidateAdmin(this))
            {
                var loaiHdnk = await _loaiHdnkService.CreateLoaiHdnk(inputData);
                var response = (loaiHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// Updates an existing type of external activity.
        /// </summary>
        /// <param name="id">The ID of the type of external activity to update.</param>
        /// <param name="inputData">The updated data for the type of external activity.</param>
        /// <returns>The updated type of external activity.</returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] KdmloaiHdnk inputData)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var loaiHdnk = await _loaiHdnkService.ChangeData(id, inputData);
                var response = (loaiHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }

        /// <summary>
        /// Deletes a type of external activity by its ID.
        /// </summary>
        /// <param name="id">The ID of the type of external activity to delete.</param>
        /// <returns>The deleted type of external activity.</returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (_auth.ValidateAdmin(this) || _auth.ValidateKhoa(this))
            {
                var loaiHdnk = await _loaiHdnkService.Delete(id);
                var response = (loaiHdnk as ObjectResult)?.Value;
                return response;
            }
            return Unauthorized("User is not authenticated.");
        }
    }
}