using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class RouteService : IRouteService
{
    private readonly IMapper _mapper;
    private readonly IRepositorySoftDeleteNewUpdate<Route> _routeRepository;
    private readonly IRepositorySoftDeleteNewUpdate<Station> _stationRepository;

    public RouteService(IMapper mapper, IRepositorySoftDeleteNewUpdate<Route> routeRepository, IRepositorySoftDeleteNewUpdate<Station> stationRepository)
    {
        _mapper = mapper;
        _routeRepository = routeRepository;
        _stationRepository = stationRepository;
    }

    public RouteModel GetById(int id)
    {
        var entity = _routeRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        
        return _mapper.Map<RouteModel>(entity);
    }

    public List<RouteModel> GetList() => _mapper.Map<List<RouteModel>>(_routeRepository.GetList());
    public List<RouteModel> GetListDeleted() => _mapper.Map<List<RouteModel>>(_routeRepository.GetList(true).Where(t => t.IsDeleted));

    public int Add(RouteModel routeModel)
    {
        var startStation = _stationRepository.GetById(routeModel.StartStation.Id);
        ThrowIfEntityNotFound(startStation, routeModel.StartStation.Id);
        var endStation = _stationRepository.GetById(routeModel.EndStation.Id);
        ThrowIfEntityNotFound(endStation, routeModel.EndStation.Id);

        var route = _mapper.Map<Route>(routeModel);
        route.StartStation = startStation!;
        route.EndStation = endStation!;

        return _routeRepository.Add(route);
    }

    public void Delete(int id)
    {
        var entity = _routeRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _routeRepository.Update(entity!, true);
    }

    public void Restore(int id)
    {
        var entity = _routeRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _routeRepository.Update(entity!, false);
    }

    public void Update(int id, RouteModel routeModel)
    {
        var entity = _routeRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _routeRepository.Update(entity!, _mapper.Map<Route>(routeModel));
    }

    private static void ThrowIfEntityNotFound<T>(T? entity, int id)
    {
        if (entity is not null)
            return;
        if (entity is Station)
            throw new NotFoundException(nameof(Station), id);
        throw new NotFoundException(nameof(Route), id);
    }
}