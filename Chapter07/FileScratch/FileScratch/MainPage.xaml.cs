using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FileScratch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // create...
            var dialog = new FileOpenPicker();
            
            // set the types...
            dialog.FileTypeFilter.Add(".jpeg");
            dialog.FileTypeFilter.Add(".jpg");

            // show...
            StorageFile file = await dialog.PickSingleFileAsync();

            // show...
            if (file != null)
                await this.ShowAlertAsync(file.Path);
            else
                await this.ShowAlertAsync("No file chosen.");
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(ApplicationData.Current.LocalFolder.Path);
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(ApplicationData.Current.RoamingFolder.Path);
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(ApplicationData.Current.TemporaryFolder.Path);
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(Package.Current.InstalledLocation.Path);
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid().ToString() + ".txt");
            using (var stream = await file.OpenStreamForWriteAsync())
                stream.WriteByte(27);

            // ok...
            await this.ShowAlertAsync("Done.");
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(KnownFolders.DocumentsLibrary.Path);
        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(KnownFolders.MusicLibrary.Path);
        }

        private async void Button_Click_9(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(KnownFolders.VideosLibrary.Path);
        }

        private async void Button_Click_10(object sender, RoutedEventArgs e)
        {
            foreach (var file in await KnownFolders.PicturesLibrary.GetFilesAsync())
                await this.ShowAlertAsync(file.Path);

            await this.ShowAlertAsync(KnownFolders.PicturesLibrary.Path);
        }

        private async void Button_Click_11(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(KnownFolders.RemovableDevices.Path);
        }

        private async void Button_Click_12(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(KnownFolders.HomeGroup.Path);
        }

        private async void Button_Click_13(object sender, RoutedEventArgs e)
        {
            await this.ShowAlertAsync(KnownFolders.MediaServerDevices.Path);
        }

        private async void HandleCopyGrafittiPicturesToLocal(object sender, RoutedEventArgs e)
        {
            await CopyGraffitiPicturesAsync(ApplicationData.Current.LocalFolder);
        }

        private async Task CopyGraffitiPicturesAsync(StorageFolder targetFolder)
        {
            try
            {
                // copy...
                var files = (await KnownFolders.PicturesLibrary.GetFilesAsync()).Where(v => v.Name.ToLower().Contains("graffiti")
                    && (v.FileType.ToLower() == ".jpg" || v.FileType.ToLower() == ".jpeg"));
			    var builder = new StringBuilder();
                foreach (var file in files)
                {
                    // get...
                    var newFile = await file.CopyAsync(targetFolder);

                    // add...
                    builder.Append("\r\n");
                    builder.Append(newFile.Path);
                }

                // show...
                if (builder.Length > 0)
                    await this.ShowAlertAsync("Copied:\r\n" + builder.ToString());
                else
                    await this.ShowAlertAsync("No files were found to copy.");
            }
            catch (Exception ex)
            {
                this.ShowAlertAsync(ex.ToString());
            }
        }

        private async void HandleCreateFileInRoamingFolder(object sender, RoutedEventArgs e)
        {
            // create...
            var file = await ApplicationData.Current.RoamingFolder.CreateFileAsync(Guid.NewGuid().ToString() + ".txt");
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                using(var writer = new StreamWriter(stream))
                    writer.WriteLine(string.Format("Hello, world. ({0})", DateTime.Now));
            }

            // ok...
            await this.ShowAlertAsync(string.Format("Created file '{0}'. Quota: {1}KB", file.Path,
                ApplicationData.Current.RoamingStorageQuota));
        }

        private async void Button_Click_14(object sender, RoutedEventArgs e)
        {
            try
            {
                var folders = await ApplicationData.Current.LocalFolder.GetFolderAsync("foobar");
                Debug.WriteLine(folders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}
