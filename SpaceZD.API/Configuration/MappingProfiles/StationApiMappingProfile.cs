using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class StationApiMappingProfile : Profile
{
    public StationApiMappingProfile()
    {
        CreateMap<StationInputModel, StationModel>();

        CreateMap<StationModel, StationShortOutputModel>();
        CreateMap<StationModel, StationFullOutputModel>();
    }
}