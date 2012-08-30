using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace StreetFoo.Client
{
    public class EditReportPageViewModel : ViewModelSingleton<ReportViewItem>, IEditReportPageViewModel
    {
        public ICommand TakePhotoCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand CaptureLocationCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        // holds the image in TempState that we're displaying...
        private IStorageFile TempImageFile { get; set; }

        public EditReportPageViewModel(IViewModelHost host)
            : base(host)
        {
            // setup the commands...
            this.TakePhotoCommand = new DelegateCommand(async (args) => await this.CaptureImageAsync());
            this.CaptureLocationCommand = new DelegateCommand(async (args) => await this.CaptureLocationAsync());
        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            // capture...
            await CaptureLocationAsync();
        }

        private async Task CaptureLocationAsync()
        {
            var result = await LocationHelper.GetCurrentLocationAsync();
            if (result.Code == LocationResultCode.Ok)
                this.Item.SetLocation(result.ToMappablePoint());
        }

        public bool IsNew
        {
            get
            {
                // we may not have an item yet...
                if (this.Item == null)
                    return true;
                else
                    return this.Item.Id == 0;
            }
        }

        protected override async void ItemChanged()
        {
            base.ItemChanged();

            // if we're not new, load the image...
            if (!(this.IsNew))
            {
                var manager = new ReportImageCacheManager();
                await this.Item.InitializeAsync(manager);
                
                // initialie the image...
                if(this.Item.HasImage)
                {
                    var image = new BitmapImage();
                    image.UriSource = new Uri(this.Item.ImageUri);
                    this.Image = image;
                }
            }

            // raise the new regardless...
            this.OnPropertyChanged("IsNew");
        }

        private async Task CaptureImageAsync()
        {
            // get the image...
            var ui = new CameraCaptureUI();
            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            // did we get one?
            if (file != null)
            {
                // do we have an old one to delete...
                await CleanupTempImageFileAsync();

                // load the image for display...
                var newImage = new BitmapImage();
                using (var stream = await file.OpenReadAsync())
                    newImage.SetSource(stream);

                // set...
                this.Image = newImage;
                this.TempImageFile = file;
            }
        }

        private async Task CleanupTempImageFileAsync()
        {
            try
            {
                if (this.TempImageFile != null)
                    await this.TempImageFile.DeleteAsync();
            }
            catch
            {
                // ignore errors...
            }
            finally
            {
                this.TempImageFile = null;
            }
        }

        public bool HasImage
        {
            get
            {
                return this.Image != null;
            }
        }

        public BitmapImage Image
        {
            get
            {
                return this.GetValue<BitmapImage>();
            }
            set
            {
                // set...
                this.SetValue(value);

                // update the flag...
                this.OnPropertyChanged("HasImage");
            }
        }

        protected override async Task CancelAsync()
        {
            // remove the temp image...
            await this.CleanupTempImageFileAsync();

            // base...
            await base.CancelAsync();
        }

        protected override async Task DoSaveAsync()
        {
            // save...
            if (this.IsNew)
            {
                // create a new one...
                await ReportItem.CreateReportItemAsync(this.Item.Title, this.Item.Description, this.Item,
                    this.TempImageFile);
            }
            else
            {
                // update the existing data in SQLite...
                await this.Item.InnerItem.Update(this.TempImageFile);
            }

            // cleanup...
            await this.CleanupTempImageFileAsync();

            // return...
            this.Host.GoBack();
        }

        protected override Task<ErrorBucket> ValidateAsync()
        {
            var bucket = new ErrorBucket();
            if (string.IsNullOrEmpty(this.Item.Title))
                bucket.AddError("Title is required.");
            if (string.IsNullOrEmpty(this.Item.Description))
                bucket.AddError("Description is required.");
            if (!(this.HasImage))
                bucket.AddError("An image is required.");
            if (this.Item.Latitude == 0 && this.Item.Longitude == 0)
                bucket.AddError("A position is required.");

            // return...
            return Task.FromResult<ErrorBucket>(bucket);
        }
    }
}
