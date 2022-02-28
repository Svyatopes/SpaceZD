using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestMocks;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class UserRepositoryTests
{
    private VeryVeryImportantContext _context;
    private UserRepository _repository;

    
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
        var entityToAdd = UserRepositoryMocks.GetTestEntity();
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
        var entitiesToAdd = UserRepositoryMocks.GetListTestEntities();
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
        var entitiesToAdd = UserRepositoryMocks.GetListTestEntities();
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
        var entitiesToAdd = UserRepositoryMocks.GetPersons();
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
        var entityToAdd = UserRepositoryMocks.GetTestEntity();

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
        var entityToAdd = UserRepositoryMocks.GetTestEntity();
        _context.Users.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = UserRepositoryMocks.GetTestEntity();
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
        var entityToEdit = UserRepositoryMocks.GetTestEntity();
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
        var entityToEdit = UserRepositoryMocks.GetTestEntity();
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
        var entityToEdit = UserRepositoryMocks.GetTestEntity();
        _context.Users.Add(entityToEdit);
        _context.SaveChanges();

        //when 
        _repository.UpdatePassword(entityToEdit, passwordHash);

        //then

        Assert.AreEqual(passwordHash, entityToEdit.PasswordHash);
    }


}