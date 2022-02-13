using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
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
    [TestCaseSource(nameof(GetNearStations))]
    public void GetNearStationsTest(Station station, List<StationModel> expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetNearStations(45);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetNearStations()
    {
        var transitFirst = new Transit { EndStation = new Station { Name = "Москва" } };
        var transitSecond = new Transit { EndStation = new Station { Name = "Челябинск", IsDeleted = true } };
        var transitThird = new Transit { EndStation = new Station { Name = "Омск" } };
        var transitFourth = new Transit { EndStation = new Station { Name = "48 км" }, IsDeleted = true };
        var transitFifth = new Transit { EndStation = new Station { Name = "Выборг" } };


        var station = new Station
        {
            Name = "0 км", TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFourth },
            Platforms = new List<Platform>()
        };
        yield return new TestCaseData(station,
            new List<StationModel>
            {
                new() { Name = transitFirst.EndStation.Name, Platforms = new List<PlatformModel>() },
                new() { Name = transitThird.EndStation.Name, Platforms = new List<PlatformModel>() }
            });


        var station2 = new Station
        {
            Name = "10 км", TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFifth },
            Platforms = new List<Platform>()
        };
        yield return new TestCaseData(station2,
            new List<StationModel>
            {
                new() { Name = transitFirst.EndStation.Name, Platforms = new List<PlatformModel>() },
                new() { Name = transitThird.EndStation.Name, Platforms = new List<PlatformModel>() },
                new() { Name = transitFifth.EndStation.Name, Platforms = new List<PlatformModel>() }
            });
    }


    // GetById
    [TestCaseSource(nameof(GetStation))]
    public void GetByIdTest(Station station)
    {
        // given
        foreach (var pl in station.Platforms)
            pl.Station = station;

        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        var expected = new StationModel { Name = station.Name, IsDeleted = station.IsDeleted, Platforms = new List<PlatformModel>() };
        foreach (var pl in station.Platforms)
            if (!pl.IsDeleted)
                expected.Platforms.Add(new PlatformModel { Number = pl.Number, Station = expected });

        // when
        StationModel actual = service.GetById(5);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetStation()
    {
        var platforms = new List<Platform>
        {
            new() { Number = 1, IsDeleted = false },
            new() { Number = 2, IsDeleted = false },
            new() { Number = 3, IsDeleted = true },
            new() { Number = 4, IsDeleted = false },
        };

        yield return new TestCaseData(new Station { Name = "Москва", IsDeleted = true, Platforms = platforms });
        yield return new TestCaseData(new Station { Name = "Челябинск", IsDeleted = false, Platforms = platforms });
        yield return new TestCaseData(new Station { Name = "Омск", IsDeleted = false, Platforms = platforms });
        yield return new TestCaseData(new Station { Name = "48 км", IsDeleted = false, Platforms = platforms });
        yield return new TestCaseData(new Station { Name = "Выборг", IsDeleted = false, Platforms = platforms });
        yield return new TestCaseData(new Station { Name = "Красное село", IsDeleted = true, Platforms = platforms });
    }
    [Test]
    public void GetByIdNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    // GetList
    [TestCaseSource(nameof(GetListStationNotDeleted))]
    public void GetListTest(List<Station> stations)
    {
        // given
        foreach (var station in stations)
            foreach (var pl in station.Platforms)
                pl.Station = station;

        _stationRepositoryMock.Setup(x => x.GetList(false)).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        var expected = stations.Select(station => new StationModel
                                {
                                    Name = station.Name,
                                    Platforms = station.Platforms
                                                       .Where(t => !t.IsDeleted)
                                                       .Select(pl => new PlatformModel
                                                        {
                                                            Number = pl.Number, Station = new StationModel { Name = pl.Station.Name }, IsDeleted = pl.IsDeleted
                                                        })
                                                       .ToList(),
                                    IsDeleted = station.IsDeleted
                                })
                               .ToList();

        // when
        var actual = service.GetList();

        // then
        _stationRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListStationNotDeleted()
    {
        var platforms = new List<Platform>
        {
            new() { Number = 1, IsDeleted = false },
            new() { Number = 2, IsDeleted = false },
            new() { Number = 3, IsDeleted = true },
            new() { Number = 4, IsDeleted = true },
            new() { Number = 5, IsDeleted = true },
            new() { Number = 6, IsDeleted = false }
        };

        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Челябинск", Platforms = platforms, IsDeleted = false },
            new() { Name = "Омск", Platforms = platforms, IsDeleted = false },
            new() { Name = "48 км", Platforms = platforms, IsDeleted = false },
            new() { Name = "Выборг", Platforms = platforms, IsDeleted = false }
        });
        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Москва", Platforms = platforms, IsDeleted = false },
            new() { Name = "Красное село", Platforms = platforms, IsDeleted = false }
        });
    }
    [TestCaseSource(nameof(GetListStationDeleted))]
    public void GetListDeletedTest(List<Station> stations)
    {
        // given
        foreach (var station in stations)
            foreach (var pl in station.Platforms)
                pl.Station = station;

        _stationRepositoryMock.Setup(x => x.GetList(true)).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        var expected = stations.Where(t => t.IsDeleted).Select(station => new StationModel
                                {
                                    Name = station.Name,
                                    Platforms = station.Platforms
                                                       .Where(t => !t.IsDeleted)
                                                       .Select(pl => new PlatformModel
                                                        {
                                                            Number = pl.Number, Station = new StationModel { Name = pl.Station.Name }, IsDeleted = pl.IsDeleted
                                                        })
                                                       .ToList(),
                                    IsDeleted = station.IsDeleted
                                })
                               .ToList();

        // when
        var actual = service.GetListDeleted();

        // then
        _stationRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListStationDeleted()
    {
        var platforms = new List<Platform>
        {
            new() { Number = 1, IsDeleted = false },
            new() { Number = 2, IsDeleted = false },
            new() { Number = 3, IsDeleted = true },
            new() { Number = 4, IsDeleted = true },
            new() { Number = 5, IsDeleted = true },
            new() { Number = 6, IsDeleted = false }
        };

        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Москва", Platforms = platforms, IsDeleted = true },
            new() { Name = "Челябинск", Platforms = platforms, IsDeleted = false },
            new() { Name = "Омск", Platforms = platforms, IsDeleted = false },
            new() { Name = "Красное село", Platforms = platforms, IsDeleted = true }
        });
        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Москва", Platforms = platforms, IsDeleted = false },
            new() { Name = "Омск", Platforms = platforms, IsDeleted = true },
            new() { Name = "48 км", Platforms = platforms, IsDeleted = true }
        });
    }


    //ReadyPlatforms
    [TestCaseSource(nameof(GetReadyPlatformsStation))]
    public void GetReadyPlatformsStationByIdTest(Station station, List<Platform> platforms)
    {
        // given
        var moment = new DateTime(2022, 1, 1);
        var expected = new List<PlatformModel>();
        foreach (var pl in station.Platforms)
        {
            pl.Station = station;
            if (pl.Number == 3)
                expected.Add(new PlatformModel { Number = pl.Number, Station = new StationModel { Name = station.Name }, IsDeleted = pl.IsDeleted });
        }
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        _stationRepositoryMock.Setup(x => x.GetReadyPlatformsStation(station, moment)).Returns(platforms);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetReadyPlatformsStationById(20, moment);

        // then
        _stationRepositoryMock.Verify(s => s.GetById(20), Times.Once);
        _stationRepositoryMock.Verify(s => s.GetReadyPlatformsStation(station, moment), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetReadyPlatformsStation()
    {
        var allTimePM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MaxValue, IsDeleted = false };
        var allTimeDeletedPM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MaxValue, IsDeleted = true };
        var notTimePM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MinValue, IsDeleted = false };
        var notTimeDeletedPM = new PlatformMaintenance { StartTime = DateTime.MinValue, EndTime = DateTime.MinValue, IsDeleted = true };

        var allTimeTS = new TripStation { ArrivalTime = DateTime.MinValue, DepartingTime = DateTime.MaxValue };
        var notTimeTS = new TripStation { ArrivalTime = DateTime.MinValue, DepartingTime = DateTime.MinValue };

        var notReadyPlatformFist = new Platform
        {
            Number = 1,
            PlatformMaintenances = new List<PlatformMaintenance> { allTimeDeletedPM, notTimeDeletedPM },
            TripStations = new List<TripStation> { allTimeTS, notTimeTS },
            IsDeleted = false
        };
        var notReadyPlatformSecond = new Platform
        {
            Number = 5,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimePM, notTimeDeletedPM, allTimePM },
            TripStations = new List<TripStation> { allTimeTS },
            IsDeleted = false
        };
        var notReadyDeletedPlatform = new Platform
        {
            Number = 2,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimePM, notTimeDeletedPM, allTimePM },
            TripStations = new List<TripStation> { allTimeTS },
            IsDeleted = true
        };
        var readyPlatform = new Platform
        {
            Number = 3,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimeDeletedPM, allTimeDeletedPM },
            TripStations = new List<TripStation> { notTimeTS },
            IsDeleted = false
        };
        var readyDeletedPlatform = new Platform
        {
            Number = 4,
            PlatformMaintenances = new List<PlatformMaintenance> { notTimeDeletedPM, allTimePM, allTimeDeletedPM },
            TripStations = new List<TripStation> { notTimeTS },
            IsDeleted = true
        };

        yield return new TestCaseData(new Station
            {
                Name = "Москва",
                Platforms = new List<Platform> { notReadyPlatformFist, notReadyPlatformSecond, notReadyDeletedPlatform, readyPlatform, readyDeletedPlatform }
            },
            new List<Platform> { readyPlatform });
        yield return new TestCaseData(new Station
                { Name = "Выборг", Platforms = new List<Platform> { readyPlatform, readyPlatform, readyPlatform, notReadyPlatformSecond } },
            new List<Platform> { readyPlatform, readyPlatform, readyPlatform });
        yield return new TestCaseData(new Station
                { Name = "Сочи", Platforms = new List<Platform> { readyDeletedPlatform, notReadyDeletedPlatform, notReadyPlatformSecond, notReadyPlatformFist } },
            new List<Platform>());
    }
    [Test]
    public void GetReadyPlatformsStationByIdNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetReadyPlatformsStationById(10, DateTime.Today));
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