using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class PersonRepository : BaseRepository, IRepositorySoftDelete<Person>
{
    public PersonRepository(VeryVeryImportantContext context) : base(context) { }

    public Person? GetById(int id) => _context.Persons.FirstOrDefault(x => x.Id == id);

    public List<Person> GetList(bool includeAll = false) => _context.Persons.Where(p => !p.IsDeleted || includeAll).ToList();

    public int Add(Person person)
    {
        _context.Persons.Add(person);
        _context.SaveChanges();
        return person.Id;
    }

    public void Update(Person oldPerson, Person newPerson)
    {
        oldPerson.FirstName = newPerson.FirstName;
        oldPerson.LastName = newPerson.LastName;
        oldPerson.Patronymic = newPerson.Patronymic;
        oldPerson.Passport = newPerson.Passport;
        _context.SaveChanges();
    }

    public void Update(Person person, bool isDeleted)
    {
        person.IsDeleted = isDeleted;
        _context.SaveChanges();
    }
}