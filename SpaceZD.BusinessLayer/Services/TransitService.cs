using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services.Interfaces;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class TransitService : ITransitService
    {
        private readonly IMapper _mapper;
        private readonly IRepositorySoftDelete<Transit> _transitRepository;
        private readonly IRepositorySoftDelete<Station> _stationRepository;

        public TransitService(IMapper mapper, IRepositorySoftDelete<Transit> transitRepository, IRepositorySoftDelete<Station> stationRepository)
        {
            _mapper = mapper;
            _transitRepository = transitRepository;
            _stationRepository = stationRepository;
        }

        public TransitModel GetById(int id)
        {
            var entity = _transitRepository.GetById(id);
            if (entity is null)
                NotFound(nameof(Transit), id);
            return _mapper.Map<TransitModel>(entity);
        }

        public List<TransitModel> GetList() => _mapper.Map<List<TransitModel>>(_transitRepository.GetList());
        public List<TransitModel> GetListDeleted() => _mapper.Map<List<TransitModel>>(_transitRepository.GetList(true).Where(t => t.IsDeleted));

        public int Add(TransitModel transitModel)
        {
            var startStation = _stationRepository.GetById(transitModel.StartStation.Id);
            if (startStation is null)
                NotFound(nameof(Station), transitModel.StartStation.Id);
            var endStation = _stationRepository.GetById(transitModel.EndStation.Id);
            if (endStation is null)
                NotFound(nameof(Station), transitModel.EndStation.Id);

            var transit = _mapper.Map<Transit>(transitModel);
            transit.StartStation = startStation!;
            transit.EndStation = endStation!;

            return _transitRepository.Add(transit);
        }

        public void Delete(int id)
        {
            if (!_transitRepository.Update(id, true))
                NotFound(nameof(Transit), id);
        }

        public void Restore(int id)
        {
            if (!_transitRepository.Update(id, false))
                NotFound(nameof(Transit), id);
        }

        public void Update(int id, TransitModel transitModel)
        {
            transitModel.Id = id;
            if (!_transitRepository.Update(_mapper.Map<Transit>(transitModel)))
                NotFound(nameof(Transit), id);
        }

        private static void NotFound(string name, int id) => throw new NotFoundException($"{name} c Id = {id} не найден");
    }
}

