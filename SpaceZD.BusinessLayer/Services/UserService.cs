using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly Role[] _allowedAllRoles = { Role.Admin, Role.User , Role.StationManager, Role.TrainRouteManager};
        private readonly Role[] _allowedRoles = { Role.Admin, Role.User};
                
        public UserService(IUserRepository userRepository, IMapper mapper) : base(mapper, userRepository) {}

        
        public UserModel GetById(int id, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entity = _userRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<UserModel>(entity);
        }
        public UserModel GetByLogin(string login, int userId)
        {
            CheckUserRole(userId, _allowedAllRoles);

            var entity = _userRepository.GetByLogin(login);
            ThrowIfEntityNotFound(entity, entity.Id);
            return _mapper.Map<UserModel>(entity);
        }

        public List<UserModel> GetList(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            CheckUserRole(userId, _allowedRoles);
            var entities = _userRepository.GetList(false);
            return _mapper.Map<List<UserModel>>(entities);
        }

        public List<PersonModel> GetListUserPersons(int userId)
        {
            CheckUserRole(userId, Role.User);

            var entities = _userRepository.GetListUserPersons(userId);
            return _mapper.Map<List<PersonModel>>(entities);
        }

        public List<UserModel> GetListDelete(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _userRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<UserModel>>(entities);

        }


        public int Add(UserModel entity, string password)
        {
            var addEntity = _mapper.Map<User>(entity);
            addEntity.PasswordHash = SecurePasswordHasher.Hash(password);
            addEntity.Role = Role.User;            
            var id = _userRepository.Add(addEntity);            
            return id;
        }



        public void Update(int id, UserModel entity, int userId)
        {
            CheckUserRole(userId, _allowedAllRoles);

            var userOld = _userRepository.GetById(id);
            ThrowIfEntityNotFound(userOld, id);
            var userNew = _mapper.Map<User>(entity);
            _userRepository.Update(userOld, userNew);

        }
        
        public void UpdateRole(int id, Role role, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var user = _userRepository.GetById(id);
            ThrowIfEntityNotFound(user, id);
            _userRepository.UpdateRole(user, role);

        }

        public void Delete(int id, int userId)
        {
            CheckUserRole(userId, _allowedAllRoles);

            var entity = _userRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _userRepository.Update(entity, true);

        }
        
        public void Restore(int id, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entity = _userRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            _userRepository.Update(entity, false);

        }
    }
}
