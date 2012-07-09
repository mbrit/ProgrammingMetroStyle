using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public interface IReportsPageViewModel : IViewModel
    {
        ICommand CreateTestReportsCommand { get; }
        ICommand RefreshCommand { get; }
        ICommand DumpSelectionCommand { get; }
        ICommand SelectionCommand { get; }

        ObservableCollection<ReportViewItem> Items
        {
            get;
        }

        bool HasSelectedItems
        {
            get;
        }
    }
}
