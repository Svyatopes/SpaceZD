using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;
internal class CarriageRepositoryTests
{

    private VeryVeryImportantContext _context;
    private IRepositorySoftDelete<Carriage> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                    .UseInMemoryDatabase("Test")
                    .Options;
        _context = new VeryVeryImportantContext(options);
        _repository = new CarriageRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();


        // seed
        var carriages = new Carriage[]
        {
                new()
                {
                    Number=1,
                    Type = new CarriageType()
                    {
                        Name = "Плацкарт"
                    }
                },
                new()
                {
                    Number=2,
                    Type = new CarriageType()
                    {
                        Name = "Ласточка"
                    }
                },
                new()
                {
                    Number=3,
                    Type = new CarriageType()
                    {
                        Name = "Купе",
                        IsDeleted=true
                    }
                }

        };
        _context.Carriages.AddRange(carriages);
        _context.SaveChanges();

    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetByIdTest(int id)
    {
        // given
        var expected = _context.Carriages.Find(id);

        // when
        var received = _repository.GetById(id);

        // then
        Assert.AreEqual(expected, received);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void GetListTest(bool includeAll)
    {
        // given
        var expected = _context.Carriages.Where(t => !t.IsDeleted || includeAll).ToList();

        // when
        var list = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, list);
    }

    [Test]
    public void AddTest()
    {
        // given
        var carriageToAdd = GetCarriage;

        // when 
        int id = _repository.Add(carriageToAdd);

        // then
        var carriageOnCreate = _context.Carriages.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(carriageOnCreate, carriageToAdd);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateCarriageTest(int id)
    {
        // given
        var oldCarriageToEdit = _context.Carriages.FirstOrDefault(o => o.Id == id);
        var newCarriageToEdit = GetCarriage;

        // when 
        _repository.Update(oldCarriageToEdit, newCarriageToEdit);

        // then
        var carriageToUpdated = _context.Carriages.FirstOrDefault(o => o.Id == oldCarriageToEdit.Id);

        Assert.AreEqual(newCarriageToEdit.Number, carriageToUpdated.Number);
        Assert.AreEqual(newCarriageToEdit.Type, carriageToUpdated.Type);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var carriageToEdit = GetCarriage;
        carriageToEdit.IsDeleted = isDeleted;
        _context.Carriages.Add(carriageToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(carriageToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, carriageToEdit.IsDeleted);
    }

    private Carriage GetCarriage => new()
    {
            Number = 5,
            Type = new CarriageType()
            {
                Name = "СВ"
            }
    };
}
