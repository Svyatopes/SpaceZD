﻿using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class TrainService : ITrainService
    {
        private readonly IRepositorySoftDelete<Train> _trainRepository;
        private readonly IMapper _mapper;

        public TrainService(IRepositorySoftDelete<Train> trainRepository, IMapper mapper)
        {
            _trainRepository = trainRepository;
            _mapper = mapper;
        }

        public TrainModel GetById(int id)
        {
            var entity = _trainRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<TrainModel>(entity);
        }

        public List<TrainModel> GetList(bool includeAll = false)
        {
            var entities = _trainRepository.GetList(includeAll);
            return _mapper.Map<List<TrainModel>>(entities);
        }

        public List<TrainModel> GetListDeleted(bool includeAll = true)
        {
            var entities = _trainRepository.GetList(includeAll).Where(t => t.IsDeleted);
            return _mapper.Map<List<TrainModel>>(entities);

        }


        public int Add(TrainModel entity)
        {
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

        public void Delete(int id)
        {
            var entity = _trainRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _trainRepository.Update(entity, true);

        }

        public void Restore(int id)
        {
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
