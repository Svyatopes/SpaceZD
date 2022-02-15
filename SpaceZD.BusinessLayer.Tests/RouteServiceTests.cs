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
using SpaceZD.BusinessLayer.Tests.TestMocks;
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
    [TestCaseSource(typeof(RouteServiceMocks), nameof(RouteServiceMocks.GetMockFromGetByIdTest))]
    public void GetByIdTest(Route route, RouteModel expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public void GetByIdNegativeTest()
    {
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    // GetList
    [TestCaseSource(typeof(RouteServiceMocks), nameof(RouteServiceMocks.GetMockFromGetListTest))]
    public void GetListTest(List<Route> routes, List<RouteModel> expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(false)).Returns(routes);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        var actual = service.GetList();

        // then
        _routeRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    [TestCaseSource(typeof(RouteServiceMocks), nameof(RouteServiceMocks.GetMockFromGetListDeletedTest))]
    public void GetListDeletedTest(List<Route> routes, List<RouteModel> expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(true)).Returns(routes);
        var service = new RouteService(_mapper, _routeRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        var actual = service.GetListDeleted();

        // then
        _routeRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
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