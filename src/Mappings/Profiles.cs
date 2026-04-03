using AutoMapper;
using metrica_back.src.Dto;
using metrica_back.src.Models;

namespace metrica_back.src.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponseDto>();
    }
}

public class WebsiteMappingProfile : Profile
{
    public WebsiteMappingProfile()
    {
        CreateMap<Website, WebsiteResponseDto>();
    }
}
