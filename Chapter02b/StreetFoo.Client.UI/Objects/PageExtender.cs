using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
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

        internal static void InitializeViewModel(this IViewModelHost page)
        {
            // create the model - ultimately we'll replace this with an IoC container...
            var model = new RegisterPageViewModel();

            // set the data context...
            ((Page)page).DataContext = model;
        }
    }
}
