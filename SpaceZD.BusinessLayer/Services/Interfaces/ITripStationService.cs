using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripStationService
{
    TripStationModel GetById(int id);
    void Update(int id, TripStationModel model);
    List<PlatformModel> GetReadyPlatforms(int id);
}