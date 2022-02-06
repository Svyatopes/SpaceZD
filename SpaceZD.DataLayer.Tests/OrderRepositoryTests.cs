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
        
        _repository = new OrderRepository(_context);
    }

    [Test]
    public void AddTest()
    {
        //arrange
        var orderToAdd = new Order()
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

        //act 
        int id = _repository.Add(orderToAdd);

        //assert
        var createdOrder = _context.Orders.FirstOrDefault(o => o.Id == id);

        Assert.IsNotNull(createdOrder);
        Assert.IsNotNull(createdOrder!.StartStation);
        Assert.IsNotNull(createdOrder.StartStation.Station);
        Assert.AreEqual("Novosibirsk", createdOrder.StartStation.Station.Name);
        Assert.IsNotNull(createdOrder.EndStation);
        Assert.IsNotNull(createdOrder.EndStation.Station);
        Assert.AreEqual("Sheregesh", createdOrder.EndStation.Station.Name);
        Assert.IsNotNull(createdOrder.Trip);
        Assert.IsNotNull(createdOrder.Trip.Route);
        Assert.AreEqual("231A", createdOrder.Trip.Route.Code);
        Assert.IsNotNull(createdOrder.User);
        Assert.AreEqual("Zagit", createdOrder.User.Name);
        Assert.AreEqual(Enums.Role.StationManager, createdOrder.User.Role);
    }
}