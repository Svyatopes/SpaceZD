using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TripService : BaseService, ITripService
{
    private readonly IRepositorySoftDelete<Trip> _repository;
    private readonly IStationRepository _stationRepository;
    private readonly IRepositorySoftDelete<Route> _routeRepository;
    private readonly IRepositorySoftDelete<Train> _trainRepository;
    private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };

    public TripService(IMapper mapper, IUserRepository userRepository, IRepositorySoftDelete<Trip> repository, IStationRepository stationRepository,
        IRepositorySoftDelete<Route> routeRepository, IRepositorySoftDelete<Train> trainRepository) : base(mapper, userRepository)
    {
        _repository = repository;
        _stationRepository = stationRepository;
        _routeRepository = routeRepository;
        _trainRepository = trainRepository;
    }


    public TripModel GetById(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<TripModel>(entity);
    }


    public List<TripModel> GetList()
    {
        var entities = _repository.GetList();
        return _mapper.Map<List<TripModel>>(entities);
    }

    public List<TripModel> GetListDeleted(int userId)
    {
        CheckUserRole(userId, Role.Admin);

        var entities = _repository.GetList(true).Where(t => t.IsDeleted);
        return _mapper.Map<List<TripModel>>(entities);
    }

    public void Delete(int userId, int id)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, true);
    }

    public void Restore(int userId, int id)
    {
        CheckUserRole(userId, Role.Admin);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, false);
    }


    public void Update(int userId, int id, TripModel tripModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        var train = _trainRepository.GetById(tripModel.Train.Id);
        ThrowIfEntityNotFound(train, tripModel.Train.Id);

        tripModel.Train = _mapper.Map<TrainModel>(train);

        _repository.Update(entity!, _mapper.Map<Trip>(tripModel));
    }


    public int Add(int userId, TripModel tripModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var route = _routeRepository.GetById(tripModel.Route.Id);
        ThrowIfEntityNotFound(route, tripModel.Route.Id);
        var train = _trainRepository.GetById(tripModel.Train.Id);
        ThrowIfEntityNotFound(train, tripModel.Train.Id);

        tripModel.StartTime = new DateTime(tripModel.StartTime.Year,
            tripModel.StartTime.Month,
            tripModel.StartTime.Day,
            route!.StartTime.Hour,
            route.StartTime.Minute,
            route.StartTime.Second);
        tripModel.IsDeleted = false;

        var trip = _mapper.Map<Trip>(tripModel);

        trip.Route = route;
        trip.Train = train!;
        trip.Stations = new List<TripStation>();

        if (!route.RouteTransits.Any())
            throw new InvalidDataException("У выбранного маршрута отсутствуют перегоны между станциями.");

        CreateTripStationForTripFromRoute(trip, route.RouteTransits.ToList());

        return _repository.Add(trip);
    }

    private void CreateTripStationForTripFromRoute(Trip tripModel, List<RouteTransit> routeTransits)
    {
        tripModel.Stations.Add(new TripStation
        {
            ArrivalTime = null,
            DepartingTime = tripModel.StartTime,
            Station = routeTransits[0].Transit.StartStation
        });

        for (int i = 0; i < routeTransits.Count - 1; i++)
        {
            tripModel.Stations.Add(new TripStation
            {
                ArrivalTime = tripModel.StartTime.Add(routeTransits[i].ArrivalTime),
                DepartingTime = tripModel.StartTime.Add(routeTransits[i + 1].DepartingTime),
                Station = routeTransits[i].Transit.EndStation
            });
        }

        tripModel.Stations.Add(new TripStation
        {
            ArrivalTime = tripModel.StartTime.Add(routeTransits[^1].ArrivalTime),
            DepartingTime = null,
            Station = routeTransits[^1].Transit.EndStation
        });
    }


    public List<CarriageSeatsModel> GetFreeSeat(int idTrip, int idStartStation, int idEndStation)
    {
        var trip = _repository.GetById(idTrip);
        ThrowIfEntityNotFound(trip, idTrip);

        var tripModel = _mapper.Map<TripModel>(trip);

        FillStationsToEndAndAfterStart(trip!.Stations.ToList(), idStartStation, idEndStation, out var stationsToTheEnd, out var stationsAfterTheStart);

        var allPlacesModels = GetCompletedAllPlacesModels(tripModel.Train.Carriages);

        MarkingOccupiedSeats(trip.Orders.Where(order => stationsToTheEnd.Contains(order.StartStation) && stationsAfterTheStart.Contains(order.EndStation))
                                 .SelectMany(order => order.Tickets),
            allPlacesModels);

        return allPlacesModels;
    }

    private List<CarriageSeatsModel> GetCompletedAllPlacesModels(IEnumerable<CarriageModel> carriages)
    {
        var allPlacesModels = new List<CarriageSeatsModel>();
        foreach (var carriage in carriages)
        {
            allPlacesModels.Add(new CarriageSeatsModel { Carriage = carriage, Seats = new List<SeatModel>() });
            for (int i = 1; i <= carriage.Type.NumberOfSeats; i++)
                allPlacesModels.Single(g => g.Carriage.Equals(carriage))
                               .Seats
                               .Add(new SeatModel { NumberOfSeats = i, IsFree = true });
        }

        return allPlacesModels;
    }

    private void FillStationsToEndAndAfterStart(List<TripStation> stations, int idStartStation, int idEndStation,
        out List<TripStation> stationsToTheEnd, out List<TripStation> stationsAfterTheStart)
    {
        var startStation = _stationRepository.GetById(idStartStation);
        ThrowIfEntityNotFound(startStation, idStartStation);
        var endStation = _stationRepository.GetById(idEndStation);
        ThrowIfEntityNotFound(endStation, idEndStation);

        int start = 0;
        int count = 0;
        var findStartStation = false;
        var findEndStation = false;

        for (int i = 0; i < stations.Count; i++)
        {
            if (findStartStation && stations[i].Station.Equals(endStation))
            {
                count = i - start;
                findEndStation = true;
                break;
            }

            if (stations[i].Station.Equals(startStation))
            {
                start = i;
                findStartStation = true;
            }
        }

        if (!findEndStation)
            throw new ArgumentException("Данная комбинация начальной и конечной станции невозможна для данного Trip");

        stationsToTheEnd = stations.Take(start + count).ToList();
        stationsAfterTheStart = stations.Skip(start + 1)
                                        .Take(stations.Count - start - 1)
                                        .ToList();
    }

    private void MarkingOccupiedSeats(IEnumerable<Ticket> tickets, List<CarriageSeatsModel> allPlacesModels)
    {
        foreach (var ticket in tickets)
            allPlacesModels
               .Single(g => g.Carriage.Equals(_mapper.Map<CarriageModel>(ticket.Carriage)))
               .Seats
               .Single(g => g.NumberOfSeats == ticket.SeatNumber)
               .IsFree = false;
    }
}