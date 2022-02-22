using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class PlatformApiMappingProfile : Profile
{
    public PlatformApiMappingProfile()
    {
        CreateMap<PlatformInputModel, PlatformModel>();

        CreateMap<PlatformAddInputModel, PlatformModel>()
            .ForMember(pm => pm.Station, opt => opt.MapFrom(pim => new StationModel { Id = pim.StationId }));

        CreateMap<PlatformModel, PlatformOutputModel>()
            .ForMember(pom => pom.StationId, opt => opt.MapFrom(pm => pm.Station.Id));

        CreateMap<PlatformModel, PlatformWithDeletedOutputModel>()
            .ForMember(pom => pom.StationId, opt => opt.MapFrom(pm => pm.Station.Id));
    }
}