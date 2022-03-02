using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class TrainService : BaseService, ITrainService
    {
        private readonly IRepositorySoftDelete<Train> _trainRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };


        public TrainService(IMapper mapper, IUserRepository userRepository, IRepositorySoftDelete<Train> trainRepository) : base(mapper, userRepository)
        {
            _trainRepository = trainRepository;
        }

        public TrainModel GetById(int id, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var entity = _trainRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<TrainModel>(entity);
        }

        public List<TrainModel> GetList(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _trainRepository.GetList(false);
            return _mapper.Map<List<TrainModel>>(entities);
        }

        public List<TrainModel> GetListDeleted(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _trainRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<TrainModel>>(entities);

        }

        public int Add(TrainModel entity, int userId)
        {
            CheckUserRole(userId, _allowedRoles);
            
            var addEntity = _mapper.Map<Train>(entity);
            var id = _trainRepository.Add(addEntity);
            return id;
        }

        public void Update(int id, TrainModel entity)
        {
            var userOld = _trainRepository.GetById(id);
            ThrowIfEntityNotFound(userOld, id);
            var userNew = _mapper.Map<Train>(entity);
            _trainRepository.Update(userOld, userNew);

        }

        public void Delete(int id, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var entity = _trainRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _trainRepository.Update(entity, true);

        }

        public void Restore(int id, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entity = _trainRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _trainRepository.Update(entity, false);

        }

        private static void ThrowIfEntityNotFound<T>(T? entity, int id)
        {
            if (entity is null)
                throw new NotFoundException(typeof(T).Name, id);
        }

    }
}
