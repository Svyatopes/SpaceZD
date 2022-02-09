using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class RouteService : IRouteService
{
    private readonly IMapper _mapper;
    private readonly IRepositorySoftDelete<Route> _routeRepository;
    private readonly IRepositorySoftDelete<Station> _stationRepository;

    public RouteService(IMapper mapper, IRepositorySoftDelete<Route> routeRepository, IRepositorySoftDelete<Station> stationRepository)
    {
        _mapper = mapper;
        _routeRepository = routeRepository;
        _stationRepository = stationRepository;
    }

    public RouteModel GetById(int id)
    {
        var entity = _routeRepository.GetById(id);
        if (entity is null)
            NotFound(nameof(Route), id);
        return _mapper.Map<RouteModel>(entity);
    }

    public List<RouteModel> GetList() => _mapper.Map<List<RouteModel>>(_routeRepository.GetList());
    public List<RouteModel> GetListDeleted() => _mapper.Map<List<RouteModel>>(_routeRepository.GetList(true).Where(t => t.IsDeleted));

    public int Add(RouteModel routeModel)
    {
        var startStation = _stationRepository.GetById(routeModel.StartStation.Id);
        if (startStation is null)
            NotFound(nameof(Station), routeModel.StartStation.Id);
        var endStation = _stationRepository.GetById(routeModel.EndStation.Id);
        if (endStation is null)
            NotFound(nameof(Station), routeModel.EndStation.Id);

        var route = _mapper.Map<Route>(routeModel);
        route.StartStation = startStation!;
        route.EndStation = endStation!;

        return _routeRepository.Add(route);
    }

    public void Delete(int id)
    {
        if (!_routeRepository.Update(id, true))
            NotFound(nameof(Route), id);
    }

    public void Restore(int id)
    {
        if (!_routeRepository.Update(id, false))
            NotFound(nameof(Route), id);
    }

    public void Update(int id, RouteModel routeModel)
    {
        routeModel.Id = id;
        if (!_routeRepository.Update(_mapper.Map<Route>(routeModel)))
            NotFound(nameof(Route), id);
    }

    private static void NotFound(string name, int id) => throw new NotFoundException($"{name} c Id = {id} не найден");
}