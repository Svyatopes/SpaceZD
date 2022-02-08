using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestCaseSources;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class OrderRepositoryTests
{
    private VeryVeryImportantContext _context;
    private OrderRepository _repository;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new OrderRepository(_context);

        //seed
        var orders = OrderRepositoryMocks.GetOrders();
        _context.Orders.AddRange(orders);
        _context.SaveChanges();
    }



    [Test]
    public void GetByIdTest()
    {
        //given
        var order = _context.Orders.First();

        //when
        var receivedOrder = _repository.GetById(order.Id);

        //then
        Assert.AreEqual(order, receivedOrder);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void GetListAllIncludedTest(bool allIncluded)
    {
        //given
        var orders = _context.Orders.Where(o => !o.IsDeleted || allIncluded);

        //when
        var ordersInDB = (List<Order>)_repository.GetList(allIncluded);

        //then
        Assert.AreEqual(orders, ordersInDB);

    }

    [Test]
    public void AddTest()
    {
        //given
        var orderToAdd = OrderRepositoryMocks.GetOrder();

        //when 
        int id = _repository.Add(orderToAdd);

        //then
        var createdOrder = _context.Orders.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(orderToAdd, createdOrder);
    }
    
    [Test]
    public void UpdateEntityTest()
    {
        //given
        var order = OrderRepositoryMocks.GetOrders()[0];

        order.Id = _context.Orders.First().Id;
        

        order.Trip = new Trip() { Route = new Route() { Code = "231B" } };
        order.StartStation = new TripStation()
        {
            Station = new Station() { Name = "Tomsk" }
        };
        order.EndStation = new TripStation()
        {
            Station = new Station() { Name = "Bolotnoe" }
        };

        var originalUser = order.User;

        order.User = new User()
        {
            Name = "Enot",
            Role = Enums.Role.User
        };

        //when 
        bool edited = _repository.Update(order);

        //then
        var updatedOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);

        Assert.AreEqual("Tomsk", updatedOrder!.StartStation.Station.Name);
        Assert.AreEqual("Bolotnoe", updatedOrder.EndStation.Station.Name);
        Assert.AreEqual("231B", updatedOrder.Trip.Route.Code);
        Assert.AreEqual(originalUser.Name, updatedOrder.User.Name);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var orderToEdit = OrderRepositoryMocks.GetOrder();
        orderToEdit.IsDeleted = !isDeleted;
        _context.Orders.Add(orderToEdit);
        _context.SaveChanges();

        //when 
        bool edited = _repository.Update(orderToEdit.Id, isDeleted);

        //then
        
        var updatedOrder = _context.Orders.FirstOrDefault(o => o.Id == orderToEdit.Id);

        Assert.IsTrue(edited);
        Assert.AreEqual(isDeleted, updatedOrder!.IsDeleted);
        Assert.AreEqual(orderToEdit, updatedOrder);
    } 
}