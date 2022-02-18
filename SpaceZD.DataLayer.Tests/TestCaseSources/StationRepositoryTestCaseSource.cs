using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestCaseSources;

public static class StationRepositoryTestCaseSource
{
    internal static List<Station> GetStations() => new List<Station>
    {
        new()
        {
            Name = "Челябинск",
            Platforms = new List<Platform>
            {
                new() { Number = 1, IsDeleted = true },
                new() { Number = 2, IsDeleted = false }
            },
            IsDeleted = false
        },
        new()
        {
            Name = "41 км",
            Platforms = new List<Platform>(),
            IsDeleted = true
        },
        new()
        {
            Name = "Таганрог",
            Platforms = new List<Platform>
            {
                new() { Number = 2, IsDeleted = false },
                new() { Number = 3, IsDeleted = true }
            },
            IsDeleted = false
        }
    };

    internal static Station GetStation() => new()
    {
        Name = "Москва",
        Platforms = new List<Platform>
        {
            new() { Number = 1, IsDeleted = false },
            new() { Number = 2, IsDeleted = false },
            new() { Number = 3, IsDeleted = true }
        },
        IsDeleted = false
    };

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        var stationFist = new Station { Name = GetStations()[0].Name, IsDeleted = false };
        stationFist.Platforms = new List<Platform> { new() { Number = 2, Station = stationFist, IsDeleted = false } };
        yield return new TestCaseData(1, stationFist);

        var stationSecond = new Station
            { Name = GetStations()[1].Name, Platforms = new List<Platform>(), IsDeleted = true };
        yield return new TestCaseData(2, stationSecond);

        var stationThird = new Station { Name = GetStations()[2].Name, IsDeleted = false };
        stationThird.Platforms = new List<Platform> { new() { Number = 2, Station = stationThird, IsDeleted = false } };
        yield return new TestCaseData(3, stationThird);

        yield return new TestCaseData(4, null);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        var notIncludeAll = new List<Station>
        {
            new()
            {
                Name = GetStations()[0].Name,
                Platforms = new List<Platform> { new() { Number = 2, Station = GetStations()[0], IsDeleted = false } },
                IsDeleted = false
            },
            new()
            {
                Name = GetStations()[2].Name,
                Platforms = new List<Platform> { new() { Number = 2, Station = GetStations()[2], IsDeleted = false } },
                IsDeleted = false
            }
        };
        yield return new TestCaseData(false, notIncludeAll);

        var includeAll = new List<Station>
        {
            new()
            {
                Name = GetStations()[0].Name,
                Platforms = new List<Platform> { new() { Number = 2, Station = GetStations()[0], IsDeleted = false } },
                IsDeleted = false
            },
            new()
            {
                Name = GetStations()[1].Name,
                Platforms = new List<Platform>(),
                IsDeleted = true
            },
            new()
            {
                Name = GetStations()[2].Name,
                Platforms = new List<Platform> { new() { Number = 2, Station = GetStations()[2], IsDeleted = false } },
                IsDeleted = false
            }
        };
        yield return new TestCaseData(true, includeAll);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForReadyPlatformsStation()
    {
        var station = new Station
             {
                 Name = "Москва",
                 
             };
        
        var platformMaintenance = new PlatformMaintenance { StartTime = new DateTime(2000, 1, 1), EndTime = new DateTime(2001, 1, 1), IsDeleted = false };
        var deletedPlatformMaintenance = new PlatformMaintenance { StartTime = new DateTime(2000, 1, 1), EndTime = new DateTime(2001, 1, 1), IsDeleted = true };

        var tripStation = new TripStation { ArrivalTime = new DateTime(1998, 1, 1), DepartingTime = new DateTime(1999, 1, 1) };

        var platformTripStation = new Platform
        {
            Number = 1,
            Station = station,
            PlatformMaintenances = new List<PlatformMaintenance>(),
            TripStations = new List<TripStation> { tripStation },
            IsDeleted = false
        };
        var platformPlatformMaintenance = new Platform
        {
            Number = 2,
            Station = station,
            PlatformMaintenances = new List<PlatformMaintenance> { platformMaintenance },
            TripStations = new List<TripStation>(),
            IsDeleted = false
        };
        var platformDeletedPlatformMaintenance = new Platform
        {
            Number = 3,
            Station = station,
            PlatformMaintenances = new List<PlatformMaintenance> { deletedPlatformMaintenance },
            TripStations = new List<TripStation>(),
            IsDeleted = false
        };
        var deletedPlatform = new Platform
        {
            Number = 4,
            Station = station,
            PlatformMaintenances = new List<PlatformMaintenance>(),
            TripStations = new List<TripStation>(),
            IsDeleted = true
        };

        station.Platforms = new List<Platform>
        {
            platformTripStation, platformPlatformMaintenance, platformDeletedPlatformMaintenance, deletedPlatform
        };

        yield return new TestCaseData(station,
            new DateTime(1977, 1, 1),
            new DateTime(1988, 1, 1),
            new List<Platform> { platformTripStation, platformPlatformMaintenance, platformDeletedPlatformMaintenance, });
        yield return new TestCaseData(station,
            new DateTime(1977, 1, 1),
            new DateTime(2200, 1, 1),
            new List<Platform> { platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1977, 1, 1),
            new DateTime(1998, 2, 1),
            new List<Platform> { platformPlatformMaintenance, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1977, 1, 1),
            new DateTime(1999, 2, 1),
            new List<Platform> { platformPlatformMaintenance, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1977, 1, 1),
            new DateTime(2000, 2, 1),
            new List<Platform> { platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1998, 2, 1),
            new DateTime(1998, 3, 1),
            new List<Platform> { platformPlatformMaintenance, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1998, 2, 1),
            new DateTime(1999, 2, 1),
            new List<Platform> { platformPlatformMaintenance, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1998, 2, 1),
            new DateTime(2000, 2, 1),
            new List<Platform> { platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1998, 2, 1),
            new DateTime(2200, 1, 1),
            new List<Platform> { platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1999, 2, 1),
            new DateTime(1999, 3, 1),
            new List<Platform> { platformTripStation, platformPlatformMaintenance, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1999, 2, 1),
            new DateTime(2000, 3, 1),
            new List<Platform> { platformTripStation, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(1999, 2, 1),
            new DateTime(2200, 1, 1),
            new List<Platform> { platformTripStation, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(2000, 2, 1),
            new DateTime(2000, 3, 1),
            new List<Platform> { platformTripStation, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(2000, 2, 1),
            new DateTime(2200, 1, 1),
            new List<Platform> { platformTripStation, platformDeletedPlatformMaintenance });
        yield return new TestCaseData(station,
            new DateTime(2100, 1, 1),
            new DateTime(2200, 1, 1),
            new List<Platform> { platformTripStation, platformPlatformMaintenance, platformDeletedPlatformMaintenance, });
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetNearStations()
    {
        var transitFirst = new Transit
            { EndStation = new Station { Name = "Москва", Platforms = new List<Platform>() } };
        var transitSecond = new Transit
            { EndStation = new Station { Name = "Челябинск", Platforms = new List<Platform>(), IsDeleted = true } };
        var transitThird = new Transit { EndStation = new Station { Name = "Омск", Platforms = new List<Platform>() } };
        var transitFourth = new Transit
            { EndStation = new Station { Name = "48 км", Platforms = new List<Platform>() }, IsDeleted = true };
        var transitFifth = new Transit
            { EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() } };


        var station = new Station
        {
            Name = "0 км",
            TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFourth },
            Platforms = new List<Platform>()
        };
        yield return new TestCaseData(station,
            new List<Station>
            {
                new() { Name = transitFirst.EndStation.Name, Platforms = new List<Platform>() },
                new() { Name = transitThird.EndStation.Name, Platforms = new List<Platform>() }
            });


        var station2 = new Station
        {
            Name = "10 км",
            TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFifth },
            Platforms = new List<Platform>()
        };
        yield return new TestCaseData(station2,
            new List<Station>
            {
                new() { Name = transitFirst.EndStation.Name, Platforms = new List<Platform>() },
                new() { Name = transitThird.EndStation.Name, Platforms = new List<Platform>() },
                new() { Name = transitFifth.EndStation.Name, Platforms = new List<Platform>() }
            });
    }
}