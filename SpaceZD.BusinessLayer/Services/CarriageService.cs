using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class CarriageService : ICarriageService
    {
        private readonly IMapper _mapper;
        private readonly IRepositorySoftDelete<Carriage> _carriageRepository;

        public CarriageService(IMapper mapper, IRepositorySoftDelete<Carriage> carriageRepository)
        {
            _mapper = mapper;
            _carriageRepository = carriageRepository;
        }

        public CarriageModel GetById(int id)
        {
            var carriage = GetCarriageById(id);
            return _mapper.Map<CarriageModel>(carriage);
        }

        public List<CarriageModel> GetList(bool allIncluded)
        {
            var carriage = _carriageRepository.GetList(allIncluded);
            return _mapper.Map<List<CarriageModel>>(carriage);
        }

        public int Add(CarriageModel carriage)
        {
            var carriageEntity = _mapper.Map<Carriage>(carriage);
            var id = _carriageRepository.Add(carriageEntity);
            return id;
        }

        public void Update(int id, CarriageModel carriage)
        {
            var carriageEntity = GetCarriageById(id);
            var newcarriageEntity = _mapper.Map<Carriage>(carriage);
            _carriageRepository.Update(carriageEntity, newcarriageEntity);
        }

        public void Restore(int id)
        {
            var carriage = GetCarriageById(id);
            _carriageRepository.Update(carriage, false);
        }

        public void Delete(int id)
        {
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
