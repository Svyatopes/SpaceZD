﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Repositories;
using System.Linq;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Tests.TestMocks;

namespace SpaceZD.DataLayer.Tests
{
    public class TripTests
    {
        private VeryVeryImportantContext _context;
        private ITripRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                         .UseInMemoryDatabase(databaseName: "Test")
                         .Options;

            _context = new VeryVeryImportantContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new TripRepository(_context);

            var trips = TripRepositoryMocks.GetTrips();
            _context.Trips.AddRange(trips);
            _context.SaveChanges();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetByIdTest(int id)
        {
            // given
            var expectedEntity = _context.Trips.Find(id);

            // when
            var receivedEntity = _repository.GetById(id);

            // then
            Assert.AreEqual(expectedEntity, receivedEntity);
        }


        [Test]
        public void GetListTest()
        {
            // given
            var expected = TripRepositoryMocks.GetShortTrips();

            // when
            var list = _repository.GetList();

            // then
            CollectionAssert.AreEqual(expected, list);
        }


        [Test]
        public void AddTest()
        {
            // given
            var entityToAdd = TripRepositoryMocks.GetTrip();

            // when 
            int id = _repository.Add(entityToAdd);

            // then
            var entityOnCreate = _context.Trips.FirstOrDefault(o => o.Id == id);

            Assert.AreEqual(entityOnCreate, entityToAdd);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void UpdateEntityTest(int id)
        {
            // given
            var entityToEdit = _context.Trips.FirstOrDefault(o => o.Id == id);
            var newEntity = TripRepositoryMocks.GetTrip();

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
            var entityToEdit = TripRepositoryMocks.GetTrip();
            entityToEdit.IsDeleted = !isDeleted;
            _context.Trips.Add(entityToEdit);
            _context.SaveChanges();

            // when 
            _repository.Update(entityToEdit, isDeleted);

            // then
            var entityToUpdated = _context.Stations.FirstOrDefault(o => o.Id == entityToEdit.Id);

            Assert.AreEqual(entityToEdit, entityToUpdated);
        }


        [TestCaseSource(typeof(TripRepositoryMocks), nameof(TripRepositoryMocks.GetTestCaseDataForMarkNonFreeSeatsInListAllSeatsTest))]
        public void MarkNonFreeSeatsInListAllSeatsTest(Trip trip, Station startStation, Station endStation, List<CarriageSeats> allPlaces, List<CarriageSeats> expected)
        {
            // when
            var actual = _repository.MarkNonFreeSeatsInListAllSeats(trip, startStation, endStation, allPlaces);

            // then
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}