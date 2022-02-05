using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.DataLayer.Tests;

public class Tests
{
    private VeryVeryImportantContext _context;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
    }

    [Test]
    public void Test1()
    {
        var repo = new UserRepository(_context);
        Assert.Pass();
    }
}