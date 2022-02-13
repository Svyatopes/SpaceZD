using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface IStationRepository : IRepositorySoftDeleteNewUpdate<Station>
{
    IEnumerable<Platform> GetReadyPlatformsStation(Station station, DateTime moment);
}