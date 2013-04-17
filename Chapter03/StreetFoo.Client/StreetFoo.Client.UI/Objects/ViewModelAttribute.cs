using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.UI
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewModelAttribute : Attribute
    {
        public Type ViewModelInterfaceType { get; private set; }

        public ViewModelAttribute(Type viewModelInterfaceType)
        {
            this.ViewModelInterfaceType = viewModelInterfaceType;
        }
    }
}
