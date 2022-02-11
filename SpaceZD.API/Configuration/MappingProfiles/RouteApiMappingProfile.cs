using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class RouteApiMappingProfile : Profile
{
    public RouteApiMappingProfile()
    {
        CreateMap<RouteInputModel, RouteModel>()
            .ForMember(rm => rm.StartStation, opt => opt.MapFrom(rim => new StationModel { Id = rim.StartStationId }))
            .ForMember(rm => rm.EndStation, opt => opt.MapFrom(rim => new StationModel { Id = rim.EndStationId }));

        CreateMap<RouteModel, RouteOutputModel>()
            .ForMember(rom => rom.StartStationId, opt => opt.MapFrom(rm => rm.StartStation.Id))
            .ForMember(rom => rom.EndStationId, opt => opt.MapFrom(rm => rm.EndStation.Id));
    }
}