using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;

        public PersonService(IPersonRepository personRepository, IMapper mapper, IUserRepository userRepository) : base(mapper)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;   
        }

        public PersonModel GetById(int id)
        {
            var entity = _personRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            return _mapper.Map<PersonModel>(entity);
        }
        
        public List<PersonModel> GetByUserLogin(string login)
        {
            var entity = _personRepository.GetByUserLogin(login);
            return _mapper.Map<List<PersonModel>>(entity);
        }


        public List<PersonModel> GetList(bool includeAll = false)
        {
            var entities = _personRepository.GetList(includeAll);
            return _mapper.Map<List<PersonModel>>(entities);
        }


        public List<PersonModel> GetListDeleted(bool includeAll = true)
        {
            var entities = _personRepository.GetList(includeAll).Where(t => t.IsDeleted);
            return _mapper.Map<List<PersonModel>>(entities);

        }


        public int Add(PersonModel entity, string login)
        {
            var user = _userRepository.GetByLogin(login);
            var addEntity = _mapper.Map<Person>(entity);
            var fullUser = _userRepository.GetById(user.Id);
            addEntity.User = fullUser;
            var id = _personRepository.Add(addEntity);            
            return id;
        }

        public void Update(int id, PersonModel entity, string login)
        {
            var personOld = _personRepository.GetById(id);
            ThrowIfEntityNotFound(personOld, id);
            var personNew = _mapper.Map<Person>(entity);
            if (personOld.User.Login == login)
                _personRepository.Update(personOld, personNew);
            else
                throw new AccessViolationException();

        }

        public void Update(int id, bool isDeleted, string login)
        {
            var entity = _personRepository.GetById(id);
            ThrowIfEntityNotFound(entity, id);
            if (entity.User.Login == login)
                _personRepository.Update(entity, isDeleted);
            else
                throw new AccessViolationException();

        }
    }
}
