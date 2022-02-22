using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;
public interface IPlatformRepository
{
    int Add(Platform platform);
    Platform? GetById(int id);
    List<Platform> GetList(int stationId, bool includeAll = false);
    void Update(Platform platform, bool isDeleted);
    void Update(Platform oldPlatform, Platform newPlatform);
}
