using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
    // extension methods for presenting MessageDialog instances...
    internal static class PageExtender
    {
        internal static Task ShowAlertAsync(this IViewModelHost page, ErrorBucket errors)
        {
            return ShowAlertAsync(page, errors.GetErrorsAsString());
        }

        internal static Task ShowAlertAsync(this IViewModelHost page, string message)
        {
            // show...
            MessageDialog dialog = new MessageDialog(message != null ? message : string.Empty);
            return dialog.ShowAsync().AsTask();
        }

        internal static void InitializeModel(this IViewModelHost page, IViewModel model)
        {
            // setup the data context...
            ((Control)page).DataContext = model;
        }

        internal static IViewModel GetModel(this IViewModelHost page)
        {
            return ((Control)page).DataContext as IViewModel;
        }
    }
}
