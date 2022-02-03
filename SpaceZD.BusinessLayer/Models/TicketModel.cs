﻿namespace SpaceZD.BusinessLayer.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        //public Order Order { get; set; }
       // public Carriage Carriage { get; set; }
        public int SeatNumber { get; set; }
        public bool IsTeaIncluded { get; set; }
        public bool IsPetPlaceIncluded { get; set; }
       // public Person Person { get; set; }
        public decimal Price { get; set; }
    }
}