using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal class CarriageServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetCarriage(), ConvertCarriageToCarriageModels(GetCarriage()), Role.Admin);
            yield return new TestCaseData(GetCarriage(), ConvertCarriageToCarriageModels(GetCarriage()), Role.TrainRouteManager);
        }

        public static IEnumerable<TestCaseData> GetTestCaseDataForGetListDeletedTest()
        {
            yield return new TestCaseData(GetCarriage(), ConvertCarriageToCarriageModels(GetCarriage()), false);
            yield return new TestCaseData(GetCarriage(), ConvertCarriageToCarriageModels(GetCarriage()));
        }
        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var carriage = GetCarriage();
            var carriageModels = ConvertCarriageToCarriageModels(GetCarriage());
            yield return new TestCaseData(carriage[0], carriageModels[0], Role.Admin);
            yield return new TestCaseData(carriage[0], carriageModels[0], Role.TrainRouteManager);
        }

        private static List<Carriage> GetCarriage() => new List<Carriage>
        {
            new Carriage
            {
                    Number=1,
                    Train=new Train
                    {
                        Carriages = new List<Carriage>()
                        {
                        new Carriage()
                        {
                            Number = 1,
                            IsDeleted = false,
                            Type = new CarriageType()
                            {
                                Name = "ffff",
                                NumberOfSeats = 1,
                                IsDeleted = false
                            }
                        }
                        },
                    IsDeleted = false
                    },
                    Type = new CarriageType{ Name="ffffff"},
                    IsDeleted=false
            },
        new Carriage
            {
                    Number=2,
                    Train=new Train
                    {
                        Carriages = new List<Carriage>()
                        {
                        new Carriage()
                        {
                            Number = 2,
                            IsDeleted = false,
                            Type = new CarriageType()
                            {
                                Name = "aaaa",
                                NumberOfSeats = 2,
                                IsDeleted = false
                            }
                        }
                        },
                    IsDeleted = false
                    },
                    Type = new CarriageType{ Name="aaaa"},
                    IsDeleted=false
            },
            new Carriage
            {
                    Number=3,
                    Train=new Train
                    {
                        Carriages = new List<Carriage>()
                        {
                        new Carriage()
                        {
                            Number = 3,
                            IsDeleted = false,
                            Type = new CarriageType()
                            {
                                Name = "ssss",
                                NumberOfSeats = 3,
                                IsDeleted = false
                            }
                        }
                        },
                    IsDeleted = false
                    },
                    Type = new CarriageType{ Name="sss"},
                    IsDeleted=false
            }
        };




        private static List<CarriageModel> ConvertCarriageToCarriageModels(List<Carriage> carriages, bool includeAll = true)
        {
            return carriages
                   .Where(c => includeAll || c.IsDeleted)
                   .Select(carriage => new CarriageModel
                   {
                       Number = carriage.Number,
                       Train = new TrainModel
                       {
                           Carriages = new List<CarriageModel>
                           {
                                new CarriageModel
                                {
                                    Number = 1,
                                    Type = new CarriageTypeModel
                                    {
                                        Name = "СВ",
                                        NumberOfSeats = 1
                                    }
                                }
                           }
                       },
                       Type = new CarriageTypeModel { Name = carriage.Type.Name, NumberOfSeats = carriage.Type.NumberOfSeats, IsDeleted = carriage.Type.IsDeleted },
                       IsDeleted = carriage.IsDeleted

                   })
                   .ToList();
        }
    }
}
