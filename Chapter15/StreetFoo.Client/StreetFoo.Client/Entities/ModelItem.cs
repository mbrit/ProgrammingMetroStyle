using MetroLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public abstract class ModelItem : INotifyPropertyChanged
    {
        // holds the model item...
        private Dictionary<string, object> Values { get; set; }

        // holds a demand created log item...
        private ILogger _logger;

        protected ModelItem()
        {
            this.Values = new Dictionary<string, object>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected T GetValue<T>([CallerMemberName] string name = null)
        {
            if (this.Values.ContainsKey(name))
                return (T)this.Values[name];
            else
                return default(T);
        }

        protected void SetValue(object value, [CallerMemberName] string name = null)
        {
            // set...
            this.Values[name] = value;

            // notify...
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }
        
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, e);
        }

        protected ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = LogManagerFactory.DefaultLogManager.GetLogger(this.GetType());
                return _logger;
            }
        }
    }
}
