using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestMocks;

public static class StationRepositoryMocks
{
    public static List<Station> GetStations() => new List<Station>
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

    public static Station GetStation() => new()
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

    public static IEnumerable<TestCaseData> GetMockFromGetByIdTest()
    {
        var stationFist = new Station { Name = GetStations()[0].Name, IsDeleted = false };
        stationFist.Platforms = new List<Platform> { new() { Number = 2, Station = stationFist, IsDeleted = false } };
        yield return new TestCaseData(1, stationFist);

        var stationSecond = new Station { Name = GetStations()[1].Name, Platforms = new List<Platform>(), IsDeleted = true };
        yield return new TestCaseData(2, stationSecond);

        var stationThird = new Station { Name = GetStations()[2].Name, IsDeleted = false };
        stationThird.Platforms = new List<Platform> { new() { Number = 2, Station = stationThird, IsDeleted = false } };
        yield return new TestCaseData(3, stationThird);

        yield return new TestCaseData(4, null);
    }

    public static IEnumerable<TestCaseData> GetMockFromGetListTest()
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

    public static IEnumerable<TestCaseData> GetMockFromReadyPlatformsStation()
    {
        var allTimePM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MaxValue, IsDeleted = false };
        var allTimeDeletedPM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MaxValue, IsDeleted = true };
        var notTimePM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MinValue, IsDeleted = false };
        var notTimeDeletedPM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MinValue, IsDeleted = true };

        var allTimeTS = new TripStation { ArrivalTime = DateTime.MinValue, DepartingTime = DateTime.MaxValue };
        var notTimeTS = new TripStation { ArrivalTime = DateTime.MinValue, DepartingTime = DateTime.MinValue };

        var notReadyPlatformFist = new Platform
        {
            Number = 1,
            PlatformMaintenances = new List<PlatformMaintenance> { allTimeDeletedPM, notTimeDeletedPM },
            TripStations = new List<TripStation> { allTimeTS, notTimeTS },
            IsDeleted = false
        };
        var notReadyPlatformSecond = new Platform
        {
            Number = 5,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimePM, notTimeDeletedPM, allTimePM },
            TripStations = new List<TripStation> { allTimeTS },
            IsDeleted = false
        };
        var notReadyDeletedPlatform = new Platform
        {
            Number = 2,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimePM, notTimeDeletedPM, allTimePM },
            TripStations = new List<TripStation> { allTimeTS },
            IsDeleted = true
        };
        var readyPlatform = new Platform
        {
            Number = 3,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimeDeletedPM, allTimeDeletedPM },
            TripStations = new List<TripStation> { notTimeTS },
            IsDeleted = false
        };
        var readyDeletedPlatform = new Platform
        {
            Number = 4,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimeDeletedPM, allTimePM, allTimeDeletedPM },
            TripStations = new List<TripStation> { notTimeTS },
            IsDeleted = true
        };

        yield return new TestCaseData(new Station
            {
                Name = "Москва",
                Platforms = new List<Platform> { notReadyPlatformFist, notReadyPlatformSecond, notReadyDeletedPlatform, readyPlatform, readyDeletedPlatform }
            },
            new List<Platform> { readyPlatform });
        yield return new TestCaseData(new Station
                { Name = "Выборг", Platforms = new List<Platform> { readyPlatform, readyPlatform, readyPlatform, notReadyPlatformSecond } },
            new List<Platform>
            {
                readyPlatform, readyPlatform, readyPlatform
            });
        yield return new TestCaseData(new Station
                { Name = "Сочи", Platforms = new List<Platform> { readyDeletedPlatform, notReadyDeletedPlatform, notReadyPlatformSecond, notReadyPlatformFist } },
            new List<Platform>());
    }

    public static IEnumerable<TestCaseData> GetMockFromGetNearStations()
    {
        var transitFirst = new Transit { EndStation = new Station { Name = "Москва", Platforms = new List<Platform>() } };
        var transitSecond = new Transit { EndStation = new Station { Name = "Челябинск", Platforms = new List<Platform>(), IsDeleted = true } };
        var transitThird = new Transit { EndStation = new Station { Name = "Омск", Platforms = new List<Platform>() } };
        var transitFourth = new Transit { EndStation = new Station { Name = "48 км", Platforms = new List<Platform>() }, IsDeleted = true };
        var transitFifth = new Transit { EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() } };


        var station = new Station
        {
            Name = "0 км", TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFourth },
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
            Name = "10 км", TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFifth },
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