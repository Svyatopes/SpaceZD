using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestCaseSources;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class PersonRepositoryTests
{
    private VeryVeryImportantContext _context;
    private PersonRepository _repository;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                      .UseInMemoryDatabase(databaseName: "Test")
                      .Options;

        _context = new VeryVeryImportantContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repository = new PersonRepository(_context);

        //seed
        var persons = PersonRepositoryMocks.GetPersons();
        _context.Persons.AddRange(persons);
        _context.SaveChanges();
    }



    [Test]
    public void GetByIdTest()
    {
        //given
        var person = _context.Persons.First();

        //when
        var receivedOrder = _repository.GetById(person.Id);

        //then
        Assert.AreEqual(person, receivedOrder);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void GetListAllIncludedTest(bool allIncluded)
    {
        //given
        var persons = _context.Persons.Where(o => !o.IsDeleted || allIncluded);

        //when
        var personsInDB = (List<Person>)_repository.GetList(allIncluded);

        //then
        Assert.AreEqual(persons, personsInDB);

    }

    [Test]
    public void AddTest()
    {
        //given
        var personToAdd = PersonRepositoryMocks.GetPerson();

        //when 
        int id = _repository.Add(personToAdd);

        //then
        var createdPerson = _context.Persons.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(personToAdd, createdPerson);
    }

    [Test]
    public void UpdateEntityTest()
    {
        //given
        var person = PersonRepositoryMocks.GetPerson();

        person.FirstName = "TestFirstName";
        person.LastName = "TestLastName";
        person.Patronymic = "TestPatronymic";
        person.Passport = "TestPassport";
        //SHOULDN'T CHANGED IN DB
        person.IsDeleted = true;

        var personToEdit = _context.Persons.First();

        //when 
        _repository.Update(personToEdit, person);

        //then
        var updatedPerson = _context.Persons.FirstOrDefault(o => o.Id == personToEdit.Id);

        Assert.AreEqual(personToEdit.Id, updatedPerson!.Id);
        Assert.AreEqual(person.FirstName, updatedPerson.FirstName);
        Assert.AreEqual(person.LastName, updatedPerson.LastName);
        Assert.AreEqual(person.Passport, updatedPerson.Passport);
        Assert.AreEqual(person.Patronymic, updatedPerson.Patronymic);
        Assert.AreNotEqual(person.IsDeleted, updatedPerson.IsDeleted);

    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        //given
        var personToEdit = PersonRepositoryMocks.GetPerson();
        personToEdit.IsDeleted = !isDeleted;
        _context.Persons.Add(personToEdit);
        _context.SaveChanges();

        //when 
        _repository.Update(personToEdit, isDeleted);

        //then

        var updatedPerson = _context.Persons.FirstOrDefault(o => o.Id == personToEdit.Id);

        Assert.AreEqual(isDeleted, updatedPerson!.IsDeleted);
        Assert.AreEqual(personToEdit, updatedPerson);
    }

}

