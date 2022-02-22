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

public class StationServiceTests
{
    private Mock<IStationRepository> _stationRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
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
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new StationService(_mapper, _userRepositoryMock.Object, _stationRepositoryMock.Object);
    }

    // Add
    [TestCase(45, Role.Admin)]
    [TestCase(45, Role.StationManager)]
    public void AddTest(int expected, Role role)
    {
        // given
        _stationRepositoryMock.Setup(x => x.Add(It.IsAny<Station>())).Returns(expected);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        int actual = _service.Add(45, new StationModel());

        // then
        _stationRepositoryMock.Verify(s => s.Add(It.IsAny<Station>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void AddNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(10, new StationModel()));
    }

    [Test]
    public void AddNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Add(10, new StationModel()));
    }


    //GetNearStations
    [TestCase(Role.Admin)]
    [TestCase(Role.StationManager)]
    public void GetNearStationsTest(Role role)
    {
        // given
        var station = new Station();
        var stations = new List<Station> { new() { Name = "Владивосток", Platforms = new List<Platform>() } };
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        _stationRepositoryMock.Setup(x => x.GetNearStations(It.IsAny<Station>())).Returns(stations);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        var actual = _service.GetNearStations(45, 45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetNearStations(station), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(
            new List<StationModel> { new() { Name = "Владивосток", Platforms = new List<PlatformModel>() } },
            actual);
    }

    [Test]
    public void GetNearStationsNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetNearStations(10, 10));
    }

    [Test]
    public void GetNearStationsNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetNearStations(10, 10));
    }


    // GetById
    [TestCase(Role.Admin)]
    [TestCase(Role.StationManager)]
    public void GetByIdTest(Role role)
    {
        // given
        var station = new Station { Name = "Владивосток", Platforms = new List<Platform>(), IsDeleted = true };
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        StationModel actual = _service.GetById(45, 5);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
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
    [TestCaseSource(typeof(StationServiceTestCaseSource), nameof(StationServiceTestCaseSource.GetTestCaseDataForGetListTest))]
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

    [TestCaseSource(typeof(StationServiceTestCaseSource), nameof(StationServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<Station> stations, List<StationModel> expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetList(true)).Returns(stations);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        var actual = _service.GetListDeleted(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetList(true), Times.Once);
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
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.StationManager });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(10));
    }


    //Delete
    [TestCase(Role.Admin)]
    [TestCase(Role.StationManager)]
    public void DeleteTest(Role role)
    {
        // given
        var stations = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), true));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(stations);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Delete(45, 45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(stations, true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), true));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
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
        var stations = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), false));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(stations);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        _service.Restore(45, 45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(stations, false), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), false));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
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
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.StationManager });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Restore(10, 10));
    }


    //Update
    [TestCase(Role.Admin)]
    [TestCase(Role.StationManager)]
    public void UpdateTest(Role role)
    {
        // given
        var station = new Station();
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), It.IsAny<Station>()));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Update(45, 45, new StationModel());

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _stationRepositoryMock.Verify(s => s.Update(station, It.IsAny<Station>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>(), It.IsAny<Station>()));
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(45, 10, new StationModel()));
    }

    [Test]
    public void UpdateNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new StationModel()));
    }

    [Test]
    public void UpdateNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new StationModel()));
    }
}