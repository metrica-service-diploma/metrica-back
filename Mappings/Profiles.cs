using System.Data;
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

    public class WebsiteMappingProfile : Profile
    {
        public WebsiteMappingProfile()
        {
            CreateMap<Website, WebsiteResponseDto>();
        }
    }

    public class TrackingEventProfile : Profile
    {
        public TrackingEventProfile()
        {
            CreateMap<IDataReader, TrackingEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src["Id"]))
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src["SessionId"]))
                .ForMember(dest => dest.WebsiteId, opt => opt.MapFrom(src => src["WebsiteId"]))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src["EventType"]))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src["Timestamp"]))
                .ForMember(dest => dest.PageUrl, opt => opt.MapFrom(src => src["PageUrl"]))
                .ForMember(dest => dest.PageTitle, opt => opt.MapFrom(src => src["PageTitle"]))
                .ForMember(dest => dest.Referrer, opt => opt.MapFrom(src => src["Referrer"]))
                .ForMember(dest => dest.UserAgent, opt => opt.MapFrom(src => src["UserAgent"]))
                .ForMember(dest => dest.ScreenWidth, opt => opt.MapFrom(src => src["ScreenWidth"]))
                .ForMember(
                    dest => dest.ScreenHeight,
                    opt => opt.MapFrom(src => src["ScreenHeight"])
                )
                .ForMember(
                    dest => dest.BrowserLanguage,
                    opt => opt.MapFrom(src => src["BrowserLanguage"])
                );
        }
    }
}
