using Microsoft.AspNetCore.Mvc;

namespace BuildCongRenLuyen.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected bool Result { get; set; } = true;
        protected int Code { get; set; } = 200;
        protected string Message { get; set; } = "";
        protected List<object> DataObject { get; set; }

        protected BaseController()
        {
            DataObject = new List<object>();
        }

        protected IActionResult CreateResponse()
        {
            if (Code == 200)
            {
                return Ok(new
                {
                    Result = true,
                    Code = Code,
                    Message = Message,
                    Data = DataObject
                });
            }
            else if (Code == 404)
            {
                return NotFound(new
                {
                    Result = false,
                    Code = Code,
                    Message = Message,
                });
            }
            else if (Code == 400)
            {
                return BadRequest(new
                {
                    Result = false,
                    Code = Code,
                    Message = Message,
                });
            }
            else if (Code == 401)
            {
                return Unauthorized(new
                {
                    Result = false,
                    Code = Code,
                    Message = Message,
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    Result = false,
                    Code = Code,
                    Message = Message,
                });
            }
        }
    }
}