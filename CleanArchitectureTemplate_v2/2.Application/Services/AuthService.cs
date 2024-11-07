using _1.Domain.Entities;
using _1.Domain.Interfaces;
using _2.Application.Common;
using _2.Application.DTOs.User;
using _2.Application.Interfaces;
using _3.Infrastructure.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _2.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<User> _userRepository => _unitOfWork.GetRepository<User>();
        private IRepository<Role> _roleRepository => _unitOfWork.GetRepository<Role>();
        private IRepository<UserRole> _userRoleRepository => _unitOfWork.GetRepository<UserRole>();
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork,
                           IJwtTokenService jwtTokenService,
                           ICurrentUserService currentUserService,
                           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public Result<string> Login(string username, string password)
        {
            var user = _userRepository.GetIQueryable().Include(p => p.UserRoles).ThenInclude(p => p.Role).FirstOrDefault(p => p.Username == username);
            if (user == null || !PasswordHelper.VerifyPasswordHash(password, user.PasswordHash))
            {
                return Result<string>.Failure(new[] { "Invalid credentials" });
            }

            var token = _jwtTokenService.GenerateJwtToken(user);
            return Result<string>.Success(token);
        }

        public Result<UserDto> GetAccountInfor()
        {
            var user = _userRepository.GetIQueryable().Include(p => p.UserRoles).ThenInclude(p => p.Role).FirstOrDefault(p => p.Id == _currentUserService.UserId);

            if (user == null)
            {
                return Result<UserDto>.Failure(new string[] { "Your account not found" });
            }

            return Result<UserDto>.Success(_mapper.Map<User, UserDto>(user));
        }

        public Result<UserDto> UpdateAccountInfor(UserDto userDto) {
            try
            {
                _unitOfWork.CreateTransaction();
                var user = _userRepository.GetIQueryable().Include(p => p.UserRoles).ThenInclude(p => p.Role).FirstOrDefault(p => p.Id == _currentUserService.UserId);

                //update base infor
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                _userRepository.Update(user);

                //remove old role
                var oldUserRoles = _userRoleRepository.GetIQueryable().Where(p => p.UserId == _currentUserService.UserId).ToList();
                _userRoleRepository.RemoveRange(oldUserRoles);

                //add new role
                List<UserRole> userRoles = new List<UserRole>();
                foreach (var role in userDto.Roles) {
                    var tmpRole = _roleRepository.Get(p => p.RoleType == role);
                    userRoles.Add(new UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = _currentUserService.UserId,
                        RoleId = tmpRole.Id,
                    });
                }
                if (userRoles.Count > 0)
                {
                    _userRoleRepository.AddRange(userRoles);
                }

                _unitOfWork.Save();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }

            return Result<UserDto>.Success(userDto);
        }
    }
}
