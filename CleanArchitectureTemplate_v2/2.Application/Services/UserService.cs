using _1.Domain.Entities;
using _1.Domain.Interfaces;
using _2.Application.DTOs.User;
using _2.Application.Interfaces;
using AutoMapper;

namespace _2.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository,
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