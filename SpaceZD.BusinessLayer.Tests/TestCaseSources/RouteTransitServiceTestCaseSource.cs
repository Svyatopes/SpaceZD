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
            var platformsMaintenance = GetRouteTransit();
            var platformsMaintenanceModels = GetRouteTransitModel();
            yield return new TestCaseData(platformsMaintenance[0], platformsMaintenanceModels[0]);
            yield return new TestCaseData(platformsMaintenance[1], platformsMaintenanceModels[1]);
        }

        private static List<RouteTransit> GetRouteTransit() => new List<RouteTransit>
        {
                new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ"},
                        EndStation=new Station{Name="Москва"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(10,0,0),
                },
                new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ"},
                        EndStation=new Station{Name="Пермь"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(24,0,0),
                },
                new RouteTransit
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ"},
                        EndStation=new Station{Name="Выборг"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(1,20,0),
                    IsDeleted=true
                }
        };

        private static List<RouteTransitModel> GetRouteTransitModel() => new List<RouteTransitModel>
        {
                new RouteTransitModel
                {
                    Transit=new TransitModel
                    {
                        StartStation=new StationModel{Name="СПБ"},
                        EndStation=new StationModel{Name="Москва"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(10,0,0),
                },
                new RouteTransitModel
                {
                    Transit=new TransitModel
                    {
                        StartStation=new StationModel{Name="СПБ"},
                        EndStation=new StationModel{Name="Пермь"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(24,0,0),
                },
                new RouteTransitModel
                {
                    Transit=new TransitModel
                    {
                        StartStation=new StationModel{Name="СПБ"},
                        EndStation=new StationModel{Name="Выборг"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(1,20,0),
                    IsDeleted=true
                }
        };
    }
}
