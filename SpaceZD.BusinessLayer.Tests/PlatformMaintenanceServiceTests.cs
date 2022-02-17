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

namespace SpaceZD.BusinessLayer.Tests
{
    public class PlatformMaintenanceServiceTests
    {
        private Mock<IRepositorySoftDelete<PlatformMaintenance>> _platformMaintenanceRepositoryMock;
        private readonly IMapper _mapper;

        public PlatformMaintenanceServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _platformMaintenanceRepositoryMock = new Mock<IRepositorySoftDelete<PlatformMaintenance>>();
        }

        //Add
        [TestCaseSource(typeof(PlatformMaintenanceServiceTestCaseSource), nameof(PlatformMaintenanceServiceTestCaseSource.GetListTestCases))]
        public void GetListTest(List<PlatformMaintenance> platformMaintenance, List<PlatformMaintenanceModel> expectedPlatformMaintenanceModels, bool allIncluded)
        {
            // given
            var platformsMaintenanceFiltredByIsDeletedProp = platformMaintenance.Where(pm => !pm.IsDeleted || allIncluded).ToList();
            _platformMaintenanceRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
                .Returns(platformsMaintenanceFiltredByIsDeletedProp);

            expectedPlatformMaintenanceModels = expectedPlatformMaintenanceModels.Where(pm => !pm.IsDeleted || allIncluded).ToList();

            var platformMaintenanceService = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when
            var platformMaintenanceModels = platformMaintenanceService.GetList(allIncluded);

            // then
            CollectionAssert.AreEqual(expectedPlatformMaintenanceModels, platformMaintenanceModels);
            _platformMaintenanceRepositoryMock.Verify(pm => pm.GetList(It.IsAny<bool>()), Times.Once);
        }

        [TestCaseSource(typeof(PlatformMaintenanceServiceTestCaseSource), nameof(PlatformMaintenanceServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(PlatformMaintenance platformMaintenance, PlatformMaintenanceModel expectedPlatformMaintenance)
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platformMaintenance);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when
            var actual = service.GetById(42);

            // then
            Assert.AreEqual(expectedPlatformMaintenance, actual);
            _platformMaintenanceRepositoryMock.Verify(pm => pm.GetById(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void GetByIdNegativeTest()
        {
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((PlatformMaintenance?)null);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            Assert.Throws<NotFoundException>(() => service.GetById(42));
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(10)]
        public void AddTest(int expected)
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.Add(It.IsAny<PlatformMaintenance>())).Returns(expected);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when
            int actual = service.Add(new PlatformMaintenanceModel
            {
                Platform = new PlatformModel { Id = 1 }
            });

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.Add(It.IsAny<PlatformMaintenance>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdateTest()
        {
            // given
            var order = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), It.IsAny<PlatformMaintenance>()));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when
            service.Update(10, new PlatformMaintenanceModel());

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(order, It.IsAny<PlatformMaintenance>()), Times.Once);
        }

        [Test]
        public void UpdateNegativeTest()
        {
            // given
            var order = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), It.IsAny<PlatformMaintenance>()));
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Update(42, new PlatformMaintenanceModel()));
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(42), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(order, It.IsAny<PlatformMaintenance>()), Times.Never);
        }

        [Test]
        public void DeleteTest()
        {
            // given
            var order = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), true));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when
            service.Delete(42);

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(42), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(order, true), Times.Once);
        }
        [Test]
        public void DeleteNegativeTest()
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), true));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((PlatformMaintenance?)null);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Delete(42));
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(42), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(It.IsAny<PlatformMaintenance>(), true), Times.Never);
        }


        //Restore
        [Test]
        public void RestoreTest()
        {
            // given
            var order = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), false));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when
            service.Restore(42);

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(42), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(order, false), Times.Once);
        }
        [Test]
        public void RestoreNegativeTest()
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), false));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((PlatformMaintenance?)null);
            var service = new PlatformMaintenanceService(_mapper, _platformMaintenanceRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Restore(42));
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(42), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(It.IsAny<PlatformMaintenance>(), true), Times.Never);
        }
    }
}
