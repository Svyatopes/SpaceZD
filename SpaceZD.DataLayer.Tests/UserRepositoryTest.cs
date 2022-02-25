using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
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
        Role = Role.TrainRouteManager,
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
            Role = Role.TrainRouteManager,
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
            Role = Role.User,
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
            Role = Role.TrainRouteManager,
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

    private List<Person> GetPersons() => new List<Person>
    {
            new Person
            {
                FirstName = "Klark",
                LastName = "Kent",
                Patronymic = "KalEl",
                Passport = "7777666555",
                User = new User(){ Name = "K" , Login = "KK", PasswordHash ="hgeurgeerj"}
            },
            new Person
            {
                FirstName = "Sara",
                LastName = "Konor",
                Patronymic = "Vyacheslavovna",
                Passport = "3005123456",
                IsDeleted = true,
                User = new User(){ Name = "S" , Login = "SS", PasswordHash ="kjhgytfdr"}
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

    [TestCase("MashahsaM")]
    [TestCase("SashahsaS")]
    [TestCase("PashahsaP")]
    public void GetByLoginTest(string login)
    {
        //given
        var entitiesToAdd = GetListTestEntities();
        foreach (var item in entitiesToAdd)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
        }

        var user = _context.Users.FirstOrDefault(t => t.Login == login);

        //when
        var received = _repository.GetByLogin(login);

        //then
        Assert.AreEqual(user, received);

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


    [TestCase(1)]
    [TestCase(2)]
    public void GetListUserPersons(int id)
    {
        //given
        var entitiesToAdd = GetPersons();
        foreach (var item in entitiesToAdd)
        {
            _context.Persons.Add(item);
            _context.SaveChanges();

        }
        var expected = _context.Persons.Where(p => p.User.Id == id && !p.IsDeleted).ToList();

        //when
        var entities = _repository.GetListUserPersons(id);

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

    [TestCase(Role.User)]
    [TestCase(Role.StationManager)]
    public void UpdateRoleTest(Role role)
    {
        //given
        var entityToEdit = GetTestEntity();
        _context.Users.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.UpdateRole(entityToEdit, role);

        //then

        Assert.AreEqual(role, entityToEdit.Role);
    }
    
    [TestCase("sdfghjkhgfd")]
    [TestCase("ertyuioiuytre")]
    public void UpdatePasswordHashTest(string passwordHash)
    {
        //given
        var entityToEdit = GetTestEntity();
        _context.Users.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.UpdatePassword(entityToEdit, passwordHash);

        //then

        Assert.AreEqual(passwordHash, entityToEdit.PasswordHash);
    }


}