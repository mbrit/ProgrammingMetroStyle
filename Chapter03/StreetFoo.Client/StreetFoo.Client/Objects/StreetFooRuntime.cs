using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public static class StreetFooRuntime
    {
        // holds a reference to how we started...
        public static string Module { get; private set; }

        // gets the base URL of our services...
        // *** NEED TO CHANGE FOR SSL ***
        internal const string ServiceUrlBase = "http://streetfoo.apphb.com/handlers/";

        // starts the application/sets up state...
        public static void Start(string module)
        {
            Module = module;

            // setup the default IoC handlers for the view models...
            ViewModelFactory.Current.SetHandler(typeof(IRegisterPageViewModel), typeof(RegisterPageViewModel));
            ViewModelFactory.Current.SetHandler(typeof(ILogonPageViewModel), typeof(LogonPageViewModel));

            // ...and then for the service proxies...
            ServiceProxyFactory.Current.SetHandler(typeof(IRegisterServiceProxy), typeof(RegisterServiceProxy));
            ServiceProxyFactory.Current.SetHandler(typeof(ILogonServiceProxy), typeof(LogonServiceProxy));
        }
    }
}
