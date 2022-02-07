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
        Price = 345
    };

    private void AsserTestEntity(Ticket entity)
    {
        Assert.IsNotNull(entity);
        Assert.IsNotNull(entity.Person);
        Assert.IsNotNull(entity.Person.LastName);
        Assert.IsNotNull(entity.Price);
        Assert.IsNotNull(entity.SeatNumber);
        Assert.IsNotNull(entity.Carriage);
        Assert.IsNotNull(entity.Carriage.Type);
        Assert.IsNotNull(entity.Carriage.Type.NumberOfSeats);
        Assert.AreEqual("Vasilev", entity.Person.LastName);
        Assert.AreEqual(345, entity.Price);
        Assert.AreEqual(56, entity.SeatNumber);
        Assert.AreEqual(67, entity.Carriage.Type.NumberOfSeats);
        Assert.IsFalse(entity.IsPetPlaceIncluded);
        Assert.IsFalse(entity.IsTeaIncluded);


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

        _repository = new TicketRepository(_context);
    }




    [Test]
    public void GetByIdTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();

        _context.Tickets.Add(entityToAdd);
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

        _context.Tickets.Add(entityToAdd);
        _context.Tickets.Add(secondEntityToAdd);
        _context.Tickets.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var entities = (List<Ticket>)_repository.GetList();


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
        var thirdTEntityToAdd = GetTestEntity();
        thirdTEntityToAdd.IsDeleted = true;

        _context.Tickets.Add(entityToAdd);
        _context.Tickets.Add(secondEntityToAdd);
        _context.Tickets.Add(thirdTEntityToAdd);
        _context.SaveChanges();

        //act
        var entities = (List<Ticket>)_repository.GetList(true);


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
        var createdEntity = _context.Tickets.FirstOrDefault(o => o.Id == id);

        AsserTestEntity(createdEntity);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();
        _context.Tickets.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = GetTestEntity();
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Person.LastName = "Petrov";
        entityToEdit.Person.FirstName = "Petya";
        entityToEdit.Price = 677;
        entityToEdit.SeatNumber = 2;
        entityToEdit.Carriage.Number = 7;
        entityToEdit.Carriage.Type.Name = "Cafe";        

           


        //act 
        bool edited = _repository.Update(entityToEdit);

        //assert
        var updatedEntity = _context.Tickets.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsNotNull(updatedEntity);
        Assert.IsNotNull(updatedEntity.Person);
        Assert.IsNotNull(updatedEntity.Person.LastName);
        Assert.IsNotNull(updatedEntity.Price);
        Assert.IsNotNull(updatedEntity.SeatNumber);
        Assert.IsNotNull(updatedEntity.Carriage);
        Assert.IsNotNull(updatedEntity.Carriage.Type);
        Assert.IsNotNull(updatedEntity.Carriage.Type.NumberOfSeats);
        Assert.AreEqual("Petrov", updatedEntity.Person.LastName);
        Assert.AreEqual("Petya", updatedEntity.Person.FirstName);
        Assert.AreEqual(677, updatedEntity.Price);
        Assert.AreEqual(2, updatedEntity.SeatNumber);
        Assert.AreEqual(7, updatedEntity.Carriage.Number);
        Assert.IsFalse(updatedEntity.IsPetPlaceIncluded);
        Assert.IsFalse(updatedEntity.IsTeaIncluded);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //arrange
        var entityToEdit = GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Tickets.Add(entityToEdit);
        _context.SaveChanges();

        //act 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        //assert

        var updatedEntity = _context.Tickets.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(updatedEntity);
        Assert.AreEqual(isDeleted, updatedEntity!.IsDeleted);
        AsserTestEntity(entityToEdit);
    }


}