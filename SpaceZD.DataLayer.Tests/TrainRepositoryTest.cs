using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class TrainRepositoryTests
{
    private VeryVeryImportantContext _context;
    private TrainRepository _repository;

    private Train GetTestEntity() => new Train()
    {
        Carriages = new List<Carriage>()
        {
            new()
            {
                Number = 5,
                Type = new CarriageType()
                {
                    Name = "Econom",
                    NumberOfSeats = 67
                },
                Tickets = new List<Ticket>
                {
                    new() { Price = 147 },
                    new() { Price = 100 },
                    new() { Price = 124 }
                }
            },
            new()
            {
                Number = 45,
                Type = new CarriageType()
                {
                    Name = "Premium",
                    NumberOfSeats = 4
                },
                Tickets = new List<Ticket>
                {
                    new() { Price = 547 },
                    new() { Price = 500 },
                    new() { Price = 524 }
                }
            }

        }
    };

    private List<Train> GetListTestEntities() => new List<Train>()
    {
        new()
        {
            Carriages = new List<Carriage>()
            {
                new()
                {
                    Number = 5,
                    Type = new CarriageType()
                    {
                        Name = "Econom",
                        NumberOfSeats = 67
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 147 },
                        new() { Price = 100 },
                        new() { Price = 124 }
                    }
                },
                new()
                {
                    Number = 45,
                    Type = new CarriageType()
                    {
                        Name = "Premium",
                        NumberOfSeats = 4
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 547 },
                        new() { Price = 500 },
                        new() { Price = 524 }
                    }
                }

            }
        },new()
        {
            Carriages = new List<Carriage>()
            {
                new()
                {
                    Number = 45,
                    Type = new CarriageType()
                    {
                        Name = "Econom",
                        NumberOfSeats = 67
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 147 },
                        new() { Price = 100 },
                        new() { Price = 124 }
                    }
                },
                new()
                {
                    Number = 45,
                    Type = new CarriageType()
                    {
                        Name = "Premium",
                        NumberOfSeats = 4
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 547 },
                        new() { Price = 500 },
                        new() { Price = 524 }
                    }
                }

            }
        },
        new()
        {
            Carriages = new List<Carriage>()
            {
                new()
                {
                    Number = 7,
                    Type = new CarriageType()
                    {
                        Name = "Cafe",
                        NumberOfSeats = 5
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 555 },
                        new() { Price = 678 },
                        new() { Price = 222 }
                    }
                },
                new()
                {
                    Number = 59,
                    Type = new CarriageType()
                    {
                        Name = "Econom",
                        NumberOfSeats = 78
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 678 },
                        new() { Price = 345 },
                        new() { Price = 134 }
                    }
                }

            }
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

        _repository = new TrainRepository(_context);
    }



    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]

    public void GetByIdTest(int id)
    {
        //given
        var entityToAdd = GetListTestEntities();
        foreach (var item in entityToAdd)
        {
            _context.Trains.Add(item);
            _context.SaveChanges();
        }
        var idAdded = _context.Trains.Find(id);

        //when
        var received = _repository.GetById(id);

        //then
        Assert.IsNotNull(received);
        Assert.IsFalse(received!.IsDeleted);
        Assert.AreEqual(idAdded.Carriages.First().Number, received.Carriages.First().Number);
        Assert.AreEqual(received, idAdded);

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
            _context.Trains.Add(item);
            _context.SaveChanges();

        }
        var expected = _context.Trains.Where(t => !t.IsDeleted || includeAll).ToList();

        //when
        var entities = (List<Train>)_repository.GetList(includeAll);

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
        var createdEntity = _context.Trains.FirstOrDefault(o => o.Id == id);

        Assert.IsNotNull(createdEntity);
        Assert.IsNotNull(createdEntity.Carriages);
        Assert.IsNotNull(createdEntity.Carriages.First().Number);
        Assert.IsNotNull(createdEntity.Carriages.First().Type);
        Assert.IsNotNull(createdEntity.Carriages.First().Tickets);
        Assert.AreEqual(5, createdEntity.Carriages.First().Number);
        Assert.AreEqual("Econom", createdEntity.Carriages.First().Type.Name);
        Assert.AreEqual(67, createdEntity.Carriages.First().Type.NumberOfSeats);
        Assert.AreEqual(147, createdEntity.Carriages.First().Tickets.First().Price);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //given
        var entityToAdd = GetTestEntity();
        _context.Trains.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = GetTestEntity();
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Carriages = entityToAdd.Carriages;
        entityToEdit.Carriages.First().Number = 111;
        entityToEdit.Carriages.First().Type.Name = "Cafe";
        entityToEdit.Carriages.First().Type.NumberOfSeats = 10;
        entityToEdit.Carriages.First().Tickets.First().Price = 1;

        //when 
        bool edited = _repository.Update(entityToEdit);

        //then
        var updatedTrain = _context.Trains.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsNotNull(updatedTrain);
        Assert.IsNotNull(updatedTrain.Carriages);
        Assert.IsNotNull(updatedTrain.Carriages.First().Number);
        Assert.IsNotNull(updatedTrain.Carriages.First().Type);
        Assert.IsNotNull(updatedTrain.Carriages.First().Tickets);
        Assert.AreEqual(111, updatedTrain.Carriages.First().Number);
        Assert.AreEqual("Cafe", updatedTrain.Carriages.First().Type.Name);
        Assert.AreEqual(10, updatedTrain.Carriages.First().Type.NumberOfSeats);
        Assert.AreEqual(1, updatedTrain.Carriages.First().Tickets.First().Price);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var entityToEdit = GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Trains.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        //then
        var updatedEntity = _context.Trains.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(updatedEntity);
        Assert.AreEqual(isDeleted, updatedEntity!.IsDeleted);
        Assert.IsNotNull(updatedEntity);
        Assert.IsNotNull(updatedEntity.Carriages);
        Assert.IsNotNull(updatedEntity.Carriages.First().Number);
        Assert.IsNotNull(updatedEntity.Carriages.First().Type);
        Assert.IsNotNull(updatedEntity.Carriages.First().Tickets);
        Assert.AreEqual(5, updatedEntity.Carriages.First().Number);
        Assert.AreEqual("Econom", updatedEntity.Carriages.First().Type.Name);
        Assert.AreEqual(67, updatedEntity.Carriages.First().Type.NumberOfSeats);
        Assert.AreEqual(147, updatedEntity.Carriages.First().Tickets.First().Price);

    }


}