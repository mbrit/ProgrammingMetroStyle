using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace StreetFoo.Client
{
    public class ReportPageViewModel : ViewModelSingleton<ReportViewItem>, IReportPageViewModel
    {
        public ICommand OpenMapCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand EditCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        public ReportPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.OpenMapCommand = new DelegateCommand(async (args) => {
                var from = new AdHocMappablePoint(51.9972M, -0.7422M, "Bletchley");
                await LocationHelper.OpenMapsAppAsync(from, this.Item);
            });

            this.EditCommand = new DelegateCommand((args) => this.Host.ShowView(typeof(IEditReportPageViewModel), this.Item));
        }

        protected override async void ItemChanged()
        {
            // setup our image...
            var manager = new ReportImageCacheManager();
            await this.Item.InitializeAsync(manager);
        }

        public override void ShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // set the basics...
            var data = args.Request.Data;
            data.Properties.Title = string.Format("StreetFoo report '{0}'", this.Item.Title);
            data.Properties.Description = string.Format("Sharing problem report #{0}", this.Item.NativeId);

            // set the basics...
            data.SetText(string.Format("{0}: {1}", this.Item.Title, this.Item.Description));
            data.SetUri(new Uri(this.Item.PublicUrl));

            // tell the caller that we'll get back to them...
            if (this.Item.HasImage)
            {
                var reference = RandomAccessStreamReference.CreateFromUri(new Uri(this.Item.ImageUri));
                data.Properties.Thumbnail = reference;
                data.SetBitmap(reference);
            }
        }
    }
}
