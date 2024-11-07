using _1.Domain.Interfaces;
using _1.Domain.Interfaces.Commons;
using _2.Application.DTOs;
using _2.Application.Interfaces;
using AutoMapper;

namespace _2.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
                           ICurrentUserService currentUserService,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public UserDto GetUserInfor(Guid Id)
        {
            var user = _userRepository.Get(p=>p.Id == Id);
            return _mapper.Map<UserDto>(user);
        }
    }
}