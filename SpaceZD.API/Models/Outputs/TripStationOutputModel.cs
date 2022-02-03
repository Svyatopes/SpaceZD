﻿namespace SpaceZD.API.Models.Outputs
{
    public class TripStationOutputModel
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