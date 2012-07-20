using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MetroWnsPush.TemplateGenerator
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // not entirely pretty - but all we want to do is get the templates out...
            foreach (ToastTemplateType type in Enum.GetValues(typeof(ToastTemplateType)))
            {
                var template = ToastNotificationManager.GetTemplateContent(type);
                await DumpTemplate(type, template);
            }
            foreach (TileTemplateType type in Enum.GetValues(typeof(TileTemplateType)))
            {
                var template = TileUpdateManager.GetTemplateContent(type);
                await DumpTemplate(type, template);
            }
            foreach (BadgeTemplateType type in Enum.GetValues(typeof(BadgeTemplateType)))
            {
                var template = BadgeUpdateManager.GetTemplateContent(type);
                await DumpTemplate(type, template);
            }

            // ok...
            var dialog = new MessageDialog("Done.");
            await dialog.ShowAsync();
        }

        private async Task DumpTemplate(object type, XmlDocument template)
        {
            string filename = string.Format("{0}.xml", type);

            StorageFile file = null;
            try
            {
                file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            }
            catch
            {
            }
            if (file == null)
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename);

            // save...
            Debug.WriteLine(string.Format("Saving '{0}'...", filename));
            FileIO.WriteTextAsync(file, template.GetXml());
        }
    }
}
