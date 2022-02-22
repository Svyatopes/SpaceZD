using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    public class OrderServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetOrders(), GetOrderModels(), false, 1);
            yield return new TestCaseData(GetOrders(), GetOrderModels(), true, 1);
            yield return new TestCaseData(GetOrders(), GetOrderModels(), false, 2);
            yield return new TestCaseData(GetOrders(), GetOrderModels(), true, 2);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var orders = GetOrders();
            var orderModels = GetOrderModels();
            yield return new TestCaseData(orders[0], orderModels[0], Role.User, 42);
            yield return new TestCaseData(orders[1], orderModels[1], Role.Admin, 1);
        }

        private static List<Order> GetOrders() => new List<Order>
        {
            new Order
            {
                StartStation = new TripStation
                {
                    Station = new Station
                    {
                        Name = "Bologoe",
                    }
                },
                EndStation = new TripStation
                {
                    Station= new Station
                    {
                        Name = "Moscow"
                    }
                },
                User = new User
                {
                  Name = "Egor",
                  Login = "Egorka",
                  PasswordHash = "askldjalsdj",
                  Role = Role.User
                },
                Trip = new Trip
                {
                    Train = new Train()
                    {
                        Carriages = new List<Carriage>
                        {
                            new Carriage
                            {
                                Number = 1,
                                Type = new CarriageType
                                {
                                    Name = "СВ",
                                    NumberOfSeats = 1
                                }
                            },
                            new Carriage
                            {
                                Number = 2,
                                Type = new CarriageType
                                {
                                    Name = "Плацкарт",
                                    NumberOfSeats = 2
                                }
                            }
                        }
                    },
                    Route = new Route
                    {
                      StartStation = new Station
                      {
                          Name = "Jeta"
                      },
                      EndStation = new Station
                      {
                          Name = "Beta"
                      }
                    }
                },
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Carriage = new Carriage
                        {
                            Number = 1,
                            Type = new CarriageType { Name = "Lux", NumberOfSeats = 10}
                        },
                        IsPetPlaceIncluded = false,
                        IsTeaIncluded = true,
                        Person = new Person
                        {
                            FirstName  = "Tiger",
                            LastName = "Woods",
                            Passport = "KFDSLK23123",
                            Patronymic = "JR"
                        },
                        Price = 350,
                        SeatNumber = 3,
                        Order = new Order
                        {
                             StartStation = new TripStation
                             {
                                 Station = new Station
                                 {
                                     Name = "Bologoe",
                                 }
                             },
                        },
                        IsDeleted = false
                    },
                    new Ticket
                    {
                        Carriage = new Carriage
                        {
                            Number = 1,
                            Type = new CarriageType { Name = "Lux", NumberOfSeats = 10}
                        },
                        IsPetPlaceIncluded = true,
                        IsTeaIncluded = false,
                        Person = new Person
                        {
                            FirstName  = "G",
                            LastName = "Man",
                            Passport = "KFDSLK23123",
                            Patronymic = "Half-Life 3"
                        },
                        Price = 420,
                        SeatNumber = 5,
                        Order = new Order
                        {
                             StartStation = new TripStation
                             {
                                 Station = new Station
                                 {
                                     Name = "Bologoe",
                                 }
                             },
                        },
                        IsDeleted = true
                    }
                },
                IsDeleted = false
            },
            new Order
            {
                StartStation = new TripStation
                {
                    Station = new Station
                    {
                        Name = "Saint-P",
                    }
                },
                EndStation = new TripStation
                {
                    Station= new Station
                    {
                        Name = "Avtovo"
                    }
                },
                User = new User
                {
                  Name = "Anton",
                  Login = "Antoshka",
                  PasswordHash = "aqweasd",
                  Role = Role.User
                },
                Trip = new Trip
                {
                     Train = new Train()
                    {
                        Carriages = new List<Carriage>
                        {
                            new Carriage
                            {
                                Number = 1,
                                Type = new CarriageType
                                {
                                    Name = "СВ",
                                    NumberOfSeats = 1
                                }
                            },
                            new Carriage
                            {
                                Number = 2,
                                Type = new CarriageType
                                {
                                    Name = "Плацкарт",
                                    NumberOfSeats = 2
                                }
                            }
                        }
                    },
                    Route = new Route
                    {
                      StartStation = new Station
                      {
                          Name = "Gama"
                      },
                      EndStation = new Station
                      {
                          Name = "Zeta"
                      }
                    }
                },
                Tickets = new List<Ticket>(),
                IsDeleted = true
            }
        };

        private static List<OrderModel> GetOrderModels() => new List<OrderModel>
        {
            new OrderModel
            {
                Tickets = new List<TicketModel>
                {
                    new TicketModel
                    {
                        Carriage = new CarriageModel
                        {
                            Number = 1,
                            Type = new CarriageTypeModel { Name = "Lux", NumberOfSeats = 10}
                        },
                        IsPetPlaceIncluded = false,
                        IsTeaIncluded = true,
                        Person = new PersonModel
                        {
                            FirstName  = "Tiger",
                            LastName = "Woods",
                            Passport = "KFDSLK23123",
                            Patronymic = "JR"
                        },
                        Price = 350,
                        SeatNumber = 3,
                        Order = new OrderModel
                        {
                             StartStation = new TripStationModel
                             {
                                 Station = new StationModel
                                 {
                                     Name = "Bologoe",
                                 }
                             },
                        },
                        IsDeleted = false
                    },
                    new TicketModel
                    {
                        Carriage = new CarriageModel
                        {
                            Number = 1,
                            Type = new CarriageTypeModel { Name = "Lux", NumberOfSeats = 10}
                        },
                        IsPetPlaceIncluded = true,
                        IsTeaIncluded = false,
                        Person = new PersonModel
                        {
                            FirstName  = "G",
                            LastName = "Man",
                            Passport = "KFDSLK23123",
                            Patronymic = "Half-Life 3"
                        },
                        Price = 420,
                        SeatNumber = 5,
                        Order = new OrderModel
                        {
                             StartStation = new TripStationModel
                             {
                                 Station = new StationModel
                                 {
                                     Name = "Bologoe",
                                 }
                             },
                        },
                        IsDeleted = true
                    }
                },
                StartStation = new TripStationModel
                {
                    Station = new StationModel
                    {
                        Name = "Bologoe",
                        Platforms = new List<PlatformModel>()
                    }
                },
                EndStation = new TripStationModel
                {
                    Station= new StationModel
                    {
                        Name = "Moscow",
                        Platforms = new List<PlatformModel>()
                    }
                },
                User = new UserModel
                {
                  Name = "Egor",
                  Login = "Egorka",
                  PasswordHash = "askldjalsdj",
                  Role = Role.User,
                  Orders = new List<OrderModel>()
                },
                Trip = new TripModel
                {
                    Train = new TrainModel()
                    {
                        Carriages = new List<CarriageModel>
                        {
                            new CarriageModel
                            {
                                Number = 1,
                                Type = new CarriageTypeModel
                                {
                                    Name = "СВ",
                                    NumberOfSeats = 1
                                }
                            },
                            new CarriageModel
                            {
                                Number = 2,
                                Type = new CarriageTypeModel
                                {
                                    Name = "Плацкарт",
                                    NumberOfSeats = 2
                                }
                            }
                        }
                    },
                    Orders = new List<OrderModel>(),
                    Stations = new List<TripStationModel>(),
                    Route = new RouteModel
                    {
                      StartStation = new StationModel
                      {
                          Name = "Jeta",
                          Platforms = new List<PlatformModel>()
                      },
                      EndStation = new StationModel
                      {
                          Name = "Beta",
                          Platforms = new List<PlatformModel>()
                      },
                      RouteTransits = new List<RouteTransitModel>()
                      
                    }
                },
                
                IsDeleted = false
            },
            new OrderModel
            {
                Tickets = new List<TicketModel>(),
                StartStation = new TripStationModel
                {
                    Station = new StationModel
                    {
                        Name = "Saint-P",
                        Platforms = new List<PlatformModel>()
                    }
                },
                EndStation = new TripStationModel
                {
                    Station= new StationModel
                    {
                        Name = "Avtovo",
                        Platforms = new List<PlatformModel>()
                    }
                },
                User = new UserModel
                {
                  Name = "Anton",
                  Login = "Antoshka",
                  PasswordHash = "aqweasd",
                  Role = Role.User,
                  Orders = new List<OrderModel>()
                },
                Trip = new TripModel
                {
                    Train = new TrainModel()
                    {
                        Carriages = new List<CarriageModel>
                        {
                            new CarriageModel
                            {
                                Number = 1,
                                Type = new CarriageTypeModel
                                {
                                    Name = "СВ",
                                    NumberOfSeats = 1
                                }
                            },
                            new CarriageModel
                            {
                                Number = 2,
                                Type = new CarriageTypeModel
                                {
                                    Name = "Плацкарт",
                                    NumberOfSeats = 2
                                }
                            }
                        }
                    },
                    Orders = new List<OrderModel>(),
                    Stations = new List<TripStationModel>(),
                    Route = new RouteModel
                    {
                      StartStation = new StationModel
                      {
                          Name = "Gama",
                          Platforms = new List<PlatformModel>()
                      },
                      EndStation = new StationModel
                      {
                          Name = "Zeta",
                          Platforms = new List<PlatformModel>()
                      },
                      RouteTransits = new List<RouteTransitModel>()

                    }
                },
                IsDeleted = true
            }
        };
    }
}
