using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface ITripRepository : IRepositorySoftDelete<Trip>
{
    public List<CarriageSeats> MarkNonFreeSeatsInListAllSeats(Trip trip, Station startStation, Station endStation, List<CarriageSeats> allPlaces);
}