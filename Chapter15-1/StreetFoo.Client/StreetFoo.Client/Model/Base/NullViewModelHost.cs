using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;

namespace StreetFoo.Client
{
    public class NullViewModelHost : IViewModelHost
    {
        public IAsyncOperation<IUICommand> ShowAlertAsync(ErrorBucket errors)
        {
            return null;
        }

        public IAsyncOperation<IUICommand> ShowAlertAsync(string message)
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

        public void SafeInvoke(Action action)
        {
            action();
        }
    }
}
