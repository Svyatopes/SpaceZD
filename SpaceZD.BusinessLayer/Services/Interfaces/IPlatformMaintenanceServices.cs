using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPlatformMaintenanceServices
    {
        int Add(PlatformMaintenanceModel platformMaintenance);
        void Deleted(int id);
        PlatformMaintenance GetById(int id);
        List<PlatformMaintenance> GetList(bool allIncluded);
        void Reastore(int id);
        void Update(int id, PlatformMaintenanceModel platformMaintenance);
    }
}