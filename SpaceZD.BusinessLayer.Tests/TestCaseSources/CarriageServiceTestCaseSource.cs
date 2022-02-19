using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal class CarriageServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetCarriage(), GetCarriageModel(), false);
            yield return new TestCaseData(GetCarriage(), GetCarriageModel(), true);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var carriage = GetCarriage();
            var carriageModels = GetCarriageModel();
            yield return new TestCaseData(carriage[0], carriageModels[0]);
            yield return new TestCaseData(carriage[1], carriageModels[1]);
        }

        private static List<Carriage> GetCarriage() => new List<Carriage>
        {
            new Carriage
            {
                    Number=1,
                    Type = new CarriageType()
                    {
                        Name = "Плацкарт"
                    }
            },
            new Carriage
            {
                    Number=2,
                    Type = new CarriageType()
                    {
                        Name = "Ласточка"
                    }
            },
                new Carriage
                {
                    Number=3,
                    Type = new CarriageType()
                    {
                        Name = "Купе",
                        IsDeleted=true
                    }
                }
            };

        private static List<CarriageModel> GetCarriageModel() => new List<CarriageModel>
        {
            new CarriageModel
            {
                    Number=1,
                    Type = new CarriageTypeModel()
                    {
                        Name = "Плацкарт"
                    }
            },
            new CarriageModel
            {
                    Number=2,
                    Type = new CarriageTypeModel()
                    {
                        Name = "Ласточка"
                    }
            },
                new CarriageModel
                {
                    Number=3,
                    Type = new CarriageTypeModel()
                    {
                        Name = "Купе",
                        IsDeleted=true
                    }
                }
            };
    }
}
