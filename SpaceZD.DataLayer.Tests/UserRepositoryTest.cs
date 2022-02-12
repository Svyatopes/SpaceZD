using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class UserRepositoryTests
{
    private VeryVeryImportantContext _context;
    private UserRepository _repository;

    private User GetTestEntity() => new User()
    {
        Name = "Sasha",
        Login = "SashahsaS",
        PasswordHash = "hdebuvjcbh",
        Role = Enums.Role.TrainRouteManager,
        Orders = new List<Order>()
        {
            new()
            {
                StartStation = new TripStation()
                {
                    Station = new Station()
                    {
                        Name = "Spb"
                    }
                },
                EndStation = new TripStation()
                {
                    Station = new Station()
                    {
                        Name = "Msk"
                    }
                }
            }
        }
    };
    private List<User> GetListTestEntities() => new List<User>()
    {
        new()
        {
            Name = "Sasha",
            Login = "SashahsaS",
            PasswordHash = "hdebuvjcbh",
            Role = Enums.Role.TrainRouteManager,
            Orders = new List<Order>()
            {
                new()
                {
                    StartStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Spb"
                        }
                    },
                    EndStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Msk"
                        }
                    }
                }
            } 
        },
        new()
        {
            Name = "Masha",
            Login = "MashahsaM",
            PasswordHash = "wertyu",
            Role = Enums.Role.User,
            Orders = new List<Order>()
            {
                new()
                {
                    StartStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Pskov"
                        }
                    },
                    EndStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Sbp"
                        }
                    }
                }
            }
        },
        new()
        {
            Name = "Pasha",
            Login = "PashahsaP",
            PasswordHash = "asdfghj",
            Role = Enums.Role.TrainRouteManager,
            Orders = new List<Order>()
            {
                new()
                {
                    StartStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Msk"
                        }
                    },
                    EndStation = new TripStation()
                    {
                        Station = new Station()
                        {
                            Name = "Vladivostok"
                        }
                    }
                }
            }
        }
    };
   

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new UserRepository(_context);
    }


    [Test]    
    public void GetByIdTest()
    {
        //given
        var entityToAdd = GetTestEntity();
        _context.Users.Add(entityToAdd);
        _context.SaveChanges();
        
        var user = _context.Users.Find(1);

        //when
        var received = _repository.GetById(1);

        //then
        Assert.AreEqual(user, received);
        Assert.AreEqual("Sasha", received.Name);
        Assert.AreEqual("SashahsaS", received.Login);

    }


    [TestCase(true)]
    [TestCase(false)]
    public void GetListTest(bool includeAll)
    {
        //given
        var entitiesToAdd = GetListTestEntities();
        entitiesToAdd.Last().IsDeleted = true;
        foreach (var item in entitiesToAdd)
        {
            _context.Users.Add(item);
            _context.SaveChanges();

        }
        var expected = _context.Users.Where(t => !t.IsDeleted || includeAll).ToList();

        //when
        var entities = (List<User>)_repository.GetList(includeAll);

        //then
        Assert.IsNotNull(entities);
        Assert.AreEqual(expected.Count, entities.Count);
        CollectionAssert.AreEqual(expected, entities);

    }

    

    [Test]
    public void AddTest()
    {
        //given
        var entityToAdd = GetTestEntity();

        //when 
        int id = _repository.Add(entityToAdd);

        //then
        var createdEntity = _context.Users.FirstOrDefault(o => o.Id == id);

        
        Assert.AreEqual("Sasha", createdEntity.Name);
        Assert.AreEqual("SashahsaS", createdEntity.Login);
        Assert.AreEqual("hdebuvjcbh", createdEntity.PasswordHash);
        Assert.AreEqual(Enums.Role.TrainRouteManager, createdEntity.Role);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //given
        var entityToAdd = GetTestEntity();
        _context.Users.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = GetTestEntity();
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Name = "Masha";
        entityToEdit.Login = "MashahsaM";
        entityToEdit.PasswordHash = "sjvbgfg";
        entityToEdit.Role = Enums.Role.User;

        //when 
        _repository.Update(entityToAdd, entityToEdit);

        //then
        var updatedEntity = _context.Users.FirstOrDefault(o => o.Id == entityToEdit.Id);

        
        Assert.AreEqual("Masha", updatedEntity.Name);
        //не должно меняться
        Assert.AreEqual("SashahsaS", updatedEntity.Login);
        Assert.AreEqual("hdebuvjcbh", updatedEntity.PasswordHash);
        Assert.AreEqual(Enums.Role.TrainRouteManager, updatedEntity.Role);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var entityToEdit = GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Users.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.Update(entityToEdit, isDeleted);

        //then
        
        Assert.AreEqual(isDeleted, entityToEdit.IsDeleted);
    }


}