using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class SearchResultsPageViewModel : ViewModel, ISearchResultsPageViewModel
    {
        public ObservableCollection<ReportViewItem> Items { get; set; }

        public SearchResultsPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.Items = new ObservableCollection<ReportViewItem>();
        }

        public async Task SearchAsync(string queryText)
        {
            // load the reports from the cache...
            var reports = await ReportItem.SearchCacheAsync(queryText);

            // update...
            this.Items.Clear();
            foreach (var report in reports)
                this.Items.Add(new ReportViewItem(report));

            // init...
            var manager = new ReportImageCacheManager();
            foreach (var report in this.Items)
                await report.InitializeAsync(manager);
        }
    }
}
