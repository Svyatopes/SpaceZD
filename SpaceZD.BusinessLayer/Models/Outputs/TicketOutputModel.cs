using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.BusinessLayer.Models
{
    public class TicketOutputModel
    {
        public int Id { get; set; }
        public OrderShortOutputModel Order { get; set; }
        public CarriageShortOutputModel Carriage { get; set; }
        public int SeatNumber { get; set; }
        public bool IsTeaIncluded { get; set; }
        public bool IsPetPlaceIncluded { get; set; }
        public PersonShortOutputModel Person { get; set; }
    }
}
