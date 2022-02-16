using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    public class TrainServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetTrain(), GetTicketModel(), false);
            yield return new TestCaseData(GetTrain(), GetTicketModel(), true);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var trains = GetTrain();
            var trainModels = GetTicketModel();
            yield return new TestCaseData(trains[0], trainModels[0]);
            yield return new TestCaseData(trains[1], trainModels[1]);
        }


        private static List<Train> GetTrain()
        {
            var listTrain = new List<Train>()
            {
                new Train
                {
                    Carriages = new List<Carriage>()
                    {
                        new Carriage()
                        {
                            Number = 8,
                            IsDeleted = false,
                            Type = new CarriageType()
                            {
                                Name = "Econom",
                                NumberOfSeats = 5,
                                IsDeleted = false
                            }
                        }
                    },
                    IsDeleted = false
                },
                new Train
                {
                    Carriages = new List<Carriage>()
                    {
                        new Carriage()
                        {
                            Number = 4,
                            IsDeleted = false,
                            Type = new CarriageType()
                            {
                                Name = "Vip",
                                NumberOfSeats = 14,
                                IsDeleted = false
                            }
                        }
                    },
                    IsDeleted = true
                }
            };

            return listTrain;
        }

        private static List<TrainModel> GetTicketModel()
        {
            var listTrainModel = new List<TrainModel>()
            {
                new TrainModel
                {
                    Carriages = new List<CarriageModel>()
                    {
                        new CarriageModel()
                        {
                            Number = 8,
                            IsDeleted = false,
                            Type = new CarriageTypeModel()
                            {
                                Name = "Econom",
                                NumberOfSeats = 5,
                                IsDeleted = false
                            }
                        }
                    },
                    IsDeleted = false
                },
                new TrainModel
                {
                    Carriages = new List<CarriageModel>()
                    {
                        new CarriageModel()
                        {
                            Number = 4,
                            IsDeleted = false,
                            Type = new CarriageTypeModel()
                            {
                                Name = "Vip",
                                NumberOfSeats = 14,
                                IsDeleted = false
                            }
                        }
                    },
                    IsDeleted = true
                }
            };
            return listTrainModel;
        }
    }
}

