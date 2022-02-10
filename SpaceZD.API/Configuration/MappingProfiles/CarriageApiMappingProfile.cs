using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class CarriageApiMappingProfile : Profile
{
    public CarriageApiMappingProfile()
    {
        CreateMap<CarriageInputModel, CarriageModel>()
            .ForMember(cm => cm.Type, opt => opt.MapFrom(cim => new CarriageTypeModel { Id = cim.TypeId }))
            .ForMember(cm => cm.Train, opt => opt.MapFrom(cim => new TrainModel { Id = cim.TrainId }));

        CreateMap<CarriageModel, CarriageOutputModel>()
            .ForMember(com => com.CarriageTypeId, opt => opt.MapFrom(cm => cm.Type.Id))
            .ForMember(com => com.TrainId, opt => opt.MapFrom(cm => cm.Train.Id));
    }
}