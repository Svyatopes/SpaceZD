using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestMocks;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class TicketRepositoryTests
{
    private VeryVeryImportantContext _context;
    private TicketRepository _repository;

    
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
        var entityToAdd = TicketRepositoryMocks.GetTestEntity();

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
        var entitiesToAdd = TicketRepositoryMocks.GetListTestEntities();
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
    
    
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void GetListByOrderId(int orderId)
    {
        //given
        var entitiesToAdd = TicketRepositoryMocks.GetListTestEntities();
        entitiesToAdd.Last().IsDeleted = true;
        foreach (var item in entitiesToAdd)
        {
            _context.Tickets.Add(item);
            _context.SaveChanges();
        }
            
    
        var expected = _context.Tickets.Where(t =>  t.Order.Id == orderId && !t.IsDeleted).ToList();

        //when
        var entities = (List<Ticket>)_repository.GetListByOrderId(orderId);

        //then
        Assert.IsNotNull(entities);
        Assert.AreEqual(expected.Count, entities.Count);
        CollectionAssert.AreEqual(expected, entities);

    }


    [Test]
    public void AddTest()
    {
        //given
        var entityToAdd = TicketRepositoryMocks.GetTestEntity();

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
        var entityToAdd = TicketRepositoryMocks.GetTestEntity();
        _context.Tickets.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = TicketRepositoryMocks.GetTestEntity();
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
        Assert.AreEqual("Petrov", updatedEntity.Person.LastName);
        //не должны были поменятся
        Assert.AreEqual(345, updatedEntity.Price);
        Assert.IsFalse(updatedEntity.IsPetPlaceIncluded);
        Assert.IsFalse(updatedEntity.IsTeaIncluded);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var entityToEdit = TicketRepositoryMocks.GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Tickets.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.Update(entityToEdit, isDeleted);

        //then
        
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
                
    }


}