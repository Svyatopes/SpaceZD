using System;
using System.Collections.Generic;
using NUnit.Framework;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestCaseSources;

public static class RouteRepositoryTestCaseSource
{
    internal static List<Route> GetRoutes() => new List<Route>
    {
        new()
        {
            Code = "F789",
            RouteTransits = new List<RouteTransit>
            {
                new()
                {
                    Transit = new Transit
                    {
                        StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>() },
                        EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() }
                    },
                    DepartingTime = new TimeSpan(0, 0, 1),
                    ArrivalTime = new TimeSpan(2, 30, 0),
                    IsDeleted = false
                }
            },
            StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>() },
            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() },
            IsDeleted = true
        },
        new()
        {
            Code = "Test",
            RouteTransits = new List<RouteTransit>(),
            StartTime = new DateTime(1999, 10, 1),
            StartStation = new Station { Name = "Москва", Platforms = new List<Platform>() },
            EndStation = new Station { Name = "Новгород", Platforms = new List<Platform>() },
            IsDeleted = true
        },
        new()
        {
            Code = "G589",
            RouteTransits = new List<RouteTransit>
            {
                new()
                {
                    Transit = new Transit
                    {
                        StartStation = new Station { Name = "Петрозаводск", Platforms = new List<Platform>() },
                        EndStation = new Station { Name = "Омск", Platforms = new List<Platform>() }
                    },
                    DepartingTime = new TimeSpan(0, 0, 0),
                    ArrivalTime = new TimeSpan(4, 0, 0),
                    IsDeleted = true
                },
                new()
                {
                    Transit = new Transit
                    {
                        StartStation = new Station { Name = "Омск", Platforms = new List<Platform>() },
                        EndStation = new Station { Name = "Сочи", Platforms = new List<Platform>() }
                    },
                    DepartingTime = new TimeSpan(4, 30, 0),
                    ArrivalTime = new TimeSpan(1, 0, 0),
                    IsDeleted = true
                }
            },
            StartTime = new DateTime(1970, 1, 1, 12, 30, 0),
            StartStation = new Station { Name = "Петрозаводск", Platforms = new List<Platform>() },
            EndStation = new Station { Name = "Сочи", Platforms = new List<Platform>() },
            IsDeleted = false
        }
    };

    internal static Route GetRoute() => new()
    {
        Code = "M456",
        RouteTransits = new List<RouteTransit>
        {
            new()
            {
                Transit = new Transit
                {
                    StartStation = new Station { Name = "Москва", Platforms = new List<Platform>() },
                    EndStation = new Station { Name = "Псков", Platforms = new List<Platform>() }
                },
                DepartingTime = new TimeSpan(1, 1, 0),
                ArrivalTime = new TimeSpan(1, 0, 0)
            },
            new()
            {
                Transit = new Transit
                {
                    StartStation = new Station { Name = "Псков", Platforms = new List<Platform>() },
                    EndStation = new Station { Name = "Новгород", Platforms = new List<Platform>() }
                },
                DepartingTime = new TimeSpan(2, 1, 0),
                ArrivalTime = new TimeSpan(2, 0, 0)
            }
        },
        StartTime = new DateTime(1999, 10, 1),
        StartStation = new Station { Name = "Москва", Platforms = new List<Platform>() },
        EndStation = new Station { Name = "Новгород", Platforms = new List<Platform>() }
    };

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        var routeFist = new Route
        {
            Code = GetRoutes()[0].Code,
            StartTime = GetRoutes()[0].StartTime,
            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>() },
            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() },
            IsDeleted = true,
            RouteTransits = new List<RouteTransit>
            {
                new()
                {
                    Transit = new Transit
                    {
                        StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>() },
                        EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() }
                    },
                    DepartingTime = new TimeSpan(0, 0, 1),
                    ArrivalTime = new TimeSpan(2, 30, 0),
                    IsDeleted = false
                }
            }
        };
        yield return new TestCaseData(1, routeFist);

        var routeSecond = new Route
        {
            Code = GetRoutes()[1].Code,
            StartTime = GetRoutes()[1].StartTime,
            RouteTransits = new List<RouteTransit>(),
            StartStation = new Station { Name = "Москва", Platforms = new List<Platform>() },
            EndStation = new Station { Name = "Новгород", Platforms = new List<Platform>() },
            IsDeleted = true
        };
        yield return new TestCaseData(2, routeSecond);

        var routeThird = new Route
        {
            Code = GetRoutes()[2].Code,
            StartTime = GetRoutes()[2].StartTime,
            RouteTransits = new List<RouteTransit>(),
            StartStation = new Station { Name = "Петрозаводск", Platforms = new List<Platform>() },
            EndStation = new Station { Name = "Сочи", Platforms = new List<Platform>() },
            IsDeleted = false
        };
        yield return new TestCaseData(3, routeThird);

        yield return new TestCaseData(4, null);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        var notIncludeAll = new List<Route>
        {
            new()
            {
                Code = GetRoutes()[2].Code,
                StartTime = GetRoutes()[2].StartTime,
                RouteTransits = new List<RouteTransit>(),
                StartStation = new Station { Name = "Петрозаводск", Platforms = new List<Platform>() },
                EndStation = new Station { Name = "Сочи", Platforms = new List<Platform>() },
                IsDeleted = false
            }
        };
        yield return new TestCaseData(false, notIncludeAll);

        var includeAll = new List<Route>
        {
            new()
            {
                Code = GetRoutes()[0].Code,
                StartTime = GetRoutes()[0].StartTime,
                StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>() },
                EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() },
                IsDeleted = true,
                RouteTransits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб", Platforms = new List<Platform>() },
                            EndStation = new Station { Name = "Выборг", Platforms = new List<Platform>() }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0),
                        IsDeleted = false
                    }
                }
            },
            new()
            {
                Code = GetRoutes()[1].Code,
                StartTime = GetRoutes()[1].StartTime,
                RouteTransits = new List<RouteTransit>(),
                StartStation = new Station { Name = "Москва", Platforms = new List<Platform>() },
                EndStation = new Station { Name = "Новгород", Platforms = new List<Platform>() },
                IsDeleted = true
            },
            new()
            {
                Code = GetRoutes()[2].Code,
                StartTime = GetRoutes()[2].StartTime,
                RouteTransits = new List<RouteTransit>(),
                StartStation = new Station { Name = "Петрозаводск", Platforms = new List<Platform>() },
                EndStation = new Station { Name = "Сочи", Platforms = new List<Platform>() },
                IsDeleted = false
            }
        };
        yield return new TestCaseData(true, includeAll);
    }
}