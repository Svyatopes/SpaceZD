using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.DataLayer.Tests;

public class CarriageTypeRepositoryTests
{
    private VeryVeryImportantContext _context;
    private IRepositorySoftDelete<CarriageType> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _repository = new CarriageTypeRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Test]
    public void GetByIdTest()
    {
        // given
        var entityToAdd = TestEntity;

        _context.CarriageTypes.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        // when
        var receivedEntity = _repository.GetById(idAdded);

        // then
        Assert.IsNotNull(receivedEntity);
        Assert.IsFalse(receivedEntity!.IsDeleted);
        AssertTestCarriageType(receivedEntity);
    }

    [Test]
    public void GetListTest()
    {
        // given
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.CarriageTypes.Add(entityToAdd);
        _context.CarriageTypes.Add(secondEntityToAdd);
        _context.CarriageTypes.Add(thirdEntityToAdd);
        _context.SaveChanges();

        // when
        var list = (List<CarriageType>)_repository.GetList();

        // then
        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Count);

        var entityToCheck = list[0];
        Assert.IsNotNull(entityToCheck);
        Assert.IsFalse(entityToCheck.IsDeleted);
        AssertTestCarriageType(entityToCheck);
    }


    [Test]
    public void GetListAllIncludedTest()
    {
        // given
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.CarriageTypes.Add(entityToAdd);
        _context.CarriageTypes.Add(secondEntityToAdd);
        _context.CarriageTypes.Add(thirdEntityToAdd);
        _context.SaveChanges();

        // when
        var list = (List<CarriageType>)_repository.GetList(true);

        // then
        Assert.IsNotNull(list);
        Assert.AreEqual(3, list.Count);

        var entityToCheck = list[2];
        Assert.IsNotNull(entityToCheck);
        Assert.IsTrue(entityToCheck.IsDeleted);
        AssertTestCarriageType(entityToCheck);
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = TestEntity;

        // when 
        int id = _repository.Add(entityToAdd);

        // then
        var createdEntity = _context.CarriageTypes.FirstOrDefault(o => o.Id == id);

        AssertTestCarriageType(createdEntity!);
    }

    [Test]
    public void UpdateEntityTest()
    {
        // given
        var entityToAdd = TestEntity;
        _context.CarriageTypes.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = TestEntity;
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Name = "qwertyuiop";
        entityToEdit.NumberOfSeats = 9;

        // when 
        bool edited = _repository.Update(entityToEdit);

        // then
        var entityToUpdated = _context.CarriageTypes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        AssertTestCarriageType(entityToUpdated!, entityToEdit.Name, entityToEdit.NumberOfSeats);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var entityToEdit = TestEntity;
        entityToEdit.IsDeleted = !isDeleted;
        _context.CarriageTypes.Add(entityToEdit);
        _context.SaveChanges();

        // when 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        // then
        var entityToUpdated = _context.CarriageTypes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(entityToUpdated);
        Assert.AreEqual(isDeleted, entityToUpdated!.IsDeleted);
        AssertTestCarriageType(entityToUpdated);
    }

    private CarriageType TestEntity => new()
    {
        Name = "Купе",
        NumberOfSeats = 4
    };

    private void AssertTestCarriageType(CarriageType carriageType, string name = "Купе", int numberOfSeats = 4)
    {
        Assert.IsNotNull(carriageType);
        Assert.AreEqual(name, carriageType.Name);
        Assert.AreEqual(numberOfSeats, carriageType.NumberOfSeats);
    }
}