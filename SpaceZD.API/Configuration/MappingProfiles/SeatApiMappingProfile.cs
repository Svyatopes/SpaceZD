using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class SeatApiMappingProfile : Profile
{
    public SeatApiMappingProfile()
    {
        CreateMap<SeatModel, SeatOutputModel>();
    }
}