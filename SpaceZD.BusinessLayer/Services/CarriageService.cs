using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class CarriageService : BaseService, ICarriageService
    {
        private readonly IRepositorySoftDelete<Carriage> _carriageRepository;
        private readonly IRepositorySoftDelete<CarriageType> _carriagetypeRepository;
        private readonly IRepositorySoftDelete<Train> _trainRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };

        public CarriageService(IMapper mapper, IRepositorySoftDelete<User> userRepository, IRepositorySoftDelete<Carriage> repository,
            IRepositorySoftDelete<CarriageType> carriagetypeRepository, IRepositorySoftDelete<Train> trainRepository)
            : base(mapper, userRepository)
        {
            _carriageRepository = repository;
            _carriagetypeRepository = carriagetypeRepository;
            _trainRepository = trainRepository;
        }

        public CarriageModel GetById(int userId, int id)
        {
            CheckUserRole(userId, _allowedRoles);
            var carriage = GetCarriageById(id);
            return _mapper.Map<CarriageModel>(carriage);
        }

        public List<CarriageModel> GetList(int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var carriage = _carriageRepository.GetList();
            return _mapper.Map<List<CarriageModel>>(carriage);
        }

        public List<CarriageModel> GetListDeleted(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _carriageRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<CarriageModel>>(entities);
        }

        public int Add(int userId, CarriageModel carriageModel)
        {
            CheckUserRole(userId, _allowedRoles);

            var carriageEntity = _mapper.Map<Carriage>(carriageModel);
            var id = _carriageRepository.Add(carriageEntity);
            return id;
        }

        public void Update(int userId, int id, CarriageModel carriageModel)
        {
            CheckUserRole(userId, _allowedRoles);

            var carriageEntity = GetCarriageById(id);
            var train = _trainRepository.GetById(carriageModel.Train.Id);
            ThrowIfEntityNotFound(train, carriageModel.Train.Id);

            carriageModel.Train = _mapper.Map<TrainModel>(train);

            _carriageRepository.Update(carriageEntity, _mapper.Map<Carriage>(carriageModel));
        }

        public void Restore(int userId, int id)
        {
            CheckUserRole(userId, Role.Admin);

            var carriage = GetCarriageById(id);
            _carriageRepository.Update(carriage, false);
        }

        public void Delete(int userId, int id)
        {
            CheckUserRole(userId, Role.Admin);

            var carriage = GetCarriageById(id);
            _carriageRepository.Update(carriage, true);
        }

        private Carriage GetCarriageById(int id)
        {
            var carriage = _carriageRepository.GetById(id);
            if (carriage == null)
                throw new NotFoundException(nameof(Carriage), id);
            return carriage;
        }
    }
}
