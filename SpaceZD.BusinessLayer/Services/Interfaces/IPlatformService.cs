using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPlatformService
    {
        int Add(int userId, PlatformModel platformModel);
        void Delete(int userId, int platformId);
        void Edit(int userId, PlatformModel platformModel);
        PlatformModel GetById(int userId, int platformId);
        List<PlatformModel> GetListByStationId(int userId, int stationId);
        void Restore(int userId, int platformId);
    }
}