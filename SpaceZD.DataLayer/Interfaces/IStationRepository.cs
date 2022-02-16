using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface IStationRepository : IRepositorySoftDelete<Station>
{
    List<Platform> GetReadyPlatformsStation(Station station, DateTime startMoment, DateTime endMoment);
    List<Station> GetNearStations(Station station);
}