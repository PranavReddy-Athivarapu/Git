using OnlinePharmacyAppAPI.Models;
using OnlinePharmacyAppAPI.Services.Interfaces;

namespace OnlinePharmacyAppAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Order> _orderRepository;

        public UserService(
            IRepository<User> userRepository,
            IRepository<Order> orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetAll()
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetAll()
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Medicine)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task UpdateUserProfileAsync(int userId, UserProfileDto profile)
        {
            var user = await _userRepository.GetAll()
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.Profile ??= new Profile();
            user.Profile.PhoneNumber = profile.PhoneNumber;
            user.Profile.Address = profile.Address;

            await _userRepository.UpdateAsync(user);
        }
    }
}
