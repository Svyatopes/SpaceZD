using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripRepository : BaseRepository, ITripRepository
{
    public TripRepository(VeryVeryImportantContext context) : base(context) {}

    public Trip? GetById(int id) =>
        _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Train)
                .Include(t => t.Stations)
                .FirstOrDefault(t => t.Id == id);

    public List<Trip> GetList(bool includeAll = false) => _context.Trips.Where(t => !t.IsDeleted || includeAll).ToList();

    public int Add(Trip trip)
    {
        _context.Trips.Add(trip);
        _context.SaveChanges();
        return trip.Id;
    }

    public void Update(Trip entityToEdit, Trip newEntity)
    {
        entityToEdit.Train = newEntity.Train;
        entityToEdit.Route = newEntity.Route;
        entityToEdit.Stations = newEntity.Stations;
        entityToEdit.StartTime = newEntity.StartTime;

        _context.SaveChanges();
    }

    public void Update(Trip entity, bool isDeleted)
    {
        entity.IsDeleted = isDeleted;

        _context.SaveChanges();
    }

    public List<CarriageSeats> MarkNonFreeSeatsInListAllSeats(Trip trip, Station startStation, Station endStation, List<CarriageSeats> allPlaces)
    {
        int start = 0;
        int count = 0;
        var stations = new List<TripStation>(trip.Stations);

        for (int i = 0; i < stations.Count; i++)
        {
            if (stations[i].Station.Equals(startStation))
                start = i;

            if (stations[i].Station.Equals(endStation))
            {
                count = i - start;
                break;
            }
        }
        var stationsToTheEnd = stations.Take(start + count).ToList();
        var stationsAfterTheStart = stations.Skip(start + 1).Take(stations.Count - start - 1).ToList();

        foreach (var order in trip.Orders)
        {
            if (stationsToTheEnd.Contains(order.StartStation) && stationsAfterTheStart.Contains(order.EndStation))
                foreach (var ticket in order.Tickets)
                {
                    allPlaces
                       .Single(g => g.Carriage.Equals(ticket.Carriage))
                       .Seats
                       .Single(g => g.NumberOfSeats == ticket.SeatNumber)
                       .IsFree = false;
                }
        }

        return allPlaces;
    }
}