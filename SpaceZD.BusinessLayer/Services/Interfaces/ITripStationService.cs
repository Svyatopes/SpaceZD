using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripStationService : IGetByIdWithUserId<TripStationModel>
{
    void Update(int userId, int id, TripStationModel model);
    void SetPlatform(int userId, int id, int idPlatform);
    List<PlatformModel> GetReadyPlatforms(int userId, int id);
}