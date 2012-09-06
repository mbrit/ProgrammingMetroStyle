using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreetFoo.Client.UI.Common;

namespace StreetFoo.Client.UI
{
    // holds a register of view-model interfaces to actual views...
    internal class ViewFactory : IocBase<LayoutAwarePage>
    {
        // holds a singleton instance...
        private static ViewFactory _current = new ViewFactory();

        private ViewFactory()
        {
            // setup the default mappings...
            this.SetHandler(typeof(IRegisterPageViewModel), typeof(RegisterPage));
            this.SetHandler(typeof(ILogonPageViewModel), typeof(LogonPage));
            this.SetHandler(typeof(IReportsPageViewModel), typeof(ReportsPage));
            this.SetHandler(typeof(IReportPageViewModel), typeof(ReportPage));
        }

        internal static ViewFactory Current
        {
            get
            {
                if (_current == null)
                    throw new ObjectDisposedException("ViewFactory");
                return _current;
            }
        }
    }
}
