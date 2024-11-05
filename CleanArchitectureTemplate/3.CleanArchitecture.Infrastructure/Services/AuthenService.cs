using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.Common.Interfaces;
using _2.CleanArchitecture.Application.DTOs.Auth;
using _2.CleanArchitecture.Application.Features.IRepositories;
using _2.CleanArchitecture.Application.Features.Services;
using _2.CleanArchitecture.Application.Helper;
using AutoMapper;

namespace _3.CleanArchitecture.Infrastructure.Services
{
    public class AuthenService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AuthenService(IUserRepository userRepository,
                             ITokenService tokenService,
                             ICurrentUserService currentUserService,
                             IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<string>> Login(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null || !CryptHelper.VerifyPasswordHash(password, user.PasswordHash))
            {
                return Result<string>.Failure(new[] { "Invalid credentials" });
            }

            var token = _tokenService.GenerateJwtToken(user);
            return Result<string>.Success(token);
        }

        public async Task<Result<UserInfor>> UpdateInfor(UpdateInforRequest request)
        {
            Result<UserInfor> result;
            try
            {
                var user = _userRepository.Get(u => u.Id == _currentUserService.UserId);

                user.Username = request.Username;
                user.Email = request.Email;
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Roles = request.Roles;

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                result = Result<UserInfor>.Success(_mapper.Map<UserInfor>(_userRepository.Get(u => u.Id == _currentUserService.UserId)));
            }
            catch (Exception ex)
            {
                result = Result<UserInfor>.Failure(new[] { $"{ex.Message}" });
            }

            return result;
        }
    }
}