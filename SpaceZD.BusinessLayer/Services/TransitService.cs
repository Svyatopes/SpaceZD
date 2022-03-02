using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Services
{
    public class TransitService : BaseService, ITransitService
    {
        private readonly IRepositorySoftDelete<Transit> _transitRepository;
        private readonly IStationRepository _stationRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.StationManager };


        public TransitService(IMapper mapper, IUserRepository userRepository, IRepositorySoftDelete<Transit> transitRepository, IStationRepository stationRepository) : base(mapper, userRepository)
        {
            _transitRepository = transitRepository;
            _stationRepository = stationRepository;
        }

        public TransitModel GetById(int id, int userId)
        {
            CheckUserRole(userId, _allowedRoles);
            
            var entity = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<TransitModel>(entity);
        }

        public List<TransitModel> GetList(int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var entities = _transitRepository.GetList(false);
            return _mapper.Map<List<TransitModel>>(entities);
        }

        public List<TransitModel> GetListDeleted(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _transitRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<TransitModel>>(entities);

        }

        public int Add(TransitModel transitModel, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var startStation = _stationRepository.GetById(transitModel.StartStation.Id);
            var endStation = _stationRepository.GetById(transitModel.EndStation.Id);
            
            var transit = _mapper.Map<Transit>(transitModel);
            transit.StartStation = startStation!;
            transit.EndStation = endStation!;

            return _transitRepository.Add(transit);
        }



        public void Delete(int id, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var entity = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _transitRepository.Update(entity, true);
        }

        public void Restore(int id, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entity = _transitRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _transitRepository.Update(entity, false);
        }

        public void Update(int id, TransitModel transitModel, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

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
    }
}

