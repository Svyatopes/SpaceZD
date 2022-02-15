using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;
using SpaceZD.DataLayer.Tests.TestMocks;

namespace SpaceZD.DataLayer.Tests;

public class PlatformRepositoryTests
{
    private VeryVeryImportantContext _context;
    private PlatformRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new PlatformRepository(_context);

        //seed
        var platforms = PlatformRepositoryMocks.GetPlatforms();
        _context.Platforms.AddRange(platforms);
        _context.SaveChanges();
    }



    [Test]
    public void GetByIdTest()
    {
        //given
        var platform = _context.Platforms.First();

        //when
        var receivedPlatform = _repository.GetById(platform.Id);

        //then
        Assert.AreEqual(platform, receivedPlatform);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void GetListAllIncludedTest(bool allIncluded)
    {
        //given
        var platforms = _context.Platforms.Where(o => !o.IsDeleted || allIncluded);

        //when
        var platformsInDB = (List<Platform>)_repository.GetList(allIncluded);

        //then
        Assert.AreEqual(platforms, platformsInDB);

    }

    [Test]
    public void AddTest()
    {
        //given
        var platformToAdd = PlatformRepositoryMocks.GetPlatform();

        //when 
        int id = _repository.Add(platformToAdd);

        //then
        var createdPlatform = _context.Platforms.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(platformToAdd, createdPlatform);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //given
        var oldPlatform = _context.Platforms.First();


        var platform = PlatformRepositoryMocks.GetPlatform();
        platform.Number = 42;

        //SHOULDN'T CHANGED IN DB, because platform can't migrate to another station physically
        platform.Station = new Station { Name = "Moscow" };
        platform.IsDeleted = true;

        //when 
        _repository.Update(oldPlatform, platform);

        //then
        var updatedPlatform = _context.Platforms.FirstOrDefault(o => o.Id == oldPlatform.Id);

        Assert.AreEqual(platform.Number, updatedPlatform!.Number);
        Assert.AreNotEqual(platform.Station.Name, updatedPlatform.Station.Name);
        Assert.AreNotEqual(platform.IsDeleted, updatedPlatform.IsDeleted);

    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var platformToEdit = PlatformRepositoryMocks.GetPlatform();
        platformToEdit.IsDeleted = !isDeleted;
        _context.Platforms.Add(platformToEdit);
        _context.SaveChanges();



        //when 
        _repository.Update(platformToEdit, isDeleted);

        //then

        var updatedPlatform = _context.Platforms.FirstOrDefault(o => o.Id == platformToEdit.Id);
        Assert.AreEqual(isDeleted, updatedPlatform!.IsDeleted);
        Assert.AreEqual(platformToEdit, updatedPlatform);
    }
}