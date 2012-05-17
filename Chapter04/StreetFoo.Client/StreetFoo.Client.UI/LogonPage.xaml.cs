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

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace StreetFoo.Client.UI
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class LogonPage : StreetFoo.Client.UI.Common.LayoutAwarePage, IViewModelSource
    {
        // hold the model...
        private ILogonPageViewModel Model { get; set; }

        public LogonPage()
        {
            this.InitializeComponent();

            // obtain a real instance of a model... 
            this.Model = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(this);
            this.InitializeModel(this.Model);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the initial item to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
