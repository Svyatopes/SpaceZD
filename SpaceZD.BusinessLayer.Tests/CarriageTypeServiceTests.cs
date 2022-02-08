using System;
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

    [SetUp]
    public void Setup() {}

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
    
    [TestCase(6)]
    public void GetByIdTest(int expected)
    {
        // given
        var gg = new CarriageType
        {
            Name = "Купе",
            NumberOfSeats = 5
        };
        _carriageTypeRepositoryMock.Setup(x => x.GetById(expected)).Returns(gg);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        // when
        var actual = service.GetById(expected);
        
        // then
        Assert.AreEqual(_mapper.Map<CarriageTypeModel>(gg), actual);
    }
    
    [Test]
    public void GetByIdNegativeTest()
    {
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        Assert.Throws<Exception>(() => service.GetById(10));
    }
}