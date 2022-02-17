using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources;

public static class TripServiceTestCaseSource
{
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
        yield return new TestCaseData(trip, stationOne, stationFour, carriageSeatsOne, false);

        var carriageSeatsTwo = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsTwo[0].Seats[0].IsFree = false;
        carriageSeatsTwo[0].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationThree, carriageSeatsTwo, false);

        var carriageSeatsThree = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsThree[0].Seats[2].IsFree = false;
        carriageSeatsThree[1].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationThree, stationFour, carriageSeatsThree, false);

        var carriageSeatsFour = new List<CarriageSeatsModel> { (CarriageSeatsModel)carriageSeats[0].Clone(), (CarriageSeatsModel)carriageSeats[1].Clone() };
        carriageSeatsFour[0].Seats[2].IsFree = false;
        yield return new TestCaseData(trip, stationOne, stationTwo, carriageSeatsFour, false);
        
        yield return new TestCaseData(trip, stationOne, stationOne, null, true);
    }

    private static CarriageModel ConvertCarriageToCarriageModel(Carriage entity)
    {
        return new CarriageModel
        {
            Number = entity.Number, IsDeleted = entity.IsDeleted,
            Type = new CarriageTypeModel { Name = entity.Type.Name, IsDeleted = entity.Type.IsDeleted, NumberOfSeats = entity.Type.NumberOfSeats }
        };
    }
}