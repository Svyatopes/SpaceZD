using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
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
        Assert.AreEqual(
            new CarriageTypeModel { Name = carriageType.Name, NumberOfSeats = carriageType.NumberOfSeats, IsDeleted = carriageType.IsDeleted },
            actual);
    }
    public static IEnumerable<TestCaseData> GetCarriageType()
    {
        yield return new TestCaseData(new CarriageType { Name = "Rbs", NumberOfSeats = 2, IsDeleted = true });
        yield return new TestCaseData(new CarriageType { Name = "Купе", NumberOfSeats = 3, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Ласточка", NumberOfSeats = 2, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Сапсан", NumberOfSeats = 3, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Плацкарт", NumberOfSeats = 4, IsDeleted = false });
        yield return new TestCaseData(new CarriageType { Name = "Сидячие места", NumberOfSeats = 5, IsDeleted = true });
    }
    [Test]
    public void GetByIdNegativeTest()
    {
        _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((CarriageType?)null);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetById(10));
    }


    // GetList
    [TestCaseSource(nameof(GetListCarriageTypeNotDeleted))]
    public void GetListTest(List<CarriageType> carriageTypes)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetList(false)).Returns(carriageTypes);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);
        var expected = carriageTypes.Select(carriageType => new CarriageTypeModel
                                    {
                                        Name = carriageType.Name, NumberOfSeats = carriageType.NumberOfSeats, IsDeleted = carriageType.IsDeleted
                                    })
                                    .ToList();

        // when
        var actual = service.GetList();

        // then
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListCarriageTypeNotDeleted()
    {
        yield return new TestCaseData(new List<CarriageType>
        {
            new() { Name = "Купе", NumberOfSeats = 3, IsDeleted = false },
            new() { Name = "Ласточка", NumberOfSeats = 2, IsDeleted = false },
            new() { Name = "Сапсан", NumberOfSeats = 3, IsDeleted = false },
            new() { Name = "Плацкарт", NumberOfSeats = 4, IsDeleted = false }
        });
        yield return new TestCaseData(new List<CarriageType>
        {
            new() { Name = "Rbs", NumberOfSeats = 2, IsDeleted = false },
            new() { Name = "Сидячие места", NumberOfSeats = 5, IsDeleted = false }
        });
    }
    [TestCaseSource(nameof(GetListCarriageTypeDeleted))]
    public void GetListDeletedTest(List<CarriageType> carriageTypes)
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.GetList(true)).Returns(carriageTypes);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);
        var expected = carriageTypes.Select(carriageType => new CarriageTypeModel
                                    {
                                        Name = carriageType.Name, NumberOfSeats = carriageType.NumberOfSeats, IsDeleted = carriageType.IsDeleted
                                    })
                                    .ToList();

        // when
        var actual = service.GetListDeleted();

        // then
        CollectionAssert.AreEqual(expected, actual);
    }
    public static IEnumerable<TestCaseData> GetListCarriageTypeDeleted()
    {
        yield return new TestCaseData(new List<CarriageType>
        {
            new() { Name = "Rbs", NumberOfSeats = 2, IsDeleted = true },
            new() { Name = "Сидячие места", NumberOfSeats = 5, IsDeleted = true }
        });
        yield return new TestCaseData(new List<CarriageType>
        {
            new() { Name = "Ласточка", NumberOfSeats = 2, IsDeleted = true },
            new() { Name = "Сапсан", NumberOfSeats = 3, IsDeleted = true },
        });
    }
    
    
    //Delete
    [Test]
    public void DeleteTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), true)).Returns(true);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        // when
        service.Delete(45);

        // then
        Assert.Pass();
    }
    [Test]
    public void DeleteNegativeTest()
    {
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), true)).Returns(false);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);
        
        Assert.Throws<NotFoundException>(() => service.Delete(10));
    }
    
    
    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), false)).Returns(true);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        // when
        service.Restore(45);

        // then
        Assert.Pass();
    }
    [Test]
    public void RestoreNegativeTest()
    {
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), false)).Returns(false);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);
        
        Assert.Throws<NotFoundException>(() => service.Restore(10));
    }
    
    
    //Update
    [Test]
    public void UpdateTest()
    {
        // given
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>())).Returns(true);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);

        // when
        service.Update(45, new CarriageTypeModel());

        // then
        Assert.Pass();
    }
    [Test]
    public void UpdateNegativeTest()
    {
        _carriageTypeRepositoryMock.Setup(x => x.Update(It.IsAny<CarriageType>())).Returns(false);
        var service = new CarriageTypeService(_mapper, _carriageTypeRepositoryMock.Object);
        
        Assert.Throws<NotFoundException>(() => service.Update(10, new CarriageTypeModel()));
    }
}