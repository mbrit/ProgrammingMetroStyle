using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.UI
{
    internal interface IViewModelSource
    {
        IViewModel Model
        {
            get;
        }
    }
}
