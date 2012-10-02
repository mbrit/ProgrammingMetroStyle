using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class SignalRegistration
    {
        internal Type SignalType { get; private set; }
        internal ISignalSink Sink { get; private set; }

        internal SignalRegistration(Type signalType, ISignalSink sink)
        {
            this.SignalType = signalType;
            this.Sink = sink;
        }
    }
}
