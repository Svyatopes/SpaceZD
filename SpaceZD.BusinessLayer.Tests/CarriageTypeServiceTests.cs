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
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class CarriageTypeServiceTests
{
    private Mock<IRepositorySoftDelete<CarriageType>> _carriageTypeRepositoryMock;
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
        _service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);
    }

    // Add
    [TestCase(45)]
    public void AddTest(int expected)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Add(It.IsAny<CarriageType>())).Returns(expected);

        // when
        int actual = _service.Add(new CarriageTypeModel());

        // then
        _carriageTypeRepositoryMock.Verify(s => s.Add(It.IsAny<CarriageType>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }


    // GetById
    [TestCaseSource(typeof(CarriageTypeServiceTestCaseSource), nameof(CarriageTypeServiceTestCaseSource.GetTestCaseDataForGetByIdTest))]
    public void GetByIdTest(CarriageType carriageType)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);

        // when
        var actual = _service.GetById(5);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        Assert.AreEqual(
            new CarriageTypeModel
            {
                Name = carriageType.Name, NumberOfSeats = carriageType.NumberOfSeats, IsDeleted = carriageType.IsDeleted
            },
            actual);
    }

    [Test]
    public void GetByIdNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.GetById(10));
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

        // when
        var actual = _service.GetListDeleted();

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        CollectionAssert.AreEqual(expected, actual);
    }


    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        var carriageType = new CarriageType();
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), true));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);

        // when
        _service.Delete(45);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _carriageTypeRepositoryMock.Verify(s => s.Update(carriageType, true), Times.Once);
    }

    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), true));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(10));
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var carriageType = new CarriageType();
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), false));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);

        // when
        _service.Restore(45);

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _carriageTypeRepositoryMock.Verify(s => s.Update(carriageType, false), Times.Once);
    }

    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), false));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(10));
    }


    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        var carriageType = new CarriageType();
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), It.IsAny<CarriageType>()));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);

        // when
        _service.Update(45, new CarriageTypeModel());

        // then
        _carriageTypeRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _carriageTypeRepositoryMock.Verify(s => s.Update(carriageType, It.IsAny<CarriageType>()), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>(), It.IsAny<CarriageType>()));
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Update(10, new CarriageTypeModel()));
    }
}