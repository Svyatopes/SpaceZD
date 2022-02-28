using System.Collections.Generic;
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

namespace SpaceZD.BusinessLayer.Tests;

public class CarriageTypeServiceTests
{
    private Mock<IRepositorySoftDelete<CarriageType>> _carriageTypeRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private ICarriageTypeService _service;
    private readonly IMapper _mapper;

    public CarriageTypeServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void Setup()
    {
        _carriageTypeRepositoryMock = new Mock<IRepositorySoftDelete<CarriageType>>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object, _userRepositoryMock.Object);
    }

    // Add
    [TestCase(45, Role.Admin)]
    [TestCase(45, Role.TrainRouteManager)]
    public void AddTest(int expected, Role role)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Add(It.IsAny<CarriageType>())).Returns(expected);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        int actual = _service.Add(45, new CarriageTypeModel());

        // then
        _carriageTypeRepositoryMock.Verify(s => s.Add(It.IsAny<CarriageType>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void AddNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Add(10, new CarriageTypeModel()));
    }

    [Test]
    public void AddNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Add(10, new CarriageTypeModel()));
    }


    // GetById
    [TestCaseSource(typeof(CarriageTypeServiceTestCaseSource), nameof(CarriageTypeServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(CarriageType carriageType, Role role)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        var actual = _service.GetById(45, 5);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        Assert.AreEqual(
            new CarriageTypeModel
            {
                Name = carriageType.Name, NumberOfSeats = carriageType.NumberOfSeats, PriceCoefficient = carriageType.PriceCoefficient, IsDeleted = carriageType.IsDeleted
            },
            actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10, 10));
    }

    [Test]
    public void GetByIdNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10, 10));
    }

    [Test]
    public void GetByIdNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetById(10, 10));
    }


    // GetList
    [TestCaseSource(typeof(CarriageTypeServiceTestCaseSource), nameof(CarriageTypeServiceTestCaseSource.GetTestCaseDataForGetListTest))]
    public void GetListTest(List<CarriageType> carriageTypes, List<CarriageTypeModel> expected)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetList(false)).Returns(carriageTypes);

        // when
        var actual = _service.GetList();

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestCaseSource(typeof(CarriageTypeServiceTestCaseSource), nameof(CarriageTypeServiceTestCaseSource.GetTestCaseDataForGetListDeletedTest))]
    public void GetListDeletedTest(List<CarriageType> carriageTypes, List<CarriageTypeModel> expected)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetList(true)).Returns(carriageTypes);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        var actual = _service.GetListDeleted(45);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetListDeletedNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetListDeleted(10));
    }

    [Test]
    public void GetListDeletedNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.TrainRouteManager });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(10));
    }


    //Delete
    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void DeleteTest(Role role)
    {
        // given
        var carriageType = new CarriageType();
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), true));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Delete(45, 45);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _carriageTypeRepositoryMock.Verify(s => s.Update(carriageType, true), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), true));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(45, 10));
    }

    [Test]
    public void DeleteNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(10, 10));
    }

    [Test]
    public void DeleteNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Delete(10, 10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var carriageType = new CarriageType();
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), false));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when
        _service.Restore(45, 45);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _carriageTypeRepositoryMock.Verify(s => s.Update(carriageType, false), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), false));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(45, 10));
    }

    [Test]
    public void RestoreNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(10, 10));
    }

    [Test]
    public void RestoreNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.TrainRouteManager });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Restore(10, 10));
    }


    //Update
    [TestCase(Role.Admin)]
    [TestCase(Role.TrainRouteManager)]
    public void UpdateTest(Role role)
    {
        // given
        var carriageType = new CarriageType();
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), It.IsAny<CarriageType>()));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        // when
        _service.Update(45, 45, new CarriageTypeModel());

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _carriageTypeRepositoryMock.Verify(s => s.Update(carriageType, It.IsAny<CarriageType>()), Times.Once);
        _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), It.IsAny<CarriageType>()));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(45, 10, new CarriageTypeModel()));
    }

    [Test]
    public void UpdateNegativeNotFoundExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new CarriageTypeModel()));
    }

    [Test]
    public void UpdateNegativeAuthorizationExceptionTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new CarriageTypeModel()));
    }
}