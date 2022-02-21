using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class StationService : BaseService, IStationService
{
    private readonly IStationRepository _repository;
    private readonly Role[] _allowedRoles = { Role.Admin, Role.StationManager };

    public StationService(IMapper mapper, IRepositorySoftDelete<User> userRepository, IStationRepository repository) 
        : base(mapper, userRepository)
    {
        _repository = repository;
    }


    public StationModel GetById(int userId, int id)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<StationModel>(entity);
    }

    public List<StationModel> GetNearStations(int userId, int id)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        var result = _repository.GetNearStations(entity!);
        return _mapper.Map<List<StationModel>>(result);
    }

    public List<StationModel> GetList(int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        var entities = _repository.GetList();
        return _mapper.Map<List<StationModel>>(entities);
    }

    public List<StationModel> GetListDeleted(int userId)
    {
        CheckUserRole(userId, Role.Admin);

        var entities = _repository.GetList(true).Where(t => t.IsDeleted);
        return _mapper.Map<List<StationModel>>(entities);
    }

    public int Add(int userId, StationModel stationModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _mapper.Map<Station>(stationModel);
        return _repository.Add(entity);
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

    public void Update(int userId, int id, StationModel stationModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        var newEntity = _mapper.Map<Station>(stationModel);

        _repository.Update(entity!, newEntity);
    }
}