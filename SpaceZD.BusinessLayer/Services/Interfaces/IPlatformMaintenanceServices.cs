using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPlatformMaintenanceServices
    {
        int Add(PlatformMaintenanceModel platformMaintenance);
        void Delete(int id);
        PlatformMaintenanceModel GetById(int id);
        List<PlatformMaintenanceModel> GetList(bool allIncluded);
        void Restore(int id);
        void Update(int id, PlatformMaintenanceModel platformMaintenance);
    }
}