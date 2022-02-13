using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
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

            // seed
            var transits = new Transit[]
            {
                new()
                {
                    StartStation = new Station() { Name = "Novosibirsk" },
                    EndStation = new Station() { Name = "Sheregesh" },
                    Price = (decimal?)23.4
                },
                new()
                {
                    StartStation = new Station() { Name = "Elisenvaara" },
                    EndStation = new Station() { Name = "Sortavala" },
                    Price = (decimal?)25.7
                },
                new()
                {
                    StartStation = new Station() { Name = "Vorkuta" },
                    EndStation = new Station() { Name = "Yamal" },
                    Price = (decimal?)24.6
                },

            };
            _context.Transits.AddRange(transits);
            _context.SaveChanges();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetByIdTest(int id)
        {
            // given
            var expectedEntity = _context.Transits.Find(id);

            // when
            var receivedEntity = _repository.GetById(id);

            // then
            Assert.AreEqual(expectedEntity, receivedEntity);
        }


        [TestCase(false)]
        [TestCase(true)]
        public void GetListTest(bool includeAll)
        {
            // given
            var expected = _context.Transits.Where(t => !t.IsDeleted || includeAll).ToList();

            // when
            var list = _repository.GetList(includeAll);

            // then
            CollectionAssert.AreEqual(expected, list);
        }


        [Test]
        public void AddTest()
        {
            // given
            var entityToAdd = TestEntity();

            // when 
            int id = _repository.Add(entityToAdd);

            // then
            var entityOnCreate = _context.Transits.FirstOrDefault(o => o.Id == id);

            Assert.AreEqual(entityOnCreate, entityToAdd);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void UpdateEntityTest(int id)
        {
            // given
            var entityToEdit = _context.Transits.FirstOrDefault(o => o.Id == id);
            var newEntity = TestEntity();

            // when 
            _repository.Update(entityToEdit!, newEntity);

            // then
            var entityUpdated = _context.Transits.FirstOrDefault(o => o.Id == entityToEdit!.Id);
            Assert.AreEqual(entityToEdit, entityUpdated);
        }


        [TestCase(true)]
        [TestCase(false)]
        public void UpdateIsDeletedTest(bool isDeleted)
        {
            // given
            var entityToEdit = TestEntity();
            entityToEdit.IsDeleted = !isDeleted;
            _context.Transits.Add(entityToEdit);
            _context.SaveChanges();

            // when 
            _repository.Update(entityToEdit, isDeleted);

            // then
            var entityToUpdated = _context.Stations.FirstOrDefault(o => o.Id == entityToEdit.Id);

            Assert.AreEqual(entityToEdit, entityToUpdated);
        }

        private Transit TestEntity() => new Transit()
        {
            StartStation = new Station() { Name = "Norilsk" },
            EndStation = new Station() { Name = "Sweden" },
            Price = (decimal?)23.4
        };
    }
}
