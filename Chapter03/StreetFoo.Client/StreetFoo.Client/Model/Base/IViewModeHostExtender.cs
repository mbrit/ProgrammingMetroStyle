using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public static class IViewModeHostExtender
    {
        public static void ShowView<T>(this IViewModelHost host, object parameter = null)
            where T : IViewModel
        {
            host.ShowView(typeof(T), parameter);
        }
    }
}
