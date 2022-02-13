using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IStationService
{
    StationModel GetById(int id);
    List<StationModel> GetNearStations(int id);
    List<StationModel> GetList();
    List<StationModel> GetListDeleted();
    int Add(StationModel stationModel);
    void Delete(int id);
    void Restore(int id);
    void Update(int id, StationModel stationModel);
    List<PlatformModel> GetReadyPlatformsByStationId(int id, DateTime moment);
}