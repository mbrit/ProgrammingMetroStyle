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

        // support field for IsBusy flag...
        private int BusyCount { get; set; }
        
        // event for the change...
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
        }

        public virtual void Initialize(IViewModelHost host)
        {
            this.Host = host;
        }


        // indicates whether the view model is busy...
        public bool IsBusy
        {
            get { return GetValue<bool>(); }
            private set { SetValue(value); }
        }

        public IDisposable EnterBusy()
        {
            this.BusyCount++;

            // trigger a UI change?
            if (this.BusyCount == 1)
                this.IsBusy = true;

            // return an object we can use to roll this back...
            return new BusyExiter(this);
        }

        private class BusyExiter : IDisposable
        {
            private ViewModel Owner { get; set; }

            internal BusyExiter(ViewModel owner)
            {
                this.Owner = owner;
            }

            public void Dispose()
            {
                this.Owner.ExitBusy();
            }
        }

        public void ExitBusy()
        {
            this.BusyCount--;

            // trigger a UI change?
            if (this.BusyCount == 0)
                this.IsBusy = false;
        }


        // called when the view is activated...
        public virtual void Activated()
        {
            this.BusyCount = 0;
            this.IsBusy = false;
        }
    }
}
