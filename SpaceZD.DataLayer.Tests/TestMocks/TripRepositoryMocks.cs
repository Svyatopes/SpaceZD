using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestMocks;

public static class TripRepositoryMocks
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
                Id = 2,
                IsDeleted = false
            },
            Route = new Route()
            {
                Id = 2,
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
                Id = 3,
                IsDeleted = false
            },
            Route = new Route()
            {
                Id = 3,
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
    };

    internal static IEnumerable<TestCaseData> GetTestCaseDataForMarkNonFreeSeatsInListAllSeatsTest()
    {
        var carriageType = new CarriageType { Name = "Купе", NumberOfSeats = 3, IsDeleted = false };

        var carriageOne = new Carriage { Number = 1, Type = carriageType, IsDeleted = false };
        var carriageTwo = new Carriage { Number = 2, Type = carriageType, IsDeleted = false };

        var train = new Train { Carriages = new List<Carriage> { carriageOne, carriageTwo } };

        var ticketOne = new Ticket { Carriage = carriageOne, SeatNumber = 1 };
        var ticketTwo = new Ticket { Carriage = carriageOne, SeatNumber = 3 };
        var ticketThree = new Ticket { Carriage = carriageTwo, SeatNumber = 3 };

        var stationOne = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false };
        var stationTwo = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false };
        var stationThree = new Station { Name = "Сочи", Platforms = new List<Platform>(), IsDeleted = false };
        var stationFour = new Station { Name = "Челябинск", Platforms = new List<Platform>(), IsDeleted = false };

        var tripStationOne = new TripStation { Station = stationOne };
        var tripStationTwo = new TripStation { Station = stationTwo };
        var tripStationThree = new TripStation { Station = stationThree };
        var tripStationFour = new TripStation { Station = stationFour };

        var orderOne = new Order { Tickets = new List<Ticket> { ticketOne }, StartStation = tripStationTwo, EndStation = tripStationThree };
        var orderTwo = new Order { Tickets = new List<Ticket> { ticketTwo }, StartStation = tripStationOne, EndStation = tripStationFour };
        var orderThree = new Order { Tickets = new List<Ticket> { ticketThree }, StartStation = tripStationThree, EndStation = tripStationFour };

        var trip = new Trip
        {
            Train = train,
            Stations = new List<TripStation> { tripStationOne, tripStationTwo, tripStationThree, tripStationFour },
            Orders = new List<Order> { orderOne, orderTwo, orderThree }
        };

        var carriageSeats = new List<CarriageSeats>();
        foreach (var carriage in train.Carriages)
        {
            carriageSeats.Add(new CarriageSeats { Carriage = carriage, Seats = new List<Seat>() });
            for (int i = 1; i <= carriage.Type.NumberOfSeats; i++)
                carriageSeats.Single(g => g.Carriage.Equals(carriage))
                             .Seats
                             .Add(new Seat { NumberOfSeats = i, IsFree = true });
        }

        var carriageSeatsOne = new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() };
        carriageSeatsOne[0].Seats[0].IsFree = false;
        carriageSeatsOne[0].Seats[2].IsFree = false;
        carriageSeatsOne[1].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationFour, new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() }, carriageSeatsOne);
        
        var carriageSeatsTwo = new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() };
        carriageSeatsTwo[0].Seats[0].IsFree = false;
        carriageSeatsTwo[0].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationThree, new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() }, carriageSeatsTwo);
        
        var carriageSeatsThree = new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() };
        carriageSeatsThree[0].Seats[2].IsFree = false;
        carriageSeatsThree[1].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationThree, stationFour, new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() }, carriageSeatsThree);
        
        var carriageSeatsFour = new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() };
        carriageSeatsFour[0].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationTwo, new List<CarriageSeats> { (CarriageSeats)carriageSeats[0].Clone(), (CarriageSeats)carriageSeats[1].Clone() }, carriageSeatsFour);
    }
}