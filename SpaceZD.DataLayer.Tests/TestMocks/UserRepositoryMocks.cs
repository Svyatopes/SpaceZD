using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System.Collections.Generic;

namespace SpaceZD.DataLayer.Tests.TestMocks
{
    public class UserRepositoryMocks
    {
        public static User GetTestEntity() => new User()
        {
            Name = "Sasha",
            Login = "SashahsaS",
            PasswordHash = "hdebuvjcbh",
            Role = Role.TrainRouteManager,
            Orders = new List<Order>()
            {
                new()
                {
                    StartStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Spb"
                        }
                    },
                    EndStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Msk"
                        }
                    }
                }
            }
        };

        public static List<User> GetListTestEntities() => new List<User>()
        {
            new()
            {
                Name = "Sasha",
                Login = "SashahsaS",
                PasswordHash = "hdebuvjcbh",
                Role = Role.TrainRouteManager,
                Orders = new List<Order>()
                {
                    new()
                    {
                        StartStation = new TripStation()
                        {
                            Station = new Station()
                            {
                                Name = "Spb"
                            }
                        },
                        EndStation = new TripStation()
                        {
                            Station = new Station()
                            {
                                Name = "Msk"
                            }
                        }
                    }
                }
            },
            new()
            {
                Name = "Masha",
                Login = "MashahsaM",
                PasswordHash = "wertyu",
                Role = Role.User,
                Orders = new List<Order>()
                {
                    new()
                    {
                        StartStation = new TripStation()
                        {
                            Station = new Station()
                            {
                                Name = "Pskov"
                            }
                        },
                        EndStation = new TripStation()
                        {
                            Station = new Station()
                            {
                                Name = "Sbp"
                            }
                        }
                    }
                }
            },
            new()
            {
                Name = "Pasha",
                Login = "PashahsaP",
                PasswordHash = "asdfghj",
                Role = Role.TrainRouteManager,
                Orders = new List<Order>()
                {
                    new()
                    {
                        StartStation = new TripStation()
                        {
                            Station = new Station()
                            {
                                Name = "Msk"
                            }
                        },
                        EndStation = new TripStation()
                        {
                            Station = new Station()
                            {
                                Name = "Vladivostok"
                            }
                        }
                    }
                }
            }
        };

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
                User = new User(){ Name = "S" , Login = "SS", PasswordHash ="kjhgytfdr"}
            }

        };
    }
}
