using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.BusinessLayer.Exceptions
{
    public class AftorizationException : Exception
    {
        public AftorizationException(string message) : base(message) { }
    }
}
