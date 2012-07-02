using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    // base class for view-model implemenations. holds 
    public abstract class ViewModel : ModelItem, IViewModel
    {
        //  somewhere to hold the host...
        protected IViewModelHost Host { get; private set; }

        //// somewhere to hold the values...
        //private Dictionary<string, object> Values { get; set; }

        // holds a busy count...
        private int BusyCount { get; set; }

        //// event for the change...
        //public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(IViewModelHost host)
        {
            this.Host = host;
            //this.Values = new Dictionary<string, object>();
        }

        // indicates whether the model is busy...
        public bool IsBusy
        {
            get { return this.GetValue<bool>(); }

            // can't set this directly - have to use EnterBusy and ExitBusy which will keep a count...
            private set { this.SetValue(value); }
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

        //// uses an optional value set to the name of the caller by default...
        //protected object GetValue([CallerMemberName] string key = null)
        //{
        //    // we don't mind if the values not set, just return null...
        //    if(this.Values.ContainsKey(key))
        //        return this.Values[key];
        //    else
        //        return null;
        //}

        //protected T GetValue<T>([CallerMemberName] string key = null)
        //{
        //    object asObject = GetValue(key);

        //    if (asObject != null)
        //        return (T)Convert.ChangeType(asObject, typeof(T));
        //    else
        //        return default(T);
        //}

        //// uses an optional value set to the name of the caller by default...
        //protected void SetValue(object value, [CallerMemberName] string key = null)
        //{
        //    // set the value...
        //    this.Values[key] = value;

        //    // raise the event...
        //    OnPropertyChanged(new PropertyChangedEventArgs(key));
        //}

        //protected void OnPropertyChanged([CallerMemberName] string name = null)
        //{
        //    this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        //}

        //public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    if (this.PropertyChanged != null)
        //        this.PropertyChanged(this, e);
        //}

        // called when the view is activated.
        public virtual void Activated()
        {
        }
    }
}
