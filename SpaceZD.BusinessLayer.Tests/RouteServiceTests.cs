using System.Collections.Generic;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.BusinessLayer.Tests.TestCaseSources;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class RouteServiceTests
{
    private Mock<IRepositorySoftDelete<Route>> _routeRepositoryMock;
    private Mock<IRepositorySoftDelete<User>> _userRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private IRouteService _service;
    private readonly IMapper _mapper;

    public RouteServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _routeRepositoryMock = new Mock<IRepositorySoftDelete<Route>>();
        _stationRepositoryMock = new Mock<IStationRepository>();
        _userRepositoryMock = new Mock<IRepositorySoftDelete<User>>();
        _service = new RouteService(_mapper, _userRepositoryMock.Object, _routeRepositoryMock.Object, _stationRepositoryMock.Object);
    }


    //Add
    [TestCase(45)]
    public void AddTest(int expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.Add(It.IsAny<Route>())).Returns(expected);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());

        // when
        int actual = _service.Add(new RouteModel { StartStation = new StationModel { Id = 5 }, EndStation = new StationModel { Id = 6 } });

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetById(6), Times.Once);
        _routeRepositoryMock.Verify(s => s.Add(It.IsAny<Route>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }


    // GetById
    [TestCaseSource(typeof(RouteServiceTestCaseSource),
        nameof(RouteServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(Route route, RouteModel expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);

        // when
        var actual = _service.GetById(5);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10));
    }


    // GetList
    [TestCaseSource(typeof(RouteServiceTestCaseSource), nameof(RouteServiceTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(List<Route> routes, List<RouteModel> expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(false)).Returns(routes);

        // when
        var actual = _service.GetList();

        // then
        _routeRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(RouteServiceTestCaseSource), nameof(RouteServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<Route> routes, List<RouteModel> expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(true)).Returns(routes);

        // when
        var actual = _service.GetListDeleted();

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

        // when
        _service.Delete(45);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, true), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), true));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), false));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);

        // when
        _service.Restore(45);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, false), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), false));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), It.IsAny<Route>()));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());

        // when
        _service.Update(45, new RouteModel { StartStation = new StationModel { Id = 1 }, EndStation = new StationModel { Id = 2 } });

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, It.IsAny<Route>()), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), It.IsAny<Route>()));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, new RouteModel()));
    }
}