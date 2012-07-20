using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Devices.Input;
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
    public sealed partial class ReportsPage : StreetFoo.Client.UI.Common.LayoutAwarePage
    {
        private IReportsPageViewModel Model { get; set; }

        public ReportsPage()
        {
            this.InitializeComponent();

            // setup model...
            this.Model = ViewModelFactory.Current.GetHandler<IReportsPageViewModel>(this);
            this.InitializeModel(this.Model);

            //// ok...
            //this.itemGridView.PointerPressed += itemGridView_PointerPressed;
            //this.itemGridView.IsItemClickEnabled = true;
            //this.itemGridView.ItemClick += itemListView_ItemClick;
        }

        void itemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (this.itemListView.SelectedItem == e.ClickedItem)
            //    this.itemListView.SelectedItem = null;
            //else
            //    this.itemListView.SelectedItem = e.ClickedItem;

            //// pass...
            //this.Model.SelectedItems = (ReportItem)this.itemListView.SelectedItem;
        }

        void itemGridView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                this.BottomAppBar.IsOpen = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]ef
        }
    }
}
