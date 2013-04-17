using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace StreetFoo.Client
{
    public static class StreetFooRuntime
    {
        // holds a reference to how we started...
        public static string Module { get; private set; }

        // gets the base URL of our services...
        internal const string ServiceUrlBase = "http://streetfoo.apphb.com/handlers/";

        // starts the application/sets up state...
        public static void Start(string module)
        {
            Module = module;        

            // initialize TinyIoC...
            TinyIoCContainer.Current.AutoRegister();
        }
    }
}
