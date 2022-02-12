using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class PlatformMaintenanceApiMappingProfile : Profile
{
    public PlatformMaintenanceApiMappingProfile()
    {
        CreateMap<PlatformMaintenanceInputModel, PlatformMaintenanceModel>()
            .ForMember(pmm => pmm.Platform, opt => opt.MapFrom(pmim => new PlatformModel { Id = pmim.PlatformId }));

        CreateMap<PlatformMaintenanceModel, PlatformMaintenanceOutputModel>()
            .ForMember(pmom => pmom.PlatformId, opt => opt.MapFrom(pmm => pmm.Platform.Id));
    }
}