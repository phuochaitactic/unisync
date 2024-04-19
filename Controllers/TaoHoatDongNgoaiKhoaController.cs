using BuildCongRenLuyen.Models.CustomModels;
using BuildCongRenLuyen.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BuildCongRenLuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaoHoatDongNgoaiKhoaController : ControllerBase
    {
        private readonly ITaoHdnkService _taoHdnkService;
        private readonly IAuthService _auth;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmsvsController"/> class.
        /// </summary>
        /// <param name="taoHdnkService">The service for managing SinhVien data.</param>
        /// <param name="auth">The authentication service.</param>
        public TaoHoatDongNgoaiKhoaController(ITaoHdnkService taoHdnkService, IAuthService auth)
        {
            this._taoHdnkService = taoHdnkService;
            this._auth = auth;
        }

        // GET: api/<TaoHoatDongNgoaiKhoaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<TaoHoatDongNgoaiKhoaController>
        [HttpPost]
        public async Task<object> Post([FromBody] TaoHdnkModel inputData)
        {
            var data = await _taoHdnkService.CreateHdnk(inputData);
            var response = (data as ObjectResult)?.Value;
            return response;
        }
    }
}