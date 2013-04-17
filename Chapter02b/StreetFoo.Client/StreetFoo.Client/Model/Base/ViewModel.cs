using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    // base class for view-model implemenations. 
    public abstract class ViewModel : ModelItem, IViewModel
    {
        //  somewhere to hold the host...
        protected IViewModelHost Host { get; private set; }

        public ViewModel()
        {
        }

        public void Initialize(IViewModelHost host)
        {
            this.Host = host;
        }
    }
}
