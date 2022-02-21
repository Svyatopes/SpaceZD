using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class TripApiMappingProfile : Profile
{
    public TripApiMappingProfile()
    {
        CreateMap<TripCreateInputModel, TripModel>()
            .ForMember(tm => tm.Route, opt => opt.MapFrom(tcim => new RouteModel { Id = tcim.RouteId }))
            .ForMember(tm => tm.Train, opt => opt.MapFrom(tcim => new TrainModel { Id = tcim.TrainId }));
        CreateMap<TripUpdateInputModel, TripModel>()
            .ForMember(tm => tm.Train, opt => opt.MapFrom(tuim => new TrainModel { Id = tuim.TrainId }));

        CreateMap<TripModel, TripShortOutputModel>()
            .ForMember(tom => tom.TrainId, opt => opt.MapFrom(tm => tm.Train.Id))
            .ForMember(tom => tom.RouteId, opt => opt.MapFrom(tm => tm.Route.Id));
        CreateMap<TripModel, TripFullOutputModel>()
            .ForMember(tom => tom.TrainId, opt => opt.MapFrom(tm => tm.Train.Id))
            .ForMember(tom => tom.RouteId, opt => opt.MapFrom(tm => tm.Route.Id));
    }
}