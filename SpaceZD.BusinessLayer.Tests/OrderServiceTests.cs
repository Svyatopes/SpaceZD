using System;
using System.Collections.Generic;
using System.IO;
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
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class OrderServiceTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IRepositorySoftDelete<Trip>> _tripRepositoryMock;
    private Mock<ITripStationRepository> _tripStationRepositoryMock;
    private readonly IMapper _mapper;

    public OrderServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tripRepositoryMock = new Mock<IRepositorySoftDelete<Trip>>();
        _tripStationRepositoryMock = new Mock<ITripStationRepository>();
    }

    [TestCaseSource(typeof(OrderServiceTestCaseSource), nameof(OrderServiceTestCaseSource.GetListTestCases))]
    public void GetListTest(List<Order> orders, List<OrderModel> expectedOrderModels, bool allIncluded, int userId)
    {
        // given
        List<Order> ordersFiltredByIsDeletedProp = orders.Where(o => !o.IsDeleted || allIncluded).ToList();
        ordersFiltredByIsDeletedProp.ForEach(o => o.Tickets = o.Tickets.Where(o => !o.IsDeleted).ToList());
        expectedOrderModels.ForEach(o => o.Tickets = o.Tickets.Where(o => !o.IsDeleted).ToList());

        _orderRepositoryMock.Setup(x => x.GetList(It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(ordersFiltredByIsDeletedProp);

        expectedOrderModels = expectedOrderModels.Where(o => !o.IsDeleted || allIncluded).ToList();
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(orders[userId - 1].User);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        var orderModels = orderService.GetList(userId, userId, allIncluded);

        // then
        CollectionAssert.AreEqual(expectedOrderModels, orderModels);
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetList(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
    }

    [TestCaseSource(typeof(OrderServiceTestCaseSource), nameof(OrderServiceTestCaseSource.GetListTestCases))]
    public void GetListByAdminTest(List<Order> orders, List<OrderModel> expectedOrderModels, bool allIncluded, int userId)
    {
        // given
        var ordersFiltredByIsDeletedProp = orders.Where(o => !o.IsDeleted || allIncluded).ToList();
        _orderRepositoryMock.Setup(x => x.GetList(It.IsAny<int>(), It.IsAny<bool>()))
            .Returns(ordersFiltredByIsDeletedProp);

        expectedOrderModels = expectedOrderModels.Where(o => !o.IsDeleted || allIncluded).ToList();
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User { Role = Role.Admin });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        var orderModels = orderService.GetList(userId, userId, allIncluded);

        // then
        CollectionAssert.AreEqual(expectedOrderModels, orderModels);
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetList(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
    }

    [TestCase(42)]
    public void GetListNegativeUserNotFoundTest(int userId)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns((User?)null);
        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        //when then
        Assert.Throws<NotFoundException>(() => orderService.GetList(userId, userId, true));
    }

    [TestCase(42, 42, Role.TrainRouteManager)]
    [TestCase(42, 42, Role.StationManager)]
    [TestCase(42, 43, Role.User)]
    public void GetListNegativeAuthorizationExceptionTest(int userId, int userOrdersId, Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User { Role = role });
        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        //when then
        Assert.Throws<AuthorizationException>(() => orderService.GetList(userId, userOrdersId, true));
    }


    [TestCaseSource(typeof(OrderServiceTestCaseSource), nameof(OrderServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Order order, OrderModel expected, Role role, int userId)
    {
        // given
        order.User.Id = 42;
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role, Id = userId });
        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        var actual = orderService.GetById(userId, 42);

        // then
        Assert.AreEqual(expected, actual);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);

    }

    [Test]
    public void GetByIdNotFoundOrderTest()
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);
        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        //when then
        Assert.Throws<NotFoundException>(() => orderService.GetById(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }

    [TestCase(Role.Admin)]
    [TestCase(Role.User)]
    public void GetByIdNotFoundOrderTest(Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);
        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        //when then
        Assert.Throws<NotFoundException>(() => orderService.GetById(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
    }

    [TestCase(Role.TrainRouteManager, 42, 42)]
    [TestCase(Role.StationManager, 42, 42)]
    [TestCase(Role.User, 42, 43)]
    public void GetByIdAuthorizationExceptionTest(Role role, int userId, int userIdInOrder)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order
            {
                User = new User { Id = userIdInOrder }
            });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        //when then
        Assert.Throws<AuthorizationException>(() => orderService.GetById(userId, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
    }



    [TestCase(42, 1, 2, 3)]
    public void AddTest(int expected, int tripId, int tripStationStartId, int tripStationEndId)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        var trip = new Trip 
        { 
            Id = tripId,
            Stations = new List<TripStation>
            {
                new TripStation { Id = tripStationStartId},
                new TripStation { Id = tripStationEndId}
            }
        };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        _orderRepositoryMock.Setup(x => x.Add(It.IsAny<Order>())).Returns(expected);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        int actual = orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            });

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void AddUserNotFoundTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = 1 },
                StartStation = new TripStationModel { Id = 2 },
                EndStation = new TripStationModel { Id = 3 }
            }));

        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Never);
    }

    [TestCase(Role.Admin)]
    [TestCase(Role.StationManager)]
    [TestCase(Role.TrainRouteManager)]
    public void AddUserAuthorizationExceptionTest(Role role)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<AuthorizationException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = 1 },
                StartStation = new TripStationModel { Id = 2 },
                EndStation = new TripStationModel { Id = 3 }
            }));

        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void AddStartStatitonNotFoundTest()
    {
        // given
        int tripId = 1;
        int tripStationStartId = 2;
        int tripStationEndId = 3;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void AddEndStatitonNotFoundTest()
    {
        // given
        int tripId = 1;
        int tripStationStartId = 2;
        int tripStationEndId = 3;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Never);
    }

    [TestCase(1, 2, 2, 1, 1)]
    [TestCase(1, 2, 3, 4, 1)]
    [TestCase(1, 2, 3, 1, 4)]
    public void AddInvalidDataExcetionTest(int tripId, int tripStationStartId, int tripStationEndId,
        int tripIdInStartStation, int tripIdInEndStation)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = new Trip { Id = tripIdInStartStation } });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = new Trip { Id = tripIdInEndStation } });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<InvalidDataException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Never);
    }

    [TestCase(1, 2, 3)]
    public void AddArgumentExceptionTest(int tripId, int tripStationStartId, int tripStationEndId)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        var trip = new Trip
        {
            Id = tripId,
            Stations = new List<TripStation>
            {
                new TripStation { Id = tripStationEndId},
                new TripStation { Id = tripStationStartId}
            }
        };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<ArgumentException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
        _orderRepositoryMock.Verify(s => s.Add(It.IsAny<Order>()), Times.Never);
    }


    [TestCase(42, 1, 2, 3)]
    public void EditTest(int expected, int tripId, int tripStationStartId, int tripStationEndId)
    {
        // given
        var userId = 1;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User, Id = userId });
        var trip = new Trip
        {
            Id = tripId,
            Stations = new List<TripStation>
            {
                new TripStation { Id = tripStationStartId, Station = new Station{Id = tripStationStartId } },
                new TripStation { Id = tripStationEndId, Station = new Station{Id = tripStationEndId} }
            }
        };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order
            {
                Status = OrderStatus.Draft,
                Trip = new Trip(),
                StartStation = new TripStation(),
                EndStation = new TripStation(),
                User = new User { Id = userId }
            });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        orderService.Update(
            userId,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            });

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Once);
    }

    [Test]
    public void EditUserNotFoundTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Update(
            1,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = 1 },
                StartStation = new TripStationModel { Id = 2 },
                EndStation = new TripStationModel { Id = 3 }
            }));

        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }

    [TestCase(Role.StationManager, 1, 1, OrderStatus.Draft)]
    [TestCase(Role.TrainRouteManager, 1, 1, OrderStatus.Draft)]
    [TestCase(Role.User, 1, 2, OrderStatus.Draft)]
    [TestCase(Role.User, 1, 2, OrderStatus.Buyed)]
    public void EditUserAuthorizationExceptionTest(Role role, int userId, int userIdInOrder, OrderStatus status)
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Id = userId, Role = role });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order { User = new User { Id = userIdInOrder }, Status = status });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<AuthorizationException>(() => orderService.Update(
            1,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = 1 },
                StartStation = new TripStationModel { Id = 2 },
                EndStation = new TripStationModel { Id = 3 }
            }));

        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void EditStartStatitonNotFoundTest()
    {
        // given
        int tripId = 1;
        int tripStationStartId = 2;
        int tripStationEndId = 3;
        int userId = 10;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User, Id = userId });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order
            {
                Trip = new Trip(),
                StartStation = new TripStation(),
                EndStation = new TripStation(),
                User = new User { Id = userId }
            });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Add(
            1,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void EditEndStatitonNotFoundTest()
    {
        // given
        int tripId = 1;
        int tripStationStartId = 2;
        int tripStationEndId = 3;
        int userId = 10;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User, Id = userId });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order
            {
                Status = OrderStatus.Draft,
                Trip = new Trip(),
                StartStation = new TripStation(),
                EndStation = new TripStation(),
                User = new User { Id = userId }
            });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Update(
            1,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }

    [Test]
    public void EditOrderNotFoundTest()
    {
        // given
        int tripId = 1;
        int tripStationStartId = 2;
        int tripStationEndId = 3; _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Update(
            1,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }

    [TestCase(1, 2, 2, 1, 1)]
    [TestCase(1, 2, 3, 4, 1)]
    [TestCase(1, 2, 3, 1, 4)]
    public void EditInvalidDataExcetionTest(int tripId, int tripStationStartId, int tripStationEndId,
        int tripIdInStartStation, int tripIdInEndStation)
    {
        // given
        int userId = 10;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User, Id = userId });
        var trip = new Trip { Id = tripId };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = new Trip { Id = tripIdInStartStation } });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = new Trip { Id = tripIdInEndStation } });

        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order
            {
                Status = OrderStatus.Draft,
                Trip = new Trip(),
                StartStation = new TripStation(),
                EndStation = new TripStation(),
                User = new User { Id = userId }
            });


        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<InvalidDataException>(() => orderService.Update(
            1,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }

    [TestCase(1, 2, 3)]
    public void EditArgumentExceptionTest(int tripId, int tripStationStartId, int tripStationEndId)
    {
        // given
        var userId = 1;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User, Id = userId });
        var trip = new Trip
        {
            Id = tripId,
            Stations = new List<TripStation>
            {
                new TripStation { Id = tripStationEndId, Station = new Station{Id = tripStationEndId} },
                new TripStation { Id = tripStationStartId, Station = new Station{Id = tripStationStartId } },
            }
        };

        _tripRepositoryMock.Setup(x => x.GetById(tripId)).Returns(trip);

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationStartId))
            .Returns(new TripStation { Id = tripStationStartId, Trip = trip });

        _tripStationRepositoryMock.Setup(x => x.GetById(tripStationEndId))
            .Returns(new TripStation { Id = tripStationEndId, Trip = trip });

        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order
            {
                Status = OrderStatus.Draft,
                Trip = new Trip(),
                StartStation = new TripStation(),
                EndStation = new TripStation(),
                User = new User { Id = userId }
            });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
             _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        Assert.Throws<ArgumentException>(() => orderService.Update(
            userId,
            2,
            new OrderModel
            {
                Trip = new TripModel { Id = tripId },
                StartStation = new TripStationModel { Id = tripStationStartId },
                EndStation = new TripStationModel { Id = tripStationEndId }
            }));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _tripStationRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Exactly(2));
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<Order>()), Times.Never);
    }


    [TestCase(Role.User, 10, 10)]
    [TestCase(Role.Admin, 10, 12)]
    public void DeleteTest(Role role, int userId, int userIdInOrder)
    {
        // given
        int orderId = 42;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role, Id = userId });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order { User = new User { Id = userIdInOrder } });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        orderService.Delete(userId, orderId);

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Once);
    }

    [Test]
    public void DeleteUserNotFoundTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Delete(42, 42));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<bool>()), Times.Never);
    }

    [Test]
    public void DeleteOrderNotFoundTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Delete(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Never);
    }

    [TestCase(Role.TrainRouteManager, 10, 10)]
    [TestCase(Role.StationManager, 10, 10)]
    [TestCase(Role.User, 10, 12)]
    public void DeleteAuthorizationExceptionTest(Role role, int userId, int userIdInOrder)
    {
        // given
        int orderId = 42;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User {Id = userId, Role = role });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order { User = new User { Id = userIdInOrder } });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<AuthorizationException>(() => orderService.Delete(userId, orderId));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Never);
    }


    [TestCase(Role.Admin, 10, 12)]
    public void RestoreTest(Role role, int userId, int userIdInOrder)
    {
        // given
        int orderId = 42;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role, Id = userId });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order { User = new User { Id = userIdInOrder } });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when
        orderService.Restore(userId, orderId);

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), false), Times.Once);
    }

    [Test]
    public void RestoreUserNotFoundTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Restore(42, 42));

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), It.IsAny<bool>()), Times.Never);
    }

    [Test]
    public void RestoreOrderNotFoundTest()
    {
        // given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Order?)null);

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<NotFoundException>(() => orderService.Restore(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.GetById(42), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Never);
    }

    [TestCase(Role.TrainRouteManager, 10, 10)]
    [TestCase(Role.StationManager, 10, 10)]
    [TestCase(Role.User, 10, 12)]
    [TestCase(Role.User, 12, 12)]
    public void RestoreAuthorizationExceptionTest(Role role, int userId, int userIdInOrder)
    {
        // given
        int orderId = 42;
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Id = userId, Role = role });
        _orderRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(
            new Order { User = new User { Id = userIdInOrder } });

        var orderService = new OrderService(_mapper, _orderRepositoryMock.Object,
            _userRepositoryMock.Object, _tripRepositoryMock.Object, _tripStationRepositoryMock.Object);

        // when then
        Assert.Throws<AuthorizationException>(() => orderService.Restore(userId, orderId));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _orderRepositoryMock.Verify(s => s.Update(It.IsAny<Order>(), true), Times.Never);
    }
}