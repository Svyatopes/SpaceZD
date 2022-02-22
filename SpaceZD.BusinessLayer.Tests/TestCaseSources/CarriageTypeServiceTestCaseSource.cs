using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources;

public class CarriageTypeServiceTestCaseSource
{
    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetByIdTest()
    {
        yield return new TestCaseData(new CarriageType { Name = "Rbs", NumberOfSeats = 2, PriceFactor = 49, IsDeleted = true }, Role.Admin);
        yield return new TestCaseData(new CarriageType { Name = "Купе", NumberOfSeats = 3, PriceFactor = 4, IsDeleted = false }, Role.Admin);
        yield return new TestCaseData(new CarriageType { Name = "Ласточка", NumberOfSeats = 2, PriceFactor = 2.1, IsDeleted = false }, Role.Admin);
        yield return new TestCaseData(new CarriageType { Name = "Сапсан", NumberOfSeats = 3, PriceFactor = 5.645, IsDeleted = false }, Role.TrainRouteManager);
        yield return new TestCaseData(new CarriageType { Name = "Плацкарт", NumberOfSeats = 4, PriceFactor = 1.64, IsDeleted = false }, Role.TrainRouteManager);
        yield return new TestCaseData(new CarriageType { Name = "Сидячие места", NumberOfSeats = 5, PriceFactor = 1.457, IsDeleted = true }, Role.TrainRouteManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        var fistList = new List<CarriageType>
        {
            new() { Name = "Купе", NumberOfSeats = 3, PriceFactor = 4, IsDeleted = false },
            new() { Name = "Ласточка", NumberOfSeats = 2, PriceFactor = 2.1, IsDeleted = false },
            new() { Name = "Сапсан", NumberOfSeats = 3, PriceFactor = 5.645, IsDeleted = false },
            new() { Name = "Плацкарт", NumberOfSeats = 4, PriceFactor = 1.64, IsDeleted = false }
        };
        var secondList = new List<CarriageType>
        {
            new() { Name = "Rbs", NumberOfSeats = 2, PriceFactor = 49, IsDeleted = false },
            new() { Name = "Сидячие места", NumberOfSeats = 5, PriceFactor = 1.457, IsDeleted = false }
        };

        yield return new TestCaseData(fistList, ConvertCarriageTypesToCarriageTypeModels(fistList));
        yield return new TestCaseData(secondList, ConvertCarriageTypesToCarriageTypeModels(secondList));
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListDeletedTest()
    {
        var fistList = new List<CarriageType>
        {
            new() { Name = "Купе", NumberOfSeats = 3, PriceFactor = 4, IsDeleted = false },
            new() { Name = "Ласточка", NumberOfSeats = 2, PriceFactor = 2.1, IsDeleted = true },
            new() { Name = "Сапсан", NumberOfSeats = 3, PriceFactor = 5.645, IsDeleted = true },
            new() { Name = "Плацкарт", NumberOfSeats = 4, PriceFactor = 1.64, IsDeleted = true }
        };
        var secondList = new List<CarriageType>
        {
            new() { Name = "Rbs", NumberOfSeats = 2, PriceFactor = 49, IsDeleted = true },
            new() { Name = "Сидячие места", NumberOfSeats = 5, PriceFactor = 1.457, IsDeleted = true }
        };

        yield return new TestCaseData(fistList, ConvertCarriageTypesToCarriageTypeModels(fistList, false));
        yield return new TestCaseData(secondList, ConvertCarriageTypesToCarriageTypeModels(secondList, false));
    }

    private static List<CarriageTypeModel> ConvertCarriageTypesToCarriageTypeModels(List<CarriageType> carriageTypes, bool includeAll = true)
    {
        return carriageTypes
               .Where(t => includeAll || t.IsDeleted)
               .Select(carriageType => new CarriageTypeModel
               {
                   Name = carriageType.Name,
                   NumberOfSeats = carriageType.NumberOfSeats,
                   PriceFactor = carriageType.PriceFactor,
                   IsDeleted = carriageType.IsDeleted
               })
               .ToList();
    }
}