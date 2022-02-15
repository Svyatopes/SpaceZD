using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface IStationRepository : IRepositorySoftDeleteNewUpdate<Station>
{
    List<Platform> GetReadyPlatformsStation(Station station, DateTime moment);
    List<Station> GetNearStations(Station station);
}