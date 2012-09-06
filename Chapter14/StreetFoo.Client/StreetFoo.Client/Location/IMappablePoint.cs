using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface IMappablePoint
    {
        decimal Latitude { get; }
        decimal Longitude { get; }
        string Name { get; }
    }
}
