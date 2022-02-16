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
                    IsDeleted = false
                },
                Route = new Route()
                {
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
                    IsDeleted = false
                },
                Route = new Route()
                {
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
                    IsDeleted = false
                },
                Route = new Route()
                {
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
                    IsDeleted = false
                },
                Route = new Route()
                {
                    Code = "F789",
                    Transits = new List<RouteTransit>
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
                    IsDeleted = false
                },
                Route = new Route()
                {
                        Code = "F799",
                        Transits = new List<RouteTransit>
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
                    IsDeleted = false
                },
                Route = new Route()
                {
                        Code = "F710",
                        Transits = new List<RouteTransit>
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
                IsDeleted = false
            },
            Route = new Route()
            {
                Code = "F700",
                Transits = new List<RouteTransit>
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
