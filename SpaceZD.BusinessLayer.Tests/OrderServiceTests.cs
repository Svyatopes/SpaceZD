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
    private Mock<IRepositorySoftDeleteNewUpdate<Order>> _orderRepositoryMock;
    private readonly IMapper _mapper;

    public OrderServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _orderRepositoryMock = new Mock<IRepositorySoftDeleteNewUpdate<Order>>();
    }

    //Add
    [TestCaseSource(typeof(OrderServiceTestCaseSource), nameof(OrderServiceTestCaseSource.GetListTestCases))]
    public void GetListTest(List<Order> orders, List<OrderModel> expectedOrderModels, bool allIncluded)
    {
        // given
        _orderRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(orders.Where(o => !o.IsDeleted || allIncluded).ToList());

        expectedOrderModels = expectedOrderModels.Where(o => !o.IsDeleted || allIncluded).ToList();

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object);
        
        // when
        var orderModels = orderService.GetList(allIncluded);

        // then
        CollectionAssert.AreEqual(expectedOrderModels, orderModels);
    }
}