using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.BusinessLayer.Configuration
{
    public static class BusinessLayerMapper
    {
        private static Mapper? _instance;

        public static Mapper GetInstance()
        {
            if (_instance is null)
                InitializeInstance();
            return _instance!;
        }

        private static void InitializeInstance()
        {
            _instance = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Carriage, CarriageModel>().ReverseMap();
                cfg.CreateMap<CarriageType, CarriageTypeModel>().ReverseMap();
                cfg.CreateMap<Order, OrderModel>().ReverseMap();
                cfg.CreateMap<Person, PersonModel>().ReverseMap();
                cfg.CreateMap<Platform, PlatformModel>().ReverseMap();
                cfg.CreateMap<PlatformMaintenance, PlatformMaintenanceModel>().ReverseMap();
                cfg.CreateMap<Route, RouteModel>().ReverseMap();
                cfg.CreateMap<RouteTransit, RouteTransitModel>().ReverseMap();
                cfg.CreateMap<Station, StationModel>().ReverseMap();
                cfg.CreateMap<Ticket, TicketModel>().ReverseMap();
                cfg.CreateMap<Train, TrainModel>().ReverseMap();
                cfg.CreateMap<Transit, TransitModel>().ReverseMap();
                cfg.CreateMap<Trip, TripModel>().ReverseMap();
                cfg.CreateMap<TripStation, TripStationModel>().ReverseMap();
                cfg.CreateMap<User, UserModel>().ReverseMap();
            }));
        }
    }
}