using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.BusinessLayer.Configuration;

public class BusinessLayerMapper : Profile
{
    public BusinessLayerMapper()
    {
        CreateMap<Carriage, CarriageModel>().ReverseMap();
        CreateMap<CarriageType, CarriageTypeModel>().ReverseMap();
        CreateMap<Order, OrderModel>().ReverseMap();
        CreateMap<Person, PersonModel>().ReverseMap();
        CreateMap<Platform, PlatformModel>().ReverseMap();
        CreateMap<PlatformMaintenance, PlatformMaintenanceModel>().ReverseMap();
        CreateMap<Route, RouteModel>().ReverseMap();
        CreateMap<RouteTransit, RouteTransitModel>().ReverseMap();
        CreateMap<Station, StationModel>().ReverseMap();
        CreateMap<Ticket, TicketModel>().ReverseMap();
        CreateMap<Train, TrainModel>().ReverseMap();
        CreateMap<Transit, TransitModel>().ReverseMap();
        CreateMap<Trip, TripModel>().ReverseMap();
        CreateMap<TripStation, TripStationModel>().ReverseMap();
        CreateMap<User, UserModel>().ReverseMap();
        
        CreateMap<CarriageSeats, CarriageSeatsModel>().ReverseMap();
        CreateMap<Seat, SeatModel>().ReverseMap();
    }
}