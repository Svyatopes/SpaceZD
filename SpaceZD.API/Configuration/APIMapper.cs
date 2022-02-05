using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration
{
    public static class ApiMapper
    {
        private static Mapper _instance;

        public static Mapper GetInstance()
        {
            if (_instance == null)
                InitializeInstance();
            return _instance;
        }

        private static void InitializeInstance()
        {
            _instance = new Mapper(new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<TicketMappingProfile>();
                cfg.AddProfile<TrainMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();

            }));
        }
    }
}
