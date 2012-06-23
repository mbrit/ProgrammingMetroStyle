using System;
using System.Collections.Generic;
using System.Linq;
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
        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, ErrorBucket errors)
        {
            return ShowAlertAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, string message)
        {
            // show...
            MessageDialog dialog = new MessageDialog(message != null ? message : string.Empty);
            return dialog.ShowAsync();
        }

        internal static void InitializeModel(this Page page, IViewModel model)
        {
            // setup the data context...
            page.DataContext = model;
        }

        internal static IViewModel GetModel(this Page page)
        {
            return page.DataContext as IViewModel;
        }
    }
}
