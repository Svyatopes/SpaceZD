using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class TicketRepositoryTests
{
    private VeryVeryImportantContext _context;
    private TicketRepository _repository;

    private Ticket GetTestEntity() => new Ticket()
    {
        Person = new Person()
        {
            FirstName = "Vasya",
            Patronymic = "Vasilevich",
            LastName = "Vasilev",
            Passport = "55585885999"
        },
        SeatNumber = 56,
        Carriage = new Carriage()
        {
            Number = 5,
            Type = new CarriageType()
            {
                Name = "Econom",
                NumberOfSeats = 67
            }
        },
        Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },
        Price = 345
    };

    private List<Ticket> GetListTestEntities() => new List<Ticket>()
    {
        new()
        {
            Id = 1,
            Person = new Person()
            {
                FirstName = "Vasya",
                Patronymic = "Vasilevich",
                LastName = "Vasilev",
                Passport = "55585885999"
            },
            SeatNumber = 56,
            Carriage = new Carriage()
            {
                Number = 5,
                Type = new CarriageType()
                {
                    Name = "Econom",
                    NumberOfSeats = 67
                }
            },
            Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },
            Price = 345

        },
        new()
        {
            Person = new Person()
            {
                FirstName = "Masha",
                Patronymic = "Mashevna",
                LastName = "Mashech",
                Passport = "5784930"
            },
            SeatNumber = 6,
            Carriage = new Carriage()
            {
                Number = 5,
                Type = new CarriageType()
                {
                    Name = "Platskart",
                    NumberOfSeats = 50
                }
            },
            Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },

            Price = 300
        },
        new()
        {
            Person = new Person()
            {
                FirstName = "Dima",
                Patronymic = "Dmitrievich",
                LastName = "Dmitr",
                Passport = "6583939585"
            },
            SeatNumber = 3,
            Carriage = new Carriage()
            {
                Number = 9,
                Type = new CarriageType()
                {
                    Name = "Bisuness",
                    NumberOfSeats = 20
                }
            },
            Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },

            Price = 555
        }
    };



    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new TicketRepository(_context);
    }



    [Test]
    public void GetByIdTest()
    {
        //given
        var entityToAdd = GetTestEntity();

        _context.Tickets.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        //when
        var received = _repository.GetById(idAdded);

        //then
        //Assert.IsNotNull(received);
        Assert.AreEqual("Vasilev", received.Person.LastName);
        Assert.AreEqual(345, received.Price);
        Assert.AreEqual(56, received.SeatNumber);
        Assert.AreEqual(67, received.Carriage.Type.NumberOfSeats);
        Assert.IsFalse(received.IsPetPlaceIncluded);
        Assert.IsFalse(received.IsTeaIncluded);
    }    



    [TestCase(true)]
    [TestCase(false)]
    public void GetListTest(bool includeAll)
    {
        //given
        var entitiesToAdd = GetListTestEntities();
        entitiesToAdd.Last().IsDeleted = true;
        foreach (var item in entitiesToAdd)
        {
            _context.Tickets.Add(item);
            _context.SaveChanges();
        }
        var expected = _context.Tickets.Where(t => !t.IsDeleted || includeAll).ToList();

        //when
        var entities = (List<Ticket>)_repository.GetList(includeAll);

        //then
        Assert.IsNotNull(entities);
        Assert.AreEqual(expected.Count, entities.Count);
        CollectionAssert.AreEqual(expected, entities);

    }


    [Test]
    public void AddTest()
    {
        //given
        var entityToAdd = GetTestEntity();

        //when 
        int id = _repository.Add(entityToAdd);

        //then
        var createdEntity = _context.Tickets.FirstOrDefault(o => o.Id == id);

        Assert.IsNotNull(createdEntity);        
        Assert.AreEqual("Vasilev", createdEntity.Person.LastName);
        Assert.AreEqual(345, createdEntity.Price);
        Assert.AreEqual(56, createdEntity.SeatNumber);
        Assert.AreEqual(67, createdEntity.Carriage.Type.NumberOfSeats);
        Assert.IsFalse(createdEntity.IsPetPlaceIncluded);
        Assert.IsFalse(createdEntity.IsTeaIncluded);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //given
        var entityToAdd = GetTestEntity();
        _context.Tickets.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = GetTestEntity();
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Person.LastName = "Petrov";
        entityToEdit.Price = 677;
        entityToEdit.SeatNumber = 2;
        entityToEdit.Carriage.Number = 7;
        entityToEdit.IsPetPlaceIncluded = true;
        entityToEdit.IsTeaIncluded = true;

        //when 
        _repository.Update(entityToAdd, entityToEdit);

        //then
        var updatedEntity = _context.Tickets.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsNotNull(updatedEntity);        
        Assert.AreEqual(7, updatedEntity.Carriage.Number);
        Assert.AreEqual(2, updatedEntity.SeatNumber);
        Assert.AreEqual(677, updatedEntity.Price);
        Assert.AreEqual("Petrov", updatedEntity.Person.LastName);
        //не должны были поменятся
        Assert.IsFalse(updatedEntity.IsPetPlaceIncluded);
        Assert.IsFalse(updatedEntity.IsTeaIncluded);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var entityToEdit = GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Tickets.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.Update(entityToEdit, isDeleted);

        //then
        
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
                
    }


}