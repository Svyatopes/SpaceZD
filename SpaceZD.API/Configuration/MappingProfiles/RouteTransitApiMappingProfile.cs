using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class RouteTransitApiMappingProfile : Profile
{
    public RouteTransitApiMappingProfile()
    {
        CreateMap<RouteTransitInputModel, RouteTransitModel>()
            .ForMember(rtm => rtm.Transit, opt => opt.MapFrom(rtim => new TransitModel { Id = rtim.TransitId }))
            .ForMember(rtm => rtm.Route, opt => opt.MapFrom(rtim => new RouteModel { Id = rtim.RouteId }));

        CreateMap<RouteTransitModel, RouteTransitOutputModel>()
            .ForMember(rtom => rtom.TransitId, opt => opt.MapFrom(rtm => rtm.Transit.Id))
            .ForMember(rtom => rtom.RouteId, opt => opt.MapFrom(rtm => rtm.Route.Id));
    }
}