using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace StreetFoo.Client.UI
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage1 : StreetFoo.Client.UI.Common.LayoutAwarePage, IViewModelSource
    {
        private IReportsPageViewModel Model { get; set; }

        public ItemsPage1()
        {
            this.InitializeComponent();

            // load...
            this.Model = ViewModelFactory.Current.GetHandler<IReportsPageViewModel>(this);
            this.InitializeModel(this.Model);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the collection of items to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DefaultViewModel["Items"] = e.Parameter;
        }

        IViewModel IViewModelSource.Model
        {
            get
            {
                return this.Model;
            }
        }
    }
}
