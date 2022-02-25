using SpaceZD.DataLayer.Entities;
using System.Collections.Generic;

namespace SpaceZD.DataLayer.Tests.TestMocks
{
    public class TrainRepositoryMocks
    {
        public static Train GetTestEntity() => new Train()
        {
            Carriages = new List<Carriage>()
            {
                new()
                {
                    Number = 5,
                    Type = new CarriageType()
                    {
                        Name = "Econom",
                        NumberOfSeats = 67
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 147 },
                        new() { Price = 100 },
                        new() { Price = 124 }
                    }
                },
                new()
                {
                    Number = 45,
                    Type = new CarriageType()
                    {
                        Name = "Premium",
                        NumberOfSeats = 4
                    },
                    Tickets = new List<Ticket>
                    {
                        new() { Price = 547 },
                        new() { Price = 500 },
                        new() { Price = 524 }
                    }
                }

            }
        };

        public static List<Train> GetListTestEntities() => new List<Train>()
        {
            new()
            {
                Carriages = new List<Carriage>()
                {
                    new()
                    {
                        Number = 5,
                        Type = new CarriageType()
                        {
                            Name = "Econom",
                            NumberOfSeats = 67
                        },
                        Tickets = new List<Ticket>
                        {
                            new() { Price = 147 },
                            new() { Price = 100 },
                            new() { Price = 124 }
                        }
                    },
                    new()
                    {
                        Number = 45,
                        Type = new CarriageType()
                        {
                            Name = "Premium",
                            NumberOfSeats = 4
                        },
                        Tickets = new List<Ticket>
                        {
                            new() { Price = 547 },
                            new() { Price = 500 },
                            new() { Price = 524 }
                        }
                    }
                }
            },
            new()
            {
                Carriages = new List<Carriage>()
                {
                    new()
                    {
                        Number = 45,
                        Type = new CarriageType()
                        {
                            Name = "Econom",
                            NumberOfSeats = 67
                        },
                        Tickets = new List<Ticket>
                        {
                            new() { Price = 147 },
                            new() { Price = 100 },
                            new() { Price = 124 }
                        }
                    },
                    new()
                    {
                        Number = 45,
                        Type = new CarriageType()
                        {
                            Name = "Premium",
                            NumberOfSeats = 4
                        },
                        Tickets = new List<Ticket>
                        {
                            new() { Price = 547 },
                            new() { Price = 500 },
                            new() { Price = 524 }
                        }
                    }

                }
            },
            new()
            {
                Carriages = new List<Carriage>()
                {
                    new()
                    {
                        Number = 7,
                        Type = new CarriageType()
                        {
                            Name = "Cafe",
                            NumberOfSeats = 5
                        },
                        Tickets = new List<Ticket>
                        {
                            new() { Price = 555 },
                            new() { Price = 678 },
                            new() { Price = 222 }
                        }
                    },
                    new()
                    {
                        Number = 59,
                        Type = new CarriageType()
                        {
                            Name = "Econom",
                            NumberOfSeats = 78
                        },
                        Tickets = new List<Ticket>
                        {
                            new() { Price = 678 },
                            new() { Price = 345 },
                            new() { Price = 134 }
                        }
                    }

                }
            }
        };
    }
}
