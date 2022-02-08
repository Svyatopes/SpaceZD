using AutoMapper;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositorySoftDelete<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepositorySoftDelete<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public UserModel GetById(int id)
        {
            var entity = _userRepository.GetById(id);
            return _mapper.Map<UserModel>(entity);
        }

        public List<UserModel> GetList(bool includeAll = false)
        {
            var entities = _userRepository.GetList(includeAll);
            return _mapper.Map<List<UserModel>>(entities);
        }

        public int Add(UserModel entity)
        {
            var addEntity = _mapper.Map<User>(entity);

            var idEntity = _userRepository.Add(addEntity);
            return idEntity;
        }

        public bool Update(UserModel entity)
        {
            var addEntity = _mapper.Map<User>(entity);
            var entityInDb = GetById(addEntity.Id);

            entityInDb.Name = addEntity.Name;
            entityInDb.PasswordHash = addEntity.PasswordHash;
            return true;

        }

        public bool Update(int id, bool isDeleted)
        {
            var user = GetById(id);
            var entity = _mapper.Map<User>(user);
            var deleteEntity = _userRepository.Update(id, isDeleted);
            
            return true;
        }

    }
}
