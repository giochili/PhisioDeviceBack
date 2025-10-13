using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PhisioDeviceAPI.DTOs.User;
using PhisioDeviceAPI.Models;
using PhisioDeviceAPI.Repository;
using PhisioDeviceAPI.Services;

namespace PhisioDeviceAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<User> RegisterAsync(UserDTO dto, CancellationToken cancellationToken = default)
        {
            var emailTaken = await _userRepository.EmailExistsAsync(dto.Email, cancellationToken);
            if (emailTaken)
            {
                throw new InvalidOperationException("Email already in use");
            }

            var user = _mapper.Map<User>(dto);
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            return await _userRepository.AddAsync(user, cancellationToken);
        }

        public async Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var verify = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (verify == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return _jwtTokenService.GenerateToken(user.Id, user.Email, user.Name, user.Role);
        }
    }
}


