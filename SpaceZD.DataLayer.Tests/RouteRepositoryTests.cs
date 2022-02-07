using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class RouteRepositoryTests
{
    private VeryVeryImportantContext _context;
    private IRepositorySoftDelete<Route> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _repository = new RouteRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Test]
    public void GetByIdTest()
    {
        // given
        var entityToAdd = TestEntity;

        _context.Routes.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        // when
        var receivedEntity = _repository.GetById(idAdded);

        // then
        Assert.IsNotNull(receivedEntity);
        Assert.IsFalse(receivedEntity!.IsDeleted);
        Assert.IsTrue(receivedEntity.Equals(entityToAdd));
    }

    [Test]
    public void GetListTest()
    {
        // given
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.Routes.Add(entityToAdd);
        _context.Routes.Add(secondEntityToAdd);
        _context.Routes.Add(thirdEntityToAdd);
        _context.SaveChanges();

        // when
        var list = (List<Route>)_repository.GetList();

        // then
        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Count);

        var entityToCheck = list[^1];
        Assert.IsNotNull(entityToCheck);
        Assert.IsFalse(entityToCheck.IsDeleted);
        Assert.IsTrue(entityToCheck.Equals(secondEntityToAdd));
    }


    [Test]
    public void GetListAllIncludedTest()
    {
        // given
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.Routes.Add(entityToAdd);
        _context.Routes.Add(secondEntityToAdd);
        _context.Routes.Add(thirdEntityToAdd);
        _context.SaveChanges();

        // when
        var list = (List<Route>)_repository.GetList(true);

        // then
        Assert.IsNotNull(list);
        Assert.AreEqual(3, list.Count);

        var entityToCheck = list[^1];
        Assert.IsNotNull(entityToCheck);
        Assert.IsTrue(entityToCheck.IsDeleted);
        Assert.IsTrue(entityToCheck.Equals(thirdEntityToAdd));
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = TestEntity;

        // when 
        int id = _repository.Add(entityToAdd);

        // then
        var createdEntity = _context.Routes.FirstOrDefault(o => o.Id == id);

        Assert.IsTrue(createdEntity!.Equals(entityToAdd));
    }

    [Test]
    public void UpdateEntityTest()
    {
        // given
        var entityToAdd = TestEntity;
        _context.Routes.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = _context.Routes.FirstOrDefault(o => o.Id == entityToAdd.Id);
        entityToEdit!.Code = "h589";
        entityToEdit.StartStation = new Station { Name = "start" };
        entityToEdit.EndStation = new Station { Name = "end" };
        entityToEdit.StartTime = DateTime.Now;

        // when 
        bool edited = _repository.Update(entityToEdit);

        // then
        var entityToUpdated = _context.Routes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsTrue(entityToUpdated!.Equals(entityToEdit));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var entityToEdit = TestEntity;
        entityToEdit.IsDeleted = !isDeleted;
        _context.Routes.Add(entityToEdit);
        _context.SaveChanges();
        entityToEdit.IsDeleted = isDeleted;

        // when 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        // then
        var entityToUpdated = _context.Routes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(entityToUpdated);
        Assert.AreEqual(isDeleted, entityToUpdated!.IsDeleted);
        Assert.IsTrue(entityToUpdated.Equals(entityToEdit));
    }

    private Route TestEntity => new()
    {
        Code = "M456",
        Transits = new List<RouteTransit>
        {
            new()
            {
                Transit = new Transit
                {
                    StartStation = new Station { Name = "Москва" },
                    EndStation = new Station { Name = "Псков" }
                },
                DepartingTime = new TimeSpan(1, 1, 0),
                ArrivalTime = new TimeSpan(1, 0, 0)
            },
            new()
            {
                Transit = new Transit
                {
                    StartStation = new Station { Name = "Псков" },
                    EndStation = new Station { Name = "Новгород" }
                },
                DepartingTime = new TimeSpan(2, 1, 0),
                ArrivalTime = new TimeSpan(2, 0, 0)
            }
        },
        StartTime = new DateTime(1999, 10, 1),
        StartStation = new Station { Name = "Москва" },
        EndStation = new Station { Name = "Новгород" }
    };
}