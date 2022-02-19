using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TripStationService : BaseService, ITripStationService
{
    private const int _separationArrivalDepartingTrainMinutes = 15;

    private readonly ITripStationRepository _repository;
    private readonly IStationRepository _stationRepository;
    private readonly IRepositorySoftDelete<Platform> _platformRepository;

    public TripStationService(IMapper mapper, ITripStationRepository repository, IStationRepository stationRepository,
                              IRepositorySoftDelete<Platform> platformRepository) : base(mapper)
    {
        _repository = repository;
        _stationRepository = stationRepository;
        _platformRepository = platformRepository;
    }

    public TripStationModel GetById(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<TripStationModel>(entity);
    }

    public void Update(int id, TripStationModel model)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        CheckTheAbilityToStopOnTheSelectedPlatform(entity!.Station, model.ArrivalTime!.Value, model.DepartingTime!.Value, model.Platform!.Id);

        var platform = _platformRepository.GetById(model.Platform.Id);
        var platformModel = _mapper.Map<PlatformModel>(platform);
        model.Platform = platformModel;

        var tripStationNew = _mapper.Map<TripStation>(model);
        _repository.Update(entity, tripStationNew);
    }

    public List<PlatformModel> GetReadyPlatforms(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        return _mapper.Map<List<PlatformModel>>(_stationRepository.GetReadyPlatformsStation(entity!.Station,
            entity.ArrivalTime!.Value.AddMinutes(-_separationArrivalDepartingTrainMinutes),
            entity.DepartingTime!.Value.AddMinutes(_separationArrivalDepartingTrainMinutes)));
    }

    private void CheckTheAbilityToStopOnTheSelectedPlatform(Station station, DateTime arrivalTime, DateTime departureTime, int platformId)
    {
        if (!_stationRepository
            .GetReadyPlatformsStation(station,
                 arrivalTime.AddMinutes(-_separationArrivalDepartingTrainMinutes),
                 departureTime.AddMinutes(_separationArrivalDepartingTrainMinutes))
            .Select(g => g.Id)
            .Contains(platformId))
            throw new InvalidOperationException($"Невозможно выбрать платформу c Id = {platformId}");
    }
}