using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace StreetFoo.Client
{
    public class NullViewModelHost : IViewModelHost
    {
        public IAsyncOperation<Windows.UI.Popups.IUICommand> ShowAlertAsync(ErrorBucket errors)
        {
            return null;
        }

        public IAsyncOperation<Windows.UI.Popups.IUICommand> ShowAlertAsync(string message)
        {
            return null;
        }

        public void ShowView(Type viewModelInterfaceType, object args = null)
        {
        }

        public void ShowAppBar()
        {
        }

        public void HideAppBar()
        {
        }

        public void GoBack()
        {
        }
    }
}
