using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IStationService : IDeleteRestoreUpdate<StationModel>, IGetByIdWithUserId<StationModel>, IAddWithUserId<StationModel>
{
    List<StationModel> GetNearStations(int userId, int id);
    List<StationModel> GetList();
    List<StationModel> GetListDeleted(int userId);
}