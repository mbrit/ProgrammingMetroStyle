using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface ISearchResultsPageViewModel : IViewModel
    {
        ObservableCollection<ReportViewItem> Items
        {
            get;
        }

        Task SearchAsync(string queryText);
    }
}
