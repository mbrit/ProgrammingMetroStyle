using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;

namespace StreetFoo.Client
{
    // provides a route back from a view-model to a view...
    public interface IViewModelHost
    {
        // show messages...
        IAsyncOperation<IUICommand> ShowAlertAsync(ErrorBucket errors);
        IAsyncOperation<IUICommand> ShowAlertAsync(string message);

        // shows a view from a given view-model...
        void ShowView(Type viewModelInterfaceType);

        // gives the host a chance to marshal a call over to the UI thread if it needs it...
        // *** WHEN SHOWING THIS - SHOW THE TOTAL LACK OF ERRORS AND GO
        // THROUGH TROUBLESHOOTING ***
        void InvokeOnUiThread(Action operation);
    }
}
