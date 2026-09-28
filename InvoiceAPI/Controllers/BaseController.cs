using InvoiceAPI.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message)
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        protected IActionResult Failure(string message)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult NotFoundResponse(string message)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = message
            });
        }
    }
}
