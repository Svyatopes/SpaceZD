using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Repositories;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests
{
    public class TripStationTests
    {
        private VeryVeryImportantContext _context;
        private TripStationRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                          .UseInMemoryDatabase(databaseName: "Test")
                          .Options;

            _context = new VeryVeryImportantContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new TripStationRepository(_context);
        }


        [Test]
        public void GetByIdTest()
        {
            // given
            var tripStationToAdd = GetTestTripStation();

            _context.TripStations.Add(tripStationToAdd);
            _context.SaveChanges();
            var idAddedTripStation = tripStationToAdd.Id;

            // when
            var receivedTripStation = _repository.GetById(idAddedTripStation);

            // then
            Assert.IsNotNull(receivedTripStation);
            AssertTestTripStation(receivedTripStation);
        }



        [Test]
        public void GetListTest()
        {
            // given
            var tripStationToAdd = GetTestTripStation();
            var secondTripStationToAdd = GetTestTripStation();
            var thirdTripStationToAdd = GetTestTripStation();

            _context.TripStations.Add(tripStationToAdd);
            _context.TripStations.Add(secondTripStationToAdd);
            _context.TripStations.Add(thirdTripStationToAdd);
            _context.SaveChanges();

            // when
            var tripStations = (List<TripStation>)_repository.GetList();

            // then

            Assert.IsNotNull(tripStations);
            Assert.AreEqual(2, tripStations.Count);

            var transitToCheck = tripStations[0];
            Assert.IsNotNull(transitToCheck);
            AssertTestTripStation(transitToCheck);
        }

        [Test]
        public void AddTest()
        {
            // given
            var tripStationToAdd = GetTestTripStation();

            // when 
            int id = _repository.Add(tripStationToAdd);

            // then
            var createdTripStation = _context.TripStations.FirstOrDefault(o => o.Id == id);

            AssertTestTripStation(createdTripStation!);
        }

        [Test]
        public void UpdateEntityTest()
        {
            // given
            var tripStationToAdd = GetTestTripStation();
            _context.TripStations.Add(tripStationToAdd);
            _context.SaveChanges();

            var tripStationToEdit = GetTestTripStation();
            tripStationToEdit.Id = tripStationToAdd.Id;

            //tripToEdit.StartStation = new Station() { Name = "Sevastopol" };
            //tripToEdit.EndStation = new Station() { Name = "Mariupol" };
            //tripToEdit.Price = (decimal?)24.4;

            // when 
            bool edited = _repository.Update(tripStationToEdit);

            // then
            var updatedTripStation = _context.TripStations.FirstOrDefault(o => o.Id == tripStationToEdit.Id);

            Assert.IsTrue(edited);
            Assert.IsNotNull(updatedTripStation);
            //Assert.IsNotNull(updatedTrip!.StartStation);
            //Assert.AreEqual("Sevastopol", updatedTrip.StartStation.Name);
            //Assert.IsNotNull(updatedTrip.EndStation);
            //Assert.AreEqual("Bolotnoe", updatedTrip.EndStation.Name);
            //Assert.IsNotNull(updatedTrip.Price);
            //Assert.AreEqual((decimal?)24.4, updatedTrip.Price);
        }

        private TripStation GetTestTripStation() => new TripStation()
        {
           

        };

        private void AssertTestTripStation(TripStation tripStation)
        {
            Assert.IsNotNull(tripStation);
            // Assert.IsNotNull(trip.StartStation);
            // Assert.AreEqual("Novosibirsk", trip.StartStation.Name);
            //   Assert.IsNotNull(trip.EndStation);
            // Assert.AreEqual("Sheregesh", trip.EndStation.Name);
            //  Assert.IsNotNull(trip.Price);
            // Assert.AreEqual((decimal?)23.4, trip.Price);
        }
    }
}
