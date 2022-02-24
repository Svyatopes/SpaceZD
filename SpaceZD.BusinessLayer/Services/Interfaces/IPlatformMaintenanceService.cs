using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPlatformMaintenanceService
    {
        int Add(int userId, PlatformMaintenanceModel platformMaintenance);
        void Delete(int userId, int id);
        PlatformMaintenanceModel GetById(int id);
        List<PlatformMaintenanceModel> GetList(int userId);
        List<PlatformMaintenanceModel> GetListDeleted(int userId);
        void Restore(int userId, int id);
        void Update(int userId, int id, PlatformMaintenanceModel platformMaintenance);
    }
}