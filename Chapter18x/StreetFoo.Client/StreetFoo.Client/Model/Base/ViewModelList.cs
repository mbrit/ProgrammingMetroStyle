using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public abstract class ViewModelList<T> : ViewModel, IViewModelList<T>
        where T : ModelItem
    {
        public ObservableCollection<T> Items { get; private set; }
        public ObservableCollection<T> SelectedItems { get; private set; }

        protected ViewModelList(IViewModelHost host)
            : base(host)
        {
            this.Items = new ObservableCollection<T>();
            this.SelectedItems = new ObservableCollection<T>();
        }

        public bool HasSelectedItems
        {
            get
            {
                return this.SelectedItems.Any();
            }
        }
    }
}
