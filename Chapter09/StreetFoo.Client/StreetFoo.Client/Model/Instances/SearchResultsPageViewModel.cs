using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public class SearchResultsPageViewModel : ViewModel, ISearchResultsPageViewModel
    {
        // the master list and filtered list...
        private List<ReportViewItem> MasterItems { get; set; }
        public ObservableCollection<ReportViewItem> Results { get; private set; }

        // filter options...
        public ObservableCollection<SearchFilter> Filters { get; private set; }

        // issued when an item is selected...
        public ICommand SelectionCommand { get; private set; }

        // track whether we've done a search...
        private bool SearchDone { get; set; }

        public SearchResultsPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.MasterItems = new List<ReportViewItem>();
            this.Results = new ObservableCollection<ReportViewItem>();
            this.Filters = new ObservableCollection<SearchFilter>();

            // command...
            this.SelectionCommand = new DelegateCommand(async (args) =>
            {
                await this.Host.ShowAlertAsync("Selected: " + ((ReportViewItem)args).Title);
            });
        }

        public string QueryText { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }
        public string QueryNarrative { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }

        public bool HasResults
        {
            get
            {
                // if we haven't done a search - be optimistic otherwise we'll flicker...
                if (!(this.SearchDone))
                    return true;

                // ok...
                return this.Results.Any();
            }
        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            // do the search...
            await SearchAsync((string)args);
        }

        private async Task SearchAsync(string queryText)
        {
            // flag...
            this.SearchDone = true;
                
            // set...
            this.QueryText = queryText;

            // set the narrative...
            if (string.IsNullOrEmpty(queryText))
                this.QueryNarrative = string.Empty;
            else
                this.QueryNarrative = string.Format("Results for '{0}'", queryText);

            // load...
            var reports = await ReportItem.SearchCacheAsync(queryText);
            this.MasterItems.Clear();
            foreach (var report in reports)
                this.MasterItems.Add(new ReportViewItem(report));

            // do we have anything?
            this.Filters.Clear();
            if (this.MasterItems.Any())
            {
                // all filter...
                var allFilter = new SearchFilter("all", this.MasterItems.Count, null, false);
                allFilter.SelectionCommand = new DelegateCommand((args) => HandleFilterActivated(allFilter));
                this.Filters.Add(allFilter);

                // build up a list of nouns...
                var nouns = new Dictionary<string, int>();
                var regex = new Regex(@"\b\w+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                foreach (var report in reports)
                {
                    var match = regex.Match(report.Title);

                    // word...
                    string noun = match.Value.ToLower();
                    if (!(nouns.ContainsKey(noun)))
                        nouns[noun] = 0;
                    nouns[noun]++;
                }

                // add the filters...
                foreach (var noun in nouns.Keys)
                {
                    var filter = new SearchFilter(noun, nouns[noun], noun);
                    filter.SelectionCommand = new DelegateCommand((args) => HandleFilterActivated(filter));
                    this.Filters.Add(filter);
                }

                // update...
                var manager = new ReportImageCacheManager();
                foreach (var report in this.MasterItems)
                    await report.InitializeAsync(manager);
            }

            // apply the filter...
            this.ApplyFilter();
        }

        private void ApplyFilter()
        {
            // reset...
            this.Results.Clear();

            // do we have a filter?
            var filter = this.ActiveFilter;
            if (filter != null)
            {
                // match...
                foreach (var report in this.MasterItems.Where(v => filter.MatchKeyword(v)))
                    this.Results.Add(report);
            }
            else
            {
                // copy in every thing...
                foreach (var report in this.MasterItems)
                    this.Results.Add(report);
            }

            // update...
            this.OnPropertyChanged("HasResults");
        }

        private SearchFilter ActiveFilter
        {
            get
            {
                return this.Filters.Where(v => v.Active).FirstOrDefault();
            }
        }

        void HandleFilterActivated(object args)
        {
            // walk...
            foreach (var filter in this.Filters)
            {
                if (filter == args)
                    filter.Active = true;
                else
                    filter.Active = false;
            }

            // update...
            this.ApplyFilter();
        }
    }
}
