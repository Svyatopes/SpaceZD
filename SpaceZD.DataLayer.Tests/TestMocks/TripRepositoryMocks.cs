using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace SpaceZD.DataLayer.Tests.TestCaseSources
{
    public class TripRepositoryMocks
    {
        public static List<Trip> GetShortTrips() => new List<Trip>
        {
            new()
            {
                Train = new Train()
                {
                    Id = 1,
                    IsDeleted = false
                },
                Route = new Route()
                {
                    Id = 1,
                    Code = "F789",
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                IsDeleted = false
            },
            new()
            {
                Train = new Train()
                {
                    Id = 2,
                    IsDeleted = false
                },
                Route = new Route()
                {
                    Id = 2,
                    Code = "F799",                        
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                IsDeleted = false
            },
            new()
            {
                Train = new Train()
                {
                    Id = 3,
                    IsDeleted = false
                },
                Route = new Route()
                {
                    Id = 3,
                    Code = "F710",
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                IsDeleted = false
            }
        };

        public static List<Trip> GetTrips() => new List<Trip>
        {
            new()
            {
                Train = new Train()
                {
                    Id = 1,
                    IsDeleted = false
                },
                Route = new Route()
                {
                    Id = 1,
                    Code = "F789",
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
            },
            new()
            {
                Train = new Train()
                {
                    Id = 2,
                    IsDeleted = false
                },
                Route = new Route()
                {
                        Id = 2,
                        Code = "F799",
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
            },
            new()
            {
                Train = new Train()
                {
                    Id = 3,
                    IsDeleted = false
                },
                Route = new Route()
                {
                        Id = 3,
                        Code = "F710",
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

        public static Trip GetTrip() => new Trip
        {
            Train = new Train()
            {
                Id = 1,
                IsDeleted = false
            },
            Route = new Route()
            {
                Id = 1,
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
        };
    }
}
