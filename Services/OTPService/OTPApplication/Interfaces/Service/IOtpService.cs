using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTPApplication.Interfaces.Service
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string identifier);
        Task<bool> ValidateOtpAsync(string identifier, string otp);
    }
}