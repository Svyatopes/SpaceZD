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
        //arrange
        var entityToAdd = TestEntity;

        _context.CarriageTypes.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        //act
        var receivedEntity = _repository.GetById(idAdded);

        //assert
        Assert.IsNotNull(receivedEntity);
        Assert.IsFalse(receivedEntity!.IsDeleted);
        AsserTestCarriageType(receivedEntity);
    }

    [Test]
    public void GetListTest()
    {
        //arrange
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.CarriageTypes.Add(entityToAdd);
        _context.CarriageTypes.Add(secondEntityToAdd);
        _context.CarriageTypes.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var list = (List<CarriageType>)_repository.GetList();

        //assert
        Assert.IsNotNull(list);
        Assert.AreEqual(2, list.Count);

        var entityToCheck = list[0];
        Assert.IsNotNull(entityToCheck);
        Assert.IsFalse(entityToCheck.IsDeleted);
        AsserTestCarriageType(entityToCheck);
    }


    [Test]
    public void GetListAllIncludedTest()
    {
        //arrange
        var entityToAdd = TestEntity;
        var secondEntityToAdd = TestEntity;
        var thirdEntityToAdd = TestEntity;
        thirdEntityToAdd.IsDeleted = true;

        _context.CarriageTypes.Add(entityToAdd);
        _context.CarriageTypes.Add(secondEntityToAdd);
        _context.CarriageTypes.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var list = (List<CarriageType>)_repository.GetList(true);

        //assert
        Assert.IsNotNull(list);
        Assert.AreEqual(3, list.Count);

        var entityToCheck = list[2];
        Assert.IsNotNull(entityToCheck);
        Assert.IsTrue(entityToCheck.IsDeleted);
        AsserTestCarriageType(entityToCheck);
    }

    [Test]
    public void AddTest()
    {
        //arrange
        var entityToAdd = TestEntity;

        //act 
        int id = _repository.Add(entityToAdd);

        //assert
        var createdEntity = _context.CarriageTypes.FirstOrDefault(o => o.Id == id);

        AsserTestCarriageType(createdEntity!);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //arrange
        var entityToAdd = TestEntity;
        _context.CarriageTypes.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = TestEntity;
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Name = "qwertyuiop";
        entityToEdit.NumberOfSeats = 9;

        //act 
        bool edited = _repository.Update(entityToEdit);

        //assert
        var entityToUpdated = _context.CarriageTypes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        AsserTestCarriageType(entityToUpdated!, entityToEdit.Name, entityToEdit.NumberOfSeats);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //arrange
        var entityToEdit = TestEntity;
        entityToEdit.IsDeleted = !isDeleted;
        _context.CarriageTypes.Add(entityToEdit);
        _context.SaveChanges();

        //act 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        //assert
        var entityToUpdated = _context.CarriageTypes.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(entityToUpdated);
        Assert.AreEqual(isDeleted, entityToUpdated!.IsDeleted);
        AsserTestCarriageType(entityToUpdated);
    }

    private CarriageType TestEntity => new()
                                       {
                                           Name = "Купе",
                                           NumberOfSeats = 4
                                       };

    private void AsserTestCarriageType(CarriageType carriageType, string name = "Купе", int numberOfSeats = 4)
    {
        Assert.IsNotNull(carriageType);
        Assert.AreEqual(name, carriageType.Name);
        Assert.AreEqual(numberOfSeats, carriageType.NumberOfSeats);
    }
}