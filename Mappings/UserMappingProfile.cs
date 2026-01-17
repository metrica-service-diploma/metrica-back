using AutoMapper;
using metrica_back.Dto;
using metrica_back.Models;

namespace metrica_back.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
