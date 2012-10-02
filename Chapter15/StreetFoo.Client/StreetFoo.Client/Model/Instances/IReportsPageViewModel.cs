using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public interface IReportsPageViewModel : IViewModelList<ReportViewItem>
    {
        ICommand RefreshCommand { get; }
        ICommand SelectionCommand { get; }
        ICommand ShowLocationCommand { get; }
        ICommand NewCommand { get; }
    }
}
