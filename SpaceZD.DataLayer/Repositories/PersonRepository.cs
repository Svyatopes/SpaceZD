using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class PersonRepository : BaseRepository, IRepositorySoftDelete<Person>
{
    public PersonRepository(VeryVeryImportantContext context) : base(context) { }

    public Person? GetById(int id) => _context.Persons.FirstOrDefault(x => x.Id == id);

    public IEnumerable<Person> GetList(bool includeAll = false) => _context.Persons.Where(p => !p.IsDeleted || includeAll).ToList();

    public int Add(Person person)
    {
        _context.Persons.Add(person);
        _context.SaveChanges();
        return person.Id;
    }

    public bool Update(Person person)
    {
        var personInDb = GetById(person.Id);

        if (personInDb == null)
            return false;

        personInDb.FirstName = person.FirstName;
        personInDb.LastName = person.LastName;
        personInDb.Patronymic = person.Patronymic;
        personInDb.Passport = person.Passport;

        _context.SaveChanges();
        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var personInDb = GetById(id);

        if (personInDb == null)
            return false;

        personInDb.IsDeleted = isDeleted;

        _context.SaveChanges();

        return true;
    }
}