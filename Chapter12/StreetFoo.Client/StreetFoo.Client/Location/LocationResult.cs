using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace StreetFoo.Client
{
    public class LocationResult
    {
        public LocationResultCode Code { get; private set; }
        public Geoposition Location { get; private set; }

        internal LocationResult(LocationResultCode code)
        {
            this.Code = code;
        }

        internal LocationResult(Geoposition location)
            : this(LocationResultCode.Ok)
        {
            this.Location = location;
        }

        public IMappablePoint ToMappablePoint(string name = "Location")
        {
            return new AdHocMappablePoint((decimal)this.Location.Coordinate.Latitude, (decimal)this.Location.Coordinate.Longitude, "Location");
        }
    }
}
