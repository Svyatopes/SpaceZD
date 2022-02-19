using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestCaseSources;

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
        _context.Stations.AddRange(StationRepositoryTestCaseSource.GetStations());
        _context.SaveChanges();
    }

    [TestCaseSource(typeof(StationRepositoryTestCaseSource),
        nameof(StationRepositoryTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(int id, Station expected)
    {
        // when
        var actual = _repository.GetById(id);

        // then
        Assert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(StationRepositoryTestCaseSource),
        nameof(StationRepositoryTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(bool includeAll, List<Station> expected)
    {
        // when
        var actual = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = StationRepositoryTestCaseSource.GetStation();

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
        var entityUpdate = StationRepositoryTestCaseSource.GetStation();
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
        var entityToEdit = StationRepositoryTestCaseSource.GetStation();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Stations.Add(entityToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(entityToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }

    [TestCaseSource(typeof(StationRepositoryTestCaseSource),
        nameof(StationRepositoryTestCaseSource.GetTestCaseDataForReadyPlatformsStation))]
    public void GetReadyPlatformsStationTest(Station station, DateTime startMoment, DateTime endMoment, List<Platform> expected)
    {
        // when 
        var actual = _repository.GetReadyPlatformsStation(station, startMoment, endMoment);

        // then
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(StationRepositoryTestCaseSource),
        nameof(StationRepositoryTestCaseSource.GetTestCaseDataForGetNearStations))]
    public void GetNearStations(Station station, List<Station> expected)
    {
        // when 
        var actual = _repository.GetNearStations(station);

        // then
        CollectionAssert.AreEqual(expected, actual);
    }
}