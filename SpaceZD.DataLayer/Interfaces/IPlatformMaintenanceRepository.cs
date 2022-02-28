using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public interface IPlatformMaintenanceRepository
    {
        int Add(PlatformMaintenance platformMaintenance);
        PlatformMaintenance? GetById(int id);
        List<PlatformMaintenance> GetListByStationId(int stationId, bool includeAll = false);
        void Update(PlatformMaintenance oldPlatformMaintenance, bool isDeleted);
        void Update(PlatformMaintenance oldPlatformMaintenance, PlatformMaintenance newPlatformMaintenance);
    }
}