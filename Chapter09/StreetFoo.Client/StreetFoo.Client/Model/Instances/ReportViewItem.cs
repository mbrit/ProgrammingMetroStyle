using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client
{
    public class ReportViewItem : WrappingModelItem<ReportItem>
    {
        internal ReportViewItem(ReportItem item)
            : base(item)
        {
        }

        public string NativeId { get { return this.InnerItem.NativeId; } }
        public string Title { get { return this.InnerItem.Title; } }
        public string Description { get { return this.InnerItem.Description; } }

        public string ImageUrl { get { return GetValue<string>(); } set { SetValue(value); } }

        internal async Task InitializeAsync(ReportImageCacheManager manager)
        {
            // get...
            var imageUrl = await manager.GetLocalImageUrlAsync(this);
            if (!(string.IsNullOrEmpty(imageUrl)))
            {
                // set it up...
                this.ImageUrl = imageUrl;
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
                return !(string.IsNullOrEmpty(this.ImageUrl));
            }
        }
    }
}
