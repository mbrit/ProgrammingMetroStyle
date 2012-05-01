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
    internal static class Alert
    {
        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, ErrorBucket errors)
        {
            return ShowAlertAsync(page, errors.GetErrorsAsString());
        }

        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, string message)
        {
            // do we need to flip threads?
            if (!(page.Dispatcher.HasThreadAccess))
            {
                IAsyncOperation<IUICommand> result = null;
                page.Dispatcher.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, (sender, e) =>
                {
                    result = ShowAlertAsync(page, message);
                }, page, null);

                // return...
                return result;
            }

            // show...
            MessageDialog dialog = new MessageDialog(message != null ? message : string.Empty);
            return dialog.ShowAsync();
        }
    }
}
