using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public abstract class SignalBase
    {
        protected SignalBase()
        {
        }

        public Task EnqueueAsync()
        {
            return SignalManager.Current.CreateSignalAsync(this);
        }

        public string Name
        {
            get
            {
                return this.GetType().FullName;
            }
        }
    }
}
