using System;
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

public class TripServiceTests
{
    private Mock<IRepositorySoftDelete<Trip>> _tripRepositoryMock;
    private Mock<IStationRepository> _stationRepositoryMock;
    private readonly IMapper _mapper;

    public TripServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _tripRepositoryMock = new Mock<IRepositorySoftDelete<Trip>>();
        _stationRepositoryMock = new Mock<IStationRepository>();
    }


    // GetFreeSeat
    [TestCaseSource(typeof(TripServiceTestCaseSource), nameof(TripServiceTestCaseSource.GetTestCaseDataForGetFreeSeatTest))]
    public void GetFreeSeatTest(Trip trip, Station startStation, Station endStation, List<CarriageSeatsModel> expected, bool negative)
    {
        // given
        _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(trip);
        _stationRepositoryMock.Setup(x => x.GetById(1)).Returns(startStation);
        _stationRepositoryMock.Setup(x => x.GetById(2)).Returns(endStation);
        var service = new TripService(_mapper, _tripRepositoryMock.Object, _stationRepositoryMock.Object);

        // when
        List<CarriageSeatsModel> actual = null;
        if (!negative)
            actual = service.GetFreeSeat(45, 1, 2);

        // then
        if (negative)
            Assert.Throws<ArgumentException>(() => service.GetFreeSeat(45, 1, 2));
        else
        {
            _stationRepositoryMock.Verify(s => s.GetById(1), Times.Once);
            _stationRepositoryMock.Verify(s => s.GetById(2), Times.Once);
            _tripRepositoryMock.Verify(s => s.GetById(45), Times.Once);
            CollectionAssert.AreEqual(expected, actual);
        }
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void GetFreeSeatNegativeTest(int variant)
    {
        switch (variant)
        {
            case 1:
                _tripRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Trip?)null);
                break;
            case 2:
                _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
                break;
            case 3:
                _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);
                break;
        }
        var service = new TripService(_mapper, _tripRepositoryMock.Object, _stationRepositoryMock.Object);

        Assert.Throws<NotFoundException>(() => service.GetFreeSeat(45, 1, 2));
    }
}