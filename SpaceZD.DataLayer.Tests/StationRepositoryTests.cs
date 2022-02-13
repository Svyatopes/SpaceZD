using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.DataLayer.Tests;

public class StationRepositoryTests
{
    private VeryVeryImportantContext _context;
    private IStationRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                     .UseInMemoryDatabase("Test")
                     .Options;

        _context = new VeryVeryImportantContext(options);
        _repository = new StationRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // seed
        var stations = new Station[]
        {
            new()
            {
                Name = "Челябинск",
                Platforms = new List<Platform>
                {
                    new() { Number = 1 },
                    new() { Number = 2 }
                }
            },
            new()
            {
                Name = "41 км",
                Platforms = new List<Platform>(),
                IsDeleted = true
            },
            new()
            {
                Name = "Таганрог",
                Platforms = new List<Platform>
                {
                    new() { Number = 2 },
                    new() { Number = 3 }
                }
            }
        };
        _context.Stations.AddRange(stations);
        _context.SaveChanges();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetByIdTest(int id)
    {
        // given
        var expectedEntity = _context.Stations.Find(id);

        // when
        var receivedEntity = _repository.GetById(id);

        // then
        Assert.AreEqual(expectedEntity, receivedEntity);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void GetListTest(bool includeAll)
    {
        // given
        var expected = _context.Stations.Where(t => !t.IsDeleted || includeAll).ToList();

        // when
        var list = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, list);
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = TestEntity;

        // when 
        int id = _repository.Add(entityToAdd);

        // then
        var entityOnCreate = _context.Stations.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(entityOnCreate, entityToAdd);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateEntityTest(int id)
    {
        // given
        var entityToEdit = _context.Stations.FirstOrDefault(o => o.Id == id);
        var entityUpdate = TestEntity;
        entityUpdate.IsDeleted = !entityToEdit!.IsDeleted;
        foreach (var pl in entityUpdate.Platforms)
            pl.Station = entityUpdate;

        // when 
        _repository.Update(entityToEdit, entityUpdate);

        // then
        Assert.AreEqual(entityUpdate.Name, entityToEdit.Name);
        Assert.AreNotEqual(entityUpdate.IsDeleted, entityToEdit.IsDeleted);
        CollectionAssert.AreNotEqual(entityUpdate.Platforms, entityToEdit.Platforms);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var entityToEdit = TestEntity;
        entityToEdit.IsDeleted = !isDeleted;
        _context.Stations.Add(entityToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(entityToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }

    [TestCaseSource(nameof(GetReadyPlatformsStation))]
    public void GetReadyPlatformsStationTest(Station station)
    {
        // given
        var expected = new List<Platform>();
        foreach (var pl in station.Platforms)
        {
            pl.Station = station;
            if (pl.Number == 3)
                expected.Add(pl);
        }

        // when 
        var actual = _repository.GetReadyPlatformsStation(station, new DateTime(2022, 1, 1));

        // then
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
        });
        yield return new TestCaseData(new Station
            { Name = "Выборг", Platforms = new List<Platform> { readyPlatform, readyPlatform, readyPlatform, notReadyPlatformSecond } });
        yield return new TestCaseData(new Station
            { Name = "Сочи", Platforms = new List<Platform> { readyDeletedPlatform, notReadyDeletedPlatform, notReadyPlatformSecond, notReadyPlatformFist } });
    }

    private Station TestEntity => new()
    {
        Name = "Москва",
        Platforms = new List<Platform>
        {
            new() { Number = 1, IsDeleted = false },
            new() { Number = 2, IsDeleted = false },
            new() { Number = 3, IsDeleted = true }
        }
    };
}