using System.Collections.Concurrent;
using NotificationApplication.Interfaces.Services;

namespace NotificationInfrastructure.Services
{
    public class OtpService(
        // IConnectionMultiplexer redis
        ) : IOtpService
    {
        // private readonly IDatabase _redis = redis.GetDatabase();

        private readonly ConcurrentDictionary<string, (string otp, DateTime Expiry)> _otpStore = new();

        public async Task<string> GenerateOtpAsync(string identifier)
        {
            var otp = new Random().Next(100000,999999).ToString();
            _otpStore[identifier] = (otp, DateTime.UtcNow.AddMinutes(5));
            return otp;
        }

        public async Task<bool> ValidateOtpAsync(string identifier, string otp)
        {
            if(_otpStore.TryGetValue(identifier, out var entry) && entry.otp == otp && entry.Expiry == DateTime.UtcNow)
            {
                _otpStore.TryRemove(identifier, out _);
                return true;
            }
            return false;
        }
    }
}