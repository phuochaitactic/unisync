using BuildCongRenLuyen.Models;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    /// <summary>
    /// KDMAdminController handles CRUD operations for admin user accounts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class KDMAdminController : ControllerBase
    {
        private readonly ILogger<KDMAdminController> _logger;
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;
        private readonly IAccountService _accountService;

        public KDMAdminController(IAuthService auth, IAdminService adminService, IAccountService accountService, ILogger<KDMAdminController> logger
            )
        {
            this._adminService = adminService;
            this._authService = auth;
            this._accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all admin accounts
        /// </summary>
        /// <returns>List of admin accounts</returns>
        [HttpGet]
        public async Task<object> Get()
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }
            var admins = await _adminService.GetAll();
            var response = (admins as ObjectResult)?.Value;
            return response;
        }

        /// <summary>
        /// Gets admin account by ID
        /// </summary>
        /// <param name="id">Admin account ID</param>
        /// <returns>Admin account details</returns>
        [HttpGet("{id}")]
        public async Task<object> GetById(long id)
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }
            var admins = await _adminService.GetById(id);
            var response = (admins as ObjectResult)?.Value;
            return response;
        }

        /// <summary>
        /// Creates a new admin account
        /// </summary>
        /// <param name="InputData">Account details</param>
        /// <returns>Newly created account</returns>
        [HttpPost]
        public async Task<object> Post([FromBody] Kdmadmin InputData)
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }

            var admins = await _adminService.CreateAdmin(InputData);
            var response = (admins as ObjectResult)?.Value;
            return response;
        }

        /// <summary>
        /// Logout admin user
        /// </summary>
        /// <returns>Logout response</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                var admin = _accountService.Logout(HttpContext);

                return Ok(admin);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                _logger.LogError(ex, "Error logging out admin");

                // Return a specific HTTP status code and error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error logging out admin", Error = ex.Message });
            }
        }

        /// <summary>
        /// Updates admin account details
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <param name="InputData">Updated info</param>
        /// <returns>Updated account</returns>
        [HttpPut("{id}")]
        public async Task<object> Put(long id, [FromBody] Kdmadmin InputData)
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }

            var admins = await _adminService.ChangeData(id, InputData);
            var response = (admins as ObjectResult)?.Value;

            return response;
        }

        /// <summary>
        /// Deletes an admin account
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns>Deletion response</returns>
        [HttpDelete("{id}")]
        public async Task<object> Delete(long id)
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }
            var admins = await _adminService.Delete(id);
            var response = (admins as ObjectResult)?.Value;
            return response;
            // Log the exception details for debugging
        }
    }
}