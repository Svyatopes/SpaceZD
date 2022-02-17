using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TripService : BaseService, ITripService
{
    private readonly IRepositorySoftDelete<Trip> _repository;
    private readonly IStationRepository _stationRepository;

    public TripService(IMapper mapper, IRepositorySoftDelete<Trip> repository, IStationRepository stationRepository) : base(mapper)
    {
        _repository = repository;
        _stationRepository = stationRepository;
    }

    public List<CarriageSeatsModel> GetFreeSeat(int idTrip, int idStartStation, int idEndStation)
    {
        var trip = _repository.GetById(idTrip);
        ThrowIfEntityNotFound(trip, idTrip);

        var startStation = _stationRepository.GetById(idStartStation);
        ThrowIfEntityNotFound(startStation, idStartStation);
        var endStation = _stationRepository.GetById(idEndStation);
        ThrowIfEntityNotFound(endStation, idEndStation);

        var tripModel = _mapper.Map<TripModel>(trip);
        var startStationModel = _mapper.Map<StationModel>(startStation);
        var endStationModel = _mapper.Map<StationModel>(endStation);

        var allPlacesModels = new List<CarriageSeatsModel>();
        foreach (var carriage in tripModel.Train.Carriages)
        {
            allPlacesModels.Add(new CarriageSeatsModel { Carriage = carriage, Seats = new List<SeatModel>() });
            for (int i = 1; i <= carriage.Type.NumberOfSeats; i++)
                allPlacesModels.Single(g => g.Carriage.Equals(carriage))
                               .Seats
                               .Add(new SeatModel { NumberOfSeats = i, IsFree = true });
        }

        int start = 0;
        int count = 0;
        var findStartStation = false;
        var findEndStation = false;

        for (int i = 0; i < tripModel.Stations.Count; i++)
        {
            if (findStartStation && tripModel.Stations[i].Station.Equals(endStationModel))
            {
                count = i - start;
                findEndStation = true;
                break;
            }
            if (tripModel.Stations[i].Station.Equals(startStationModel))
            {
                start = i;
                findStartStation = true;
            }
        }
        if (!findEndStation)
            throw new ArgumentException("Данная комбинация начальной и конечной станции невозможна для данного Trip");

        var stationsToTheEnd = tripModel.Stations.Take(start + count).ToList();
        var stationsAfterTheStart = tripModel.Stations
                                             .Skip(start + 1)
                                             .Take(tripModel.Stations.Count - start - 1)
                                             .ToList();

        foreach (var ticket in tripModel.Orders
                                        .Where(order => stationsToTheEnd.Contains(order.StartStation) && stationsAfterTheStart.Contains(order.EndStation))
                                        .SelectMany(order => order.Tickets))
        {
            allPlacesModels
               .Single(g => g.Carriage.Equals(ticket.Carriage))
               .Seats
               .Single(g => g.NumberOfSeats == ticket.SeatNumber)
               .IsFree = false;
        }

        return allPlacesModels;
    }
}