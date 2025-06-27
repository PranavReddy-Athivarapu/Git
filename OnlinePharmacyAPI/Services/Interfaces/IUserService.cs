using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task UpdateUserProfileAsync(int userId, UserProfileDto profile);
    }
}
