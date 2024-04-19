using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected readonly IAccountService _accountService;
        protected readonly IAuthService _authService;

        public AccountController(IAccountService accountService, IAuthService authService)
        {
            this._accountService = accountService;
            this._authService = authService;
        }

        // GET: api/<TaiKhoanController>
        [HttpGet]
        public async Task<object> Get()
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }
            var accounts = await _accountService.GetAll();
            var response = (accounts as ObjectResult)?.Value;

            return response;
        }

        // GET api/<TaiKhoanController>/5
        [HttpGet("{username}")]
        public async Task<object> GetByUsername(string username)
        {
            if (!_authService.ValidateAdmin(this))
            {
                return Unauthorized("User is not authenticated.");
            }

            var accounts = await _accountService.GetAll();
            var response = (accounts as ObjectResult)?.Value;

            return response;
        }

        // POST api/<TaiKhoanController>
        [HttpPost("login")]
        public async Task<object> Login([FromBody] LoginModel model)
        {
            var account = await _accountService.Login(HttpContext, model);
            var response = (account as ObjectResult)?.Value;
            return response;
        }

        [HttpPost("quantri")]
        public async Task<object> LoginAdmin([FromBody] LoginModel model)
        {
            var admin = await _accountService.LoginAdmin(HttpContext, model);
            var response = (admin as ObjectResult)?.Value;
            return response;
        }
    }
}