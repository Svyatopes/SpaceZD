using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IStationService
{
    StationModel GetById(int userId, int id);
    List<StationModel> GetNearStations(int userId, int id);
    List<StationModel> GetList();
    List<StationModel> GetListDeleted(int userId);
    int Add(int userId, StationModel stationModel);
    void Delete(int userId, int id);
    void Restore(int userId, int id);
    void Update(int userId, int id, StationModel stationModel);
}