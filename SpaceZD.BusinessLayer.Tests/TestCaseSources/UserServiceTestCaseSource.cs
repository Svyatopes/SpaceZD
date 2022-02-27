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
