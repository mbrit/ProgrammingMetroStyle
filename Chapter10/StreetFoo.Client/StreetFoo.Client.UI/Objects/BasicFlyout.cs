using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace StreetFoo.Client.UI
{
    public class BasicFlyout
    {
        private BasicFlyoutWidth _width = BasicFlyoutWidth.Narrow;
        private Popup Popup { get; set; }
        private bool IsActivated { get; set; }

        private const int NarrowWidthInPixels = 343;
        private const int WideWidthInPixels = 646;

        public BasicFlyout(Control view)
        {
            // create a popup...
            this.Popup = new Popup();

            // indicate that we can dismiss it implicitly...
            this.Popup.IsLightDismissEnabled = true;

            // put the view on it...
            this.Popup.Child = view;

            // set the width - this will redo the layout...
            this.Width = BasicFlyoutWidth.Narrow;

            // subscribe to handle system dismiss...
            var window = Window.Current;
            window.Activated += window_Activated;
            this.Popup.Closed += Popup_Closed;

            // bind dismiss?
            if (this.ViewModel is IDismissCommand)
                ((IDismissCommand)this.ViewModel).DismissCommand = new DelegateCommand((args) => this.Hide());
        }

        void Popup_Closed(object sender, object e)
        {
            // stop listening...
            Window.Current.Activated -= window_Activated;
        }

        void window_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
                this.Hide();
        }

        public BasicFlyoutWidth Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                this.RedoLayout();
            }
        }

        private void RedoLayout()
        {
            int resolvedWidth = NarrowWidthInPixels;
            if (this.Width == BasicFlyoutWidth.Wide)
                resolvedWidth = WideWidthInPixels;

            // configure the window...
            var bounds = Window.Current.Bounds;
            this.Popup.Height = bounds.Height;
            this.Popup.Width = resolvedWidth;
            this.Popup.SetValue(Canvas.LeftProperty, bounds.Width - resolvedWidth);
            this.Popup.SetValue(Canvas.TopProperty, 0);
            this.Popup.Loaded += Popup_Loaded;

            // move...
            this.View.Width = resolvedWidth;
            this.View.Height = bounds.Height;
        }

        void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            // activate it (if we haven't already done so)... 
            if (this.ViewModel != null && !(this.IsActivated))
            {
                this.IsActivated = true;
                this.ViewModel.Activated(null);
            }
        }

        private IViewModel ViewModel
        {
            get
            {
                return View.DataContext as IViewModel;
            }
        }

        private Control View
        {
            get
            {
                return (Control)this.Popup.Child;
            }
        }

        public void Show()
        {
            this.Popup.IsOpen = true;
        }

        public void Hide()
        {
            this.Popup.IsOpen = false;
        }
    }
}
