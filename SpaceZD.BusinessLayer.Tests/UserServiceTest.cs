using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class UserServiceTest
{
    /*
    private Mock<IUserRepository> _repositoryMock;
    private readonly IMapper _mapper;

    public UserServiceTest()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }
    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IUserRepository>();
    }

    public static IEnumerable<TestCaseData> GetUser()
    {
        yield return new TestCaseData(new User { Id = 1, Name = "Sasha", Login = "Sashaaa", PasswordHash = "ierhkjdfhds", Role = Role.StationManager, IsDeleted = false });
        yield return new TestCaseData(new User { Id = 2, Name = "Masha", Login = "Mashaaa", PasswordHash = "ewdfrgthgfrde", Role = Role.User, IsDeleted = true });
        yield return new TestCaseData(new User { Id = 3, Name = "Dasha", Login = "Dashaaa", PasswordHash = "hjngtrfewdrt", Role = Role.Admin, IsDeleted = false });
        yield return new TestCaseData(new User { Id = 4, Name = "Pasha", Login = "Pashaaa", PasswordHash = "erfgthnjytgr", Role = Role.TrainRouteManager, IsDeleted = false });
    }

    public static IEnumerable<TestCaseData> GetListUsersNotDeleted()
    {
        yield return new TestCaseData(new List<User>
        {
            new() { Id = 1, Name = "Sasha", Login = "Sashaaa", PasswordHash = "ierhkjdfhds", Role = Role.StationManager, IsDeleted = false },
            new() { Id = 2, Name = "Masha", Login = "Mashaaa", PasswordHash = "ewdfrgthgfrde", Role = Role.User, IsDeleted = false },
            new() { Id = 3, Name = "Dasha", Login = "Dashaaa", PasswordHash = "hjngtrfewdrt", Role = Role.Admin, IsDeleted = false },
            new() { Id = 4, Name = "Pasha", Login = "Pashaaa", PasswordHash = "erfgthnjytgr", Role = Role.TrainRouteManager, IsDeleted = false }
        });
        yield return new TestCaseData(new List<User>
        {
            new() { Id = 5, Name = "Natasha", Login = "Natashaaa", PasswordHash = "fgbhnjngfd", Role = Role.User, IsDeleted = false },
            new() { Id = 6, Name = "Lesha", Login = "Leshaaa", PasswordHash = "rtbethhah", Role = Role.TrainRouteManager, IsDeleted = false },
            new() { Id = 7, Name = "Misha", Login = "Mishaaa", PasswordHash = "dfvgbtredfvgbtr", Role = Role.User, IsDeleted = false },
            new() { Id = 8, Name = "Grisha", Login = "Grishaaa", PasswordHash = "kmjhytgfbnhjytr", Role = Role.StationManager, IsDeleted = false }

        });
    }

    public List<Person> GetListPersonFromUser()
    {
        return new List<Person>
        {
            new() { Id = 1, FirstName = "Sasha", LastName = "Sashaaa", Patronymic = "Sashaaaaa", Passport = "4567", User = new User(){ Id = 1 }, IsDeleted = false },
            new() { Id = 1, FirstName = "Masha", LastName = "Mashaaa", Patronymic = "Mashaaaaa", Passport = "8765", User = new User(){ Id = 2 }, IsDeleted = false },
            new() { Id = 1, FirstName = "Pasha", LastName = "Pashaaa", Patronymic = "Pashaaaaa", Passport = "66666", User = new User(){ Id = 1 }, IsDeleted = false },
            new() { Id = 1, FirstName = "Dasha", LastName = "Dashaaa", Patronymic = "Dashaaaaa", Passport = "987654", User = new User(){ Id = 3 }, IsDeleted = false }
        };
    }

    public static IEnumerable<TestCaseData> GetListUsersDeleted()
    {
        yield return new TestCaseData(new List<User>
        {
            new() { Id = 9, Name = "Sasha", Login = "Sashaaa", PasswordHash = "ierhkjdfhds", Role = Role.StationManager, IsDeleted = true },
            new() { Id = 10, Name = "Masha", Login = "Mashaaa", PasswordHash = "ewdfrgthgfrde", Role = Role.User, IsDeleted = true },
            new() { Id = 11, Name = "Dasha", Login = "Dashaaa", PasswordHash = "hjngtrfewdrt", Role = Role.Admin, IsDeleted = false },
            new() { Id = 12, Name = "Pasha", Login = "Pashaaa", PasswordHash = "erfgthnjytgr", Role = Role.TrainRouteManager, IsDeleted = true }
    });
        yield return new TestCaseData(new List<User>
        {
            new() { Id = 13, Name = "Natasha", Login = "Natashaaa", PasswordHash = "fgbhnjngfd", Role = Role.User, IsDeleted = false },
            new() { Id = 14, Name = "Lesha", Login = "Leshaaa", PasswordHash = "rtbethhah", Role = Role.TrainRouteManager, IsDeleted = true },
            new() { Id = 15, Name = "Misha", Login = "Mishaaa", PasswordHash = "dfvgbtredfvgbtr", Role = Role.User, IsDeleted = true },
            new() { Id = 16, Name = "Grisha", Login = "Grishaaa", PasswordHash = "kmjhytgfbnhjytr", Role = Role.StationManager, IsDeleted = false }

        });
    }




    [TestCaseSource(nameof(GetListUsersNotDeleted))]
    public void GetListTest(List<User> entities)
    {
        // given
        _repositoryMock.Setup(x => x.GetList(false)).Returns(entities);
        var service = new UserService(_repositoryMock.Object, _mapper);
        var expected = entities.Select(entity => new UserModel
        {
            Name = entity.Name,
            Login = entity.Login,
            Role = entity.Role,
            IsDeleted = entity.IsDeleted,
            PasswordHash = entity.PasswordHash
        }).ToList();  
                                   
        // when
        var actual = service.GetList();

        // then
        CollectionAssert.AreEqual(expected, actual);
    }


    [TestCaseSource(nameof(GetListUsersDeleted))]
    public void GetListDeletedTest(List<User> entities)
    {
        // given
        _repositoryMock.Setup(x => x.GetList(true)).Returns(entities);
        var service = new UserService(_repositoryMock.Object, _mapper);        
        var expected = entities.Where(t => t.IsDeleted).Select(entity => new UserModel
        {
            Id = entity.Id, 
            Name = entity.Name,
            Login = entity.Login,
            Role = entity.Role,
            IsDeleted = entity.IsDeleted,
            PasswordHash = entity.PasswordHash
        }).ToList();

        // when
        var actual = service.GetListDeleted();

        // then
        CollectionAssert.AreEqual(expected, actual);
    }



    

    [TestCaseSource(nameof(GetUser))]
    public void GetByIdTest(User entity)
    {
        // given
        _repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
        var service = new UserService(_repositoryMock.Object, _mapper);

        // when
        var actual = service.GetById(2);

        // then
        Assert.AreEqual(new UserModel
        {
            Name = entity.Name,
            Login = entity.Login,             
            IsDeleted = entity.IsDeleted
        }, actual);        
    }
    
    [TestCaseSource(nameof(GetUser))]
    public void GetByLogin(User entity)
    {
        // given
        _repositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns(entity);
        var service = new UserService(_repositoryMock.Object, _mapper);

        // when
        var actual = service.GetByLogin("Masha");

        // then
        Assert.AreEqual(new UserModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Login = entity.Login,             
            IsDeleted = entity.IsDeleted
        }, actual);        
    }

    
    [TestCase(3)]
    public void AddTest(int expected)
    {
        // given
        _repositoryMock.Setup(x => x.Add(It.IsAny<User>())).Returns(expected);
        var service = new UserService(_repositoryMock.Object, _mapper);

        // when
        int actual = service.Add(new UserModel(), "gtgt");

        // then
        _repositoryMock.Verify(s => s.Add(It.IsAny<User>()), Times.Once);
        Assert.AreEqual(expected, actual);
        
        
    }

            
    [TestCase(true)]
    [TestCase(false)]
    public void DeleteTest(bool isDeleted)
    {
        // given
        var entity = new User();
        _repositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
        _repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
        var service = new UserService(_repositoryMock.Object, _mapper);

        // when
        service.Update(5, isDeleted);

        // then
        _repositoryMock.Verify(s => s.GetById(5), Times.Once);
        _repositoryMock.Verify(s => s.Update(entity, isDeleted), Times.Once);
        
    }

       

    
    [Test]
    public void UpdateTest()
    {
        // given
        var entity = new User();
        _repositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
        _repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
        var service = new UserService(_repositoryMock.Object, _mapper);
        var entityNew = new UserModel() { Name = "test" };
        // when
        service.Update(5, entityNew);

        // then
        _repositoryMock.Verify(s => s.GetById(5), Times.Once);
        _repositoryMock.Verify(s => s.Update(entity, It.IsAny<User>()), Times.Once);
    }
    */
    
}