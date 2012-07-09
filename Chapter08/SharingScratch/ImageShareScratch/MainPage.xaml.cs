using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImageShareScratch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IStorageFile SharedFile { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
        }

        void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if(this.SharedFile != null)
            {
                var props = args.Request.Data.Properties;
                props.Title = "ImageShareScratch";
                props.Description = this.SharedFile.Path;

                var reference = RandomAccessStreamReference.CreateFromFile(this.SharedFile);
                props.Thumbnail = reference;
                args.Request.Data.SetBitmap(reference);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // pick...
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            this.SharedFile = await picker.PickSingleFileAsync();

            // did we?
            if (this.SharedFile == null)
                return;

            // share...
            DataTransferManager.ShowShareUI();
        }
    }
}
