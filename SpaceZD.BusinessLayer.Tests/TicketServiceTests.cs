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

namespace SpaceZD.BusinessLayer.Tests;

public class TicketServiceTests
{
    private Mock<ITicketRepository> _ticketRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IRepositorySoftDelete<Carriage>> _carriageRepositoryMock;
    private ITicketService _service;
    private readonly IMapper _mapper;

    public TicketServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _carriageRepositoryMock = new Mock<IRepositorySoftDelete<Carriage>>();

        _service = new TicketService
            (_mapper, _ticketRepositoryMock.Object, _userRepositoryMock.Object,
            _orderRepositoryMock.Object, _personRepositoryMock.Object, _carriageRepositoryMock.Object);
    }




    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetListTestCases))]

    public void GetListTest(List<Ticket> tickets, List<TicketModel> expectedTicketModels, int userId)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetList(false)).Returns(tickets);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User() { Role = Role.Admin, Id = userId });

        // when
        var ticketModels = _service.GetList(userId);

        // then
        CollectionAssert.AreEqual(expectedTicketModels, ticketModels);
        _ticketRepositoryMock.Verify(s => s.GetList(false), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [TestCase(42)]
    public void GetListNegativeUserNotFoundExceptionTest(int userId)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns((User?)null);

        //when then
        Assert.Throws<NotFoundException>(() => _service.GetList(userId));
    }


    [TestCase(42, Role.TrainRouteManager)]
    [TestCase(42, Role.StationManager)]
    [TestCase(42, Role.User)]
    public void GetListNegativeAuthorizationExceptionTest(int userId, Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User { Role = role });

        //when then
        Assert.Throws<AuthorizationException>(() => _service.GetList(userId));
    }



    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetListDeleteTestCases))]
    public void GetListDeleteTest(List<Ticket> tickets, List<TicketModel> expectedTicketModels, int userId)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetList(true)).Returns(tickets);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User() { Role = Role.Admin, Id = userId });

        // when
        var ticketModels = _service.GetListDeleted(userId);

        // then
        CollectionAssert.AreEqual(expectedTicketModels, ticketModels);
        _ticketRepositoryMock.Verify(s => s.GetList(true), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }

    [TestCase(42, Role.TrainRouteManager)]
    [TestCase(42, Role.StationManager)]
    [TestCase(42, Role.User)]
    public void GetListDeleteNegativeAuthorizationExceptionTest(int userId, Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User { Role = role });

        //when then
        Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(userId));
    }



    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Ticket ticket, TicketModel expected, int userId)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User() { Role = Role.Admin, Id = userId });

        // when
        var actual = _service.GetById(5, userId);

        // then
        Assert.AreEqual(expected, actual);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [Test]
    public void GetByIdNegativeNotFoundUserExceptionTest()
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        //when then
        Assert.Throws<NotFoundException>(() => _service.GetById(42, 42));
    }


    [Test]
    public void GetByIdNegativeNotFoundTicketExceptionTest()
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = 42 });
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Ticket?)null);

        //when then
        Assert.Throws<NotFoundException>(() => _service.GetById(42, 42));
    }


    [TestCase(Role.StationManager)]
    [TestCase(Role.TrainRouteManager)]
    public void GetByIdNegativeAuthorizationExceptionTest(Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 5 });
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Ticket());

        //when then
        Assert.Throws<AuthorizationException>(() => _service.GetById(5, 42));
    }



    [Test]
    public void DeleteTest()
    {
        // given
        var ticket = new Ticket() { Id = 56 };
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Id = 5, Role = Role.Admin });

        // when
        _service.Delete(5, 56);

        // then
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, true), Times.Once);
    }


    [Test]
    public void DeleteNegativeUserNotFoundExceptionTest()
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }


    [TestCase(Role.Admin)]
    [TestCase(Role.User)]
    public void DeleteNegativeTicketNotFoundExceptionTest(Role role)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Ticket)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 42 });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Delete(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
    }


    [TestCase(Role.TrainRouteManager)]
    [TestCase(Role.StationManager)]
    public void DeleteNegativeAuthorizationExceptionTest(Role role)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Ticket());
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = 42 });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Delete(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);

    }



    [TestCase(Role.Admin, 5)]
    public void RestoreTest(Role role, int userId)
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.Update(It.IsAny<Ticket>(), false));
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Id = userId, Role = role });

        // when
        _service.Restore(5, userId);

        // then
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, false), Times.Once);
    }


    [Test]
    public void RestoreNegativeUserNotFoundExceptionTest()
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }


    [Test]
    public void RestoreNegativeTicketNotFoundExceptionTest()
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Ticket)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = 42 });

        // when then
        Assert.Throws<NotFoundException>(() => _service.Restore(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
    }



    [TestCase(Role.StationManager, 1)]
    [TestCase(Role.TrainRouteManager, 21)]
    [TestCase(Role.User, 41)]

    public void RestoreNegativeAuthorizationExceptionTest(Role role, int userId)
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = userId });

        // when then
        Assert.Throws<AuthorizationException>(() => _service.Restore(42, userId));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }

}