using LMS.Models;

namespace LMS.Services
{
    public interface ITokenService
    {
        string GenerateJSONWebToken(string key, string issuer, User user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
