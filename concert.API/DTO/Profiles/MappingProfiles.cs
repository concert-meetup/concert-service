using AutoMapper;
using concert.API.Models;

namespace concert.API.DTO.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Concert, ConcertDTO>();
        CreateMap<Venue, VenueDTO>();
        
        CreateMap<Concert, ConcertSummaryDTO>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist))
            .ForMember(dest => dest.ConcertDate, opt => opt.MapFrom(src => src.ConcertDate))
            .ForMember(dest => dest.VenueSummary, opt => opt.MapFrom(src => src.Venue));

        CreateMap<Venue, VenueSummaryDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));

        CreateMap<Venue, VenueInfoDTO>()
            .ForMember(d => d.VenueName, opt => opt.MapFrom(src => src.Name))
            .ForMember(d => d.City, opt => opt.MapFrom(src => src.City))
            .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country));

        CreateMap<Concert, ConcertInfoDTO>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Artist, opt => opt.MapFrom(src => src.Artist))
            .ForMember(d => d.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(d => d.ConcertDate, opt => opt.MapFrom(src => src.ConcertDate))
            .ForMember(d => d.VenueInfo, opt => opt.MapFrom(src => src.Venue));
    }
}