using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class CarriageSeatsApiMappingProfile : Profile
{
    public CarriageSeatsApiMappingProfile()
    {
        CreateMap<CarriageSeatsModel, CarriageSeatsOutputModel>();
    }
}