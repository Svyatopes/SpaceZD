using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class PersonRepository
    {
        public Person GetPersonById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var person = context.Persons.FirstOrDefault(o => o.Id == id);
            return person;
        }

        public Person GetPersonByIdWithTickets(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var person = context.Persons.Include(o => o.Tickets)
                                        .FirstOrDefault(o => o.Id == id);
            return person;
        }

        public List<Person> GetPersons()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var persons = context.Persons.ToList();
            return persons;
        }

        public void AddPerson(Person person)
        {
            var context = VeryVeryImportantContext.GetInstance();
            context.Persons.Add(person);
            context.SaveChanges();
        }

        public void EditPerson(Person person)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var personInDB = GetPersonById(person.Id);

            if (personInDB == null)
                throw new Exception($"Not found person with {person.Id} to edit");

            if (personInDB.FirstName != person.FirstName)
                personInDB.FirstName = person.FirstName;

            if (personInDB.LastName != person.LastName)
                personInDB.LastName = person.LastName;

            if (personInDB.Patronymic != person.Patronymic)
                personInDB.Patronymic = person.Patronymic;

            if (personInDB.Passport != person.Passport)
                personInDB.Passport = person.Passport;

            context.SaveChanges();
        }

        public void DeletePerson(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var personInDB = context.Persons.FirstOrDefault(o => o.Id == id);

            if (personInDB == null)
                throw new Exception($"Not found person with {id} to delete");

            personInDB.IsDeleted = true;

            context.SaveChanges();
        }

        public void RestorePerson(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var personInDB = context.Persons.FirstOrDefault(o => o.Id == id);

            if (personInDB == null)
                throw new Exception($"Not found person with {id} to restore");

            personInDB.IsDeleted = false;

            context.SaveChanges();
        }
    }
}
