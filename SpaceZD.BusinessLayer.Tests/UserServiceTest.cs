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
        List<User> usersFiltredByIsDeletedProp = entities.Where(o => !o.IsDeleted).ToList();

        _userRepositoryMock.Setup(x => x.GetList(false)).Returns(usersFiltredByIsDeletedProp);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(entities[0]);

        List<UserModel> expectedFiltredByIsDeletedProp = expected.Where(o => !o.IsDeleted).ToList();

        // when
        var actual = _service.GetList(userId);

        // then       
        CollectionAssert.AreEqual(expectedFiltredByIsDeletedProp, actual);
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
        Assert.Throws<AuthorizationException>(()=>_service.GetList(5));
    }



    [TestCaseSource(typeof(UserServiceTestCaseSource), nameof(UserServiceTestCaseSource.GetListTestCases))]
    public void GetListDeletedTest(List<User> entities, List<UserModel> expected, int userId)
    {
        // given
        List<User> usersFiltredByIsDeletedProp = entities.Where(o => o.IsDeleted).ToList();

        _userRepositoryMock.Setup(x => x.GetList(true)).Returns(usersFiltredByIsDeletedProp);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(entities[0]);

        List<UserModel> expectedFiltredByIsDeletedProp = expected.Where(o => o.IsDeleted).ToList();

        // when
        var actual = _service.GetListDelete(userId);

        // then       
        CollectionAssert.AreEqual(expectedFiltredByIsDeletedProp, actual);
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



    //[TestCaseSource(nameof(GetUser))]
    //public void GetByIdTest(User entity, int userId)
    //{
    //    // given
    //    _repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
    //    var service = new UserService(_repositoryMock.Object, _mapper);

    //    // when
    //    var actual = service.GetById(2, userId);

    //    // then
    //    Assert.AreEqual(new UserModel
    //    {
    //        Name = entity.Name,
    //        Login = entity.Login,             
    //        IsDeleted = entity.IsDeleted
    //    }, actual);        
    //}

    //[TestCaseSource(nameof(GetUser))]
    //public void GetByLogin(User entity, int userId)
    //{
    //    // given
    //    _repositoryMock.Setup(x => x.GetByLogin(It.IsAny<string>())).Returns(entity);
    //    var service = new UserService(_repositoryMock.Object, _mapper);

    //    // when
    //    var actual = service.GetByLogin("Masha", userId);

    //    // then
    //    Assert.AreEqual(new UserModel
    //    {
    //        Id = entity.Id,
    //        Name = entity.Name,
    //        Login = entity.Login,             
    //        IsDeleted = entity.IsDeleted
    //    }, actual);        
    //}





    //[TestCase(true, 1)]
    //[TestCase(false, 1)]
    //public void DeleteTest(bool isDeleted, int userId)
    //{
    //    // given
    //    var entity = new User();
    //    _repositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
    //    _repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
    //    var service = new UserService(_repositoryMock.Object, _mapper);

    //    // when
    //    //service.Update(5, isDeleted, userId);

    //    // then
    //    _repositoryMock.Verify(s => s.GetById(5), Times.Once);
    //    _repositoryMock.Verify(s => s.Update(entity, isDeleted), Times.Once);

    //}




    //[Test]
    //public void UpdateTest(int userId)
    //{
    //    // given
    //    var entity = new User();
    //    _repositoryMock.Setup(x => x.Update(It.IsAny<User>(), true));
    //    _repositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(entity);
    //    var service = new UserService(_repositoryMock.Object, _mapper);
    //    var entityNew = new UserModel() { Name = "test" };
    //    // when
    //    service.Update(5, entityNew, userId);

    //    // then
    //    _repositoryMock.Verify(s => s.GetById(5), Times.Once);
    //    _repositoryMock.Verify(s => s.Update(entity, It.IsAny<User>()), Times.Once);
    //}


}