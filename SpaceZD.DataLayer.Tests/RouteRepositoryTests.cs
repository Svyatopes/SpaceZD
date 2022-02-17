using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;
using SpaceZD.DataLayer.Tests.TestCaseSources;

namespace SpaceZD.DataLayer.Tests;

public class RouteRepositoryTests
{
    private VeryVeryImportantContext _context;
    private IRouteRepository _repository;

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
        _context.Routes.AddRange(RouteRepositoryTestCaseSource.GetRoutes());
        _context.SaveChanges();
    }

    [TestCaseSource(typeof(RouteRepositoryTestCaseSource),
        nameof(RouteRepositoryTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(int id, Route expected)
    {
        // when
        var actual = _repository.GetById(id);

        // then
        Assert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(RouteRepositoryTestCaseSource),
        nameof(RouteRepositoryTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(bool includeAll, List<Route> expected)
    {
        // when
        var list = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, list);
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = RouteRepositoryTestCaseSource.GetRoute();

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
        var entityUpdate = RouteRepositoryTestCaseSource.GetRoute();
        entityUpdate.IsDeleted = !entityToEdit!.IsDeleted;
        foreach (var rt in entityUpdate.RouteTransits)
            rt.Route = entityUpdate;

        // when 
        _repository.Update(entityToEdit, entityUpdate);

        // then
        Assert.AreEqual(entityUpdate.Code, entityToEdit.Code);
        Assert.AreEqual(entityUpdate.StartTime, entityToEdit.StartTime);
        Assert.AreEqual(entityUpdate.StartStation, entityToEdit.StartStation);
        Assert.AreEqual(entityUpdate.EndStation, entityToEdit.EndStation);
        Assert.AreNotEqual(entityUpdate.IsDeleted, entityToEdit.IsDeleted);
        CollectionAssert.AreNotEqual(entityUpdate.RouteTransits, entityToEdit.RouteTransits);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var entityToEdit = RouteRepositoryTestCaseSource.GetRoute();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Routes.Add(entityToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(entityToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }


    [TestCaseSource(typeof(RouteRepositoryTestCaseSource),
        nameof(RouteRepositoryTestCaseSource.GetTestCaseDataForAddRouteTransitForRouteTest))]
    public void AddRouteTransitForRouteTest(Route route, RouteTransit routeTransit, Route expected)
    {
        // when 
        _repository.AddRouteTransitForRoute(route, routeTransit);

        // then
        Assert.AreEqual(expected, route);
    }
}