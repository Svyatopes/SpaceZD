using System;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class CarriageTypeServiceTests
{
    private readonly Mock<IRepositorySoftDelete<CarriageType>> _carriageTypeRepositoryMock;
    private readonly IMapper _mapper;

    public CarriageTypeServiceTests()
    {
        _carriageTypeRepositoryMock = new Mock<IRepositorySoftDelete<CarriageType>>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    // Add
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(28)]
    public void AddTest(int expected)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Add(It.IsAny<CarriageType>())).Returns(expected);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        // when
        int actual = service.Add(new CarriageTypeModel());

        // then
        Assert.AreEqual(expected, actual);
    }


    // GetById
    [TestCaseSource(nameof(GetCarriageType))]
    public void GetByIdTest(CarriageType carriageType)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriageType);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        Assert.AreEqual(new CarriageTypeModel { Name = carriageType.Name, NumberOfSeats = carriageType.NumberOfSeats, IsDeleted = carriageType.IsDeleted }, actual);
    }
    [Test]
    public void GetByIdNegativeTest()
    {
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        Assert.Throws<Exception>(() => service.GetById(10));
    }
    
    // GetList
    


    public static IEnumerable<TestCaseData> GetCarriageType()
    {
        yield return new TestCaseData(new CarriageType { Name = "Rbs", NumberOfSeats = 2, IsDeleted = true });
        yield return new TestCaseData(new CarriageType { Name = "Купе", NumberOfSeats = 3, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Ласточка", NumberOfSeats = 2, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Сапсан", NumberOfSeats = 3, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Плацкарт", NumberOfSeats = 4, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Сидячие места", NumberOfSeats = 5, IsDeleted = true });
    }
    
}