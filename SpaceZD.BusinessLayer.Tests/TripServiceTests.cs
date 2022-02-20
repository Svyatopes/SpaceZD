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
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class TripServiceTests
{
    private Mock<IRepositorySoftDelete<Trip>> _tripRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private Mock<IRepositorySoftDelete<Train>> _trainRepositoryMock;
    private Mock<IRepositorySoftDelete<Route>> _routeRepositoryMock;
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
        _service = new TripService(_mapper, _tripRepositoryMock.Object, _stationRepositoryMock.Object, _routeRepositoryMock.Object, _trainRepositoryMock.Object);
    }


    //Add
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForAddTest))]
    public void AddTest(TripModel tripModel, Trip expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.Add(It.IsAny<Trip>())).Returns(45);
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(expected.Route);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(expected.Train);

        // when
        int actual = _service.Add(tripModel);

        // then
        _tripRepositoryMock.Verify(s => s.Add(expected), Times.Once);
        Assert.AreEqual(45, actual);
    }

    [Test]
    public void AddNegativeTrainNullTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Route());
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Train?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(new TripModel { Route = new RouteModel { Id = 10 }, Train = new TrainModel { Id = 10 } }));
    }

    [Test]
    public void AddNegativeRouteNullTest()
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Route?)null);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(new TripModel { Route = new RouteModel { Id = 10 }, Train = new TrainModel { Id = 10 } }));
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForAddNegativeInvalidDataExceptionTest))]
    public void AddNegativeInvalidDataExceptionTest(Trip trip)
    {
        // given
        _routeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip.Route);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip.Train);

        // when then
        Assert.Throws<InvalidDataException>(() => _service.Add(new TripModel { Route = new RouteModel { Id = 10 }, Train = new TrainModel { Id = 10 } }));
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
    public void GetListDeletedTest(List<Trip> trips, List<TripModel> expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetList(true)).Returns(trips);

        // when
        var actual = _service.GetListDeleted();

        // then
        _tripRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }


    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        var trips = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), true));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trips);

        // when
        _service.Delete(45);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trips, true), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), true));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var trips = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), false));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trips);

        // when
        _service.Restore(45);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trips, false), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), false));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var trip = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), It.IsAny<Trip>()));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());

        // when
        _service.Update(45, new TripModel { Train = new TrainModel { Id = 10 } });

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _trainRepositoryMock.Verify(s => s.GetById(10), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trip, It.IsAny<Trip>()), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), It.IsAny<Trip>()));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, new TripModel()));
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
        var actual = _service.GetFreeSeat(45, 1, 2, false);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(1), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetById(2), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetOnlyFreeSeatTest))]
    public void GetOnlyFreeSeatTest(Trip trip, Station startStation, Station endStation, List<CarriageSeatsModel> expected)
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