using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class StationService : IStationService
{
    private readonly IMapper _mapper;
    private readonly IRepositorySoftDelete<Station> _stationRepository;

    public StationService(IMapper mapper, IRepositorySoftDelete<Station> stationRepository)
    {
        _mapper = mapper;
        _stationRepository = stationRepository;
    }

    public StationModel GetById(int id)
    {
        var entity = _stationRepository.GetById(id);
        if (entity is null)
            NotFound(id);
        return _mapper.Map<StationModel>(entity);
    }

    public List<StationModel> GetNearStations(int id)
    {
        var entity = _stationRepository.GetById(id);
        if (entity is null)
            NotFound(id);
        return _mapper.Map<List<StationModel>>(
            entity!.TransitsWithStartStation
                   .Where(t => !t.IsDeleted)
                   .Select(t => t.EndStation)
                   .Where(t => !t.IsDeleted)
                   .ToList());
    }

    public List<StationModel> GetList() => _mapper.Map<List<StationModel>>(_stationRepository.GetList());
    public List<StationModel> GetListDeleted() => _mapper.Map<List<StationModel>>(_stationRepository.GetList(true).Where(t => t.IsDeleted));
    public int Add(StationModel stationModel) => _stationRepository.Add(_mapper.Map<Station>(stationModel));

    public void Delete(int id)
    {
        if (!_stationRepository.Update(id, true))
            NotFound(id);
    }

    public void Restore(int id)
    {
        if (!_stationRepository.Update(id, false))
            NotFound(id);
    }

    public void Update(int id, StationModel stationModel)
    {
        stationModel.Id = id;
        if (!_stationRepository.Update(_mapper.Map<Station>(stationModel)))
            NotFound(id);
    }

    private static void NotFound(int id) => throw new NotFoundException($"Station c Id = {id} не найден");
}