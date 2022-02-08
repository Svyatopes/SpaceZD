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
                      .UseInMemoryDatabase("Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _repository = new RouteRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // seed
        var routes = new Route[]
        {
            new()
            {
                Code = "F789",
                Transits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "С-Пб" },
                            EndStation = new Station { Name = "Выборг" }
                        },
                        DepartingTime = new TimeSpan(0, 0, 1),
                        ArrivalTime = new TimeSpan(2, 30, 0)
                    }
                },
                StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                StartStation = new Station { Name = "С-Пб" },
                EndStation = new Station { Name = "Выборг" }
            },
            new()
            {
                Code = "Test",
                StartTime = new DateTime(1999, 10, 1),
                StartStation = new Station { Name = "Москва" },
                EndStation = new Station { Name = "Новгород" },
                IsDeleted = true
            },
            new()
            {
                Code = "G589",
                Transits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "Петрозаводск" },
                            EndStation = new Station { Name = "Омск" }
                        },
                        DepartingTime = new TimeSpan(0, 0, 0),
                        ArrivalTime = new TimeSpan(4, 0, 0)
                    },
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "Омск" },
                            EndStation = new Station { Name = "Сочи" }
                        },
                        DepartingTime = new TimeSpan(4, 30, 0),
                        ArrivalTime = new TimeSpan(10, 0, 0)
                    }
                },
                StartTime = new DateTime(1970, 1, 1, 12, 30, 0),
                StartStation = new Station { Name = "Петрозаводск" },
                EndStation = new Station { Name = "Сочи" }
            }
        };
        _context.Routes.AddRange(routes);
        _context.SaveChanges();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetByIdTest(int id)
    {
        // given
        var expectedEntity = _context.Routes.Find(id);

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
        var expected = _context.Routes.Where(t => !t.IsDeleted || includeAll).ToList();

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
        var entityOnCreate = _context.Routes.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(entityOnCreate, entityToAdd);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateEntityTest(int id)
    {
        // given
        var entityToEdit = _context.Routes.FirstOrDefault(o => o.Id == id);
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

        // when 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        // then
        var entityToUpdated = _context.Routes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.AreEqual(entityToEdit, entityToUpdated);
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