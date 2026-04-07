using AutoMapper;
using metrica_back.src.Core.Dtos;
using metrica_back.src.Core.Models;

namespace metrica_back.src.Core.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>();
    }
}

public class WebsiteProfile : Profile
{
    public WebsiteProfile()
    {
        CreateMap<Website, WebsiteResponseDto>();
    }
}
