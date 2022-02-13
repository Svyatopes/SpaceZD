using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class RouteServiceTests
{
    private Mock<IRepositorySoftDeleteNewUpdate<Route>> _routeRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private readonly IMapper _mapper;

    public RouteServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _routeRepositoryMock = new Mock<IRepositorySoftDeleteNewUpdate<Route>>();
        _stationRepositoryMock = new Mock<IStationRepository>();
    }


    //Add
    [TestCase(45)]
    public void AddTest(int expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.Add(It.IsAny<Route>())).Returns(expected);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        int actual = service.Add(new RouteModel { StartStation = new StationModel { Id = 5 }, EndStation = new StationModel { Id = 6 } });

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetById(6), Times.Once);
        _routeRepositoryMock.Verify(s => s.Add(It.IsAny<Route>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }


    // GetById
    [TestCaseSource(nameof(GetRoute))]
    public void GetByIdTest(Route route)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        var expected = new RouteModel
        {
            Code = route.Code,
            Transits = new List<RouteTransitModel>(),
            StartStation = new StationModel { Name = route.StartStation.Name, Platforms = new List<PlatformModel>() },
            EndStation = new StationModel { Name = route.EndStation.Name, Platforms = new List<PlatformModel>() },
            StartTime = route.StartTime,
            IsDeleted = route.IsDeleted
        };
        foreach (var rt in route.Transits.Where(rt => !rt.IsDeleted))
            expected.Transits.Add(new RouteTransitModel
            {
                ArrivalTime = rt.ArrivalTime,
                DepartingTime = rt.DepartingTime,
                IsDeleted = rt.IsDeleted,
                Transit = new TransitModel
                {
                    StartStation = new StationModel { Name = rt.Transit.StartStation.Name, Platforms = new List<PlatformModel>() },
                    EndStation = new StationModel { Name = rt.Transit.EndStation.Name, Platforms = new List<PlatformModel>() },
                    IsDeleted = rt.Transit.IsDeleted
                }
            });

        // when
        var actual = service.GetById(5);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetRoute()
    {
        var startStation = new Station { Name = "Москва" };
        var endStation = new Station { Name = "Санкт-Петербург", Platforms = new List<Platform>() };
        var transit = new Transit { StartStation = startStation, EndStation = endStation, IsDeleted = false };
        var transits = new List<RouteTransit>
        {
            new() { ArrivalTime = new TimeSpan(0, 31, 0), DepartingTime = new TimeSpan(0, 40, 0), Transit = transit, IsDeleted = false },
            new() { ArrivalTime = new TimeSpan(0, 32, 0), DepartingTime = new TimeSpan(0, 50, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(0, 33, 0), DepartingTime = new TimeSpan(1, 0, 0), Transit = transit, IsDeleted = false },
            new() { ArrivalTime = new TimeSpan(0, 34, 0), DepartingTime = new TimeSpan(1, 30, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(0, 35, 0), DepartingTime = new TimeSpan(1, 40, 0), Transit = transit, IsDeleted = false }
        };

        yield return new TestCaseData(new Route
        {
            Code = "V468",
            Transits = transits,
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 12, 0, 0),
            IsDeleted = true
        });
        yield return new TestCaseData(new Route
        {
            Code = "О875",
            Transits = transits,
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 14, 30, 0),
            IsDeleted = false
        });
        yield return new TestCaseData(new Route
        {
            Code = "Г465",
            Transits = transits,
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 4, 4, 0),
            IsDeleted = false
        });
        yield return new TestCaseData(new Route
        {
            Code = "Q784",
            Transits = transits,
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 5, 20, 0),
            IsDeleted = false
        });
        yield return new TestCaseData(new Route
        {
            Code = "T982",
            Transits = transits,
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 13, 0, 50),
            IsDeleted = false
        });
        yield return new TestCaseData(new Route
        {
            Code = "Y554",
            Transits = transits,
            StartStation = startStation,
            EndStation = endStation,
            StartTime = new DateTime(1970, 1, 1, 8, 0, 10),
            IsDeleted = true
        });
    }
    [Test]
    public void GetByIdNegativeTest()
    {
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    // GetList
    [TestCaseSource(nameof(GetListRouteNotDeleted))]
    public void GetListTest(List<Route> routes)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(false)).Returns(routes);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        var expected = routes.Select(route => new RouteModel
                             {
                                 Code = route.Code,
                                 Transits = route.Transits
                                                 .Where(t => !t.IsDeleted)
                                                 .Select(rt => new RouteTransitModel
                                                 {
                                                     DepartingTime = rt.DepartingTime, ArrivalTime = rt.ArrivalTime,
                                                     Transit = new TransitModel
                                                     {
                                                         StartStation = new StationModel
                                                             { Name = rt.Transit.StartStation.Name, Platforms = new List<PlatformModel>() },
                                                         EndStation = new StationModel
                                                             { Name = rt.Transit.EndStation.Name, Platforms = new List<PlatformModel>() },
                                                         IsDeleted = rt.Transit.IsDeleted
                                                     },
                                                     IsDeleted = rt.IsDeleted
                                                 })
                                                 .ToList(),
                                 StartStation = new StationModel { Name = route.StartStation.Name, Platforms = new List<PlatformModel>() },
                                 EndStation = new StationModel { Name = route.EndStation.Name, Platforms = new List<PlatformModel>() },
                                 StartTime = route.StartTime,
                                 IsDeleted = route.IsDeleted
                             })
                             .ToList();

        // when
        var actual = service.GetList();

        // then
        _routeRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListRouteNotDeleted()
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
            new() { ArrivalTime = new TimeSpan(0, 30, 0), DepartingTime = new TimeSpan(0, 40, 0), Transit = transit, IsDeleted = false },
            new() { ArrivalTime = new TimeSpan(0, 40, 0), DepartingTime = new TimeSpan(0, 50, 0), Transit = transit, IsDeleted = false },
            new() { ArrivalTime = new TimeSpan(0, 50, 0), DepartingTime = new TimeSpan(1, 0, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(1, 0, 0), DepartingTime = new TimeSpan(1, 10, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(1, 10, 0), DepartingTime = new TimeSpan(1, 20, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(1, 20, 0), DepartingTime = new TimeSpan(1, 30, 0), Transit = transit, IsDeleted = false }
        };

        yield return new TestCaseData(new List<Route>
        {
            new()
            {
                Code = "V468", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "О875", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 14, 30, 0), IsDeleted = false
            },
            new()
            {
                Code = "Q784", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 5, 20, 0), IsDeleted = false
            },
            new()
            {
                Code = "Y554", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = false
            }
        });
        yield return new TestCaseData(new List<Route>
        {
            new()
            {
                Code = "V468", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "Y554", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = false
            }
        });
    }
    [TestCaseSource(nameof(GetListRouteDeleted))]
    public void GetListDeletedTest(List<Route> routes)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(true)).Returns(routes);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);
        var expected = routes.Where(t => t.IsDeleted).Select(route => new RouteModel
                             {
                                 Code = route.Code,
                                 Transits = route.Transits
                                                 .Where(t => !t.IsDeleted)
                                                 .Select(rt => new RouteTransitModel
                                                 {
                                                     DepartingTime = rt.DepartingTime, ArrivalTime = rt.ArrivalTime,
                                                     Transit = new TransitModel
                                                     {
                                                         StartStation = new StationModel
                                                             { Name = rt.Transit.StartStation.Name, Platforms = new List<PlatformModel>() },
                                                         EndStation = new StationModel
                                                             { Name = rt.Transit.EndStation.Name, Platforms = new List<PlatformModel>() },
                                                         IsDeleted = rt.Transit.IsDeleted
                                                     },
                                                     IsDeleted = rt.IsDeleted
                                                 })
                                                 .ToList(),
                                 StartStation = new StationModel { Name = route.StartStation.Name, Platforms = new List<PlatformModel>() },
                                 EndStation = new StationModel { Name = route.EndStation.Name, Platforms = new List<PlatformModel>() },
                                 StartTime = route.StartTime,
                                 IsDeleted = route.IsDeleted
                             })
                             .ToList();

        // when
        var actual = service.GetListDeleted();

        // then
        _routeRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListRouteDeleted()
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
            new() { ArrivalTime = new TimeSpan(0, 30, 0), DepartingTime = new TimeSpan(0, 40, 0), Transit = transit, IsDeleted = false },
            new() { ArrivalTime = new TimeSpan(0, 40, 0), DepartingTime = new TimeSpan(0, 50, 0), Transit = transit, IsDeleted = false },
            new() { ArrivalTime = new TimeSpan(0, 50, 0), DepartingTime = new TimeSpan(1, 0, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(1, 0, 0), DepartingTime = new TimeSpan(1, 10, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(1, 10, 0), DepartingTime = new TimeSpan(1, 20, 0), Transit = transit, IsDeleted = true },
            new() { ArrivalTime = new TimeSpan(1, 20, 0), DepartingTime = new TimeSpan(1, 30, 0), Transit = transit, IsDeleted = false }
        };

        yield return new TestCaseData(new List<Route>
        {
            new()
            {
                Code = "V468", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "T982", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 13, 0, 50), IsDeleted = true
            },
            new()
            {
                Code = "Y554", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 8, 0, 10), IsDeleted = false
            }
        });
        yield return new TestCaseData(new List<Route>
        {
            new()
            {
                Code = "V468", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 12, 0, 0), IsDeleted = false
            },
            new()
            {
                Code = "О875", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 14, 30, 0), IsDeleted = true
            },
            new()
            {
                Code = "Г465", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 4, 4, 0), IsDeleted = false
            },
            new()
            {
                Code = "Q784", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 5, 20, 0), IsDeleted = true
            },
            new()
            {
                Code = "T982", Transits = transits, StartStation = startStation, EndStation = endStation,
                StartTime = new DateTime(1970, 1, 1, 13, 0, 50), IsDeleted = true
            }
        });
    }


    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), true));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        service.Delete(45);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, true), Times.Once);
    }
    [Test]
    public void DeleteNegativeTest()
    {
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), true));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), false));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        service.Restore(45);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, false), Times.Once);
    }
    [Test]
    public void RestoreNegativeTest()
    {
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), false));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), It.IsAny<Route>()));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        service.Update(45, new RouteModel());

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, It.IsAny<Route>()), Times.Once);
    }
    [Test]
    public void UpdateNegativeTest()
    {
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), It.IsAny<Route>()));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Update(10, new RouteModel()));
    }
}