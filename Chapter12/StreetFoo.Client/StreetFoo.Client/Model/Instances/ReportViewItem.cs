using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client
{
    public class ReportViewItem : WrappingModelItem<ReportItem>, IMappablePoint
    {
        public ReportViewItem(ReportItem item)
            : base(item)
        {
        }

        public int Id { get { return this.InnerItem.Id; } }
        public string NativeId { get { return this.InnerItem.NativeId; } }
        public string Title { get { return this.InnerItem.Title; } set { this.InnerItem.Title = value; } }
        public string Description { get { return this.InnerItem.Description; } set { this.InnerItem.Description = value; } }
        public decimal Latitude { get { return this.InnerItem.Latitude; } }
        public decimal Longitude { get { return this.InnerItem.Longitude; } }

        public string ImageUri { get { return GetValue<string>(); } set { SetValue(value); } }

        internal async Task InitializeAsync(ReportImageCacheManager manager)
        {
            // get...
            var imageUrl = await manager.GetLocalImageUriAsync(this);
            if (!(string.IsNullOrEmpty(imageUrl)))
            {
                // set it up...
                this.ImageUri = imageUrl;
            }
            else
            {
                // enqueue an image...
                manager.EnqueueImageDownload(this);
            }
        }

        public string PublicUrl
        {
            get
            {
                return string.Format("https://streetfoo.apphb.com/PublicReport.aspx?api={0}&id={1}", ServiceProxy.ApiKey, this.NativeId);
            }
        }

        public bool HasImage
        {
            get
            {
                return !(string.IsNullOrEmpty(this.ImageUri));
            }
        }

        string IMappablePoint.Name
        {
            get
            {
                return ((IMappablePoint)this.InnerItem).Name;
            }
        }

        internal void SetLocation(IMappablePoint point)
        {
            this.InnerItem.SetLocation(point);

            // update...
            this.OnPropertyChanged("LocationNarrative");
        }

        public string LocationNarrative
        {
            get
            {
                if (this.Latitude != 0 && this.Longitude != 0)
                    return string.Format("{0:n5},{1:n5}", this.Latitude, this.Longitude);
                else
                    return string.Empty;
            }
        }

        internal Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
