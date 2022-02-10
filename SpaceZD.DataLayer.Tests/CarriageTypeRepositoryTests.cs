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
        var carriageTypes = new CarriageType[]
        {
            new()
            {
                Name = "Плацкарт",
                NumberOfSeats = 5
            },
            new()
            {
                Name = "Ласточка",
                NumberOfSeats = 7,
                IsDeleted = true
            },
            new()
            {
                Name = "Сапсан",
                NumberOfSeats = 8
            }
        };
        _context.CarriageTypes.AddRange(carriageTypes);
        _context.SaveChanges();
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetByIdTest(int id)
    {
        // given
        var expectedEntity = _context.CarriageTypes.Find(id);

        // when
        var receivedEntity = _repository.GetById(id);

        // then
        Assert.AreEqual(expectedEntity, receivedEntity);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void GetListTest(bool includeAll)
    {
        // given
        var expected = _context.CarriageTypes.Where(t => !t.IsDeleted || includeAll).ToList();

        // when
        var list = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, list);
    }

    [Test]
    public void AddTest()
    {
        // given
        var entityToAdd = TestEntity;

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
        var entityToEdit = TestEntity;
        entityToEdit.IsDeleted = !isDeleted;
        _context.CarriageTypes.Add(entityToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(entityToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }

    private CarriageType TestEntity => new()
    {
        Name = "Купе",
        NumberOfSeats = 4
    };
}