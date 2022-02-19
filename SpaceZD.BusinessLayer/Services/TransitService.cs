using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.BusinessLayer.Services;

namespace SpaceZD.BusinessLayer.Services
{
    public class TransitService : BaseService, ITransitService
    {
        private readonly IRepositorySoftDelete<Transit> _transitRepository;
        private readonly IStationRepository _stationRepository;

        public TransitService(IRepositorySoftDelete<Transit> transitRepository, IStationRepository stationRepository, IMapper mapper) : base(mapper)
        {
            _transitRepository = transitRepository;
            _stationRepository = stationRepository;
        }

        public TransitModel GetById(int id)
        {
            var entity = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<TransitModel>(entity);
        }

        public List<TransitModel> GetList(bool includeAll = false)
        {
            var entities = _transitRepository.GetList(includeAll);
            return _mapper.Map<List<TransitModel>>(entities);
        }

        public List<TransitModel> GetListDeleted(bool includeAll = true)
        {
            var entities = _transitRepository.GetList(includeAll).Where(t => t.IsDeleted);
            return _mapper.Map<List<TransitModel>>(entities);

        }

        public int Add(TransitModel transitModel)
        {
            var startStation = _stationRepository.GetById(transitModel.StartStation.Id);
            var endStation = _stationRepository.GetById(transitModel.EndStation.Id);
            
            var transit = _mapper.Map<Transit>(transitModel);
            transit.StartStation = startStation!;
            transit.EndStation = endStation!;

            return _transitRepository.Add(transit);
        }



        public void Delete(int id)
        {
            var entity = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _transitRepository.Update(entity, true);
        }

        public void Restore(int id)
        {
            var entity = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _transitRepository.Update(entity, false);
        }

        public void Update(int id, TransitModel transitModel)
        {
            var transitOld = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(transitOld, id);
            var transitNew = _mapper.Map<Transit>(transitModel);
            if (!(transitNew.StartStation is null || transitNew.EndStation is null))
            {
                transitNew.StartStation = _stationRepository.GetById(transitNew.StartStation.Id);
                transitNew.EndStation = _stationRepository.GetById(transitNew.EndStation.Id);
                _transitRepository.Update(transitOld, transitNew);
            }
        }

        private static void NotFound(string name, int id) => throw new NotFoundException($"{name} c Id = {id} не найден");
    }
}

