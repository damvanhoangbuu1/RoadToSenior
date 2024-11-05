using _1.CleanArchitecture.Domain.Entities;
using _2.CleanArchitecture.Application.DTOs.Auth;
using AutoMapper;

namespace _2.CleanArchitecture.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserInfor>().ReverseMap();
        }
    }
}