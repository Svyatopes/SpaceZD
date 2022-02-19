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
    internal class CarriageServiceTests
    {
        private Mock<IRepositorySoftDelete<Carriage>> _carriageRepositoryMock;
        private readonly IMapper _mapper;

        public CarriageServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _carriageRepositoryMock = new Mock<IRepositorySoftDelete<Carriage>>();
        }

        //Add
        [TestCaseSource(typeof(CarriageServiceTestCaseSource), nameof(CarriageServiceTestCaseSource.GetListTestCases))]
        public void GetListTest(List<Carriage> carriage, List<CarriageModel> expectedCarriageModels, bool allIncluded)
        {
            // given
            var CarriageFiltredByIsDeletedProp = carriage.Where(c => !c.IsDeleted || allIncluded).ToList();
            _carriageRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
                .Returns(CarriageFiltredByIsDeletedProp);

            expectedCarriageModels = expectedCarriageModels.Where(c => !c.IsDeleted || allIncluded).ToList();

            var carriageService = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when
            var carriageModels = carriageService.GetList(allIncluded);

            // then
            CollectionAssert.AreEqual(expectedCarriageModels, carriageModels);
            _carriageRepositoryMock.Verify(c => c.GetList(It.IsAny<bool>()), Times.Once);
        }

        [TestCaseSource(typeof(CarriageServiceTestCaseSource), nameof(CarriageServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(Carriage Carriage, CarriageModel expectedCarriage)
        {
            // given
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Carriage);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when
            var actual = service.GetById(10);

            // then
            Assert.AreEqual(expectedCarriage, actual);
            _carriageRepositoryMock.Verify(pm => pm.GetById(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void GetByIdNegativeTest()
        {
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            Assert.Throws<NotFoundException>(() => service.GetById(10));
            _carriageRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(10)]
        public void AddTest(int expected)
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Add(It.IsAny<Carriage>())).Returns(expected);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when
            int actual = service.Add(new CarriageModel
            {
                Type = new CarriageTypeModel { Id = 1 },
                Train = new TrainModel { Id = 2 }
            });

            // then
            _carriageRepositoryMock.Verify(s => s.Add(It.IsAny<Carriage>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdateTest()
        {
            // given
            var order = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), It.IsAny<Carriage>()));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when
            service.Update(10, new CarriageModel());

            // then
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(order, It.IsAny<Carriage>()), Times.Once);
        }

        [Test]
        public void UpdateNegativeTest()
        {
            // given
            var order = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), It.IsAny<Carriage>()));
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Update(10, new CarriageModel()));
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(order, It.IsAny<Carriage>()), Times.Never);
        }

        [Test]
        public void DeleteTest()
        {
            // given
            var order = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), true));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when
            service.Delete(10);

            // then
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(order, true), Times.Once);
        }

        [Test]
        public void DeleteNegativeTest()
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), true));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Delete(10));
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(It.IsAny<Carriage>(), true), Times.Never);
        }

        //Restore
        [Test]
        public void RestoreTest()
        {
            // given
            var order = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), false));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(order);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when
            service.Restore(10);

            // then
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(order, false), Times.Once);
        }

        [Test]
        public void RestoreNegativeTest()
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), false));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);
            var service = new CarriageService(_mapper, _carriageRepositoryMock.Object);

            // when then
            Assert.Throws<NotFoundException>(() => service.Restore(10));
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(It.IsAny<Carriage>(), true), Times.Never);
        }
    }
}
