using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void Activated();
    }
}
