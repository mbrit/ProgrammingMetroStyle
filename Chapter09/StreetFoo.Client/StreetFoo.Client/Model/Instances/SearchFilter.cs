using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public class SearchFilter : ModelItem
    {
        private string Keyword { get; set; }

        public ICommand SelectedCommand { get; private set; }

        internal event EventHandler Selected;

        public SearchFilter(string description, int numItems, string keyword, bool active = false)
        {
            this.Description = string.Format("{0} ({1})", description, numItems);
            this.Keyword = keyword;
            this.Active = active;

            // selected...
            this.SelectedCommand = new DelegateCommand((args) =>
            {
                this.OnSelected(EventArgs.Empty);
            });
        }

        public string Description { get { return this.GetValue<string>(); } private set { this.SetValue(value); } }
        public bool Active { get { return this.GetValue<bool>(); } internal set { this.SetValue(value); } }

        protected virtual void OnSelected(EventArgs e)
        {
            if (this.Selected != null)
                this.Selected(this, e);
        }

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
