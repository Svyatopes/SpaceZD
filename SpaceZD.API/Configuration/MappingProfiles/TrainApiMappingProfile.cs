using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration
{
    public class TrainApiMappingProfile : Profile
    {
        public TrainApiMappingProfile()
        {
            CreateMap<TrainModel, TrainOutputModel>();
        }
    }
}
