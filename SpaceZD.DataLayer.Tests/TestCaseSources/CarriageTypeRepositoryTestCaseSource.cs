using System.Collections.Generic;
using NUnit.Framework;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestCaseSources;

public class CarriageTypeRepositoryTestCaseSource
{
    internal static List<CarriageType> GetCarriageTypes() => new List<CarriageType>
    {
        new()
        {
            Name = "Плацкарт",
            NumberOfSeats = 5,
            PriceCoefficient = 2,
            IsDeleted = false
        },
        new()
        {
            Name = "Ласточка",
            NumberOfSeats = 7,
            PriceCoefficient = new decimal(1.5),
            IsDeleted = true
        },
        new()
        {
            Name = "Сапсан",
            NumberOfSeats = 8,
            PriceCoefficient = new decimal(0.5),
            IsDeleted = false
        }
    };

    internal static CarriageType GetCarriageType() => new()
    {
        Name = "Купе",
        NumberOfSeats = 4
    };

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        var carriageTypeFist = new CarriageType
            { Name = GetCarriageTypes()[0].Name, NumberOfSeats = 5, PriceCoefficient = GetCarriageTypes()[0].PriceCoefficient, IsDeleted = false };
        yield return new TestCaseData(1, carriageTypeFist);

        var carriageTypeSecond = new CarriageType
            { Name = GetCarriageTypes()[1].Name, NumberOfSeats = 7, PriceCoefficient = GetCarriageTypes()[1].PriceCoefficient, IsDeleted = true };
        yield return new TestCaseData(2, carriageTypeSecond);

        var carriageTypeThird = new CarriageType
            { Name = GetCarriageTypes()[2].Name, NumberOfSeats = 8, PriceCoefficient = GetCarriageTypes()[2].PriceCoefficient, IsDeleted = false };
        yield return new TestCaseData(3, carriageTypeThird);

        yield return new TestCaseData(4, null);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        var notIncludeAll = new List<CarriageType>
        {
            new()
            {
                Name = GetCarriageTypes()[0].Name,
                NumberOfSeats = 5,
                PriceCoefficient = GetCarriageTypes()[0].PriceCoefficient,
                IsDeleted = false
            },
            new()
            {
                Name = GetCarriageTypes()[2].Name,
                NumberOfSeats = 8,
                PriceCoefficient = GetCarriageTypes()[2].PriceCoefficient,
                IsDeleted = false
            }
        };
        yield return new TestCaseData(false, notIncludeAll);

        var includeAll = new List<CarriageType>
        {
            new()
            {
                Name = GetCarriageTypes()[0].Name,
                NumberOfSeats = 5,
                PriceCoefficient = GetCarriageTypes()[0].PriceCoefficient,
                IsDeleted = false
            },
            new()
            {
                Name = GetCarriageTypes()[1].Name,
                NumberOfSeats = 7,
                PriceCoefficient = GetCarriageTypes()[1].PriceCoefficient,
                IsDeleted = true
            },
            new()
            {
                Name = GetCarriageTypes()[2].Name,
                NumberOfSeats = 8,
                PriceCoefficient = GetCarriageTypes()[2].PriceCoefficient,
                IsDeleted = false
            }
        };
        yield return new TestCaseData(true, includeAll);
    }
}