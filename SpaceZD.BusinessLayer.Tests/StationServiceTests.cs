using System;
using System.Collections.Generic;
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

public class StationServiceTests
{
    private Mock<IStationRepository> _stationRepositoryMock;
    private readonly IMapper _mapper;

    public StationServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _stationRepositoryMock = new Mock<IStationRepository>();
    }

    // Add
    [TestCase(45)]
    public void AddTest(int expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.Add(It.IsAny<Station>())).Returns(expected);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        int actual = service.Add(new StationModel());

        // then
        _stationRepositoryMock.Verify(s => s.Add(It.IsAny<Station>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }


    //GetNearStations
    [Test]
    public void GetNearStationsTest()
    {
        // given
        var station = new Station();
        var stations = new List<Station> { new() { Name = "Владивосток", Platforms = new List<Platform>() } };
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        _stationRepositoryMock.Setup(x => x.GetNearStations(It.IsAny<Station>())).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetNearStations(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetNearStations(station), Times.Once);
        CollectionAssert.AreEqual(new List<StationModel> { new() { Name = "Владивосток", Platforms = new List<PlatformModel>() } }, actual);
    }


    // GetById
    [Test]
    public void GetByIdTest()
    {
        // given
        var station = new Station { Name = "Владивосток", Platforms = new List<Platform>(), IsDeleted = true };
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        StationModel actual = service.GetById(5);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(new StationModel { Name = station.Name, Platforms = new List<PlatformModel>(), IsDeleted = station.IsDeleted }, actual);
    }
    [Test]
    public void GetByIdNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    // GetList
    [TestCaseSource(typeof(StationServiceMocks), nameof(StationServiceMocks.GetMockFromGetListTest))]
    public void GetListTest(List<Station> stations, List<StationModel> expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetList(false)).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetList();

        // then
        _stationRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    [TestCaseSource(typeof(StationServiceMocks), nameof(StationServiceMocks.GetMockFromGetListDeletedTest))]
    public void GetListDeletedTest(List<Station> stations, List<StationModel> expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetList(true)).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetListDeleted();

        // then
        _stationRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }


    //ReadyPlatforms
    [Test]
    public void GetReadyPlatformsByStationIdTest()
    {
        // given
        var moment = new DateTime(2022, 1, 1);
        var station = new Station { Name = "Санкт-Петербург" };
        var platforms = new List<Platform> { new() { Number = 2, Station = station } };
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        _stationRepositoryMock.Setup(x => x.GetReadyPlatformsStation(station, moment)).Returns(platforms);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetReadyPlatformsByStationId(20, moment);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(20), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetReadyPlatformsStation(station, moment), Times.Once);
        CollectionAssert.AreEqual(new List<PlatformModel> { new() { Number = 2, Station = new StationModel { Name = station.Name } } }, actual);
    }
    [Test]
    public void GetReadyPlatformsByStationIdNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetReadyPlatformsByStationId(10, DateTime.Today));
    }


    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        var stations = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), true));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        service.Delete(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(stations, true), Times.Once);
    }
    [Test]
    public void DeleteNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), true));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var stations = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), false));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        service.Restore(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(stations, false), Times.Once);
    }
    [Test]
    public void RestoreNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), false));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var station = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), It.IsAny<Station>()));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        service.Update(45, new StationModel());

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(station, It.IsAny<Station>()), Times.Once);
    }
    [Test]
    public void UpdateNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), It.IsAny<Station>()));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Update(10, new StationModel()));
    }
}