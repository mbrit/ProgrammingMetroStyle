using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.System;

namespace StreetFoo.Client
{
    public static class LocationHelper
    {
        public static async Task<LocationResult> GetCurrentLocationAsync()
        {
            try
            {
                var locator = new Geolocator();
                var position = await locator.GetGeopositionAsync();

                // return...
                return new LocationResult(position);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine("Geolocation access denied: " + ex.ToString());
                return new LocationResult(LocationResultCode.AccessDenied);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Geolocation failure: " + ex.ToString());
                return new LocationResult(LocationResultCode.UnknownError);
            }
        }

        internal static async Task OpenMapsAppAsync(IMappablePoint point, bool showTraffic = true)
        {
            string trafficFlag = "0";
            if(showTraffic)
                trafficFlag = "1";

            // create the uri...
            var uri = string.Format("bingmaps://open/?cp={0:n5}~{1:n5}&lvl=15&trfc={2}", point.Latitude, point.Longitude, trafficFlag);
            Debug.WriteLine("Navigating: {0}", uri);

            // open...
            await Launcher.LaunchUriAsync(new Uri(uri));
        }

        internal static async Task OpenMapsAppAsync(IMappablePoint from, IMappablePoint to, bool showTraffic = true)
        {
            string trafficFlag = "0";
            if (showTraffic)
                trafficFlag = "1";

            // create the uri...
            var uri = string.Format("bingmaps://open/?rtp=pos.{0:n5}_{1:n5}~pos.{2:n5}_{3:n5}&trfc={4}", from.Latitude, from.Longitude, 
                to.Latitude, to.Longitude, trafficFlag);
            Debug.WriteLine("Navigating: {0}", uri);

            // open...
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}
