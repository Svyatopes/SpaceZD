using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.BusinessLayer.Models
{
    public class TransitModel
    {
        public int Id { get; set; }
        public virtual StationModel StartStation { get; set; }
        public virtual StationModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<RouteTransitModel> RouteTransit { get; set; }
    }
}
