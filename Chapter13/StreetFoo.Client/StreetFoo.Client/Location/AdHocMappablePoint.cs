using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class AdHocMappablePoint : IMappablePoint
    {
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public string Name { get; private set; }

        public AdHocMappablePoint(IMappablePoint point)
            : this(point.Latitude, point.Longitude, point.Name)
        {
        }

        public AdHocMappablePoint(decimal latitude, decimal longitude, string name)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Name = name;
        }
    }
}
