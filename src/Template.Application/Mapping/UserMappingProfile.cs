using AutoMapper;
using Template.Application.DTOs;
using Template.Domain.Entities;

namespace Template.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
