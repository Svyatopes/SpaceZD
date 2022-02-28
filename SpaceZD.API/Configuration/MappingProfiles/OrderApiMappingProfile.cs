using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class OrderApiMappingProfile : Profile
{
    public OrderApiMappingProfile()
    {
         CreateMap<OrderAddInputModel, OrderModel>()
             .ForMember(om => om.Trip, opt => opt.MapFrom(oim => new TripModel { Id = oim.TripId }))
             .ForMember(om => om.StartStation, opt => opt.MapFrom(oim => new TripStationModel { Id = oim.StartTripStationId }))
             .ForMember(om => om.EndStation, opt => opt.MapFrom(oim => new TripStationModel { Id = oim.EndTripStationId }));

         CreateMap<OrderModel, OrderShortOutputModel>()
             .ForMember(oom => oom.TripId, opt => opt.MapFrom(om => om.Trip.Id))
             .ForMember(oom => oom.UserId, opt => opt.MapFrom(om => om.User.Id))
             .ForMember(oom => oom.StartTripStationId, opt => opt.MapFrom(om => om.StartStation.Id))
             .ForMember(oom => oom.EndTripStationId, opt => opt.MapFrom(om => om.EndStation.Id));
         CreateMap<OrderModel, OrderFullOutputModel>()
             .ForMember(oom => oom.TripId, opt => opt.MapFrom(om => om.Trip.Id))
             .ForMember(oom => oom.UserId, opt => opt.MapFrom(om => om.User.Id))
             .ForMember(oom => oom.StartTripStationId, opt => opt.MapFrom(om => om.StartStation.Id))
             .ForMember(oom => oom.EndTripStationId, opt => opt.MapFrom(om => om.EndStation.Id));
    }
}