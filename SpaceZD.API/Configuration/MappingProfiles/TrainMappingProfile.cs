using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration
{
    public class TrainMappingProfile : Profile
    {
        public TrainMappingProfile()
        {
            CreateMap<TrainModel, TrainOutputModel>();
        }
    }
}
