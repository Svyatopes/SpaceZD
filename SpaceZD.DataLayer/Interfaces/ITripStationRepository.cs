using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface ITripStationRepository
{
    TripStation? GetById(int id);
    List<TripStation> GetList();
    void Update(TripStation entityToEdit, TripStation newEntity);
}