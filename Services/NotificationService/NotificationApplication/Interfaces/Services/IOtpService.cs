using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationApplication.Interfaces.Services
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string identifier);
        Task<bool> ValidateOtpAsync(string identifier, string otp);
    }
}