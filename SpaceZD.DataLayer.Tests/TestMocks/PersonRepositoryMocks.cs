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
                Passport = "7777666555",
                User = new User(){ Name = "K" , Login = "KK", PasswordHash ="hgeurgeerj"}
            },
            new Person
            {
                FirstName = "Sara",
                LastName = "Konor",
                Patronymic = "Vyacheslavovna",
                Passport = "3005123456",
                IsDeleted = true,
                User = new User(){ Name = "S", Login = "SS", PasswordHash ="jhgfdsdfgh"}
            },
            new Person
            {
                FirstName = "Enot",
                LastName = "Poloskun",
                Patronymic = "Enotovich",
                Passport = "1234123456",
                User = new User(){ Name = "E", Login = "EE", PasswordHash ="ythythyhy"}
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
