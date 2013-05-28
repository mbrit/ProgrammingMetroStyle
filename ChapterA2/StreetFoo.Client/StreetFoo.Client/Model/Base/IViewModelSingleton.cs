using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface IViewModelSingleton<T> : IViewModel
        where T : ModelItem
    {
        T Item { get; }
    }
}
