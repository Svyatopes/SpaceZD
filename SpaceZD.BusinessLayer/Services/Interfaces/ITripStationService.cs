using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripStationService
{
    TripStationModel GetById(int id);
    void Update(int id, TripStationModel model);
    public void SetPlatform(int id, int idPlatform);
    List<PlatformModel> GetReadyPlatforms(int id);
}