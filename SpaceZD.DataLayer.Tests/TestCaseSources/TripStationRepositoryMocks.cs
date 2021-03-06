using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace SpaceZD.DataLayer.Tests.TestCaseSources
{
    public class TripStationRepositoryMocks
    {
        public static List<TripStation> GetTripStations() => new List<TripStation>
        {
            new()
            {
                Station = new Station()
                {
                    Name = "Rostov",
                    Platforms = new List<Platform>
                    {
                        new() { Number = 1 },
                        new() { Number = 2 }
                    }
                },
                Platform = new Platform()
                {
                    Number = 1
                },
                ArrivalTime = new DateTime(2002, 12, 31, 22, 56, 59),

                DepartingTime = new DateTime(2003, 12, 31, 22, 56, 59),

                Trip = new Trip()
                {
                    Train = new Train()
                    {
                        IsDeleted = false
                    },
                    Route = new Route()
                    {
                        Code = "F700",
                        RouteTransits = new List<RouteTransit>
                        {
                            new()
                            {
                                Transit = new Transit
                                {
                                    StartStation = new Station { Name = "С-Пб" },
                                    EndStation = new Station { Name = "Выборг" }
                                },
                                DepartingTime = new TimeSpan(0, 0, 1),
                                ArrivalTime = new TimeSpan(2, 30, 0)
                            }
                        },
                        StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                        StartStation = new Station { Name = "С-Пб" },
                        EndStation = new Station { Name = "Выборг" }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    IsDeleted = false
                }
            },
            new()
            {
                Station = new Station()
                {
                    Name = "Vorcuta",
                    Platforms = new List<Platform>
                    {
                        new() { Number = 1 },
                        new() { Number = 2 }
                    }
                },
                Platform = new Platform()
                {
                    Number = 1
                },
                ArrivalTime = new DateTime(2002, 12, 31, 22, 56, 59),

                DepartingTime = new DateTime(2003, 12, 31, 22, 56, 59),

                Trip = new Trip()
                {
                    Train = new Train()
                    {
                        IsDeleted = false
                    },
                    Route = new Route()
                    {
                        Code = "F345",
                        RouteTransits = new List<RouteTransit>
                        {
                            new()
                            {
                                Transit = new Transit
                                {
                                    StartStation = new Station { Name = "С-Пб" },
                                    EndStation = new Station { Name = "Выборг" }
                                },
                                DepartingTime = new TimeSpan(0, 0, 1),
                                ArrivalTime = new TimeSpan(2, 30, 0)
                            }
                        },
                        StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                        StartStation = new Station { Name = "С-Пб" },
                        EndStation = new Station { Name = "Выборг" }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    IsDeleted = false
                }
            },
            new()
            {
                Station = new Station()
                {
                    Name = "Yamaika",
                    Platforms = new List<Platform>
                    {
                        new() { Number = 1 },
                        new() { Number = 2 }
                    }
                },
                Platform = new Platform()
                {
                    Number = 1
                },
                ArrivalTime = new DateTime(2002, 12, 31, 22, 56, 59),

                DepartingTime = new DateTime(2003, 12, 31, 22, 56, 59),

                Trip = new Trip()
                {
                    Train = new Train()
                    {
                        IsDeleted = false
                    },
                    Route = new Route()
                    {
                        Code = "F790",
                        RouteTransits = new List<RouteTransit>
                        {
                            new()
                            {
                                Transit = new Transit
                                {
                                    StartStation = new Station { Name = "С-Пб" },
                                    EndStation = new Station { Name = "Выборг" }
                                },
                                DepartingTime = new TimeSpan(0, 0, 1),
                                ArrivalTime = new TimeSpan(2, 30, 0)
                            }
                        },
                        StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                        StartStation = new Station { Name = "С-Пб" },
                        EndStation = new Station { Name = "Выборг" }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    IsDeleted = false
                }
            }
        };

        public static TripStation GetTripStation() => new TripStation
        {
            Station = new Station()
            {
                Name = "Pscov",
                Platforms = new List<Platform>
                {
                    new() { Number = 1 },
                    new() { Number = 2 }
                }
            },
            Platform = new Platform()
            {
                Number = 1
            },
            ArrivalTime = new DateTime(2002, 12, 31, 22, 56, 59),

            DepartingTime = new DateTime(2003, 12, 31, 22, 56, 59),

            Trip = new Trip()
            {
                Train = new Train()
                {
                    IsDeleted = false
                },
                Route = new Route()
                {
                    Code = "F400",
                    RouteTransits = new List<RouteTransit>
                    {
                        new()
                        {
                            Transit = new Transit
                            {
                                StartStation = new Station { Name = "С-Пб" },
                                EndStation = new Station { Name = "Выборг" }
                            },
                            DepartingTime = new TimeSpan(0, 0, 1),
                            ArrivalTime = new TimeSpan(2, 30, 0)
                        }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                IsDeleted = false
            }
        };
    }
}