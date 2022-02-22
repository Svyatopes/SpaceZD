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
    private readonly IPlatformRepository _platformRepository;

    public TripStationService(IMapper mapper, IRepositorySoftDelete<User> userRepository, ITripStationRepository repository, IStationRepository stationRepository,
        IPlatformRepository platformRepository) : base(mapper, userRepository)
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
        var tripStationNew = _mapper.Map<TripStation>(model);
        tripStationNew.Platform = platform;

        _repository.Update(entity, tripStationNew);
    }

    public void SetPlatform(int id, int idPlatform)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        if (entity!.ArrivalTime is null)
            CheckTheAbilityToStopOnTheSelectedPlatform(entity.Station, entity.DepartingTime!.Value, entity.DepartingTime!.Value, idPlatform);
        else if (entity.DepartingTime is null)
            CheckTheAbilityToStopOnTheSelectedPlatform(entity.Station, entity.ArrivalTime!.Value, entity.ArrivalTime!.Value, idPlatform);
        else
            CheckTheAbilityToStopOnTheSelectedPlatform(entity.Station, entity.ArrivalTime!.Value, entity.DepartingTime!.Value, idPlatform);

        entity.Platform = _platformRepository.GetById(idPlatform);

        _repository.Update(entity, entity);
    }

    public List<PlatformModel> GetReadyPlatforms(int id)
    {
        var entity = _repository.GetById(id);
        ThrowIfEntityNotFound(entity, id);

        if (entity!.ArrivalTime is null && entity.DepartingTime is not null)
            return _mapper.Map<List<PlatformModel>>(_stationRepository.GetReadyPlatformsStation(entity.Station,
                entity.DepartingTime!.Value.AddMinutes(-_separationArrivalDepartingTrainMinutes),
                entity.DepartingTime!.Value.AddMinutes(_separationArrivalDepartingTrainMinutes)));

        if (entity.DepartingTime is null && entity.ArrivalTime is not null)
            return _mapper.Map<List<PlatformModel>>(_stationRepository.GetReadyPlatformsStation(entity.Station,
                entity.ArrivalTime!.Value.AddMinutes(-_separationArrivalDepartingTrainMinutes),
                entity.ArrivalTime!.Value.AddMinutes(_separationArrivalDepartingTrainMinutes)));

        if (entity.ArrivalTime is not null && entity.DepartingTime is not null)
            return _mapper.Map<List<PlatformModel>>(_stationRepository.GetReadyPlatformsStation(entity!.Station,
                entity.ArrivalTime!.Value.AddMinutes(-_separationArrivalDepartingTrainMinutes),
                entity.DepartingTime!.Value.AddMinutes(_separationArrivalDepartingTrainMinutes)));

        throw new InvalidDataException();
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