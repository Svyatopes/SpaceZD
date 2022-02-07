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

    private void AsserTestEntity(Train entity)
    {
        Assert.IsNotNull(entity);
        Assert.IsNotNull(entity.Carriages);
        Assert.IsNotNull(entity.Carriages.First().Number);
        Assert.IsNotNull(entity.Carriages.First().Type);
        Assert.IsNotNull(entity.Carriages.First().Tickets);
        Assert.AreEqual(5, entity.Carriages.First().Number);
        Assert.AreEqual("Econom", entity.Carriages.First().Type.Name);
        Assert.AreEqual(67, entity.Carriages.First().Type.NumberOfSeats);
        Assert.AreEqual(147, entity.Carriages.First().Tickets.First().Price);
        

    }

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




    [Test]
    public void GetByIdTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();

        _context.Trains.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        //act
        var received = _repository.GetById(idAdded);

        //assert
        Assert.IsNotNull(received);
        Assert.IsFalse(received!.IsDeleted);
        AsserTestEntity(received);
    }



    [Test]
    public void GetListTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();
        var secondEntityToAdd = GetTestEntity();
        var thirdEntityToAdd = GetTestEntity();
        thirdEntityToAdd.IsDeleted = true;

        _context.Trains.Add(entityToAdd);
        _context.Trains.Add(secondEntityToAdd);
        _context.Trains.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var entities = (List<Train>)_repository.GetList();


        //assert

        Assert.IsNotNull(entities);
        Assert.AreEqual(2, entities.Count);

        var userToCheck = entities[0];
        Assert.IsNotNull(userToCheck);
        Assert.IsFalse(userToCheck.IsDeleted);
        AsserTestEntity(userToCheck);

    }


    [Test]
    public void GetListAllIncludedTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();
        var secondEntityToAdd = GetTestEntity();
        var thirdEntityToAdd = GetTestEntity();
        thirdEntityToAdd.IsDeleted = true;

        _context.Trains.Add(entityToAdd);
        _context.Trains.Add(secondEntityToAdd);
        _context.Trains.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var entities = (List<Train>)_repository.GetList(true);


        //assert

        Assert.IsNotNull(entities);
        Assert.AreEqual(3, entities.Count);

        var entityToCheck = entities[2];
        Assert.IsNotNull(entityToCheck);
        Assert.IsTrue(entityToCheck.IsDeleted);
        AsserTestEntity(entityToCheck);

    }

    [Test]
    public void AddTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();

        //act 
        int id = _repository.Add(entityToAdd);

        //assert
        var createdEntity = _context.Trains.FirstOrDefault(o => o.Id == id);

        AsserTestEntity(createdEntity);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //arrange
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



        //act 
        bool edited = _repository.Update(entityToEdit);

        //assert
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
        //arrange
        var entityToEdit = GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Trains.Add(entityToEdit);
        _context.SaveChanges();

        //act 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        //assert

        var updatedEntity = _context.Trains.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(updatedEntity);
        Assert.AreEqual(isDeleted, updatedEntity!.IsDeleted);
        AsserTestEntity(entityToEdit);
    }


}