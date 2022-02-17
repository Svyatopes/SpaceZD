using System;
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

public class TripServiceTests
{
    private Mock<IRepositorySoftDelete<Trip>> _tripRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private Mock<IRepositorySoftDelete<Train>> _trainRepositoryMock;
    private Mock<IRepositorySoftDelete<Route>> _routeRepositoryMock;
    private readonly IMapper _mapper;

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
    }


    // GetById
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(Trip trip, TripModel expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    // GetList
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(List<Trip> trips, List<TripModel> expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetList(false)).Returns(trips);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        var actual = service.GetList();

        // then
        _tripRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<Trip> trips, List<TripModel> expected)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetList(true)).Returns(trips);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        var actual = service.GetListDeleted();

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
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        service.Delete(45);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trips, true), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), true));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var trips = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), false));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trips);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        service.Restore(45);

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trips, false), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), false));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var trip = new Trip();
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), It.IsAny<Trip>()));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        service.Update(45, new TripModel());

        // then
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripRepositoryMock.Verify(s => s.Update(trip, It.IsAny<Trip>()), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        _tripRepositoryMock.Setup(x => x.Update(It.IsAny<Trip>(), It.IsAny<Trip>()));
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Update(10, new TripModel()));
    }


    // GetFreeSeat
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetFreeSeatTest))]
    public void GetFreeSeatTest(Trip trip, Station startStation, Station endStation, List<CarriageSeatsModel> expected)
    {

        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _stationRepositoryMock.Setup(x => x.GetById(1)).Returns(startStation);
        _stationRepositoryMock.Setup(x => x.GetById(2)).Returns(endStation);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when
        var actual = service.GetFreeSeat(45, 1, 2);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(1), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetById(2), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCase(1)]
    [TestCase(2)]
    public void GetFreeSeatNegativeTest(int variant)
    {
        switch (variant)
        {
            case 1:
                _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
                break;
            case 2:
                _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
                _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
                break;
        }
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetFreeSeat(45, 1, 2));
    }

    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetFreeSeatNegativeTest))]
    public void GetFreeSeatNegativeArgumentExceptionTest(Trip trip, Station station)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        var service = new TripService(_mapper,
            _tripRepositoryMock.Object,
            _stationRepositoryMock.Object,
            _routeRepositoryMock.Object,
            _trainRepositoryMock.Object);

        // when then
        Assert.Throws<ArgumentException>(() => service.GetFreeSeat(45, 1, 2));
    }
}