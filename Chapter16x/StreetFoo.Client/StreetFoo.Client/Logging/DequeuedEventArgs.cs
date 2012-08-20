using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog;

namespace StreetFoo.Client
{
    internal class DequeuedEventArgs<T>
    {
        internal T Item { get; private set; }

        internal DequeuedEventArgs(T info)
        {
            this.Item = info;
        }
    }
}
