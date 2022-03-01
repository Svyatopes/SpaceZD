using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IPlatformMaintenanceService : IDeleteRestoreUpdate<PlatformMaintenanceModel>, IGetByIdWithUserId<PlatformMaintenanceModel>, IAddWithUserId<PlatformMaintenanceModel>
{
    List<PlatformMaintenanceModel> GetListByStationId(int stationId, int userId);
    List<PlatformMaintenanceModel> GetListDeletedByStationId(int stationId, int userId);
}