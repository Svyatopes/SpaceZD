using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace SpaceZD.DataLayer.Tests.TestCaseSources
{
    public class TripStationRepositoryMocks
    {
        public static List<TripStation> GetTrips() => new List<TripStation>
        {

        };

        public static TripStation GetTrip() => new TripStation
        {
            Station = new Station()
            {

            },
            Platform = new Platform()
            {

            },
            ArrivalTime = new DateTime(2002, 12, 31, 22, 56, 59),

            DepartingTime = new DateTime(2003, 12, 31, 22, 56, 59),

            Trip = new Trip()
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
    }
}