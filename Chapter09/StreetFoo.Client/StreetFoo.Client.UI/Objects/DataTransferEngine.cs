using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
    internal static class DataTransferEngine
    {
        internal static void Initialize()
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
        }

        static void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // find the view model and dereference...
            if (Window.Current != null && Window.Current.Content is Frame)
            {
                var viewModel = ((Frame)Window.Current.Content).GetViewModel();
                if (viewModel != null)
                    viewModel.ShareDataRequested(sender, args);
            }
        }
    }
}
