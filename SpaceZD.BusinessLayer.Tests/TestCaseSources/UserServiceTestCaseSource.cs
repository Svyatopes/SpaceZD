using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    public class UserServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetUsers(), GetUserModels(), 1);
            yield return new TestCaseData(GetUsers(), GetUserModels(), 2);
            
        }
        public static IEnumerable<TestCaseData> GetListWithDeleteTestCases()
        {
            yield return new TestCaseData(GetUsers(), GetUserModels(), false, 1);
            yield return new TestCaseData(GetUsers(), GetUserModels(), true, 1);
            yield return new TestCaseData(GetUsers(), GetUserModels(), false, 2);
            yield return new TestCaseData(GetUsers(), GetUserModels(), true, 2);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var users = GetUsers();
            var userModels = GetUserModels();
            yield return new TestCaseData(users[0], userModels[0], Role.User, 42);
            yield return new TestCaseData(users[1], userModels[1], Role.Admin, 1);
        }

        public static IEnumerable<TestCaseData> GetListPersonsFromUserTestCases()
        {
            yield return new TestCaseData(GetPersonsFromUser(), GetPersonsFromUserModel(), false, 1);
            yield return new TestCaseData(GetPersonsFromUser(), GetPersonsFromUserModel(), true, 1);
            yield return new TestCaseData(GetPersonsFromUser(), GetPersonsFromUserModel(), false, 2);
            yield return new TestCaseData(GetPersonsFromUser(), GetPersonsFromUserModel(), true, 2);
        }

        private static List<Person> GetPersonsFromUser() => new List<Person>
        {
            new Person() { FirstName = "Sasha", LastName = "Sashaaa", Patronymic = "Sashaaaaa", Passport = "4567", User = new User(){ Login = "ii" }, IsDeleted = false },
            new Person() { FirstName = "Masha", LastName = "Mashaaa", Patronymic = "Mashaaaaa", Passport = "8765", User = new User(){ Login = "rr" }, IsDeleted = true },
            new Person() { FirstName = "Pasha", LastName = "Pashaaa", Patronymic = "Pashaaaaa", Passport = "66666", User = new User(){ Login = "dd" }, IsDeleted = false },
            new Person() { FirstName = "Dasha", LastName = "Dashaaa", Patronymic = "Dashaaaaa", Passport = "987654", User = new User(){ Login = "hh" }, IsDeleted = true }
        };


        private static List<PersonModel> GetPersonsFromUserModel() => new List<PersonModel>
        {
            new PersonModel() { FirstName = "Sasha", LastName = "Sashaaa", Patronymic = "Sashaaaaa", Passport = "4567", User = new UserModel(){ Login = "ii" }, IsDeleted = false },
            new PersonModel() { FirstName = "Masha", LastName = "Mashaaa", Patronymic = "Mashaaaaa", Passport = "8765", User = new UserModel(){ Login = "rr" }, IsDeleted = true },
            new PersonModel() { FirstName = "Pasha", LastName = "Pashaaa", Patronymic = "Pashaaaaa", Passport = "66666", User = new UserModel(){ Login = "dd" }, IsDeleted = false },
            new PersonModel() { FirstName = "Dasha", LastName = "Dashaaa", Patronymic = "Dashaaaaa", Passport = "987654", User = new UserModel(){ Login = "hh" }, IsDeleted = true }
        };

        private static List<User> GetUsers() => new List<User>
        {
            new User
            {
                Name = "Sasha",
                Login = "Sashaaa",
                PasswordHash = "ierhkjdfhds",
                Role = Role.Admin,
                IsDeleted = false,
                Persons = new List<Person>()
                {
                    new Person()
                    { 
                        FirstName = "dfg",
                        LastName ="fgtyh",
                        Passport =  "sdfghj",
                        Patronymic =    "erty",
                        IsDeleted = false
                    }
                }
            },
            new User
            {
                Name = "Masha",
                Login = "Mashaaa",
                PasswordHash = "ewdfrgthgfrde",
                Role = Role.User,
                IsDeleted = true,
                Persons = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "dfg",
                        LastName ="fgtyh",
                        Passport =  "sdfghj",
                        Patronymic =    "erty",
                        IsDeleted = false

                    }
                }
            },
            new User
            {
                Name = "Dasha", 
                Login = "Dashaaa", 
                PasswordHash = "hjngtrfewdrt", 
                Role = Role.StationManager, 
                IsDeleted = false,
                Persons = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "dfg",
                        LastName ="fgtyh",
                        Passport =  "sdfghj",
                        Patronymic =    "erty",
                        IsDeleted = false

                    }
                }
            },
            new User
            {
                Name = "Pasha", 
                Login = "Pashaaa", 
                PasswordHash = "erfgthnjytgr", 
                Role = Role.TrainRouteManager, 
                IsDeleted = false,
                Persons = new List<Person>()
                {
                    new Person()
                    {
                        FirstName = "dfg",
                        LastName ="fgtyh",
                        Passport =  "sdfghj",
                        Patronymic =    "erty",
                        IsDeleted = false

                    }
                }
            }                   
        };

        private static List<UserModel> GetUserModels() => new List<UserModel>
        {
            new UserModel
            {
                Name = "Sasha",
                Login = "Sashaaa",
                PasswordHash = "ierhkjdfhds",
                Role = Role.Admin,
                IsDeleted = false,
                Persons = new List<PersonModel>()
                {
                    new PersonModel()
                    {
                        FirstName = "S",
                        LastName = "Sss",
                        Passport =  "7654356754",
                        Patronymic = "SSSSS",
                        IsDeleted = false
                    }
                }
            },
            new UserModel
            {
                Name = "Masha",
                Login = "Mashaaa",
                PasswordHash = "ewdfrgthgfrde",
                Role = Role.User,
                IsDeleted = true,
                Persons = new List<PersonModel>()
                {
                    new PersonModel()
                    {
                        FirstName = "M",
                        LastName = "Mmm",
                        Passport = "234567",
                        Patronymic = "MMMMMM",
                        IsDeleted = false

                    }
                }
            },
            new UserModel
            {
                Name = "Dasha",
                Login = "Dashaaa",
                PasswordHash = "hjngtrfewdrt",
                Role = Role.StationManager,
                IsDeleted = false,
                Persons = new List<PersonModel>()
                {
                    new PersonModel()
                    {
                        FirstName = "D",
                        LastName = "Ddd",
                        Passport =  "8765432",
                        Patronymic = "DDDD",
                        IsDeleted = false

                    }
                }
            },
            new UserModel
            {
                Name = "Pasha",
                Login = "Pashaaa",
                PasswordHash = "erfgthnjytgr",
                Role = Role.TrainRouteManager,
                IsDeleted = false,
                Persons = new List<PersonModel>()
                {
                    new PersonModel()
                    {
                        FirstName = "P",
                        LastName = "Ppp",
                        Passport = "66446646",
                        Patronymic = "PPPP",
                        IsDeleted = false
                    }
                }
            }
        };
    }
}
