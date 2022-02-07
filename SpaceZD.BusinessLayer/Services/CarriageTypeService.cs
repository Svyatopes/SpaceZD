using AutoMapper;
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
        if (entity is null)
            NotFound(id);
        return _mapper.Map<CarriageTypeModel>(entity);
    }

    public List<CarriageTypeModel> GetList() => _mapper.Map<List<CarriageTypeModel>>(_repository.GetList());
    public List<CarriageTypeModel> GetListDeleted() => _mapper.Map<List<CarriageTypeModel>>(_repository.GetList(true).Where(t => t.IsDeleted));
    public int Add(CarriageTypeModel carriageTypeModel) => _repository.Add(_mapper.Map<CarriageType>(carriageTypeModel));

    public void Delete(int id)
    {
        if (!_repository.Update(id, true))
            NotFound(id);
    }

    public void Restore(int id)
    {
        if (!_repository.Update(id, false))
            NotFound(id);
    }

    public void Update(int id, CarriageTypeModel carriageTypeModel)
    {
        carriageTypeModel.Id = id;
        if (!_repository.Update(_mapper.Map<CarriageType>(carriageTypeModel)))
            NotFound(id);
    }

    private static void NotFound(int id) => throw new Exception($"CarriageType c Id = {id} не найден");
}