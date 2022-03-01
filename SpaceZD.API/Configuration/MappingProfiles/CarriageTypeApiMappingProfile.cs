using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class CarriageTypeApiMappingProfile : Profile
{
    public CarriageTypeApiMappingProfile()
    {
        CreateMap<CarriageTypeInputModel, CarriageTypeModel>();

        CreateMap<CarriageTypeModel, CarriageTypeFullOutputModel>();
        CreateMap<CarriageTypeModel, CarriageTypeShortOutputModel>();
    }
}