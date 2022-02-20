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

public class TripStationServiceTests
{
    private Mock<ITripStationRepository> _tripStationRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private Mock<IRepositorySoftDelete<Platform>> _platformRepositoryMock;
    private ITripStationService _service;
    private readonly IMapper _mapper;

    public TripStationServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _tripStationRepositoryMock = new Mock<ITripStationRepository>();
        _stationRepositoryMock = new Mock<IStationRepository>();
        _platformRepositoryMock = new Mock<IRepositorySoftDelete<Platform>>();
        _service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object, _platformRepositoryMock.Object);
    }


    // GetById
    [TestCaseSource(typeof(TripStationServiceTestCaseSource), nameof(TripStationServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(TripStation tripStation, TripStationModel expected)
    {
        // given
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);

        // when
        var actual = _service.GetById(5);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((TripStation?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10));
    }


    //Update
    [TestCaseSource(typeof(TripStationServiceTestCaseSource), nameof(TripStationServiceTestCaseSource.GetTestCaseDataForUpdateTest))]
    public void UpdateTest(TripStationModel model, TripStationModel expected)
    {
        // given
        var tripStation = new TripStation { Station = new Station() };
        _tripStationRepositoryMock.Setup(x => x.Update(It.IsAny<TripStation>(), It.IsAny<TripStation>()));
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);
        _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Platform
            { Number = 2, Station = new Station { Name = "Москва" }, IsDeleted = false });
        _stationRepositoryMock
            .Setup(x => x.GetReadyPlatformsStation(It.IsAny<Station>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(new List<Platform> { new() { Id = 1 } });

        // when
        _service.Update(45, model);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.Update(tripStation, It.IsAny<TripStation>()), Times.Once);
        Assert.AreEqual(model, expected);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _tripStationRepositoryMock.Setup(x => x.Update(It.IsAny<TripStation>(), It.IsAny<TripStation>()));
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((TripStation?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, new TripStationModel()));
    }

    [Test]
    public void UpdateNegativeInvalidOperationExceptionTest()
    {
        // given
        var tripStation = new TripStation { Station = new Station() };
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);
        _stationRepositoryMock
            .Setup(x => x.GetReadyPlatformsStation(It.IsAny<Station>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(new List<Platform>());

        // when then
        Assert.Throws<InvalidOperationException>(() => _service.Update(10,
            new TripStationModel
            {
                ArrivalTime = new DateTime(12, 1, 22), DepartingTime = new DateTime(12, 1, 22),
                Platform = new PlatformModel { Id = 1 }
            }));
    }


    //SetPlatform
    [Test]
    public void SetPlatformTest()
    {
        // given
        var tripStation = new TripStation { Station = new Station(), ArrivalTime = null, DepartingTime = new DateTime(1990, 1, 1) };
        _tripStationRepositoryMock.Setup(x => x.Update(It.IsAny<TripStation>(), It.IsAny<TripStation>()));
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);
        _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Platform
            { Number = 2, Station = new Station { Name = "Москва" }, IsDeleted = false });
        _stationRepositoryMock
           .Setup(x => x.GetReadyPlatformsStation(It.IsAny<Station>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
           .Returns(new List<Platform> { new() { Id = 1 } });

        // when
        _service.SetPlatform(45, 1);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.Update(tripStation, tripStation), Times.Once);
    }
    
    [Test]
    public void SetPlatformNegativeTest()
    {
        // given
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((TripStation?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.SetPlatform(10, 1));
    }
    
    [Test]
    public void SetPlatformNegativeInvalidOperationExceptionTest()
    {
        // given
        var tripStation = new TripStation { Station = new Station(), ArrivalTime = null, DepartingTime = new DateTime(1990, 1, 1) };
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);
        _stationRepositoryMock
           .Setup(x => x.GetReadyPlatformsStation(It.IsAny<Station>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
           .Returns(new List<Platform>());

        // when then
        Assert.Throws<InvalidOperationException>(() => _service.SetPlatform(10, 1));
    }


    //GetReadyPlatforms
    [TestCaseSource(typeof(TripStationServiceTestCaseSource),
        nameof(TripStationServiceTestCaseSource.GetTestCaseDataForGetReadyPlatformsTest))]
    public void GetReadyPlatformsTest(TripStation tripStation, List<Platform> platforms, List<PlatformModel> expected)
    {
        // given
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);
        _stationRepositoryMock
            .Setup(x => x.GetReadyPlatformsStation(It.IsAny<Station>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(platforms);

        // when
        var actual = _service.GetReadyPlatforms(45);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
}