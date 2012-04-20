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
    public abstract class ViewModel : IViewModel
    {
        //  somewhere to hold the host...
        protected IViewModelHost Host { get; private set; }

        // somewhere to hold the values...
        private Dictionary<string, object> Values { get; set; }

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

        protected string GetStringValue([CallerMemberName] string key = null)
        {
            object asObject = GetValue(key);
            return Convert.ToString(asObject);
        }

        // uses an optional value set to the name of the caller by default...
        protected void SetValue(object value, [CallerMemberName] string key = null)
        {
            // set the value...
            this.Values[key] = value;

            // raise the event...
            OnPropertyChanged(new PropertyChangedEventArgs(key));
        }

        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }    
    }
}
