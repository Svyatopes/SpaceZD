using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.BusinessLayer.Models
{
    public class StationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<Platform> Platforms { get; set; }
        public bool IsDeleted { get; set; }
        public List<RouteModel> RoutesWithStartStation { get; set; }
        public List<RouteModel> RoutesWithEndStation { get; set; }
        //public List<Transit> TransitsWithStartStation { get; set; }
        //public List<Transit> TransitsWithEndStation { get; set; }
        //public List<TripStation> TripStations { get; set; }
    }
}
