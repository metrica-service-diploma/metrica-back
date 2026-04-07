using AutoMapper;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Common.Mappings;

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
