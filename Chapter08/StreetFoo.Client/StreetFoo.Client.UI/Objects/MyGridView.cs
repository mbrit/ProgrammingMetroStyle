using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace StreetFoo.Client.UI
{
    public class MyGridView : GridView
    {
        public static readonly DependencyProperty SelectionCommandProperty =
            DependencyProperty.Register("SelectionCommand", typeof(ICommand), typeof(MyGridView), new PropertyMetadata(null));
        public static readonly DependencyProperty OpenAppBarsOnMultipleSelectionProperty =
            DependencyProperty.Register("OpenAppBarsOnMultipleSelection", typeof(bool), typeof(MyGridView), new PropertyMetadata(true));
        public static readonly DependencyProperty OpenAppBarsOnRightClickProperty =
            DependencyProperty.Register("OpenAppBarsOnRightClick", typeof(bool), typeof(MyGridView), new PropertyMetadata(true));

        public MyGridView()
        {
            // wire up the selection changes...
            this.SelectionChanged += MyGridView_SelectionChanged;
        }

        public bool OpenAppBarsOnRightClick
        {
            get { return (bool)GetValue(OpenAppBarsOnRightClickProperty); }
            set { SetValue(OpenAppBarsOnRightClickProperty, value); }
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            // do we care?
            if (this.OpenAppBarsOnRightClick && e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                this.OpenAppBarsOnPage(false);

            // base...
            base.OnPointerPressed(e);
        }

        public ICommand SelectionCommand
        {
            get { return (ICommand)GetValue(SelectionCommandProperty); }
            set { SetValue(SelectionCommandProperty, value); }
        }

        public bool OpenAppBarsOnMultipleSelection
        {
            get { return (bool)GetValue(OpenAppBarsOnMultipleSelectionProperty); }
            set { SetValue(OpenAppBarsOnMultipleSelectionProperty, value); }
        }

        void MyGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.SelectionCommand == null)
                return;

            // pass...
            var selected = new List<object>(this.SelectedItems);
            if(this.SelectionCommand.CanExecute(selected))
                this.SelectionCommand.Execute(selected);

            // do we care about multiple items?
            if (this.OpenAppBarsOnMultipleSelection && selected.Count > 1)
                this.OpenAppBarsOnPage(true);
            else if (this.OpenAppBarsOnMultipleSelection && selected.Count == 0)
                this.HideAppBarsOnPage();
        }
    }
}
