using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SQLite;

namespace StreetFoo.Client
{
    /// <summary>
    /// Concrement implementation of the view-model for the Reports page.
    /// </summary>
    public class ReportsPageViewModel : ViewModel, IReportsPageViewModel
    {
        public ObservableCollection<ReportItem> Reports { get; private set; }

        public ICommand CreateTestReportsCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public ReportsPageViewModel(IViewModelHost host)
            : base(host)
        {
            // setup...
            this.Reports = new ObservableCollection<ReportItem>();

            // commands...
            this.CreateTestReportsCommand = new DelegateCommand((args) => DoCreateTestReports(args as CommandExecutionContext));
            this.RefreshCommand = new DelegateCommand((args) => DoRefresh(args as CommandExecutionContext));
        }

        private void DoCreateTestReports(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            // run...
            this.EnterBusy();
            IEnsureTestReportsServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<IEnsureTestReportsServiceProxy>();
            proxy.EnsureTestReports(async () => {

                await this.Host.ShowAlertAsync("The test reports have been created.");

                // refresh the local cache and update the ui...
                this.DoRefresh(null);

            }, this.GetFailureHandler(), this.GetCompleteHandler());
        }

        private void DoRefresh(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            // run...
            this.EnterBusy();
            ReportItem.UpdateCache(() =>
            {
                // reload the items...
                this.ReloadReportsFromCache();

            }, this.GetFailureHandler(), this.GetCompleteHandler());
        }

        private void ReloadReportsFromCache()
        {
            // setup a load operation to populate the collection from the cache...
            this.EnterBusy();
            ReportItem.ReadCache((reports) =>
            {
                // patch them in...
                this.Host.InvokeOnUiThread(() =>
                {
                    this.Reports.Clear();
                    foreach (ReportItem report in reports)
                        this.Reports.Add(report);

                });

            }, this.GetFailureHandler(), this.GetCompleteHandler());
        }

        public override void Activated()
        {
            this.ReloadReportsFromCache();
        }
    }
}
