using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StreetFoo.Client.UI
{
    public sealed partial class UnsnapWidget : UserControl
    {
        public UnsnapWidget()
        {
            this.InitializeComponent();

            // by default, it's hidden...
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void HandleUnsnap(object sender, RoutedEventArgs e)
        {
            ApplicationView.TryUnsnap();
        }
    }
}
