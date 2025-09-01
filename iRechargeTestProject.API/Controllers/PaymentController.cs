using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRechargeTestProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
             _paymentService = paymentService;
        }
        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPaymentAsync([FromBody] object paymentInfo)
        {
            // Placeholder for payment processing logic
            return Ok(new { Status = "Success", Message = "Payment processed successfully." });
        }

        public class PaymentWebhookDto
        {
            public string Reference { get; set; }
            public string Status { get; set; }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> PaymentWebhook([FromBody] PaymentWebhookDto dto)
        {
            try
            {
                var signature = Request.Headers["X-Signature"];
                var secret = "your_shared_secret";
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                var computedSignature = ComputeHmac(body, secret);

                if (signature != computedSignature)
                    return Unauthorized();

                // Process webhook...
                var paymentResult = await _paymentService.ProcessPaymentAsync(dto);
                if (paymentResult == null)
                    return NotFound(new { Message = "Payment not found or could not be processed." });

                return Ok(new { Status = "Success", Message = "Webhook processed successfully." });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument in webhook payload.");
                return BadRequest(new { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing webhook.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "An unexpected error occurred." });
            }
        }

        private string ComputeHmac(string data, string secret)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
            return Convert.ToHexString(hash);
        }

    }
}
