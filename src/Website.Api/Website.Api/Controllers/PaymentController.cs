using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;
using Serilog;
using Website.Api.Models.VnPays;
using Website.Api.Services;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpGet("vnp-return")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateVnPaymentAsync([FromQuery] VnPayReturnInput input)
        {
            await _vnPayService.PaymentResultAsync(input);
            return Redirect("https://mail.google.com/mail/u/0/#inbox/FMfcgzGxSRSCHXZBqDGsnhbXJxwCtZDw");
        }

        [HttpPost("vnp")]
        public async Task<IActionResult> CreateVnPaymentAsync([FromBody] VnPayCreating input)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(remoteIpAddress))
            {
                return BadRequest("Không thể xác định địa chỉ IP của client.");
            }

            return Ok(
                new {
                    PaymentUrl = await _vnPayService.CreateNewPaymentAsync(input, remoteIpAddress)
                }
            );
        }
    }
}
