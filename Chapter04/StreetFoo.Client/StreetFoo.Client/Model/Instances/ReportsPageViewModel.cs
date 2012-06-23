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
        public ObservableCollection<ReportItem> Items { get; private set; }

        public ICommand CreateTestReportsCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public ReportsPageViewModel(IViewModelHost host)
            : base(host)
        {
            // setup...
            this.Items = new ObservableCollection<ReportItem>();

            // commands...
            this.RefreshCommand = new DelegateCommand(async (e) => await this.DoRefresh(true));
        }

        private void DoMagic()
        {
            // commands...
            this.CreateTestReportsCommand = new DelegateCommand((args) => DoCreateTestReports(args as CommandExecutionContext));
            this.RefreshCommand = new DelegateCommand(async (args) => await DoRefresh(true));
        }

        private async void DoCreateTestReports(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            // run...
            using(this.EnterBusy())
            {
                IEnsureTestReportsServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<IEnsureTestReportsServiceProxy>();
                await proxy.EnsureTestReportsAsync();

                // refresh the local cache and update the ui...
                await this.DoRefresh(true);

                // be explicit about what we tell the user...
                await this.Host.ShowAlertAsync("The test reports have been created.");
            }
        }

        private async Task DoRefresh(bool force)
        {
            // run...
            using (this.EnterBusy())
            {
                // update the local cache...
                if (force || await ReportItem.IsCacheEmpty())
                    await ReportItem.UpdateCacheFromServerAsync();

                // reload the items...
                await this.ReloadReportsFromCache();
            }
        }

        private async Task ReloadReportsFromCache()
        {
            // setup a load operation to populate the collection from the cache...
            using (this.EnterBusy())
            {
                var reports = await ReportItem.GetAllFromCacheAsync();

                // update the model...
                this.Items.Clear();
                foreach (ReportItem report in reports)
                    this.Items.Add(report);
            }
        }

        public override async void Activated()
        {
            await DoRefresh(false);
        }
    }
}
