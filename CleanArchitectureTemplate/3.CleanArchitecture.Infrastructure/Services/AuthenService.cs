using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.Common.Crypt;
using _2.CleanArchitecture.Application.Common.Interfaces;
using _2.CleanArchitecture.Application.Features.IRepositories;
using _2.CleanArchitecture.Application.Features.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3.CleanArchitecture.Infrastructure.Services
{
    public class AuthenService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
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
    }
}
