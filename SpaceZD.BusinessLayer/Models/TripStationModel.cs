using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.BusinessLayer.Models
{
    public class TripStationModel
    {
        public int Id { get; set; }
        public virtual StationModel Station { get; set; }
        public virtual PlatformModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public virtual TripModel Trip { get; set; }
        public virtual ICollection<OrderModel> OrdersWithStartStation { get; set; }
        public virtual ICollection<OrderModel> OrdersWithEndStation { get; set; }
    }
}
