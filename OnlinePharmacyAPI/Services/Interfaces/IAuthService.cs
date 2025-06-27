using Microsoft.AspNetCore.Identity.Data;
using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<bool> ResetPasswordAsync(string email);
    }
}
