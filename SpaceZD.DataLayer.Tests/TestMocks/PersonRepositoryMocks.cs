using System.Collections.Generic;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestMocks
{
    public class PersonRepositoryMocks
    {

        public static List<Person> GetPersons() => new List<Person>
        {
            new Person
            {
                FirstName = "Klark",
                LastName = "Kent",
                Patronymic = "KalEl",
                Passport = "7777666555"
            },
            new Person
            {
                FirstName = "Sara",
                LastName = "Konor",
                Patronymic = "Vyacheslavovna",
                Passport = "3005123456",
                IsDeleted = true
            },
            new Person
            {
                FirstName = "Enot",
                LastName = "Poloskun",
                Patronymic = "Enotovich",
                Passport = "1234123456"
            }
        };

        public static Person GetPerson() => new Person
        {
            FirstName = "Renata",
            LastName = "Litvinova",
            Patronymic = "Batevichna",
            Passport = "9421365128",
        };
    }
}
