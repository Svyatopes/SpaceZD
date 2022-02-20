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
    }


    // GetById
    [TestCaseSource(typeof(TripStationServiceTestCaseSource),
        nameof(TripStationServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(TripStation tripStation, TripStationModel expected)
    {
        // given
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(tripStation);
        var service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object,
            _platformRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((TripStation?)null);
        var service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object,
            _platformRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    //Update
    [TestCaseSource(typeof(TripStationServiceTestCaseSource),
        nameof(TripStationServiceTestCaseSource.GetTestCaseDataForUpdateTest))]
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
        var service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object,
            _platformRepositoryMock.Object);

        // when
        service.Update(45, model);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.Update(tripStation, It.IsAny<TripStation>()), Times.Once);
        Assert.AreEqual(model, expected);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        _tripStationRepositoryMock.Setup(x => x.Update(It.IsAny<TripStation>(), It.IsAny<TripStation>()));
        _tripStationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((TripStation?)null);
        var service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object,
            _platformRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Update(10, new TripStationModel()));
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
        var service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object,
            _platformRepositoryMock.Object);

        // when then
        Assert.Throws<InvalidOperationException>(() => service.Update(10,
            new TripStationModel
            {
                ArrivalTime = new DateTime(12, 1, 22), DepartingTime = new DateTime(12, 1, 22),
                Platform = new PlatformModel { Id = 1 }
            }));
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
        var service = new TripStationService(_mapper, _tripStationRepositoryMock.Object, _stationRepositoryMock.Object,
            _platformRepositoryMock.Object);

        // when
        var actual = service.GetReadyPlatforms(45);

        // then
        _tripStationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
}