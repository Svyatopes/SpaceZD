using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests
{
    public class TransitTests
    {
        private VeryVeryImportantContext _context;
        private TransitRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                          .UseInMemoryDatabase(databaseName: "Test")
                          .Options;

            _context = new VeryVeryImportantContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new TransitRepository(_context);
        }


        [Test]
        public void GetByIdTest()
        {
            //arrange
            var transitToAdd = GetTestTransit();

            _context.Transits.Add(transitToAdd);
            _context.SaveChanges();
            var idAddedTransit = transitToAdd.Id;

            //act
            var receivedTransit = _repository.GetById(idAddedTransit);

            //assert
            Assert.IsNotNull(receivedTransit);
            Assert.IsFalse(receivedTransit!.IsDeleted);
            AssertTestTransit(receivedTransit);
        }



        [Test]
        public void GetListTest()
        {
            //arrange
            var transitToAdd = GetTestTransit();
            var secondTransitToAdd = GetTestTransit();
            var thirdTransitToAdd = GetTestTransit();
            thirdTransitToAdd.IsDeleted = true;

            _context.Transits.Add(transitToAdd);
            _context.Transits.Add(secondTransitToAdd);
            _context.Transits.Add(thirdTransitToAdd);
            _context.SaveChanges();

            //act
            var transits = (List<Transit>)_repository.GetList();

            //assert

            Assert.IsNotNull(transits);
            Assert.AreEqual(2, transits.Count);

            var transitToCheck = transits[0];
            Assert.IsNotNull(transitToCheck);
            Assert.IsFalse(transitToCheck.IsDeleted);
            AssertTestTransit(transitToCheck);
        }


        [Test]
        public void GetListAllIncludedTest()
        {
            //arrange
            var transitToAdd = GetTestTransit();
            var secondTransitToAdd = GetTestTransit();
            var thirdTransitToAdd = GetTestTransit();
            thirdTransitToAdd.IsDeleted = true;

            _context.Transits.Add(transitToAdd);
            _context.Transits.Add(secondTransitToAdd);
            _context.Transits.Add(thirdTransitToAdd);
            _context.SaveChanges();

            //act
            var transits = (List<Transit>)_repository.GetList(true);


            //assert

            Assert.IsNotNull(transits);
            Assert.AreEqual(3, transits.Count);

            var orderToCheck = transits[2];
            Assert.IsNotNull(orderToCheck);
            Assert.IsTrue(orderToCheck.IsDeleted);
            AssertTestTransit(orderToCheck);
        }

        [Test]
        public void AddTest()
        {
            //arrange
            var transitToAdd = GetTestTransit();

            //act 
            int id = _repository.Add(transitToAdd);

            //assert
            var createdTransit = _context.Transits.FirstOrDefault(o => o.Id == id);

            AssertTestTransit(createdTransit!);
        }

        [Test]
        public void UpdateEntityTest()
        {
            //arrange
            var transitToAdd = GetTestTransit();
            _context.Transits.Add(transitToAdd);
            _context.SaveChanges();

            var transitToEdit = GetTestTransit();
            transitToEdit.Id = transitToAdd.Id;

            transitToEdit.StartStation = new Station() { Name = "Sevastopol" };
            transitToEdit.EndStation = new Station() { Name = "Mariupol" };
            transitToEdit.Price = (decimal?)24.4;

            //act 
            bool edited = _repository.Update(transitToEdit);

            //assert
            var updatedTransit = _context.Transits.FirstOrDefault(o => o.Id == transitToEdit.Id);

            Assert.IsTrue(edited);
            Assert.IsNotNull(updatedTransit);
            Assert.IsNotNull(updatedTransit!.StartStation);
            Assert.AreEqual("Sevastopol", updatedTransit.StartStation.Name);
            Assert.IsNotNull(updatedTransit.EndStation);
            Assert.AreEqual("Bolotnoe", updatedTransit.EndStation.Name);
            Assert.IsNotNull(updatedTransit.Price);
            Assert.AreEqual((decimal?)24.4, updatedTransit.Price);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UpdateIsDeletedTest(bool isDeleted)
        {
            //arrange
            var transitToEdit = GetTestTransit();
            transitToEdit.IsDeleted = !isDeleted;
            _context.Transits.Add(transitToEdit);
            _context.SaveChanges();

            //act 
            bool edited = _repository.Update(transitToEdit.Id, isDeleted);

            //assert
            var updatedTransit = _context.Transits.FirstOrDefault(o => o.Id == transitToEdit.Id);

            Assert.IsTrue(edited);
            Assert.IsNotNull(updatedTransit);
            Assert.AreEqual(isDeleted, updatedTransit!.IsDeleted);
            AssertTestTransit(updatedTransit);
        }

        private Transit GetTestTransit() => new Transit()
        {
            StartStation = new Station() { Name = "Novosibirsk" },
            EndStation = new Station() { Name = "Sheregesh" },
            Price = (decimal?)23.4
        };

        private void AssertTestTransit(Transit transit)
        {
            Assert.IsNotNull(transit);
            Assert.IsNotNull(transit.StartStation);
            Assert.AreEqual("Novosibirsk", transit.StartStation.Name);
            Assert.IsNotNull(transit.EndStation);
            Assert.AreEqual("Sheregesh", transit.EndStation.Name);
            Assert.IsNotNull(transit.Price);
            Assert.AreEqual((decimal?)23.4, transit.Price);
        }
    }
}
