using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SharingScratch
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : SharingScratch.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // subscribe...
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
        }

        void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // do the basics...
            var data = args.Request.Data;
            data.Properties.Title = "Deferred image";
            data.Properties.Description = "I'll have to be downloaded first!";

            // get a deferral...
            data.SetDataProvider(StandardDataFormats.Bitmap, async (request) => 
            {
                var deferral = request.GetDeferral();
                try
                {
                    // download...
                    using (var inStream = await new HttpClient().GetStreamAsync("http://streetfoo.apphb.com/images/graffiti00.jpg"))
                    {
                        // copy the stream... but we'll need to obtain a facade
                        // to map between WinRT and .NET...
                        var outStream = new InMemoryRandomAccessStream();
                        inStream.CopyTo(outStream.AsStream());

                        // send that...
                        var reference = RandomAccessStreamReference.CreateFromStream(outStream);
                        request.SetData(reference);
                    }
                }
                finally
                {
                    deferral.Complete();
                }

            });
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
