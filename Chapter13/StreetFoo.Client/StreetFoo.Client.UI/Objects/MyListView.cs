using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StreetFoo.Client.UI
{
    public class MyListView : ListView
    {
        // as per the grid, but watch the 
        public static readonly DependencyProperty ItemClickedCommandProperty =
            DependencyProperty.Register("ItemClickedCommand", typeof(ICommand), typeof(MyListView),
            new PropertyMetadata(null, (d, e) => ((MyListView)d).SelectionCommand = (ICommand)e.NewValue));

        public MyListView()
        {
            this.ItemClick += MyListView_ItemClick;
        }

        void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.ItemClickedCommand == null)
                return;

            // ok...
            Debug.WriteLine("hi");
        }

        public ICommand ItemClickedCommand
        {
            get { return (ICommand)GetValue(ItemClickedCommandProperty); }
            set { SetValue(ItemClickedCommandProperty, value); }
        }
    }
}
