using Microsoft.AspNetCore.Mvc;
using NotificationApplication.Interfaces.Services;

namespace Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController(ILogger<NotificationController> logger, IOtpService otpService) : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger = logger;
        private readonly IOtpService _otpService = otpService;

        [HttpPost("generate")]
        public async Task<ActionResult<string>> GenerateOtpAsync(string identifier)
        {
            if(string.IsNullOrEmpty(identifier))
                return BadRequest("Identifier cannot be null or empty");
            
            var otp = _otpService.GenerateOtpAsync(identifier);
            return Ok(new { Otp = otp, Message = "OTP Generated Successfully."});
        }

        [HttpPost("validate")]
        public async Task<ActionResult> ValidateOtpAsync(OtpValidationRequest request)
        {
            if(string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Otp))
                return BadRequest("Identifier and OTP are required");
            
            var isValid = await _otpService.ValidateOtpAsync(request.Identifier, request.Otp);
            if(isValid)
                return Ok(new {Message = "OTP validated successfully"});

            return BadRequest(new {Message = "Invalid or Expired OTP"});
        }
    }
}

public class OtpValidationRequest
{
    public string Identifier { get; set; }
    public string Otp { get; set; }
}