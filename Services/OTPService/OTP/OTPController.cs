using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OTPApplication.Interfaces.Service;

namespace OTP
{
    [Route("[controller]")]
    public class OTPController(ILogger<OTPController> logger, IOtpService otpService) : Controller
    {
        private readonly ILogger<OTPController> _logger = logger;
        private readonly IOtpService _otpService = otpService;

        public async Task<ActionResult<string>> GenerateOtpAsync(string identifier)
        {
            if(string.IsNullOrEmpty(identifier))
                return BadRequest("Identifier cannot be null or empty");
            
            var otp = _otpService.GenerateOtpAsync(identifier);
            return Ok(new { Otp = otp, Message = "OTP Generated Successfully."});
        }

        public async Task<ActionResult> ValidateOtpAsync(OtpValidationRequest request)
        {
            if(string.IsNullOrEmpty(request.Identifier) || string.IsNullOrEmpty(request.Otp))
                return BadRequest("Identifier and OTP are required");
            
            var isValid = await _otpService.ValidateOtpAsync(request.Identifier, request.Otp);
            if(isValid)
                return Ok(new {Message = "OTP validated successfully"});

            return BadRequest(new {Message = "Invalid or Expired OTP"});
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}

public class OtpValidationRequest
{
    public string Identifier { get; set; }
    public string Otp { get; set; }
}