using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestMocks;

namespace SpaceZD.DataLayer.Tests;

public class CarriageTypeRepositoryTests
{
    private VeryVeryImportantContext _context;
    private IRepositorySoftDeleteNewUpdate<CarriageType> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase("Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _repository = new CarriageTypeRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // seed
        _context.CarriageTypes.AddRange(CarriageTypeRepositoryMocks.GetCarriageTypes());
        _context.SaveChanges();
    }

    [TestCaseSource(typeof(CarriageTypeRepositoryMocks), nameof(CarriageTypeRepositoryMocks.GetMockFromGetByIdTest))]
    public void GetByIdTest(int id, CarriageType expected)
    {
        // when
        var actual = _repository.GetById(id);

        // then
        Assert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(CarriageTypeRepositoryMocks), nameof(CarriageTypeRepositoryMocks.GetMockFromGetListTest))]
    public void GetListTest(bool includeAll, List<CarriageType> expected)
    {
        // when
        var actual = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = CarriageTypeRepositoryMocks.GetCarriageType();

        // when 
        int id = _repository.Add(entityToAdd);

        // then
        var entityOnCreate = _context.CarriageTypes.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(entityOnCreate, entityToAdd);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateEntityTest(int id)
    {
        // given
        var entityToEdit = _context.CarriageTypes.FirstOrDefault(o => o.Id == id);
        var entityUpdate = new CarriageType { Name = "qwertyuiop", NumberOfSeats = 3, IsDeleted = !entityToEdit!.IsDeleted };

        // when 
        _repository.Update(entityToEdit, entityUpdate);

        // then
        Assert.AreEqual(entityUpdate.Name, entityToEdit.Name);
        Assert.AreEqual(entityUpdate.NumberOfSeats, entityToEdit.NumberOfSeats);
        Assert.AreNotEqual(entityUpdate.IsDeleted, entityToEdit.IsDeleted);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var entityToEdit = CarriageTypeRepositoryMocks.GetCarriageType();
        entityToEdit.IsDeleted = !isDeleted;
        _context.CarriageTypes.Add(entityToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(entityToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }
}