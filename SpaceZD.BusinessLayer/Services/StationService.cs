using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class StationService : BaseService, IStationService
{
    private readonly IStationRepository _repository;

    public StationService(IMapper mapper, IStationRepository repository) : base(mapper)
    {
        _repository = repository;
    }

    public StationModel GetById(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        
        return _mapper.Map<StationModel>(entity);
    }

    public List<PlatformModel> GetReadyPlatformsByStationId(int id, DateTime moment)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<List<PlatformModel>>(_repository.GetReadyPlatformsStation(entity!, moment));
    }

    public List<StationModel> GetNearStations(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<List<StationModel>>(_repository.GetNearStations(entity!));
    }

    public List<StationModel> GetList() => _mapper.Map<List<StationModel>>(_repository.GetList());
    public List<StationModel> GetListDeleted() => _mapper.Map<List<StationModel>>(_repository.GetList(true).Where(t => t.IsDeleted));
    public int Add(StationModel stationModel) => _repository.Add(_mapper.Map<Station>(stationModel));

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

    public void Update(int id, StationModel stationModel)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        _repository.Update(entity!, _mapper.Map<Station>(stationModel));
    }
}