using System.Collections.Generic;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources;

public static class StationServiceTestCaseSource
{
    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListTest()
    {
        yield return new TestCaseData(new List<Station>
            {
                new() { Name = "Челябинск", Platforms = new List<Platform>(), IsDeleted = false },
                new() { Name = "Омск", Platforms = new List<Platform>(), IsDeleted = false },
                new() { Name = "48 км", Platforms = new List<Platform>(), IsDeleted = false },
                new() { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = false }
            },
            new List<StationModel>
            {
                new() { Name = "Челябинск", Platforms = new List<PlatformModel>(), IsDeleted = false },
                new() { Name = "Омск", Platforms = new List<PlatformModel>(), IsDeleted = false },
                new() { Name = "48 км", Platforms = new List<PlatformModel>(), IsDeleted = false },
                new() { Name = "Выборг", Platforms = new List<PlatformModel>(), IsDeleted = false }
            },
            Role.Admin);
        yield return new TestCaseData(new List<Station>
            {
                new() { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = false },
                new() { Name = "Красное село", Platforms = new List<Platform>(), IsDeleted = false }
            },
            new List<StationModel>
            {
                new() { Name = "Москва", Platforms = new List<PlatformModel>(), IsDeleted = false },
                new() { Name = "Красное село", Platforms = new List<PlatformModel>(), IsDeleted = false }
            },
            Role.StationManager);
    }

    internal static IEnumerable<TestCaseData> GetTestCaseDataForGetListDeletedTest()
    {
        yield return new TestCaseData(new List<Station>
            {
                new() { Name = "Челябинск", Platforms = new List<Platform>(), IsDeleted = false },
                new() { Name = "Омск", Platforms = new List<Platform>(), IsDeleted = true },
                new() { Name = "48 км", Platforms = new List<Platform>(), IsDeleted = false },
                new() { Name = "Выборг", Platforms = new List<Platform>(), IsDeleted = true }
            },
            new List<StationModel>
            {
                new() { Name = "Омск", Platforms = new List<PlatformModel>(), IsDeleted = true },
                new() { Name = "Выборг", Platforms = new List<PlatformModel>(), IsDeleted = true }
            });
        yield return new TestCaseData(new List<Station>
            {
                new() { Name = "Москва", Platforms = new List<Platform>(), IsDeleted = true },
                new() { Name = "Красное село", Platforms = new List<Platform>(), IsDeleted = true }
            },
            new List<StationModel>
            {
                new() { Name = "Москва", Platforms = new List<PlatformModel>(), IsDeleted = true },
                new() { Name = "Красное село", Platforms = new List<PlatformModel>(), IsDeleted = true }
            });
    }
}