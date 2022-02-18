using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TripService : BaseService, ITripService
{
    private readonly IRepositorySoftDelete<Trip> _repository;
    private readonly IStationRepository _stationRepository;
    private readonly IRepositorySoftDelete<Route> _routeRepository;
    private readonly IRepositorySoftDelete<Train> _trainRepository;

    public TripService(IMapper mapper, IRepositorySoftDelete<Trip> repository, IStationRepository stationRepository, IRepositorySoftDelete<Route> routeRepository,
                       IRepositorySoftDelete<Train> trainRepository) : base(mapper)
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

    public List<TripModel> GetList() => _mapper.Map<List<TripModel>>(_repository.GetList());

    public List<TripModel> GetListDeleted() => _mapper.Map<List<TripModel>>(_repository.GetList(true).Where(t => t.IsDeleted));

    public void Delete(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, true);
    }

    public void Restore(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, false);
    }

    public void Update(int id, TripModel tripModel)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, _mapper.Map<Trip>(tripModel));
    }

    public int Add(TripModel tripModel)
    {
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

        var routeModel = _mapper.Map<RouteModel>(route);
        var trainModel = _mapper.Map<TrainModel>(train);

        tripModel.Route = routeModel;
        tripModel.Train = trainModel;

        if (!routeModel.RouteTransits.Any())
            throw new InvalidDataException("У выбранного маршрута отсутствуют перегоны между станциями.");
        
        
        tripModel.Stations.Add(new TripStationModel
        {
            ArrivalTime = new DateTime(), //TODO поменять после миграций
            DepartingTime = tripModel.StartTime,
            Station = routeModel.RouteTransits[0].Transit.StartStation
        });

        for (int i = 0; i < routeModel.RouteTransits.Count - 1; i++)
        {
            tripModel.Stations.Add(new TripStationModel
            {
                ArrivalTime = tripModel.StartTime.Add(routeModel.RouteTransits[i].ArrivalTime),
                DepartingTime = tripModel.StartTime.Add(routeModel.RouteTransits[i + 1].DepartingTime),
                Station = routeModel.RouteTransits[i].Transit.EndStation
            });
        }

        tripModel.Stations.Add(new TripStationModel
        {
            ArrivalTime = tripModel.StartTime.Add(routeModel.RouteTransits[^1].ArrivalTime),
            DepartingTime = new DateTime(), //TODO поменять после миграций
            Station = routeModel.RouteTransits[^1].Transit.EndStation
        });
        
        var trip = _mapper.Map<Trip>(tripModel);
        return _repository.Add(trip);
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