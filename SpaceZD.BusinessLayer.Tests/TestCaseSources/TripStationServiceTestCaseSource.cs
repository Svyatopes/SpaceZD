using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources;

public class TripStationServiceTestCaseSource
{
    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        yield return new TestCaseData(new TripStation
            {
                Station = new Station { Name = "Москва", Platforms = new List<Platform>() },
                Platform = new Platform { Number = 4, Station = new Station { Name = "456" } },
                ArrivalTime = new DateTime(1904, 4, 4),
                DepartingTime = new DateTime(1987, 4, 6)
            },
            new TripStationModel
            {
                Station = new StationModel { Name = "Москва", Platforms = new List<PlatformModel>() },
                Platform = new PlatformModel { Number = 4, Station = new StationModel { Name = "456" } },
                ArrivalTime = new DateTime(1904, 4, 4),
                DepartingTime = new DateTime(1987, 4, 6)
            },
            Role.Admin);
        yield return new TestCaseData(new TripStation
            {
                Station = new Station { Name = "Москва", Platforms = new List<Platform>() },
                Platform = new Platform { Number = 4, Station = new Station { Name = "456" } },
                ArrivalTime = new DateTime(1904, 4, 4),
                DepartingTime = new DateTime(1987, 4, 6)
            },
            new TripStationModel
            {
                Station = new StationModel { Name = "Москва", Platforms = new List<PlatformModel>() },
                Platform = new PlatformModel { Number = 4, Station = new StationModel { Name = "456" } },
                ArrivalTime = new DateTime(1904, 4, 4),
                DepartingTime = new DateTime(1987, 4, 6)
            },
            Role.StationManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForUpdateTest()
    {
        yield return new TestCaseData(new TripStationModel
            {
                ArrivalTime = new DateTime(1999, 2, 2, 1, 3, 4),
                DepartingTime = new DateTime(1999, 2, 3, 4, 5, 6),
                Platform = new PlatformModel { Id = 1 },
                Station = new StationModel { Name = "Выборг", Platforms = new List<PlatformModel>() }
            },
            new TripStationModel
            {
                ArrivalTime = new DateTime(1999, 2, 2, 1, 3, 4),
                DepartingTime = new DateTime(1999, 2, 3, 4, 5, 6),
                Platform = new PlatformModel
                    { Number = 2, Station = new StationModel { Name = "Москва" }, IsDeleted = false },
                Station = new StationModel { Name = "Выборг", Platforms = new List<PlatformModel>() }
            },
            Role.Admin);
        yield return new TestCaseData(new TripStationModel
            {
                ArrivalTime = new DateTime(1999, 2, 2, 1, 3, 4),
                DepartingTime = new DateTime(1999, 2, 3, 4, 5, 6),
                Platform = new PlatformModel { Id = 1 },
                Station = new StationModel { Name = "Выборг", Platforms = new List<PlatformModel>() }
            },
            new TripStationModel
            {
                ArrivalTime = new DateTime(1999, 2, 2, 1, 3, 4),
                DepartingTime = new DateTime(1999, 2, 3, 4, 5, 6),
                Platform = new PlatformModel
                    { Number = 2, Station = new StationModel { Name = "Москва" }, IsDeleted = false },
                Station = new StationModel { Name = "Выборг", Platforms = new List<PlatformModel>() }
            },
            Role.StationManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetReadyPlatformsTest()
    {
        var tripStation = new TripStation
        {
            Station = new Station(), ArrivalTime = new DateTime(1999, 2, 2, 1, 3, 4),
            DepartingTime = new DateTime(1999, 2, 3, 4, 5, 6)
        };

        yield return new TestCaseData(tripStation,
            new List<Platform>
            {
                new() { Number = 1, Station = new Station { Name = "Москва" }, IsDeleted = false },
                new() { Number = 2, Station = new Station { Name = "Москва" }, IsDeleted = false }
            },
            new List<PlatformModel>
            {
                new() { Number = 1, Station = new StationModel { Name = "Москва" }, IsDeleted = false },
                new() { Number = 2, Station = new StationModel { Name = "Москва" }, IsDeleted = false }
            },
            Role.Admin);
        yield return new TestCaseData(tripStation, new List<Platform>(), new List<PlatformModel>(), Role.StationManager);
    }
}