using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.BusinessLayer.Tests.TestCaseSources;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.BusinessLayer.Tests;

public class UserServiceTest
{
    private Mock<IUserRepository> _userRepositoryMock;
    private IUserService _service;
    private readonly IMapper _mapper;

    public UserServiceTest()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }


    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new UserService(_mapper, _userRepositoryMock.Object);
    }


    [TestCase(6)]
    [TestCase(3)]
    public void AddTest(int expected)
    {
        // given
        _userRepositoryMock.Setup(u => u.Add(It.IsAny<User>())).Returns(expected);

        // when
        int actual = _service.Add(new UserModel(), "gtgt");

        // then
        _userRepositoryMock.Verify(s => s.Add(It.IsAny<User>()), Times.Once);
        Assert.AreEqual(expected, actual);

    }


    [TestCaseSource(typeof(UserServiceTestCaseSource), nameof(UserServiceTestCaseSource.GetListTestCases))]
    public void GetListTest(List<User> entities, List<UserModel> expected, int userId)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetList(false)).Returns(entities);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(entities[0]);

        // when
        var actual = _service.GetList(userId);

        // then       
        CollectionAssert.AreEqual(expected, actual);
        _userRepositoryMock.Verify(s => s.GetList(false), Times.Once);

    }


    [TestCase(Role.StationManager)]
    [TestCase(Role.TrainRouteManager)]
    [TestCase(Role.User)]
    public void GetListNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when        
        // then
        Assert.Throws<AuthorizationException>(() => _service.GetList(5));
    }


    [TestCaseSource(typeof(UserServiceTestCaseSource), nameof(UserServiceTestCaseSource.GetListDeletedTestCases))]
    public void GetListDeletedTest(List<User> entities, List<UserModel> expected, int userId)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetList(true)).Returns(entities);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(entities[0]);

        // when
        var actual = _service.GetListDelete(userId);

        // then       
        CollectionAssert.AreEqual(expected, actual);
        _userRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(userId), Times.Once);

    }


    [TestCase(Role.StationManager)]
    [TestCase(Role.TrainRouteManager)]
    [TestCase(Role.User)]
    public void GetListDeleteNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when        
        // then
        Assert.Throws<AuthorizationException>(() => _service.GetListDelete(5));
    }


    [Test]
    public void DeleteTest()
    {
        // given
        var entity = new User() { Role = Role.User, Id = 5 };
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);

        // when
        _service.Delete(5, 1);

        // then
        _userRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _userRepositoryMock.Verify(s => s.Update(entity, true), Times.Once);

    }


    [Test]
    public void DeleteNegativeAuthorizationExceptionTest()
    {
        // given
        var entity = new User() { };
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);

        // when
        // then
        Assert.Throws<AuthorizationException>(() => _service.Delete(5, 1));

    }


    [TestCase(Role.Admin, 10)]
    public void RestoreTest(Role role, int userId)
    {
        // given
        var entity = new User() { Role = Role.User, Name = "h", Login = "l", PasswordHash = "uj" };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role, Id = userId });
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>(), false));

        // when
        _service.Restore(15, userId);

        // then
        _userRepositoryMock.Verify(s => s.Update(It.IsAny<User>(), false), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
    }


    [TestCase(Role.User, 10)]
    [TestCase(Role.TrainRouteManager, 10)]
    [TestCase(Role.StationManager, 10)]
    public void RestoreNegativeAuthorizationExceptionTest(Role role, int userId)
    {
        // given
        var entity = new User() { Role = Role.User, Name = "h", Login = "l", PasswordHash = "uj" };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role, Id = userId });
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>(), false));

        // when     
        // then
        Assert.Throws<AuthorizationException>(() => _service.Restore(15, userId));

    }


    [Test]
    public void RestoreNegativeNotFoundExceptionTest()
    {
        // given
        var entity = new User() { Id = 6, Role = Role.User, Name = "h", Login = "l", PasswordHash = "uj" };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin, Id = 1 });
        _userRepositoryMock.Setup(x => x.Update(entity, false));
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when     
        // then
        Assert.Throws<NotFoundException>(() => _service.Restore(6, 1));

    }


    [Test]
    public void GetByIdTest()
    {
        // given
        var user = new User() { Name = "f", Role = Role.Admin, IsDeleted = false, Login = "y" };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(user);

        // when
        var actual = _service.GetById(2, user.Id);

        // then
        Assert.AreEqual(new UserModel
        {
            Name = user.Name,
            Login = user.Login,
            Role = user.Role,
            IsDeleted = user.IsDeleted
        }, actual);
    }


    [Test]
    public void GetByIdNegativeAuthorizationExceptionTest()
    {
        // given
        var user = new User() { Name = "f", Role = Role.User, IsDeleted = false, Login = "y" };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(user);

        // when
        // then
        Assert.Throws<AuthorizationException>(() => _service.GetById(2, user.Id));

    }


    [Test]
    public void GetByIdNegativeNotFoundExceptionTest()
    {
        // given
        var user = new User() { Name = "f", Role = Role.User, IsDeleted = false, Login = "y" };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.GetById(2, user.Id));

    }


    [Test]
    public void GetByLoginTest()
    {
        // given
        var user = new User() { Name = "f", Role = Role.Admin, IsDeleted = false, Login = "y" };

        _userRepositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns(user);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(user);

        // when
        var actual = _service.GetByLogin("y", user.Id);

        // then
        _userRepositoryMock.Verify(s => s.GetByLogin(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        Assert.AreEqual(new UserModel
        {
            Id = user.Id,
            Name = user.Name,
            Login = user.Login,
            IsDeleted = user.IsDeleted
        }, actual);
    }


    [Test]
    public void GetByLoginNegativeNotFoundExceptionTest()
    {
        // given
        var user = new User() { Role = Role.Admin, Login = "y" };

        _userRepositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns((User?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(user);

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.GetByLogin("y", user.Id));
    }


    [TestCase(Role.User)]
    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    [TestCase(Role.StationManager)]
    public void UpdateTest(Role role)
    {
        // given
        var entity = new User() { Role = role };
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
        var entityNew = new UserModel() { Name = "test" };

        // when
        _service.Update(5, entity.Id, entityNew);

        // then
        _userRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _userRepositoryMock.Verify(s => s.Update(entity, It.IsAny<User>()), Times.Once);
    }


    [TestCase(Role.User)]
    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    [TestCase(Role.StationManager)]
    public void UpdateNegativeNotFoundExceptionTest(Role role)
    {
        // given
        var entity = new User() { Role = role };
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);
        var entityNew = new UserModel() { Name = "test" };

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.Update(5, entity.Id, entityNew));
    }


    [TestCase(Role.Admin, Role.User)]
    [TestCase(Role.Admin, Role.TrainRouteManager)]
    [TestCase(Role.Admin, Role.StationManager)]
    public void UpdateRoleTest(Role role, Role newRole)
    {
        // given
        var entity = new User { Role = role, Id = 5 };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);

        // when
        _service.UpdateRole(5, newRole, 5);

        // then
        _userRepositoryMock.Verify(s => s.GetById(5), Times.Exactly(2));
        _userRepositoryMock.Verify(s => s.UpdateRole(It.IsAny<User>(), It.IsAny<Role>()), Times.Once);
    }


    [TestCase(Role.User)]
    [TestCase(Role.TrainRouteManager)]
    [TestCase(Role.StationManager)]
    public void UpdateRoleNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        var entity = new User { Role = role, Id = 5 };
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);

        // when
        // then
        Assert.Throws<AuthorizationException>(() => _service.UpdateRole(5, Role.Admin, 5));
    }


    [Test]
    public void UpdateRoleNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.UpdateRole(5, Role.Admin, 5));
    }
}