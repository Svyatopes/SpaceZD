using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class TransitApiMappingProfile : Profile
{
    public TransitApiMappingProfile()
    {
        CreateMap<TransitCreateInputModel, TransitModel>()
            .ForMember(tm => tm.StartStation, opt => opt.MapFrom(tcm => new StationModel { Id = tcm.StartStationId }))
            .ForMember(tm => tm.EndStation, opt => opt.MapFrom(tcm => new StationModel { Id = tcm.EndStationId }));
        CreateMap<TransitUpdateInputModel, TransitModel>()
            .ForMember(tm => tm.StartStation, opt => opt.MapFrom(tcm => new StationModel { Id = tcm.StartStationId }))
            .ForMember(tm => tm.EndStation, opt => opt.MapFrom(tcm => new StationModel { Id = tcm.EndStationId }));

        CreateMap<TransitModel, TransitOutputModel>()
            .ForMember(tom => tom.StartStationId, opt => opt.MapFrom(tm => tm.StartStation.Id))
            .ForMember(tom => tom.EndStationId, opt => opt.MapFrom(tm => tm.EndStation.Id));
    }
}