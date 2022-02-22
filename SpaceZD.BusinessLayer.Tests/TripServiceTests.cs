using System;
using System.Collections.Generic;
using System.IO;
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

public class TripServiceTests
{
    private Mock<IRepositorySoftDelete<Trip>> _tripRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private Mock<IRepositorySoftDelete<Train>> _trainRepositoryMock;
    private Mock<IRepositorySoftDelete<Route>> _routeRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private readonly IMapper _mapper;
    private ITripService _service;

    public TripServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _tripRepositoryMock = new Mock<IRepositorySoftDelete<Trip>>();
        _trainRepositoryMock = new Mock<IRepositorySoftDelete<Train>>();
        _routeRepositoryMock = new Mock<IRepositorySoftDelete<Route>>();
        _stationRepositoryMock = new Mock<IStationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new TripService(_mapper,
            _userRepositoryMock.Object,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);
    }


    //Add
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForAddTest))]
    public void AddTest(TripModel tripModel, Trip expected, Role role)
    {
        // given
        _tripRepositoryMock.Setup(x => x.Add(It.IsAny<Trip>())).Returns(45);
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(expected.Route);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(expected.Train);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        int actual = _service.Add(45, tripModel);

        // then
        _tripRepositoryMock.Verify(s => s.Add(expected), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        Assert.AreEqual(45, actual);
    }

    [Test]
    public void AddNegativeTrainNullTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Route());
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Train?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(45, new TripModel { Route = new RouteModel { Id = 10 }, Train = new TrainModel { Id = 10 } }));
    }

    [Test]
    public void AddNegativeRouteNullTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(45, new TripModel { Route = new RouteModel { Id = 10 }, Train = new TrainModel { Id = 10 } }));
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForAddNegativeInvalidDataExceptionTest))]
    public void AddNegativeInvalidDataExceptionTest(Trip trip)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip.Route);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip.Train);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<InvalidDataException>(() => _service.Add(45, new TripModel { Route = new RouteModel { Id = 10 }, Train = new TrainModel { Id = 10 } }));
    }

    [Test]
    public void AddNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(10, new TripModel()));
    }

    [Test]
    public void AddNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Add(10, new TripModel()));
    }


    // GetById
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(Trip trip, TripModel expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);

        // when
        var actual = _service.GetById(5);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10));
    }


    // GetList
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(List<Trip> trips, List<TripModel> expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetList(false)).Returns(trips);

        // when
        var actual = _service.GetList();

        // then
        _tripRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<Trip> trips, List<TripModel> expected, Role role)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetList(true)).Returns(trips);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        var actual = _service.GetListDeleted(45);

        // then
        _tripRepositoryMock.Verify(s => s.GetList(true), Times.Once);
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
        var trips = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), true));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trips);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Delete(45, 45);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trips, true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), true));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
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
        var trips = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), false));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trips);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        _service.Restore(45, 45);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trips, false), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), false));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
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
        var trip = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), It.IsAny<Trip>()));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Update(45, 45, new TripModel { Train = new TrainModel { Id = 10 } });

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _trainRepositoryMock.Verify(s => s.GetById(10), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trip, It.IsAny<Trip>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), It.IsAny<Trip>()));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(45, 10, new TripModel()));
    }

    [Test]
    public void UpdateNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new TripModel()));
    }

    [Test]
    public void UpdateNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new TripModel()));
    }


    // GetFreeSeat
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetFreeSeatTest))]
    public void GetFreeSeatTest(Trip trip, Station startStation, Station endStation, List<CarriageSeatsModel> expected)
    {

        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _stationRepositoryMock.Setup(x => x.GetById(1)).Returns(startStation);
        _stationRepositoryMock.Setup(x => x.GetById(2)).Returns(endStation);

        // when
        var actual = _service.GetFreeSeat(45, 1, 2);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(1), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetById(2), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetFreeSeatNegativeTripNullTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetFreeSeat(45, 1, 2));
    }

    [Test]
    public void GetFreeSeatNegativeStationNullTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Trip { Stations = new List<TripStation>() });
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetFreeSeat(45, 1, 2));
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetFreeSeatNegativeTest))]
    public void GetFreeSeatNegativeArgumentExceptionTest(Trip trip, Station station)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);

        // when then
        Assert.Throws<ArgumentException>(() => _service.GetFreeSeat(45, 1, 2));
    }
}