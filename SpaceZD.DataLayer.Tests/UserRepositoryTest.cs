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
        Role = Enums.Role.TrainRouteManager       
    };

    private void AsserTestEntity(User entity)
    {
        Assert.IsNotNull(entity);
        Assert.IsNotNull(entity.Name);
        Assert.IsNotNull(entity.Login);
        Assert.IsNotNull(entity.PasswordHash);
        Assert.IsNotNull(entity.Role);
        Assert.AreEqual("Sasha", entity.Name);
        Assert.AreEqual("SashahsaS", entity.Login);
        Assert.AreEqual("hdebuvjcbh", entity.PasswordHash);
        Assert.AreEqual(Enums.Role.TrainRouteManager, entity.Role);
        
    }

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
        //arrange
        var entityToAdd = GetTestEntity();

        _context.Users.Add(entityToAdd);
        _context.SaveChanges();
        var idAdded = entityToAdd.Id;

        //act
        var received = _repository.GetById(idAdded);

        //assert
        Assert.IsNotNull(received);
        Assert.IsFalse(received!.IsDeleted);
        AsserTestEntity(received);
    }



    [Test]
    public void GetListTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();
        var secondEntityToAdd = GetTestEntity();
        var thirdEntityToAdd = GetTestEntity();
        thirdEntityToAdd.IsDeleted = true;

        _context.Users.Add(entityToAdd);
        _context.Users.Add(secondEntityToAdd);
        _context.Users.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var entities = (List<User>)_repository.GetList();


        //assert

        Assert.IsNotNull(entities);
        Assert.AreEqual(2, entities.Count);

        var entityToCheck = entities[0];
        Assert.IsNotNull(entityToCheck);
        Assert.IsFalse(entityToCheck.IsDeleted);
        AsserTestEntity(entityToCheck);

    }


    [Test]
    public void GetListAllIncludedTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();
        var secondEntityToAdd = GetTestEntity();
        var thirdEntityToAdd = GetTestEntity();
        thirdEntityToAdd.IsDeleted = true;

        _context.Users.Add(entityToAdd);
        _context.Users.Add(secondEntityToAdd);
        _context.Users.Add(thirdEntityToAdd);
        _context.SaveChanges();

        //act
        var entities = (List<User>)_repository.GetList(true);


        //assert

        Assert.IsNotNull(entities);
        Assert.AreEqual(3, entities.Count);

        var entityToCheck = entities[2];
        Assert.IsNotNull(entityToCheck);
        Assert.IsTrue(entityToCheck.IsDeleted);
        AsserTestEntity(entityToCheck);

    }

    [Test]
    public void AddTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();

        //act 
        int id = _repository.Add(entityToAdd);

        //assert
        var createdEntity = _context.Users.FirstOrDefault(o => o.Id == id);

        AsserTestEntity(createdEntity);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //arrange
        var entityToAdd = GetTestEntity();
        _context.Users.Add(entityToAdd);
        _context.SaveChanges();

        var entityToEdit = GetTestEntity();
        entityToEdit.Id = entityToAdd.Id;
        entityToEdit.Name = "Masha";
        entityToEdit.Login = "MashahsaM";
        entityToEdit.PasswordHash = "sjvbgfg";

        

        //act 
        bool edited = _repository.Update(entityToEdit);

        //assert
        var updatedEntity = _context.Users.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsNotNull(updatedEntity);
        Assert.IsNotNull(updatedEntity.Name);
        Assert.IsNotNull(updatedEntity.Login);
        Assert.IsNotNull(updatedEntity.PasswordHash);
        Assert.IsNotNull(updatedEntity.Role);
        Assert.AreEqual("Masha", updatedEntity.Name);
        Assert.AreEqual("MashahsaM", updatedEntity.Login);
        Assert.AreEqual("sjvbgfg", updatedEntity.PasswordHash);
        Assert.AreEqual(Enums.Role.TrainRouteManager, updatedEntity.Role);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //arrange
        var entityToEdit = GetTestEntity();
        entityToEdit.IsDeleted = !isDeleted;
        _context.Users.Add(entityToEdit);
        _context.SaveChanges();

        //act 
        bool edited = _repository.Update(entityToEdit.Id, isDeleted);

        //assert

        var updatedEntity = _context.Users.FirstOrDefault(o => o.Id == entityToEdit.Id);

        Assert.IsTrue(edited);
        Assert.IsNotNull(updatedEntity);
        Assert.AreEqual(isDeleted, updatedEntity!.IsDeleted);
        AsserTestEntity(updatedEntity);
    }

    
}