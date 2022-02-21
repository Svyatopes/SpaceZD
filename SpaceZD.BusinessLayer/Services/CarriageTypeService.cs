using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class CarriageTypeService : BaseService, ICarriageTypeService
{
    private readonly IRepositorySoftDelete<CarriageType> _repository;
    private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };

    public CarriageTypeService(IMapper mapper, IRepositorySoftDelete<CarriageType> repository, IRepositorySoftDelete<User> userRepository) : base(mapper,
        userRepository)
    {
        _repository = repository;
    }

    public CarriageTypeModel GetById(int userId, int id)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<CarriageTypeModel>(entity);
    }

    public List<CarriageTypeModel> GetList(int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        var entities = _repository.GetList();
        return _mapper.Map<List<CarriageTypeModel>>(entities);
    }
    public List<CarriageTypeModel> GetListDeleted(int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        var entities = _repository.GetList(true).Where(t => t.IsDeleted);
        return _mapper.Map<List<CarriageTypeModel>>(entities);
    }
    public int Add(int userId, CarriageTypeModel carriageTypeModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _mapper.Map<CarriageType>(carriageTypeModel);
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
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, false);
    }

    public void Update(int userId, int id, CarriageTypeModel carriageTypeModel)
    {
        CheckUserRole(userId, _allowedRoles);

        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        var newEntity = _mapper.Map<CarriageType>(carriageTypeModel);

        _repository.Update(entity!, newEntity);
    }
}