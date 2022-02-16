using System;
using System.Collections.Generic;
using System.Linq;
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

public class OrderServiceTests
{
    private Mock<IRepositorySoftDelete<Order>> _orderRepositoryMock;
    private readonly IMapper _mapper;

    public OrderServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _orderRepositoryMock = new Mock<IRepositorySoftDelete<Order>>();
    }

    //Add
    [TestCaseSource(typeof(OrderServiceTestCaseSource), nameof(OrderServiceTestCaseSource.GetListTestCases))]
    public void GetListTest(List<Order> orders, List<OrderModel> expectedOrderModels, bool allIncluded)
    {
        // given
        var ordersFiltredByIsDeletedProp = orders.Where(o => !o.IsDeleted || allIncluded).ToList();
        _orderRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(ordersFiltredByIsDeletedProp);

        expectedOrderModels = expectedOrderModels.Where(o => !o.IsDeleted || allIncluded).ToList();

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when
        var orderModels = orderService.GetList(allIncluded);

        // then
        CollectionAssert.AreEqual(expectedOrderModels, orderModels);
        _orderRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
    }

    [TestCaseSource(typeof(OrderServiceTestCaseSource), nameof(OrderServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Order order, OrderModel expected)
    {
        // given
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when
        var actual = service.GetById(42);

        // then
        Assert.AreEqual(expected, actual);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);

    }

    [Test]
    public void GetByIdNegativeTest()
    {
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);
        
        Assert.Throws<NotFoundException>(() => service.GetById(42));
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
    }

    [TestCase(42)]
    public void AddTest(int expected)
    {
        // given
        _orderRepositoryMock.Setup(x => x.Add(It.IsAny<Order>())).Returns(expected);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when
        int actual = service.Add(new OrderModel
        {
            User = new UserModel { Id = 1 },
            StartStation = new TripStationModel { Id = 2 },
            EndStation = new TripStationModel { Id = 3 },
        });

        // then
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void UpdateTest()
    {
        // given
        var order = new Order();
        _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>(), It.IsAny<Order>()));
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when
        service.Update(45, new OrderModel());

        // then
        _orderRepositoryMock.Verify(s => s.GetById(45), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(order, It.IsAny<Order>()), Times.Once);
    }

    [Test]
    public void UpdateNegativeTest()
    {
        // given
        var order = new Order();
        _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>(), It.IsAny<Order>()));
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => service.Update(42, new OrderModel()));
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(order, It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void DeleteTest()
    {
        // given
        var order = new Order();
        _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>(), true));
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when
        service.Delete(42);

        // then
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(order, true), Times.Once);
    }
    [Test]
    public void DeleteNegativeTest()
    {
        // given
        _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>(), true));
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => service.Delete(42));
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Never);
    }


    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var order = new Order();
        _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>(), false));
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when
        service.Restore(42);

        // then
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(order, false), Times.Once);
    }
    [Test]
    public void RestoreNegativeTest()
    {
        // given
        _orderRepositoryMock.Setup(x => x.Update(It.IsAny<Order>(), false));
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);
        var service = new OrderService(_mapper, _orderRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => service.Restore(42));
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Never);
    }
}