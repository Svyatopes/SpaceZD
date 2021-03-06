using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources;

public static class TripServiceTestCaseSource
{
    internal static IEnumerable<TestCaseData> GetTestCaseDataForAddTest()
    {
        var train = new Train { Id = 10, Carriages = new List<Carriage>(), IsDeleted = false };

        var stationOne = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false };
        var stationTwo = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false };
        var stationThree = new Station { Name = "Сочи", Platforms = new List<Platform>(), IsDeleted = false };
        var stationFour = new Station { Name = "Челябинск", Platforms = new List<Platform>(), IsDeleted = false };

        var transitOne = new Transit { StartStation = stationOne, EndStation = stationTwo, IsDeleted = false };
        var transitTwo = new Transit { StartStation = stationTwo, EndStation = stationThree, IsDeleted = false };
        var transitThree = new Transit { StartStation = stationThree, EndStation = stationFour, IsDeleted = false };

        var routeTransitOne = new RouteTransit
            { Transit = transitOne, DepartingTime = new TimeSpan(0, 0, 0), ArrivalTime = new TimeSpan(1, 0, 0), IsDeleted = false };
        var routeTransitTwo = new RouteTransit
            { Transit = transitTwo, DepartingTime = new TimeSpan(1, 10, 0), ArrivalTime = new TimeSpan(2, 0, 0), IsDeleted = false };
        var routeTransitThree = new RouteTransit
            { Transit = transitThree, DepartingTime = new TimeSpan(2, 10, 0), ArrivalTime = new TimeSpan(3, 0, 0), IsDeleted = false };

        var trip = new Trip
        {
            Train = train,
            Route = new Route
            {
                Id = 10,
                Code = "F700",
                StartStation = stationOne,
                EndStation = stationFour,
                RouteTransits = new List<RouteTransit> { routeTransitOne, routeTransitTwo, routeTransitThree },
                StartTime = new DateTime(1, 1, 1, 5, 30, 0),
                IsDeleted = false
            },
            Stations = new List<TripStation>(),
            StartTime = new DateTime(1970, 1, 1),
            IsDeleted = false
        };

        var tripModel = ConvertTripToTripModel(trip);

        trip.StartTime = new DateTime(1970, 1, 1, 5, 30, 0);
        trip.Stations = new List<TripStation>
        {
            new() { Station = stationOne, ArrivalTime = null, DepartingTime = new DateTime(1970, 1, 1, 5, 30, 0) },
            new() { Station = stationTwo, ArrivalTime = new DateTime(1970, 1, 1, 6, 30, 0), DepartingTime = new DateTime(1970, 1, 1, 6, 40, 0) },
            new() { Station = stationThree, ArrivalTime = new DateTime(1970, 1, 1, 7, 30, 0), DepartingTime = new DateTime(1970, 1, 1, 7, 40, 0) },
            new() { Station = stationFour, ArrivalTime = new DateTime(1970, 1, 1, 8, 30, 0), DepartingTime = null }
        };

        yield return new TestCaseData(tripModel, trip, Role.Admin);
        yield return new TestCaseData(tripModel, trip, Role.TrainRouteManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForAddNegativeInvalidDataExceptionTest()
    {
        var trip = new Trip
        {
            Train = new Train { Id = 10, Carriages = new List<Carriage>(), IsDeleted = false },
            Route = new Route { Id = 10, RouteTransits = new List<RouteTransit>() },
            IsDeleted = false
        };

        yield return new TestCaseData(trip);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        var trip = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "F700",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = false
        };

        yield return new TestCaseData(trip, ConvertTripToTripModel(trip));
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListDeletedTest()
    {
        var tripOne = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "F700",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = true
        };
        var tripTwo = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "F999",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1999, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1978, 1, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = false
        };
        var tripThree = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "П548",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1944, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1944, 2, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = true
        };

        yield return new TestCaseData(new List<Trip> { tripOne, tripTwo, tripThree },
            new List<TripModel> { ConvertTripToTripModel(tripOne), ConvertTripToTripModel(tripThree) },
            Role.Admin);
        yield return new TestCaseData(new List<Trip> { tripOne }, new List<TripModel> { ConvertTripToTripModel(tripOne) }, Role.Admin);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        var tripOne = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "F700",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = false
        };
        var tripTwo = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "F999",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1999, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1978, 1, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = true
        };
        var tripThree = new Trip
        {
            Train = new Train
            {
                Carriages = new List<Carriage>(),
                IsDeleted = false
            },
            Route = new Route
            {
                Code = "П548",
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                            EndStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1944, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>(), IsDeleted = false },
                EndStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false }
            },
            StartTime = new DateTime(1944, 2, 1, 5, 30, 0),
            Stations = new List<TripStation>(),
            IsDeleted = false
        };

        yield return new TestCaseData(new List<Trip> { tripOne, tripTwo, tripThree },
            new List<TripModel> { ConvertTripToTripModel(tripOne), ConvertTripToTripModel(tripTwo), ConvertTripToTripModel(tripThree) });
        yield return new TestCaseData(new List<Trip> { tripOne, tripThree },
            new List<TripModel> { ConvertTripToTripModel(tripOne), ConvertTripToTripModel(tripThree) });
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetFreeSeatTest()
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

        var carriageSeats = new List<CarriageSeatsModel>();
        foreach (var carriage in train.Carriages)
        {
            carriageSeats.Add(new CarriageSeatsModel { Carriage = ConvertCarriageToCarriageModel(carriage), Seats = new List<SeatModel>() });
            for (int i = 1; i <= carriage.Type.NumberOfSeats; i++)
                carriageSeats.Single(g => g.Carriage.Equals(ConvertCarriageToCarriageModel(carriage)))
                             .Seats
                             .Add(new SeatModel { NumberOfSeats = i, IsFree = true });
        }

        var carriageSeatsOne = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsOne[0].Seats[0].IsFree = false;
        carriageSeatsOne[0].Seats[2].IsFree = false;
        carriageSeatsOne[1].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationFour, carriageSeatsOne);

        var carriageSeatsTwo = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsTwo[0].Seats[0].IsFree = false;
        carriageSeatsTwo[0].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationThree, carriageSeatsTwo);

        var carriageSeatsThree = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsThree[0].Seats[2].IsFree = false;
        carriageSeatsThree[1].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationThree, stationFour, carriageSeatsThree);

        var carriageSeatsFour = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsFour[0].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationTwo, carriageSeatsFour);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetFreeSeatNegativeTest()
    {
        var stationOne = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false };
        var trip = new Trip
        {
            Train = new Train { Carriages = new List<Carriage>() },
            Stations = new List<TripStation> { new() { Station = stationOne } },
            Orders = new List<Order>()
        };
        yield return new TestCaseData(trip, stationOne);
    }

    private static CarriageModel ConvertCarriageToCarriageModel(Carriage entity)
    {
        return new CarriageModel
        {
            Number = entity.Number, IsDeleted = entity.IsDeleted,
            Type = new CarriageTypeModel { Name = entity.Type.Name, IsDeleted = entity.Type.IsDeleted, NumberOfSeats = entity.Type.NumberOfSeats }
        };
    }

    private static TripModel ConvertTripToTripModel(Trip entity)
    {
        return new TripModel
        {
            Route = new RouteModel
            {
                Code = entity.Route.Code,
                EndStation = new StationModel
                {
                    Name = entity.Route.EndStation.Name,
                    Platforms = new List<PlatformModel>(),
                    IsDeleted = entity.Route.EndStation.IsDeleted
                },
                StartStation = new StationModel
                {
                    Name = entity.Route.StartStation.Name,
                    Platforms = new List<PlatformModel>(),
                    IsDeleted = entity.Route.StartStation.IsDeleted
                },
                StartTime = entity.Route.StartTime,
                RouteTransits = entity.Route.RouteTransits.Select(routeTransit => new RouteTransitModel
                                       {
                                           Transit = new TransitModel
                                           {
                                               StartStation =
                                                   new StationModel { Name = routeTransit.Transit.StartStation.Name, Platforms = new List<PlatformModel>() },
                                               EndStation = new StationModel
                                                   { Name = routeTransit.Transit.EndStation.Name, Platforms = new List<PlatformModel>() }
                                           },
                                           DepartingTime = routeTransit.DepartingTime,
                                           ArrivalTime = routeTransit.ArrivalTime
                                       })
                                      .ToList(),
                IsDeleted = entity.Route.IsDeleted
            },
            StartTime = entity.StartTime,
            Train = new TrainModel { Carriages = new List<CarriageModel>(), IsDeleted = entity.Train.IsDeleted },
            Stations = new List<TripStationModel>(),
            IsDeleted = entity.IsDeleted
        };
    }
}