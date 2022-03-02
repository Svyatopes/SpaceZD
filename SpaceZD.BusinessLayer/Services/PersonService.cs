using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.User };

        public PersonService(IPersonRepository personRepository, IUserRepository userRepository, IMapper mapper) : base(mapper, userRepository)
        {
            _personRepository = personRepository;
        }

        public PersonModel GetById(int id, int userId)
        {
            CheckUserRole(userId, Role.Admin);
            var entity = _personRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<PersonModel>(entity);
        }


        public List<PersonModel> GetByUserId(int userId)
        {
            CheckUserRole(userId, Role.User);

            var entity = _personRepository.GetByUserId(userId);
            return _mapper.Map<List<PersonModel>>(entity);
        }


        public List<PersonModel> GetList(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _personRepository.GetList(false);
            return _mapper.Map<List<PersonModel>>(entities);
        }


        public List<PersonModel> GetListDeleted(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entities = _personRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<PersonModel>>(entities);

        }


        public int Add(int userId, PersonModel entity)
        {
            CheckUserRole(userId, _allowedRoles);

            var user = _userRepository.GetById(userId);
            var addEntity = _mapper.Map<Person>(entity);
            addEntity.User = user;
            var id = _personRepository.Add(addEntity);
            return id;
        }


        public void Update(int userId, int id, PersonModel entity)
        {
            CheckUserRole(userId, _allowedRoles);

            var personOld = _personRepository.GetById(id);
            ThrowIfEntityNotFound(personOld, id);
            var personNew = _mapper.Map<Person>(entity);
            if (personOld.User.Id == userId)
                _personRepository.Update(personOld, personNew);
            else
                throw new AccessViolationException();

        }

        public void Delete(int id, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var entity = _personRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            if (entity.User.Id == userId)
                _personRepository.Update(entity, true);
            else
                throw new AccessViolationException();

        }

        public void Restore(int id, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var entity = _personRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            if (entity.User.Id == userId)
                _personRepository.Update(entity, false);
            else
                throw new AccessViolationException();

        }
    }
}
