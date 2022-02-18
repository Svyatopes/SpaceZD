using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IRepositorySoftDelete<User> _userRepository;

        public UserService(IRepositorySoftDelete<User> userRepository, IMapper mapper) : base(mapper)
        {
            _userRepository = userRepository;
        }

        public UserModel GetById(int id)
        {
            var entity = _userRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<UserModel>(entity);
        }

        public List<UserModel> GetList(bool includeAll = false)
        {
            var entities = _userRepository.GetList(includeAll);
            return _mapper.Map<List<UserModel>>(entities);
        }

        public List<UserModel> GetListDeleted(bool includeAll = true)
        {
            var entities = _userRepository.GetList(includeAll).Where(t => t.IsDeleted);
            return _mapper.Map<List<UserModel>>(entities);

        }


        public int Add(UserModel entity, string password)
        {
            var addEntity = _mapper.Map<User>(entity);
            addEntity.PasswordHash = SecurePasswordHasher.Hash(password);
            var id = _userRepository.Add(addEntity);            
            return id;
        }

        public void Update(int id, UserModel entity)
        {
            var userOld = _userRepository.GetById(id);
            ThrowIfEntityNotFound(userOld, id);
            var userNew = _mapper.Map<User>(entity);
            _userRepository.Update(userOld, userNew);

        }

        public void Update(int id, bool isDeleted)
        {
            var entity = _userRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _userRepository.Update(entity, isDeleted);

        }
    }
}
