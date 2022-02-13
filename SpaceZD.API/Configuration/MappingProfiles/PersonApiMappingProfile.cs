using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class PersonApiMappingProfile : Profile
{
    public PersonApiMappingProfile()
    {
        CreateMap<PersonInputModel, PersonModel>();
        
        CreateMap<PersonModel, PersonOutputModel>();
    }
}