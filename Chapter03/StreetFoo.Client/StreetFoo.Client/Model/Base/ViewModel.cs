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
    public abstract class ViewModel : IViewModel
    {
        //  somewhere to hold the host...
        protected IViewModelHost Host { get; private set; }

        // somewhere to hold the values...
        private Dictionary<string, object> Values { get; set; }

        // support field for IsBusy flag...
        private int BusyCounter { get; set; }
        
        // event for the change...
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(IViewModelHost host)
        {
            this.Host = host;
            this.Values = new Dictionary<string, object>();
        }

        // uses an optional value set to the name of the caller by default...
        protected object GetValue([CallerMemberName] string key = null)
        {
            // we don't mind if the values not set, just return null...
            if(this.Values.ContainsKey(key))
                return this.Values[key];
            else
                return null;
        }

        protected T GetValue<T>([CallerMemberName] string key = null)
        {
            object asObject = GetValue(key);

            if (asObject != null)
                return (T)Convert.ChangeType(asObject, typeof(T));
            else
                return default(T);
        }

        // uses an optional value set to the name of the caller by default...
        protected void SetValue(object value, [CallerMemberName] string key = null)
        {
            // set the value...
            this.Values[key] = value;

            // raise the event...
            this.Host.InvokeOnUiThread(() => OnPropertyChanged(new PropertyChangedEventArgs(key)));
        }

        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }

        // gets a delegate that can be told about fatal errors...
        protected virtual FailureHandler GetFailureHandler()
        {
            return (sender, errors) => this.Host.ShowAlertAsync(errors);
        }

        // gets a delegate that can be told about completions...
        protected virtual Action GetCompleteHandler(bool exitBusy = false)
        {
            if (exitBusy)
                return () => this.ExitBusy();
            else
                return () => { };
        }

        // indicates whether the view model is busy...
        public bool IsBusy
        {
            get { return GetValue<bool>(); }
            private set { SetValue(value); }
        }

        // indicates that the view-model is running a background task...
        protected void EnterBusy()
        {
            this.BusyCounter++;

            // set the flag?
            if (this.BusyCounter == 1)
                this.IsBusy = true;
        }

        // indicates that the view-model is no longer running a background task...
        protected void ExitBusy()
        {
            if(this.BusyCounter > 0)
                this.BusyCounter--;

            // set the flag?
            if (this.BusyCounter == 0)
                this.IsBusy = false;
        }

        // called when the view is activated...
        public virtual void Activated()
        {
            this.BusyCounter = 0;
            this.IsBusy = false;
        }
    }
}
