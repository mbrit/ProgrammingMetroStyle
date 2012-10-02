using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace StreetFoo.Client
{
    // base class for view-model implementations...
    public interface IViewModel : INotifyPropertyChanged
    {
        // property to indicate whether the model is busy working...
        bool IsBusy
        {
            get;
        }

        // called when the view is activated...
        void Activated(object args);

        // called when the view-model might have some data to share...
        void ShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args);
    }
}
