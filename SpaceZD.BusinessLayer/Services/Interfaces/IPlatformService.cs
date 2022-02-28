using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IPlatformService : IDeleteRestoreUpdate<PlatformModel>, IGetByIdWithUserId<PlatformModel>, IAddWithUserId<PlatformModel>
{
    List<PlatformModel> GetListByStationId(int userId, int stationId);
}