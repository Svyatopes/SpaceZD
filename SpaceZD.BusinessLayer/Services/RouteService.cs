using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class RouteService : BaseService, IRouteService
{
    private readonly IRepositorySoftDelete<Route> _repository;
    private readonly IStationRepository _stationRepository;
    private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };

    public RouteService(IMapper mapper, IUserRepository userRepository, IRepositorySoftDelete<Route> routeRepository, IStationRepository stationRepository) : base(mapper, userRepository)
    {
        _repository = routeRepository;
        _stationRepository = stationRepository;
    }


    public RouteModel GetById(int userId, int id)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<RouteModel>(entity);
    }

    public List<RouteModel> GetList(int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        var entities = _repository.GetList();
        return _mapper.Map<List<RouteModel>>(entities);
    }

    public List<RouteModel> GetListDeleted(int userId)
    {
        CheckUserRole(userId, Role.Admin);

        var entities = _repository.GetList(true).Where(t => t.IsDeleted);
        return _mapper.Map<List<RouteModel>>(entities);
    }

    public int Add(int userId, RouteModel routeModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var startStation = _stationRepository.GetById(routeModel.StartStation.Id);
        ThrowIfEntityNotFound(startStation, routeModel.StartStation.Id);
        var endStation = _stationRepository.GetById(routeModel.EndStation.Id);
        ThrowIfEntityNotFound(endStation, routeModel.EndStation.Id);

        var route = _mapper.Map<Route>(routeModel);
        route.StartStation = startStation!;
        route.EndStation = endStation!;

        return _repository.Add(route);
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

    public void Update(int userId, int id, RouteModel routeModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        var startStation = _stationRepository.GetById(routeModel.StartStation.Id);
        ThrowIfEntityNotFound(startStation, routeModel.StartStation.Id);
        var endStation = _stationRepository.GetById(routeModel.EndStation.Id);
        ThrowIfEntityNotFound(endStation, routeModel.EndStation.Id);

        var route = _mapper.Map<Route>(routeModel);
        route.StartStation = startStation!;
        route.EndStation = endStation!;

        _repository.Update(entity!, route);
    }
}