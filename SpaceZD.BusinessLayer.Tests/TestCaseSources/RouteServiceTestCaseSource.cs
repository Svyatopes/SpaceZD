using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources;

internal static class RouteServiceTestCaseSource
{
    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        var startStation = new Station { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false };
        var endStation = new Station { Name = "Санкт-Петербург", Platforms = new List<Platform>(), IsDeleted = false };

        var route = new Route
        {
            Code = "V468",
            RouteTransits = new List<RouteTransit>
            {
                new()
                {
                    ArrivalTime = new TimeSpan(0, 31, 0),
                    DepartingTime = new TimeSpan(0, 40, 0),
                    Transit = new Transit { StartStation = startStation, EndStation = endStation, IsDeleted = false },
                    IsDeleted = false
                }
            },
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 12, 0, 0),
            IsDeleted = true
        };
        var startStationModel = new StationModel { Name = startStation.Name, Platforms = new List<PlatformModel>(), IsDeleted = startStation.IsDeleted };
        var endStationModel = new StationModel { Name = endStation.Name, Platforms = new List<PlatformModel>(), IsDeleted = endStation.IsDeleted };
        var routeModel = new RouteModel
        {
            Code = "V468",
            RouteTransits = new List<RouteTransitModel>
            {
                new()
                {
                    ArrivalTime = new TimeSpan(0, 31, 0),
                    DepartingTime = new TimeSpan(0, 40, 0),
                    Transit = new TransitModel
                        { StartStation = startStationModel, EndStation = endStationModel, IsDeleted = false },
                    IsDeleted = false
                }
            },
            StartStation = startStationModel,
            EndStation = endStationModel,
            StartTime = new DateTime(1970, 1, 1, 12, 0, 0),
            IsDeleted = true
        };

        yield return new TestCaseData(route, routeModel, Role.Admin);
        yield return new TestCaseData(route, routeModel, Role.TrainRouteManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        var startStation = new Station { Name = "Москва", Platforms = new List<Platform>() };
        var endStation = new Station { Name = "Санкт-Петербург", Platforms = new List<Platform>() };
        var transit = new Transit
        {
            StartStation = startStation,
            EndStation = endStation,
            IsDeleted = false
        };
        var transits = new List<RouteTransit>
        {
            new()
            {
                ArrivalTime = new TimeSpan(0, 30, 0), DepartingTime = new TimeSpan(0, 40, 0), Transit = transit,
                IsDeleted = false
            },
            new()
            {
                ArrivalTime = new TimeSpan(0, 40, 0), DepartingTime = new TimeSpan(0, 50, 0), Transit = transit,
                IsDeleted = false
            },
            new()
            {
                ArrivalTime = new TimeSpan(1, 20, 0), DepartingTime = new TimeSpan(1, 30, 0), Transit = transit,
                IsDeleted = false
            }
        };

        var fistList = new List<Route>
        {
            new()
            {
                Code = "V468",
                RouteTransits = transits,
                StartStation = startStation,
                EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "О875", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 14, 30, 0), IsDeleted = false
            },
            new()
            {
                Code = "Q784", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 5, 20, 0), IsDeleted = false
            },
            new()
            {
                Code = "Y554", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = false
            }
        };
        var secondList = new List<Route>
        {
            new()
            {
                Code = "V468", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "Y554", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = false
            }
        };

        yield return new TestCaseData(fistList, ConvertRoutesToRotesModels(fistList), Role.Admin);
        yield return new TestCaseData(secondList, ConvertRoutesToRotesModels(secondList), Role.TrainRouteManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListDeletedTest()
    {
        var startStation = new Station { Name = "Москва", Platforms = new List<Platform>() };
        var endStation = new Station { Name = "Санкт-Петербург", Platforms = new List<Platform>() };
        var transit = new Transit
        {
            StartStation = startStation,
            EndStation = endStation,
            IsDeleted = false
        };
        var transits = new List<RouteTransit>
        {
            new()
            {
                ArrivalTime = new TimeSpan(0, 30, 0), DepartingTime = new TimeSpan(0, 40, 0), Transit = transit,
                IsDeleted = false
            },
            new()
            {
                ArrivalTime = new TimeSpan(0, 40, 0), DepartingTime = new TimeSpan(0, 50, 0), Transit = transit,
                IsDeleted = false
            },
            new()
            {
                ArrivalTime = new TimeSpan(1, 20, 0), DepartingTime = new TimeSpan(1, 30, 0), Transit = transit,
                IsDeleted = false
            }
        };

        var fistList = new List<Route>
        {
            new()
            {
                Code = "V468", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "О875", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 14, 30, 0), IsDeleted = true
            },
            new()
            {
                Code = "Q784", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 5, 20, 0), IsDeleted = true
            },
            new()
            {
                Code = "Y554", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = true
            }
        };
        var secondList = new List<Route>
        {
            new()
            {
                Code = "V468", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "Y554", RouteTransits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = false
            }
        };

        yield return new TestCaseData(fistList, ConvertRoutesToRotesModels(fistList, false));
        yield return new TestCaseData(secondList, ConvertRoutesToRotesModels(secondList, false));
    }


    private static List<RouteModel> ConvertRoutesToRotesModels(List<Route> routes, bool includeAll = true)
    {
        return routes.Where(r => includeAll || r.IsDeleted)
                     .Select(route => new RouteModel
                     {
                         Code = route.Code,
                         RouteTransits = route.RouteTransits
                                         .Select(rt => new RouteTransitModel
                                         {
                                             DepartingTime = rt.DepartingTime, ArrivalTime = rt.ArrivalTime,
                                             Transit = new TransitModel
                                             {
                                                 StartStation = new StationModel
                                                 {
                                                     Name = rt.Transit.StartStation.Name,
                                                     Platforms = new List<PlatformModel>()
                                                 },
                                                 EndStation = new StationModel
                                                 {
                                                     Name = rt.Transit.EndStation.Name,
                                                     Platforms = new List<PlatformModel>()
                                                 },
                                                 IsDeleted = rt.Transit.IsDeleted
                                             },
                                             IsDeleted = rt.IsDeleted
                                         })
                                         .ToList(),
                         StartStation = new StationModel
                             { Name = route.StartStation.Name, Platforms = new List<PlatformModel>() },
                         EndStation = new StationModel
                             { Name = route.EndStation.Name, Platforms = new List<PlatformModel>() },
                         StartTime = route.StartTime,
                         IsDeleted = route.IsDeleted
                     })
                     .ToList();
    }
}