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
using System;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests
{
    public class RouteTransitServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRepositorySoftDelete<RouteTransit>> _routeTransitRepositoryMock;
        private Mock<IRepositorySoftDelete<Route>> _routeRepositoryMock;
        private Mock<IRepositorySoftDelete<Transit>> _transitRepositoryMock;
        private IRouteTransitService _service;

        private readonly IMapper _mapper;

        public RouteTransitServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _routeTransitRepositoryMock = new Mock<IRepositorySoftDelete<RouteTransit>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _routeRepositoryMock = new Mock<IRepositorySoftDelete<Route>>();
            _transitRepositoryMock = new Mock<IRepositorySoftDelete<Transit>>();
            _service = new RouteTransitService(_mapper, _userRepositoryMock.Object, _routeTransitRepositoryMock.Object,
                _transitRepositoryMock.Object, _routeRepositoryMock.Object);
        }

        // GetList
        [TestCaseSource(typeof(RouteTransitServiceTestCaseSource), nameof(RouteTransitServiceTestCaseSource.GetListTestCases))]
        public void GetListTest(List<RouteTransit> routeTransit, List<RouteTransitModel> expectedRouteTransitModels, Role role)
        {
            // given
            _routeTransitRepositoryMock.Setup(x => x.GetList(false)).Returns(routeTransit);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            var routeTransitModels = _service.GetList(10);

            // then
            _userRepositoryMock.Verify(x => x.GetById(10), Times.Once());
            _routeTransitRepositoryMock.Verify(rt => rt.GetList(It.IsAny<bool>()), Times.Once);
            CollectionAssert.AreEqual(expectedRouteTransitModels, routeTransitModels);
        }

        [Test]
        public void GetListNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetList(10));
        }

        [Test]
        public void GetListNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetList(10));
        }

        // GetList
        [TestCaseSource(typeof(RouteTransitServiceTestCaseSource), nameof(RouteTransitServiceTestCaseSource.GetListDeletdTestCases))]
        public void GetListDeletedTest(List<RouteTransit> routeTransit, List<RouteTransitModel> expectedRouteTransitModels, Role role)
        {
            // given
            _routeTransitRepositoryMock.Setup(x => x.GetList(true)).Returns(routeTransit);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            var routeTransitModels = _service.GetListDeleted(10);

            // then
            _userRepositoryMock.Verify(x => x.GetById(10), Times.Once());
            _routeTransitRepositoryMock.Verify(rt => rt.GetList(true), Times.Once);
            CollectionAssert.AreEqual(expectedRouteTransitModels, routeTransitModels);
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
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(10));
        }

        // GetById
        [TestCaseSource(typeof(RouteTransitServiceTestCaseSource), nameof(RouteTransitServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(RouteTransit routeTransit, RouteTransitModel expectedRouteTransit, Role role)
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });
            _routeTransitRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(routeTransit);

            // when
            var actual = _service.GetById(10, 10);

            // then
            _userRepositoryMock.Verify(x => x.GetById(10), Times.Once());
            _routeTransitRepositoryMock.Verify(pm => pm.GetById(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(expectedRouteTransit, actual);

        }

        [Test]
        public void GetByIdNegativeTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });
            _routeTransitRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((RouteTransit?)null);

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

        //Add
        [TestCase(10, Role.Admin)]
        [TestCase(10, Role.TrainRouteManager)]
        public void AddTest(int expected, Role role)
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });
            _routeTransitRepositoryMock.Setup(rt => rt.Add(It.IsAny<RouteTransit>())).Returns(expected);

            // when
            int actual = _service.Add(10, new RouteTransitModel
            {
                Transit = new TransitModel { Id = 1 },
                Route = new RouteModel { Id = 2 }
            });

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.Add(It.IsAny<RouteTransit>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.Add(10, new RouteTransitModel()));
        }

        [Test]
        public void AddNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Add(10, new RouteTransitModel()));
        }

        //Update
        [TestCase(Role.Admin)]
        [TestCase(Role.TrainRouteManager)]
        public void UpdateTest(Role role)
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), It.IsAny<RouteTransit>()));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            _service.Update(10, 10, new RouteTransitModel
            {
                Transit = new TransitModel
                {
                    StartStation = new StationModel { Name = "11", Platforms = new List<PlatformModel>(), IsDeleted = false },
                    EndStation = new StationModel { Name = "11", Platforms = new List<PlatformModel>(), IsDeleted = false }
                },
                DepartingTime = new TimeSpan(0, 0, 0),
                ArrivalTime = new TimeSpan(10, 0, 0),
                Route = new RouteModel
                {
                    Code = "1cc",
                    RouteTransits = new List<RouteTransitModel>(),
                    StartTime = new DateTime(2020, 1, 1, 1, 1, 1),
                    StartStation = new StationModel { Name = "СПБ", Platforms = new List<PlatformModel>(), IsDeleted = false },
                    EndStation = new StationModel { Name = "11", Platforms = new List<PlatformModel>(), IsDeleted = false },
                    IsDeleted = false
                }
            }
            );

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, It.IsAny<RouteTransit>()), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(10), Times.Once);

        }

        [Test]
        public void UpdateNegativeTest()
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), It.IsAny<RouteTransit>()));
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });


            // when then
            Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new RouteTransitModel()));
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, It.IsAny<RouteTransit>()), Times.Never);
        }

        [Test]
        public void UpdateNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new RouteTransitModel()));
        }

        [Test]
        public void UpdateNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new RouteTransitModel()));
        }

        //Delete
        [TestCase(Role.Admin)]
        [TestCase(Role.TrainRouteManager)]
        public void DeleteTest(Role role)
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), true));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });


            // when
            _service.Delete(10, 10);

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, true), Times.Once);
        }

        [Test]
        public void DeleteNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

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
        [TestCase(Role.Admin)]
        public void RestoreTest(Role role)
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), false));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });


            // when
            _service.Restore(10, 10);

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, false), Times.Once);
        }

        [Test]
        public void RestoreNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

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

    }
}
