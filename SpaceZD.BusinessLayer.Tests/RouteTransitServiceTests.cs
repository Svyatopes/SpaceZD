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

namespace SpaceZD.BusinessLayer.Tests
{
    public class RouteTransitServiceTests
    {
        private Mock<IRepositorySoftDelete<RouteTransit>> _routeTransitRepositoryMock;
        private readonly IMapper _mapper;

        public RouteTransitServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _routeTransitRepositoryMock = new Mock<IRepositorySoftDelete<RouteTransit>>();
        }

        //Add
        [TestCaseSource(typeof(RouteTransitServiceTestCaseSource), nameof(RouteTransitServiceTestCaseSource.GetListTestCases))]
        public void GetListTest(List<RouteTransit> routeTransit, List<RouteTransitModel> expectedRouteTransitModels, bool allIncluded)
        {
            // given
            var routeTransitFiltredByIsDeletedProp = routeTransit.Where(rt => !rt.IsDeleted || allIncluded).ToList();
            _routeTransitRepositoryMock.Setup(rt => rt.GetList(It.IsAny<bool>()))
                .Returns(routeTransitFiltredByIsDeletedProp);

            expectedRouteTransitModels = expectedRouteTransitModels.Where(rt => !rt.IsDeleted || allIncluded).ToList();

            var routeTransitService = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when
            var routeTransitModels = routeTransitService.GetList(allIncluded);

            // then
            CollectionAssert.AreEqual(expectedRouteTransitModels, routeTransitModels);
            _routeTransitRepositoryMock.Verify(rt => rt.GetList(It.IsAny<bool>()), Times.Once);
        }

        [TestCaseSource(typeof(RouteTransitServiceTestCaseSource), nameof(RouteTransitServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(RouteTransit routeTransit, RouteTransitModel expectedRouteTransit)
        {
            // given
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when
            var actual = service.GetById(10);

            // then
            Assert.AreEqual(expectedRouteTransit, actual);
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void GetByIdNegativeTest()
        {
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns((RouteTransit?)null);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            Assert.Throws<NotFoundException>(() => service.GetById(10));
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(10)]
        public void AddTest(int expected)
        {
            // given
            _routeTransitRepositoryMock.Setup(rt => rt.Add(It.IsAny<RouteTransit>())).Returns(expected);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when
            int actual = service.Add(new RouteTransitModel
            {
                Transit = new TransitModel { Id = 1 },
                Route =new RouteModel { Id = 2 }
            });

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.Add(It.IsAny<RouteTransit>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdateTest()
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), It.IsAny<RouteTransit>()));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when
            service.Update(10, new RouteTransitModel());

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, It.IsAny<RouteTransit>()), Times.Once);
        }

        [Test]
        public void UpdateNegativeTest()
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), It.IsAny<RouteTransit>()));
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Update(10, new RouteTransitModel()));
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, It.IsAny<RouteTransit>()), Times.Never);
        }

        [Test]
        public void DeleteTest()
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), true));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when
            service.Delete(10);

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, true), Times.Once);
        }
        [Test]
        public void DeleteNegativeTest()
        {
            // given
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), true));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns((RouteTransit?)null);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Delete(10));
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(It.IsAny<RouteTransit>(), true), Times.Never);
        }


        //Restore
        [Test]
        public void RestoreTest()
        {
            // given
            var routeTransit = new RouteTransit();
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), false));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns(routeTransit);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when
            service.Restore(10);

            // then
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(routeTransit, false), Times.Once);
        }
        [Test]
        public void RestoreNegativeTest()
        {
            // given
            _routeTransitRepositoryMock.Setup(rt => rt.Update(It.IsAny<RouteTransit>(), false));
            _routeTransitRepositoryMock.Setup(rt => rt.GetById(It.IsAny<int>())).Returns((RouteTransit?)null);
            var service = new RouteTransitService(_mapper, _routeTransitRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Restore(10));
            _routeTransitRepositoryMock.Verify(rt => rt.GetById(10), Times.Once);
            _routeTransitRepositoryMock.Verify(rt => rt.Update(It.IsAny<RouteTransit>(), true), Times.Never);
        }

    }
}
