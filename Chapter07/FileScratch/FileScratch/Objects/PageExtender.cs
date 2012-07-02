using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace FileScratch
{
    internal static class PageExtender
    {
        internal static IAsyncOperation<IUICommand> ShowAlertAsync(this Page page, string message)
        {
            var dialog = new MessageDialog(message);
            return dialog.ShowAsync();
        }
    }
}
