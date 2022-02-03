using AutoMapper;

namespace SpaceZD.API.Configuration
{
    public static class APIMapper
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

        }
    }
}
