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
    private IRepositorySoftDelete<Station> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _repository = new StationRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Test]
    public void GetByIdTest()
    {
        //arrange
        var entityToAdd = TestEntity;

        _context.Stations.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        //act
        var receivedEntity = _repository.GetById(idAdded);

        //assert
        Assert.IsNotNull(receivedEntity);
        Assert.IsFalse(receivedEntity!.IsDeleted);
        AssertTestStation(receivedEntity);
    }

    [Test]
    public void GetListTest()
    {
        //arrange
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.Stations.Add(entityToAdd);
        _context.Stations.Add(secondEntityToAdd);
        _context.Stations.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var list = (List<Station>)_repository.GetList();

        //assert
        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Count);

        var entityToCheck = list[0];
        Assert.IsNotNull(entityToCheck);
        Assert.IsFalse(entityToCheck.IsDeleted);
        AssertTestStation(entityToCheck);
    }


    [Test]
    public void GetListAllIncludedTest()
    {
        //arrange
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.Stations.Add(entityToAdd);
        _context.Stations.Add(secondEntityToAdd);
        _context.Stations.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var list = (List<Station>)_repository.GetList(true);

        //assert
        Assert.IsNotNull(list);
        Assert.AreEqual(3, list.Count);

        var entityToCheck = list[2];
        Assert.IsNotNull(entityToCheck);
        Assert.IsTrue(entityToCheck.IsDeleted);
        AssertTestStation(entityToCheck);
    }

    [Test]
    public void AddTest()
    {
        //arrange
        var entityToAdd = TestEntity;

        //act 
        int id = _repository.Add(entityToAdd);

        //assert
        var createdEntity = _context.Stations.FirstOrDefault(o => o.Id == id);

        AssertTestStation(createdEntity!);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //arrange
        var entityToAdd = TestEntity;
        _context.Stations.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = TestEntity;
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Name = "qwertyuiop";

        //act 
        bool edited = _repository.Update(entityToEdit);

        //assert
        var entityToUpdated = _context.Stations.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        AssertTestStation(entityToUpdated!, "qwertyuiop");
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //arrange
        var entityToEdit = TestEntity;
        entityToEdit.IsDeleted = !isDeleted;
        _context.Stations.Add(entityToEdit);
        _context.SaveChanges();

        //act 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        //assert
        var entityToUpdated = _context.Stations.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(entityToUpdated);
        Assert.AreEqual(isDeleted, entityToUpdated!.IsDeleted);
        AssertTestStation(entityToUpdated);
    }

    private Station TestEntity => new()
    {
        Name = "Челябинск",
        Platforms = new List<Platform>
        {
            new() { Number = 1 },
            new() { Number = 2 },
            new() { Number = 3 }
        }
    };

    private void AssertTestStation(Station station, string name = "Челябинск", int countPlatform = 3)
    {
        Assert.IsNotNull(station);
        Assert.IsNotNull(station.Platforms);
        Assert.AreEqual(countPlatform, station.Platforms.Count);
        Assert.AreEqual(name, station.Name);
        Assert.AreEqual(station.Platforms.First().Number, 1);
        Assert.AreEqual(station.Platforms.First().Station, station);
    }
}