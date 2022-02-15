using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class CarriageTypeService : ICarriageTypeService
{
    private readonly IMapper _mapper;
    private readonly IRepositorySoftDelete<CarriageType> _repository;

    public CarriageTypeService(IMapper mapper, IRepositorySoftDelete<CarriageType> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public CarriageTypeModel GetById(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        
        return _mapper.Map<CarriageTypeModel>(entity);
    }

    public List<CarriageTypeModel> GetList() => _mapper.Map<List<CarriageTypeModel>>(_repository.GetList());
    public List<CarriageTypeModel> GetListDeleted() => _mapper.Map<List<CarriageTypeModel>>(_repository.GetList(true).Where(t => t.IsDeleted));
    public int Add(CarriageTypeModel carriageTypeModel) => _repository.Add(_mapper.Map<CarriageType>(carriageTypeModel));

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

    public void Update(int id, CarriageTypeModel carriageTypeModel)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, _mapper.Map<CarriageType>(carriageTypeModel));
    }

    private static void ThrowIfEntityNotFound(CarriageType? entity, int id)
    {
        if (entity is null)
            throw new NotFoundException(nameof(CarriageType), id);
    }
}