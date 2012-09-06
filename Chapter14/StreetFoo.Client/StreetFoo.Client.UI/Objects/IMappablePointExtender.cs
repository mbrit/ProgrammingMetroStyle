using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing.Maps;

namespace StreetFoo.Client.UI
{
    public static class IMappablePointExtender
    {
        public static Location ToLocation(this IMappablePoint point)
        {
            return new Location((double)point.Latitude, (double)point.Longitude);
        }
    }
}
