using _1.Domain.Entities;
using _2.Application.DTOs.User;
using AutoMapper;

namespace _2.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles,
                       opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleType).ToList()));
        }
    }
}
