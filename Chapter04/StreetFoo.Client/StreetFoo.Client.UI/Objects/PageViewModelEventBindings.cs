using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace StreetFoo.Client.UI
{
    internal class PageViewModelEventBindings
    {
        private IViewModel Model { get; set; }

        internal PageViewModelEventBindings(Page page, IViewModel model)
        {
            this.Model = model;

            // call the view-model's Activated method when we're navigated to...
        }
    }
}
