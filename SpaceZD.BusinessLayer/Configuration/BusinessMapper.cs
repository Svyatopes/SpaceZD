using AutoMapper;

namespace SpaceZD.BusinessLayer.Configuration
{
    public static class BusinessMapper
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

                cfg.AddProfile<UserMapping>();

            }));
        }
    }
}

