using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public interface ISearchResultsPageViewModel : IViewModel
    {
        string QueryText { get; }
        string QueryNarrative { get; }

        ObservableCollection<ReportViewItem> Results { get; }
        ObservableCollection<SearchFilter> Filters { get; }

 //       bool ShowFilters { get; }
        bool HasResults { get; }

        ICommand SelectionCommand { get; }
    }
}
