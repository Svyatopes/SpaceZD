using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class CarriageApiMappingProfile : Profile
{
    public CarriageApiMappingProfile()
    {
        CreateMap<CarriageInputModel, CarriageModel>()
            .ForMember(cm => cm.Type, opt => opt.MapFrom(cim => new CarriageTypeModel { Id = cim.TypeId }));

        CreateMap<CarriageModel, CarriageShortOutputModel>();
        CreateMap<CarriageModel, CarriageFullOutputModel>()
            .ForMember(cfom => cfom.CarriageTypeId, opt => opt.MapFrom(cm => cm.Type.Id));
    }
}