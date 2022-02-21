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
using SpaceZD.DataLayer.Enums;
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
    [TestCase(45, Role.Admin)]
    [TestCase(45, Role.TrainRouteManager)]
    public void AddTest(int expected, Role role)
    {
        // given
        _routeRepositoryMock.Setup(x => x.Add(It.IsAny<Route>())).Returns(expected);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        int actual = _service.Add(45, new RouteModel { StartStation = new StationModel { Id = 5 }, EndStation = new StationModel { Id = 6 } });

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetById(6), Times.Once);
        _routeRepositoryMock.Verify(s => s.Add(It.IsAny<Route>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void AddNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(10, new RouteModel()));
    }

    [Test]
    public void AddNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Add(10, new RouteModel()));
    }

    // GetById
    [TestCaseSource(typeof(RouteServiceTestCaseSource), nameof(RouteServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(Route route, RouteModel expected, Role role)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        var actual = _service.GetById(45, 5);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(45, 10));
    }

    [Test]
    public void GetByIdNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10, 10));
    }

    [Test]
    public void GetByIdNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetById(10, 10));
    }


    // GetList
    [TestCaseSource(typeof(RouteServiceTestCaseSource), nameof(RouteServiceTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(List<Route> routes, List<RouteModel> expected, Role role)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(false)).Returns(routes);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        var actual = _service.GetList(45);

        // then
        _routeRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetListNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetList(10));
    }

    [Test]
    public void GetListNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetList(10));
    }

    [TestCaseSource(typeof(RouteServiceTestCaseSource), nameof(RouteServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<Route> routes, List<RouteModel> expected)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetList(true)).Returns(routes);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        var actual = _service.GetListDeleted(45);

        // then
        _routeRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetListDeletedNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetListDeleted(10));
    }

    [Test]
    public void GetListDeletedNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.TrainRouteManager });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(10));
    }


    //Delete
    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void DeleteTest(Role role)
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), true));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Delete(45, 45);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), true));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(45, 10));
    }

    [Test]
    public void DeleteNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(10, 10));
    }

    [Test]
    public void DeleteNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Delete(10, 10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), false));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        _service.Restore(45, 45);

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, false), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), false));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(45, 10));
    }

    [Test]
    public void RestoreNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(10, 10));
    }

    [Test]
    public void RestoreNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.TrainRouteManager });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Restore(10, 10));
    }


    //Update
    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void UpdateTest(Role role)
    {
        // given
        var route = new Route();
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), It.IsAny<Route>()));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(route);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Update(45, 45, new RouteModel { StartStation = new StationModel { Id = 1 }, EndStation = new StationModel { Id = 2 } });

        // then
        _routeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _routeRepositoryMock.Verify(s => s.Update(route, It.IsAny<Route>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.Update(It.IsAny<Route>(), It.IsAny<Route>()));
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(45, 10, new RouteModel()));
    }

    [Test]
    public void UpdateNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new RouteModel()));
    }

    [Test]
    public void UpdateNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new RouteModel()));
    }
}