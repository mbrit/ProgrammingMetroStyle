using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    // provides a route back from a view-model to a view...
    public interface IViewModelHost
    {
        void ShowAlertAsync(ErrorBucket errors);
        void ShowAlertAsync(string message);
    }
}
