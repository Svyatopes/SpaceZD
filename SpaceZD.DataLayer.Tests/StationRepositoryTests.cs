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
    private IRepositorySoftDeleteNewUpdate<Station> _repository;

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