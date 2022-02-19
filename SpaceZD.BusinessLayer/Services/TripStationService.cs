using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TripStationService : BaseService, ITripStationService
{
    private const int SeparationArrivalDepartingTrainMinutes = 15;

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

        if (!_stationRepository
            .GetReadyPlatformsStation(entity!.Station,
                 model.ArrivalTime!.Value.AddMinutes(-SeparationArrivalDepartingTrainMinutes),
                 model.DepartingTime!.Value.AddMinutes(SeparationArrivalDepartingTrainMinutes))
            .Select(g => g.Id)
            .Contains(model.Platform!.Id))
            throw new InvalidOperationException($"Невозможно выбрать платформу c Id = {model.Platform.Id}");

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
            entity.ArrivalTime!.Value.AddMinutes(-SeparationArrivalDepartingTrainMinutes),
            entity.DepartingTime!.Value.AddMinutes(SeparationArrivalDepartingTrainMinutes)));
    }
}