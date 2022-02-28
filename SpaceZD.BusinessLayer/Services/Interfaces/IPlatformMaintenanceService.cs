using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPlatformMaintenanceService
    {
        int Add(int userId, PlatformMaintenanceModel platformMaintenanceModel);
        void Delete(int userId, int id);
        PlatformMaintenanceModel GetById(int id, int userId);
        List<PlatformMaintenanceModel> GetListByStationId(int stationId, int userId);
        List<PlatformMaintenanceModel> GetListDeletedByStationId(int stationId, int userId);
        void Restore(int userId, int id);
        void Update(int userId, int id, PlatformMaintenanceModel platformMaintenanceModel);
    }
}