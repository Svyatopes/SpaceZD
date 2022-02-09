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
    private Mock<IRepositorySoftDelete<Station>> _stationRepositoryMock;
    private readonly IMapper _mapper;

    public StationServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _stationRepositoryMock = new Mock<IRepositorySoftDelete<Station>>();
    }

    // Add
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(28)]
    public void AddTest(int expected)
    {
        // given
        _stationRepositoryMock.Setup(x => x.Add(It.IsAny<Station>())).Returns(expected);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        int actual = service.Add(new StationModel());

        // then
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
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetNearStations()
    {
        var transitFirst = new Transit { EndStation = new Station { Name = "Москва" } };
        var transitSecond = new Transit { EndStation = new Station { Name = "Челябинск", IsDeleted = true } };
        var transitThird = new Transit { EndStation = new Station { Name = "Омск" } };
        var transitFourth = new Transit { EndStation = new Station { Name = "48 км" }, IsDeleted = true };
        var transitFifth = new Transit { EndStation = new Station { Name = "Выборг"} };


        var station = new Station { Name = "0 км", TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFourth } };
        yield return new TestCaseData(station,
            new List<StationModel>
            {
                new() { Name = transitFirst.EndStation.Name },
                new() { Name = transitThird.EndStation.Name }
            });
        
        
        var station2 = new Station { Name = "10 км", TransitsWithStartStation = new List<Transit> { transitFirst, transitSecond, transitThird, transitFifth } };
        yield return new TestCaseData(station2,
            new List<StationModel>
            {
                new() { Name = transitFirst.EndStation.Name },
                new() { Name = transitThird.EndStation.Name },
                new() { Name = transitFifth.EndStation.Name }
            });
    }


    // GetById
    [TestCaseSource(nameof(GetStation))]
    public void GetByIdTest(Station station)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(station);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        Assert.AreEqual(new StationModel { Name = station.Name, IsDeleted = station.IsDeleted }, actual);
    }
    public static IEnumerable<TestCaseData> GetStation()
    {
        yield return new TestCaseData(new Station { Name = "Москва", IsDeleted = true });
        yield return new TestCaseData(new Station { Name = "Челябинск", IsDeleted = false });
        yield return new TestCaseData(new Station { Name = "Омск", IsDeleted = false });
        yield return new TestCaseData(new Station { Name = "48 км", IsDeleted = false });
        yield return new TestCaseData(new Station { Name = "Выборг", IsDeleted = false });
        yield return new TestCaseData(new Station { Name = "Красное село", IsDeleted = true });
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
        _stationRepositoryMock.Setup(x => x.GetList(false)).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);
        var expected = stations.Select(station => new StationModel
                               {
                                   Name = station.Name, IsDeleted = station.IsDeleted
                               })
                               .ToList();

        // when
        var actual = service.GetList();

        // then
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListStationNotDeleted()
    {
        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Челябинск", IsDeleted = false },
            new() { Name = "Омск", IsDeleted = false },
            new() { Name = "48 км", IsDeleted = false },
            new() { Name = "Выборг", IsDeleted = false }
        });
        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Москва", IsDeleted = false },
            new() { Name = "Красное село", IsDeleted = false }
        });
    }
    [TestCaseSource(nameof(GetListStationDeleted))]
    public void GetListDeletedTest(List<Station> stations)
    {
        // given
        _stationRepositoryMock.Setup(x => x.GetList(true)).Returns(stations);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);
        var expected = stations.Where(t => t.IsDeleted).Select(station => new StationModel
                               {
                                   Name = station.Name, IsDeleted = station.IsDeleted
                               })
                               .ToList();

        // when
        var actual = service.GetListDeleted();

        // then
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListStationDeleted()
    {
        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Москва", IsDeleted = true },
            new() { Name = "Челябинск", IsDeleted = false },
            new() { Name = "Омск", IsDeleted = false },
            new() { Name = "Красное село", IsDeleted = true }
        });
        yield return new TestCaseData(new List<Station>
        {
            new() { Name = "Москва", IsDeleted = false },
            new() { Name = "Омск", IsDeleted = true },
            new() { Name = "48 км", IsDeleted = true },
        });
    }


    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), true)).Returns(true);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        service.Delete(45);

        // then
        _stationRepositoryMock.Verify(s => s.Update(It.IsAny<int>(), true), Times.Once());
        Assert.Pass();
    }
    [Test]
    public void DeleteNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), true)).Returns(false);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), false)).Returns(true);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        service.Restore(45);

        // then
        _stationRepositoryMock.Verify(s => s.Update(It.IsAny<int>(), false), Times.Once());
        Assert.Pass();
    }
    [Test]
    public void RestoreNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), false)).Returns(false);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>())).Returns(true);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        // when
        service.Update(45, new StationModel());

        // then
        _stationRepositoryMock.Verify(s => s.Update(It.IsAny<Station>()), Times.Once());
        Assert.Pass();
    }
    [Test]
    public void UpdateNegativeTest()
    {
        _stationRepositoryMock.Setup(x => x.Update(It.IsAny<Station>())).Returns(false);
        var service = new StationService(_mapper, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.Update(10, new StationModel()));
    }
}