using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
    public class MvvmAwareControl : UserControl, IViewModelHost
    {
        public MvvmAwareControl()
        {
        }

        IAsyncOperation<IUICommand> IViewModelHost.ShowAlertAsync(ErrorBucket errors)
        {
            return PageExtender.ShowAlertAsync(this, errors);
        }

        IAsyncOperation<IUICommand> IViewModelHost.ShowAlertAsync(string message)
        {
            return PageExtender.ShowAlertAsync(this, message);
        }

        public void ShowView(Type viewModelInterfaceType, object args = null)
        {
            throw new NotImplementedException();
        }

        public void ShowAppBar()
        {
            throw new NotImplementedException();
        }

        public void HideAppBar()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        public async void SafeInvoke(Action action)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
