using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.BusinessLayer.Models
{
    public class TrainOutputModel
    {
        public int Id { get; set; }
        public List<CarriageShortOutputModel> Carriages { get; set; }
        
    }
}
