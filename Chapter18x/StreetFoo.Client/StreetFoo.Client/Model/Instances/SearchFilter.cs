using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace StreetFoo.Client
{
    public class SearchFilter : ModelItem
    {
        // holds the keyword that we're bound to...
        internal string Keyword { get; private set; }

        // command to raise when we're selected...
        public ICommand SelectionCommand { get; internal set; }

        public SearchFilter(string description, int numItems, string keyword, bool active = false)
        {
            this.Description = string.Format("{0} ({1})", description, numItems);
            this.Keyword = keyword;
            this.Active = active;
        }

        // holds the description...
        public string Description { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }

        // holds a flag to indicate that we were active...
        public bool Active { get { return this.GetValue<bool>(); } internal set { this.SetValue(value); } }

        internal bool MatchKeyword(ReportViewItem item)
        {
            // if we have a keyword, match it, otherwise assume it's ok...
            if (!(string.IsNullOrEmpty(this.Keyword)))
                return item.Title.ToLower().EndsWith(this.Keyword);
            else
                return true;
        }
    }
}
