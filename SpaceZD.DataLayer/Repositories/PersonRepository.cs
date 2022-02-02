using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories
{
    public class PersonRepository : BaseRepository, IRepository<Person>, ISoftDelete<Person>
    {
        public Person GetEntity(int id) => _context.Persons.FirstOrDefault(x => x.Id == id);

        public List<Person> GetList(bool includeAll = false) => _context.Persons.Where(p => !p.IsDeleted || includeAll).ToList();

        public void Add(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
        }

        public bool Update(Person person)
        {
            var personInDB = GetEntity(person.Id);

            if (personInDB == null)
                return false;

            personInDB.FirstName = person.FirstName;
            personInDB.LastName = person.LastName;
            personInDB.Patronymic = person.Patronymic;
            personInDB.Passport = person.Passport;

            _context.SaveChanges();
            return true;
        }

        public bool Update(int id, bool isDeleted)
        {
            var personInDB = GetEntity(id);

            if (personInDB == null)
                return false;

            personInDB.IsDeleted = isDeleted;

            _context.SaveChanges();

            return true;

        }
    }
}
