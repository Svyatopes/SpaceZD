using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal static class RouteTransitServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetRouteTransit(), GetRouteTransitModel(), false);
            yield return new TestCaseData(GetRouteTransit(), GetRouteTransitModel(), true);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var routeTransit = GetRouteTransit();
            var routeTransitModels = GetRouteTransitModel();
            yield return new TestCaseData(routeTransit[0], routeTransitModels[0]);
            yield return new TestCaseData(routeTransit[1], routeTransitModels[1]);
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
    }
}
