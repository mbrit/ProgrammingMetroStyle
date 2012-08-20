using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Capture;
using Windows.UI.Xaml.Media.Imaging;

namespace StreetFoo.Client
{
    public class EditReportPageViewModel : ViewModelSingleton<ReportViewItem>, IEditReportPageViewModel
    {
        public ICommand TakePhotoCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand CaptureLocationCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        public EditReportPageViewModel(IViewModelHost host)
            : base(host)
        {
            // setup the commands...
            this.TakePhotoCommand = new DelegateCommand(async (args) => await this.CaptureImageAsync());
            this.CaptureLocationCommand = new DelegateCommand(async (args) => await this.CaptureLocationAsync());
        }

        public string Caption
        {
            get
            {
                if (this.Item.Id == 0)
                    return "New report";
                else
                    return "Edit report";
            }
        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            // ** DON'T FORGET TO SET PERMISSIONS **

            // show...
            // **** THIS WASN'T IN CHAPTER 5 ****
            this.Host.ShowAppBar();

            // capture...
            await CaptureLocationAsync();
        }

        private async Task CaptureImageAsync()
        {
            var ui = new CameraCaptureUI();
            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            // did we get one?
            if (file != null)
            {
                try
                {
                    var newImage = new BitmapImage();
                    using (var stream = await file.OpenReadAsync())
                        newImage.SetSource(stream);

                    // set...
                    this.Image = newImage;
                }
                finally
                {
                    // delete it...
                    file.DeleteAsync();
                }
            }
        }

        private async Task CaptureLocationAsync()
        {
            var result = await LocationHelper.GetCurrentLocationAsync();
            if(result.Code == LocationResultCode.Ok)
                this.Item.SetLocation(result.ToMappablePoint());
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
    }
}
