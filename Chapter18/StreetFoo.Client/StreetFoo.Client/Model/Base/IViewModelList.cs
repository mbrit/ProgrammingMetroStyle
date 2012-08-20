using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface IViewModelList<T> : IViewModel
        where T : ModelItem
    {
        ObservableCollection<T> Items { get; }
        ObservableCollection<T> SelectedItems { get; }

        bool HasSelectedItems { get; }
    }
}
