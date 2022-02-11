using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using System;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class RouteTransitRepositoryTests
{
    private VeryVeryImportantContext _context;
    private IRepositorySoftDeleteNewUpdate<RouteTransit> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                    .UseInMemoryDatabase("Test")
                    .Options;
        _context = new VeryVeryImportantContext(options);
        _repository = new RouteTransitRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();


        // seed
        var routetransuts = new RouteTransit[]
        {
                new()
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ"},
                        EndStation=new Station{Name="Москва"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(10,0,0),
                },
                new()
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ"},
                        EndStation=new Station{Name="Пермь"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(24,0,0),
                },
                new()
                {
                    Transit=new Transit
                    {
                        StartStation=new Station{Name="СПБ"},
                        EndStation=new Station{Name="Выборг"}
                    },
                    DepartingTime=new TimeSpan(0,0,0),
                    ArrivalTime=new TimeSpan(1,20,0),
                    IsDeleted=true
                }

        };
        _context.RouteTransits.AddRange(routetransuts);
        _context.SaveChanges();

    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetByIdTest(int id)
    {
        // given
        var expectedEntity = _context.RouteTransits.Find(id);

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
        var expected = _context.RouteTransits.Where(t => !t.IsDeleted || includeAll).ToList();

        // when
        var list = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, list);
    }

    [Test]
    public void AddTest()
    {
        // given
        var routeTransitToAdd = GetRouteTransit;

        // when 
        int id = _repository.Add(routeTransitToAdd);

        // then
        var routeTransitOnCreate = _context.RouteTransits.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(routeTransitOnCreate, routeTransitToAdd);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateRouteTransitTest(int id)
    {
        // given
        var oldRouteTransitToEdit = _context.RouteTransits.FirstOrDefault(o => o.Id == id);
        var newRouteTransitToEdit = GetRouteTransit;

        // when 
        _repository.Update(oldRouteTransitToEdit, newRouteTransitToEdit);

        // then
        var routeTransitToUpdated = _context.RouteTransits.FirstOrDefault(o => o.Id == oldRouteTransitToEdit.Id);

        Assert.AreEqual(newRouteTransitToEdit.Transit, routeTransitToUpdated.Transit);
        Assert.AreEqual(newRouteTransitToEdit.DepartingTime, routeTransitToUpdated.DepartingTime);
        Assert.AreEqual(newRouteTransitToEdit.ArrivalTime, routeTransitToUpdated.ArrivalTime);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var routeTransitToEdit = GetRouteTransit;
        routeTransitToEdit.IsDeleted = isDeleted;
        _context.RouteTransits.Add(routeTransitToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(GetRouteTransit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, routeTransitToEdit.IsDeleted);
    }

    private RouteTransit GetRouteTransit => new()
    {
        Transit = new Transit
        {
            StartStation = new Station { Name = "Москва" },
            EndStation = new Station { Name = "Луга" }
        },
        DepartingTime = new TimeSpan(0, 0, 0),
        ArrivalTime = new TimeSpan(5, 0, 0),
    };
}

