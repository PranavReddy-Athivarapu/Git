using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using OnlinePharmacyAppAPI.Models;
using OnlinePharmacyAppAPI.Services.Interfaces;

namespace OnlinePharmacyAppAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthService(
            IRepository<User> userRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.GetAll().AnyAsync(u => u.Email == request.Email))
                throw new ArgumentException("Email already registered");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Role = "Customer"
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return new AuthResponse
            {
                Token = _tokenGenerator.GenerateToken(user),
                UserId = user.UserId
            };
        }
    }
}
