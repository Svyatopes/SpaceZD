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
        private readonly IRepositorySoftDelete<CarriageType> _carriageTypeRepository;
        private readonly IRepositorySoftDelete<Train> _trainRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };

        public CarriageService(IMapper mapper, IUserRepository userRepository, IRepositorySoftDelete<Carriage> repository,
            IRepositorySoftDelete<CarriageType> carriagetypeRepository, IRepositorySoftDelete<Train> trainRepository)
            : base(mapper, userRepository)
        {
            _carriageRepository = repository;
            _carriageTypeRepository = carriagetypeRepository;
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


            var newEntity = _mapper.Map<Carriage>(carriageModel);
            newEntity.Train = train;

            _carriageRepository.Update(carriageEntity, newEntity);
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
