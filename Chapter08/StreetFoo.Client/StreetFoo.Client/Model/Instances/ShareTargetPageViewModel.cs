using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace StreetFoo.Client
{
    public class ShareTargetPageViewModel : ViewModel, IShareTargetPageViewModel
    {
        private ShareOperation ShareOperation { get; set; }

        public string Title { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }
        public string Description { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }
        public string Comment { get { return this.GetValue<string>(); } set { this.SetValue(value); } }

        public string SharedText { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }
        public BitmapImage SharedImage { get { return this.GetValue<BitmapImage>(); } private set { this.SetValue(value); } }

        public bool ShowImage { get { return this.GetValue<bool>(); } private set { this.SetValue(value); } }
        public bool SupportsComment { get { return this.GetValue<bool>(); } private set { this.SetValue(value); } }
        public bool Sharing { get { return this.GetValue<bool>(); } private set { this.SetValue(value); } }

        public ICommand ShareCommand { get; private set; }

        public ShareTargetPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.ShowImage = false;
            this.Sharing = false;
            this.SupportsComment = true;

            this.ShareCommand = new DelegateCommand(async (args) => await HandleShareCommandAsync());
        }

        private async Task HandleShareCommandAsync()
        {
            try
            {
                // tell the OS that we've started...
                this.ShareOperation.ReportStarted();

                // placeholder message...
                await this.Host.ShowAlertAsync("Shared.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Sharing failed: " + ex.ToString());
                this.ShareOperation.ReportError("The sharing operation failed.");
            }
            finally
            {
                this.ShareOperation.ReportCompleted();
            }
        }

        public async Task SetupShareDataAsync(ShareOperation share)
        {
            // store the share operation - we need to do this to hold a 
            // reference otherwise the sharing subsystem will assume 
            // that we've finished...
            this.ShareOperation = share;

            // get the properties out...
            var data = share.Data;
            var props = data.Properties;
            this.Title = props.Title;
            this.Description = props.Description;

            // now the text...
            if (data.Contains(StandardDataFormats.Text))
                this.SharedText = await data.GetTextAsync();

            // do we have an image? if so, load it...
            if (data.Contains(StandardDataFormats.StorageItems) || data.Contains(StandardDataFormats.Bitmap))
            {
                IRandomAccessStreamReference reference = null;

                // load the first one...
                if (data.Contains(StandardDataFormats.StorageItems))
                {
                    var file = (IStorageFile)(await data.GetStorageItemsAsync()).FirstOrDefault();
                    reference = RandomAccessStreamReference.CreateFromFile(file);
                }
                else
                    reference = await data.GetBitmapAsync();

                // load it into an image...
                var image = new BitmapImage();
                using (var stream = await reference.OpenReadAsync())
                    image.SetSource(stream);

                // set...
                this.SharedImage = image;
                this.ShowImage = true;
            }

            // tell the OS that we have the data...
            share.ReportDataRetrieved();
        }
    }
}
