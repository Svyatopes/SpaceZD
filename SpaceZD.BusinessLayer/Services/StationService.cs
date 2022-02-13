using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class StationService : IStationService
{
    private readonly IMapper _mapper;
    private readonly IStationRepository _repository;

    public StationService(IMapper mapper, IStationRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public StationModel GetById(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        entity!.Platforms = entity.Platforms.Where(x => !x.IsDeleted).ToList();

        return _mapper.Map<StationModel>(entity);
    }

    public List<PlatformModel> GetReadyPlatformsStationById(int id, DateTime moment)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        
        return _mapper.Map<List<PlatformModel>>(_repository.GetReadyPlatformsStation(entity!, moment));
    }

    public List<StationModel> GetNearStations(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<List<StationModel>>(
            entity!.TransitsWithStartStation
                   .Where(t => !t.IsDeleted)
                   .Select(t => t.EndStation)
                   .Where(t => !t.IsDeleted)
                   .ToList());
    }

    public List<StationModel> GetList()
    {
        var entities = _repository.GetList();
        foreach (var station in entities)
            station.Platforms = station.Platforms.Where(t => !t.IsDeleted).ToList();
        
        return _mapper.Map<List<StationModel>>(entities);
    }
    
    public List<StationModel> GetListDeleted()
    {
        var entities = _repository.GetList(true).Where(t => t.IsDeleted);
        foreach (var station in entities)
            station.Platforms = station.Platforms.Where(t => !t.IsDeleted).ToList();
        
        return _mapper.Map<List<StationModel>>(entities);
    }

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

    private static void ThrowIfEntityNotFound(Station? entity, int id)
    {
        if (entity is null)
            throw new NotFoundException(nameof(Station), id);
    }
}