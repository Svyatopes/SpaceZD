using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
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
    }




    [Test]
    public void GetByIdTest()
    {
        //arrange
        var orderToAdd = GetTestOrder();

        _context.Orders.Add(orderToAdd);
        _context.SaveChanges();
        var idAddedOrder = orderToAdd.Id;

        //act
        var receivedOrder = _repository.GetById(idAddedOrder);

        //assert
        Assert.IsNotNull(receivedOrder);
        Assert.IsFalse(receivedOrder!.IsDeleted);
        AssertTestOrder(receivedOrder);
    }



    [Test]
    public void GetListTest()
    {
        //arrange
        var orderToAdd = GetTestOrder();
        var secondOrderToAdd = GetTestOrder();
        var thirdOrderToAdd = GetTestOrder();
        thirdOrderToAdd.IsDeleted = true;

        _context.Orders.Add(orderToAdd);
        _context.Orders.Add(secondOrderToAdd);
        _context.Orders.Add(thirdOrderToAdd);
        _context.SaveChanges();

        //act
        var orders = (List<Order>)_repository.GetList();


        //assert

        Assert.IsNotNull(orders);
        Assert.AreEqual(2, orders.Count);

        var orderToCheck = orders[0];
        Assert.IsNotNull(orderToCheck);
        Assert.IsFalse(orderToCheck.IsDeleted);
        AssertTestOrder(orderToCheck);

    }


    [Test]
    public void GetListAllIncludedTest()
    {
        //arrange
        var orderToAdd = GetTestOrder();
        var secondOrderToAdd = GetTestOrder();
        var thirdOrderToAdd = GetTestOrder();
        thirdOrderToAdd.IsDeleted = true;

        _context.Orders.Add(orderToAdd);
        _context.Orders.Add(secondOrderToAdd);
        _context.Orders.Add(thirdOrderToAdd);
        _context.SaveChanges();

        //act
        var orders = (List<Order>)_repository.GetList(true);


        //assert

        Assert.IsNotNull(orders);
        Assert.AreEqual(3, orders.Count);

        var orderToCheck = orders[2];
        Assert.IsNotNull(orderToCheck);
        Assert.IsTrue(orderToCheck.IsDeleted);
        AssertTestOrder(orderToCheck);

    }

    [Test]
    public void AddTest()
    {
        //arrange
        var orderToAdd = GetTestOrder();

        //act 
        int id = _repository.Add(orderToAdd);

        //assert
        var createdOrder = _context.Orders.FirstOrDefault(o => o.Id == id);

        AssertTestOrder(createdOrder);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //arrange
        var orderToAdd = GetTestOrder();
        _context.Orders.Add(orderToAdd);
        _context.SaveChanges();

        var orderToEdit = GetTestOrder();
        orderToEdit.Id = orderToAdd.Id;

        orderToEdit.Trip = new Trip() { Route = new Route() { Code = "231B" } };
        orderToEdit.StartStation = new TripStation()
        {
            Station = new Station() { Name = "Tomsk" }
        };
        orderToEdit.EndStation = new TripStation()
        {
            Station = new Station() { Name = "Bolotnoe" }
        };
        orderToEdit.User = new User()
        {
            Name = "Enot",
            Role = Enums.Role.User
        };

        //act 
        bool edited = _repository.Update(orderToEdit);

        //assert
        var updatedOrder = _context.Orders.FirstOrDefault(o => o.Id == orderToEdit.Id);

        Assert.IsNotNull(updatedOrder);
        Assert.IsNotNull(updatedOrder!.StartStation);
        Assert.IsNotNull(updatedOrder.StartStation.Station);
        Assert.AreEqual("Tomsk", updatedOrder.StartStation.Station.Name);
        Assert.IsNotNull(updatedOrder.EndStation);
        Assert.IsNotNull(updatedOrder.EndStation.Station);
        Assert.AreEqual("Bolotnoe", updatedOrder.EndStation.Station.Name);
        Assert.IsNotNull(updatedOrder.Trip);
        Assert.IsNotNull(updatedOrder.Trip.Route);
        Assert.AreEqual("231B", updatedOrder.Trip.Route.Code);
        Assert.IsNotNull(updatedOrder.User);
        //ORIGINAL USER NOT CHANGED, AND IT'S FINE 
        Assert.AreEqual("Zagit", updatedOrder.User.Name);
        Assert.AreEqual(Enums.Role.StationManager, updatedOrder.User.Role);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //arrange
        var orderToEdit = GetTestOrder();
        orderToEdit.IsDeleted = !isDeleted;
        _context.Orders.Add(orderToEdit);
        _context.SaveChanges();

        //act 
        bool edited = _repository.Update(orderToEdit.Id, isDeleted);

        //assert
        
        var updatedOrder = _context.Orders.FirstOrDefault(o => o.Id == orderToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(updatedOrder);
        Assert.AreEqual(isDeleted, updatedOrder!.IsDeleted);
        AssertTestOrder(updatedOrder);
    }

    private Order GetTestOrder() => new Order()
    {
        StartStation = new TripStation()
        {
            Station = new Station() { Name = "Novosibirsk" }
        },
        EndStation = new TripStation()
        {
            Station = new Station() { Name = "Sheregesh" }
        },
        Trip = new Trip() { Route = new Route() { Code = "231A" } },
        User = new User()
        {
            Name = "Zagit",
            Login = "Sleep2000",
            PasswordHash = "SDJKLSAJDLJASLDJASLDKJASD",
            Role = Enums.Role.StationManager
        }
    };

    private void AssertTestOrder(Order order)
    {
        Assert.IsNotNull(order);
        Assert.IsNotNull(order!.StartStation);
        Assert.IsNotNull(order.StartStation.Station);
        Assert.AreEqual("Novosibirsk", order.StartStation.Station.Name);
        Assert.IsNotNull(order.EndStation);
        Assert.IsNotNull(order.EndStation.Station);
        Assert.AreEqual("Sheregesh", order.EndStation.Station.Name);
        Assert.IsNotNull(order.Trip);
        Assert.IsNotNull(order.Trip.Route);
        Assert.AreEqual("231A", order.Trip.Route.Code);
        Assert.IsNotNull(order.User);
        Assert.AreEqual("Zagit", order.User.Name);
        Assert.AreEqual(Enums.Role.StationManager, order.User.Role);
    }
}