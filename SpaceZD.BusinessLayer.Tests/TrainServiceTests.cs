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

public class TrainServiceTests
{
    private Mock<IRepositorySoftDelete<Train>> _trainRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private ITrainService _service;
    private readonly IMapper _mapper;

    public TrainServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _trainRepositoryMock = new Mock<IRepositorySoftDelete<Train>>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);
    }


    [TestCaseSource(typeof(TrainServiceTestCaseSource), nameof(TrainServiceTestCaseSource.GetListTestCases))]
    public void GetListTest(List<Train> trains, List<TrainModel> expectedTrainModels, int userId)
    {
        // given
        var trainsFiltredByIsDeletedProp = trains.Where(o => !o.IsDeleted || false).ToList();
        _trainRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(trainsFiltredByIsDeletedProp);
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = userId });
        expectedTrainModels = expectedTrainModels.Where(o => !o.IsDeleted || false).ToList();
        
        // when   
        var trainModels = _service.GetList(userId);

        // then
        CollectionAssert.AreEqual(expectedTrainModels, trainModels);
        _trainRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [TestCase(Role.StationManager)]
    [TestCase(Role.User)]
    public void GetListNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when        
        // then
        Assert.Throws<AuthorizationException>(() => _service.GetList(5));
    }


    [TestCaseSource(typeof(TrainServiceTestCaseSource), nameof(TrainServiceTestCaseSource.GetListTestCases))]
    public void GetListDeleteTest(List<Train> trains, List<TrainModel> expectedTrainModels, int userId)
    {
        // given
        var trainsFiltredByIsDeletedProp = trains.Where(o => !o.IsDeleted || true).ToList();
        _trainRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(trainsFiltredByIsDeletedProp);
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = userId });
        expectedTrainModels = expectedTrainModels.Where(o => !o.IsDeleted || true).ToList();
        
        // when   
        var trainModels = _service.GetList(userId);

        // then
        CollectionAssert.AreEqual(expectedTrainModels, trainModels);
        _trainRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [TestCase(Role.StationManager)]
    [TestCase(Role.User)]
    public void GetListDeleteNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when        
        // then
        Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(5));
    }


    [TestCaseSource(typeof(TrainServiceTestCaseSource), nameof(TrainServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Train train, TrainModel expected, int userId)
    {
        // given
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = userId });

        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(train);

        // when
        var actual = _service.GetById(5, userId);

        // then
        Assert.AreEqual(expected, actual);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        _trainRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
    }


    [TestCase(Role.StationManager)]
    [TestCase(Role.User)]
    public void GetByIdNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 3 });
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());

        // when
        // then
        Assert.Throws<AuthorizationException>(() => _service.GetById(5, 3));
    }


    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void GetByIdNegativeNotFoundExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 3 });
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Train?)null);

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.GetById(5, 3));
    }


    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void AddTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 3 });

        // when
        int actual = _service.Add(3);

        // then
        _trainRepositoryMock.Verify(s => s.Add(It.IsAny<Train>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }
    
    
    [TestCase(Role.User)]
    [TestCase(Role.StationManager)]
    public void AddNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 3 });

        // when     
        // then
        Assert.Throws<AuthorizationException>(() => _service.Add(3));
    }


    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void DeleteTest(Role role)
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train() { IsDeleted = false});
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 4 });

        // when
        _service.Delete(5, 4);

        // then
        _trainRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _trainRepositoryMock.Verify(s => s.Update(It.IsAny<Train>(), true), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [TestCase(Role.User)]
    [TestCase(Role.StationManager)]
    public void DeleteNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train() { IsDeleted = false});
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 4 });

        // when
        // then
        Assert.Throws<AuthorizationException>(() => _service.Delete(5, 4));
    }


    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void DeleteNegativeNotFoundExceptionTest(Role role)
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Train?)null);
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 4 });

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.Delete(5, 4));
    }


    [Test]    
    public void RestoreTest()
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train() { IsDeleted = true});
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = 4 });

        // when
        _service.Restore(5, 4);

        // then
        _trainRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _trainRepositoryMock.Verify(s => s.Update(It.IsAny<Train>(), false), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [TestCase(Role.User)]
    [TestCase(Role.StationManager)]
    [TestCase(Role.TrainRouteManager)]
    public void RestoreNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train() { IsDeleted = true});
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 4 });

        // when
        // then
        Assert.Throws<AuthorizationException>(() => _service.Restore(5, 4));
    }


    [Test]    
    public void RestoreNegativeNotFoundExceptionTest()
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Train?)null);
        _userRepositoryMock.Setup(u => u.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = 4 });

        // when
        // then
        Assert.Throws<NotFoundException>(() => _service.Restore(5, 4));
    }    
}