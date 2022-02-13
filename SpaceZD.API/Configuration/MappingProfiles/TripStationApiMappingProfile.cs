using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class TripStationApiMappingProfile : Profile
{
    public TripStationApiMappingProfile()
    {
        CreateMap<TripStationUpdateInputModel, TripStationModel>()
            .ForMember(tsm => tsm.Platform, opt => opt.MapFrom(tsuim => new PlatformModel { Id = tsuim.PlatformId }));

        CreateMap<TripStationModel, TripStationOutputModel>()
            .ForMember(tsom => tsom.PlatformId, opt => opt.MapFrom(tsm => tsm.Platform.Id))
            .ForMember(tsom => tsom.StationId, opt => opt.MapFrom(tsm => tsm.Station.Id));
    }
}