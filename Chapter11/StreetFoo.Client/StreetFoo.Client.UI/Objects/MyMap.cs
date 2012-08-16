using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
    public class MyMap : ContentControl
    {
        // containerized map...
        private Map InnerMap { get; set; }

        // dependency properties...
        public static readonly DependencyProperty PushpinPointProperty =
            DependencyProperty.Register("PushpinPoint", typeof(AdHocMappablePoint), typeof(MyMap),
            new PropertyMetadata(null, (d, e) => ((MyMap)d).SetPushpinPoint((AdHocMappablePoint)e.NewValue)));
        public static readonly DependencyProperty ShowTrafficProperty =
            DependencyProperty.Register("ShowTraffic", typeof(bool), typeof(MyMap),
            new PropertyMetadata(null, (d, e) => ((MyMap)d).InnerMap.ShowTraffic = (bool)e.NewValue));

        // credentials...
        private const string BingMapsApiKey = "AhzHhvjTrVlqP1bs9D53ZWcLv5RsHkh_3BEFtTSfVoTjPxDl_PfkpbyfIh0a_H0a";

        // defines a standard zoom into street level...
        private const int StandardZoom = 15;

        public MyMap()
        {
            this.InnerMap = new Map();
            this.InnerMap.Credentials = BingMapsApiKey;

            // show it...
            this.Content = this.InnerMap;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            this.InnerMap.Width = finalSize.Width;
            this.InnerMap.Height = finalSize.Height;
            return base.ArrangeOverride(finalSize);
        }

        public Pushpin AddPushpin(IMappablePoint point)
        {
            // create a pin and set it's position...
            var pin = new Pushpin();
            pin.Text = point.Name;
            MapLayer.SetPosition(pin, point.ToLocation());

            // ...then add it...
            this.InnerMap.Children.Add(pin);

            // return...
            return pin;
        }

        public void AddPushpinAndCenterAndZoom(IMappablePoint point, bool animate = true)
        {
            var pin = this.AddPushpin(point);

            // show...
            var duration = MapAnimationDuration.Default;
            if (!(animate))
                duration = MapAnimationDuration.None;

            // show...
            this.InnerMap.SetView(point.ToLocation(), StandardZoom, duration);
        }

        public AdHocMappablePoint PushpinPoint
        {
            get { return (AdHocMappablePoint)GetValue(PushpinPointProperty); }
            set { SetValue(PushpinPointProperty, value); }
        }

        private void SetPushpinPoint(IMappablePoint point)
        {
            // set...
            this.ClearPushpins();

            // set...
            if (point != null)
                this.AddPushpinAndCenterAndZoom(point, false);
        }

        private void ClearPushpins()
        {
            this.InnerMap.Children.Clear();
        }

        public bool ShowTraffic
        {
            get { return (bool)GetValue(ShowTrafficProperty); }
            set { SetValue(ShowTrafficProperty, value); }
        }
    }
}
