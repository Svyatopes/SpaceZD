using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestMocks;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class TrainRepositoryTests
{
    private VeryVeryImportantContext _context;
    private TrainRepository _repository;   



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
        var entityToAdd = TrainRepositoryMocks.GetListTestEntities();
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
        var entitiesToAdd = TrainRepositoryMocks.GetListTestEntities();
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
        var entityToAdd = TrainRepositoryMocks.GetTestEntity();

        //when 
        int id = _repository.Add(entityToAdd);

        //then
        var createdEntity = _context.Trains.FirstOrDefault(o => o.Id == id);

        
        Assert.AreEqual("Econom", createdEntity.Carriages.First().Type.Name);
        Assert.AreEqual(67, createdEntity.Carriages.First().Type.NumberOfSeats);
        Assert.AreEqual(147, createdEntity.Carriages.First().Tickets.First().Price);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //given
        var entityToAdd = TrainRepositoryMocks.GetTestEntity();
        _context.Trains.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = TrainRepositoryMocks.GetTestEntity();
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Id = 7;
        entityToEdit.Carriages = entityToAdd.Carriages;
        entityToEdit.Carriages.First().Number = 111;
        

        //when 
        _repository.Update(entityToAdd, entityToEdit);

        //then
        
        Assert.IsNotNull(entityToAdd.Carriages);
        Assert.AreEqual(111, entityToEdit.Carriages.First().Number);
        //не должно меняться
        Assert.AreEqual(1, entityToAdd.Id);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var entityToEdit = TrainRepositoryMocks.GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Trains.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.Update(entityToEdit, isDeleted);

        //then
        
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }


}