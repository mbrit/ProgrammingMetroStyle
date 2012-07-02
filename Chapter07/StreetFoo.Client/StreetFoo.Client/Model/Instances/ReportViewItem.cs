using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client
{
    public class ReportViewItem : ModelItem<ReportItem>
    {
        internal ReportViewItem(ReportItem item)
            : base(item)
        {
            // we'll initialize the image later...
            this.HasImage = false;
        }

        public string NativeId { get { return this.InnerItem.NativeId; } }
        public string Title { get { return this.InnerItem.Title; } }
        public string Description { get { return this.InnerItem.Description; } }

        public bool HasImage { get { return GetValue<bool>(); } set { SetValue(value); } }
        public string Image { get { return GetValue<string>(); } set { SetValue(value); } }

        internal async Task InitializeAsync(ReportImageCacheManager manager)
        {
            // get...
            var imageUrl = await manager.GetLocalImageUrlAsync(this);
            if (!(string.IsNullOrEmpty(imageUrl)))
            {
                // set it up...
                this.SetLocalImageUrl(imageUrl);
            }
            else
            {
                // enqueue an image...
                manager.EnqueueImageDownload(this);
            }
        }

        internal void SetLocalImageUrl(string imageUrl)
        {
            // sets out internal state to indicate that we have an image...
            this.Image = imageUrl;
            this.HasImage = true;
        }
    }
}
