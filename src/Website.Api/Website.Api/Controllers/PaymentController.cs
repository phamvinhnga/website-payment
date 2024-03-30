using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;
using Serilog;
using Website.Api.Models.VnPays;
using Website.Api.Services;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("vnp")]
        public async Task<IActionResult> CreateVnPaymentAsync([FromBody] VnPayCreating input)
        {
           return Ok(await _vnPayService.CreateNewPaymentAsync(input, ""));
        }
    }
}
