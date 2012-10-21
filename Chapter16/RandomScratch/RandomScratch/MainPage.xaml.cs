using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.InteropServices.WindowsRuntime;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RandomScratch
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

        private void HandleRandomInteger(object sender, RoutedEventArgs e)
        {
            this.buttonInteger.Content = CryptographicBuffer.GenerateRandomNumber().ToString();
        }

        private void HandleRandomData(object sender, RoutedEventArgs e)
        {
            var buffer = CryptographicBuffer.GenerateRandom(1024);
            this.textRandom.Text = CryptographicBuffer.EncodeToHexString(buffer);
        }

        private void HandleRandomLong(object sender, RoutedEventArgs e)
        {
            // get eight bytes of data...
            var buffer = CryptographicBuffer.GenerateRandom(8);

            // convert it...
            long val = BitConverter.ToInt64(buffer.ToArray(), 0);
            this.buttonLong.Content = val.ToString();
        }
    }
}
