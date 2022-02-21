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

public class StationServiceTests
{
    private Mock<IStationRepository> _stationRepositoryMock;
    private Mock<IRepositorySoftDelete<User>> _userRepositoryMock;
    private IStationService _service;
    private readonly IMapper _mapper;

    public StationServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _stationRepositoryMock = new Mock<IStationRepository>();
        _userRepositoryMock = new Mock<IRepositorySoftDelete<User>>();
        _service = new StationService(_mapper, _userRepositoryMock.Object, _stationRepositoryMock.Object);
    }

    // Add
    [TestCase(45)]
    public void AddTest(int expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.Add(It.IsAny<Station>())).Returns(expected);

        // when
        int actual = _service.Add(new StationModel());

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

        // when
        var actual = _service.GetNearStations(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetNearStations(station), Times.Once);
        CollectionAssert.AreEqual(
            new List<StationModel> { new() { Name = "Владивосток", Platforms = new List<PlatformModel>() } },
            actual);
    }


    // GetById
    [Test]
    public void GetByIdTest()
    {
        // given
        var station = new Station { Name = "Владивосток", Platforms = new List<Platform>(), IsDeleted = true };
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);

        // when
        StationModel actual = _service.GetById(5);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(
            new StationModel
                { Name = station.Name, Platforms = new List<PlatformModel>(), IsDeleted = station.IsDeleted },
            actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10));
    }


    // GetList
    [TestCaseSource(typeof(StationServiceTestCaseSource),
        nameof(StationServiceTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(List<Station> stations, List<StationModel> expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetList(false)).Returns(stations);

        // when
        var actual = _service.GetList();

        // then
        _stationRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(StationServiceTestCaseSource),
        nameof(StationServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<Station> stations, List<StationModel> expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetList(true)).Returns(stations);

        // when
        var actual = _service.GetListDeleted();

        // then
        _stationRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }


    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        var stations = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), true));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(stations);

        // when
        _service.Delete(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(stations, true), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), true));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var stations = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), false));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(stations);

        // when
        _service.Restore(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(stations, false), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), false));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var station = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), It.IsAny<Station>()));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);

        // when
        _service.Update(45, new StationModel());

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(station, It.IsAny<Station>()), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), It.IsAny<Station>()));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, new StationModel()));
    }
}