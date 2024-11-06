using _1.Domain.Entities;
using _1.Domain.Interfaces;
using _1.Domain.Interfaces.Commons;
using _2.Application.Common;
using _2.Application.DTOs;
using _2.Application.Interfaces;
using _3.Infrastructure.Services;
using AutoMapper;

namespace _2.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository,
                             IJwtTokenService jwtTokenService,
                             ICurrentUserService currentUserService,
                             IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<string>> Login(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null || !PasswordHelper.VerifyPasswordHash(password, user.PasswordHash))
            {
                return Result<string>.Failure(new[] { "Invalid credentials" });
            }

            var useDto = _mapper.Map<User,UserDto>(user);

            var token = _jwtTokenService.GenerateJwtToken(user);
            return Result<string>.Success(token);
        }
    }
}
