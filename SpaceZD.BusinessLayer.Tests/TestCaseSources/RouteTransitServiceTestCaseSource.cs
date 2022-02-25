using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal static class RouteTransitServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetRouteTransit(), ConvertRouteTransitToRouteTransitModels(GetRouteTransit()), Role.Admin);
            yield return new TestCaseData(GetRouteTransit(), ConvertRouteTransitToRouteTransitModels(GetRouteTransit()), Role.TrainRouteManager);
        }
        public static IEnumerable<TestCaseData> GetListDeletdTestCases()
        {
            yield return new TestCaseData(GetRouteTransit(), ConvertRouteTransitToRouteTransitModels(GetRouteTransitDelted()), Role.Admin);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var routeTransit = GetRouteTransit();
            yield return new TestCaseData(routeTransit[0], ConvertRouteTransitToRouteTransitModels(GetRouteTransit())[0]);
            yield return new TestCaseData(routeTransit[1], ConvertRouteTransitToRouteTransitModels(GetRouteTransit())[1]);
        }

        private static List<RouteTransit> GetRouteTransit() => new List<RouteTransit>
        {
                new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                        EndStation=new Station{Name="Москва", Platforms = new List<Platform>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(10,0,0),
                    Route=new Route{Code="1cc", RouteTransits=new List<RouteTransit>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1), StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                                    EndStation=new Station{Name="Москва", Platforms = new List<Platform>(), IsDeleted=false},IsDeleted=false}
                },
                new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                        EndStation=new Station{Name="Пермь", Platforms = new List<Platform>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(24,0,0),
                    Route=new Route{Code="1cc", RouteTransits=new List<RouteTransit>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1), StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                                    EndStation=new Station{Name="Москва", Platforms = new List<Platform>(), IsDeleted=false},IsDeleted=false}
                },
                new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                        EndStation=new Station{Name="Выборг", Platforms = new List<Platform>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(1,20,0),
                    Route=new Route{Code="1cc", RouteTransits=new List<RouteTransit>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1), StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                                    EndStation=new Station{Name="Москва", Platforms = new List<Platform>(), IsDeleted=false},IsDeleted=false},
                    IsDeleted=true
                }
        };

        private static List<RouteTransit> GetRouteTransitDelted() => new List<RouteTransit>
        {
            new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                        EndStation=new Station{Name="Выборг", Platforms = new List<Platform>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(1,20,0),
                    Route=new Route{Code="1cc", RouteTransits=new List<RouteTransit>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1), StartStation=new Station{Name="СПБ", Platforms = new List<Platform>(), IsDeleted=false},
                                    EndStation=new Station{Name="Москва", Platforms = new List<Platform>(), IsDeleted=false},IsDeleted=false},
                    IsDeleted=true
                }
        };

        private static List<RouteTransitModel> GetRouteTransitModel() => new List<RouteTransitModel>
        {
                new RouteTransitModel
                {
                    Transit=new TransitModel
                    {
                        StartStation=new StationModel{Name="СПБ", Platforms = new List<PlatformModel>(), IsDeleted=false},
                        EndStation=new StationModel{Name="Москва", Platforms = new List<PlatformModel>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(10,0,0),
                    Route=new RouteModel{Code="1cc", RouteTransits=new List<RouteTransitModel>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1),
                                        StartStation=new StationModel{Name="СПБ", Platforms = new List<PlatformModel>(), IsDeleted=false},
                                        EndStation=new StationModel{Name="Москва", Platforms = new List<PlatformModel>(), IsDeleted=false},IsDeleted=false}
                },
                new RouteTransitModel
                {
                    Transit=new TransitModel
                    {
                        StartStation=new StationModel{Name="СПБ", Platforms = new List<PlatformModel>(), IsDeleted=false},
                        EndStation=new StationModel{Name="Пермь", Platforms = new List<PlatformModel>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(24,0,0),
                    Route=new RouteModel{Code="1cc", RouteTransits=new List<RouteTransitModel>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1),
                                        StartStation=new StationModel{Name="СПБ", Platforms = new List<PlatformModel>(), IsDeleted=false},
                                        EndStation=new StationModel{Name="Москва", Platforms = new List<PlatformModel>(), IsDeleted=false},IsDeleted=false}
                },
                new RouteTransitModel
                {
                    Transit=new TransitModel
                    {
                        StartStation=new StationModel{Name="СПБ", Platforms = new List<PlatformModel>(), IsDeleted=false},
                        EndStation=new StationModel{Name="Выборг", Platforms = new List<PlatformModel>(), IsDeleted=false}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(1,20,0),
                    Route=new RouteModel{Code="1cc", RouteTransits=new List<RouteTransitModel>(),StartTime= new DateTime(2020, 1, 1, 1, 1,1),
                                        StartStation=new StationModel{Name="СПБ", Platforms = new List<PlatformModel>(), IsDeleted=false},
                                        EndStation=new StationModel{Name="Москва", Platforms = new List<PlatformModel>(), IsDeleted=false},IsDeleted=false},
                    IsDeleted=true
                }
        };

        private static List<RouteTransitModel> ConvertRouteTransitToRouteTransitModels(List<RouteTransit> routetransit, bool includeAll = true)
        {
            return routetransit
                .Where(pm => includeAll || pm.IsDeleted)
                .Select(routetransit => new RouteTransitModel
                {
                    Id = routetransit.Id,
                    Transit = new TransitModel
                    {
                        Id = routetransit.Transit.Id,
                        StartStation = new StationModel
                        {
                            Id = routetransit.Transit.StartStation.Id,
                            Name = routetransit.Transit.StartStation.Name,
                            Platforms = new List<PlatformModel>(),
                            IsDeleted = routetransit.Transit.StartStation.IsDeleted
                        },
                        EndStation = new StationModel
                        {
                            Id = routetransit.Transit.EndStation.Id,
                            Name = routetransit.Transit.EndStation.Name,
                            Platforms = new List<PlatformModel>(),
                            IsDeleted = routetransit.Transit.EndStation.IsDeleted
                        },
                        Price = routetransit.Transit.Price,
                        IsDeleted = routetransit.Transit.IsDeleted
                    },
                    DepartingTime = routetransit.DepartingTime,
                    ArrivalTime = routetransit.ArrivalTime,
                    Route = new RouteModel
                    {
                        Id = routetransit.Route.Id,
                        Code = routetransit.Route.Code,
                        RouteTransits = new List<RouteTransitModel>(),
                        StartTime = routetransit.Route.StartTime,
                        StartStation = new StationModel
                        {
                            Id = routetransit.Transit.StartStation.Id,
                            Name = routetransit.Transit.StartStation.Name,
                            Platforms = new List<PlatformModel>(),
                            IsDeleted = routetransit.Transit.StartStation.IsDeleted
                        },
                        EndStation = new StationModel
                        {
                            Id = routetransit.Transit.EndStation.Id,
                            Name = routetransit.Transit.EndStation.Name,
                            Platforms = new List<PlatformModel>(),
                            IsDeleted = routetransit.Transit.EndStation.IsDeleted
                        },
                        IsDeleted = routetransit.Route.IsDeleted

                    },
                    IsDeleted = routetransit.IsDeleted
                }).ToList();

        }
    }
}
