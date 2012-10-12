using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SQLite;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using System.Globalization;

namespace StreetFoo.Client
{
    /// <summary>
    /// Concrement implementation of the view-model for the Reports page.
    /// </summary>
    public class ReportsPageViewModel : ViewModelList<ReportViewItem>, IReportsPageViewModel
    {
        public ICommand RefreshCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand SelectionCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand ShowLocationCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand NewCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        public ReportsPageViewModel(IViewModelHost host)
            : base(host)
        {
            // commands...
            this.RefreshCommand = new DelegateCommand(async (e) =>
            {
                this.Host.HideAppBar();
                await this.DoRefresh(true);
                
                // toast...
                string message = StringHelper.Format("Reports_Toast_IFound1Report");
                if (this.Items.Count != 1)
                    message = StringHelper.Format("Reports_Toast_IFoundNReports", this.Items.Count);
                var toast = new ToastNotificationBuilder(new string[] { StringHelper.Format("Reports_Toast_ReportsRefreshed"), message });
                toast.ImageUri = "ms-appx:///Assets/Toast.jpg";
                toast.Update();
            });

            // open the singleton report view...
            this.SelectionCommand = new NavigateCommand<IReportPageViewModel>(this.Host);

            // show the location...
            this.ShowLocationCommand = new DelegateCommand(async (e) =>
            {
                // get the location...
                var result = await LocationHelper.GetCurrentLocationAsync();
                if (result.Code == LocationResultCode.Ok)
                {
                    await this.Host.ShowAlertAsync(string.Format("Lat: {0}, Long: {1}, Accuracy: {2}",
                        result.Location.Coordinate.Latitude, result.Location.Coordinate.Longitude,
                        result.Location.Coordinate.Accuracy));
                }
                else
                    await this.Host.ShowAlertAsync("Failed to get location: " + result.Code.ToString());
            });

            // add...
            this.NewCommand = new DelegateCommand((e) => this.Host.ShowView(typeof(IEditReportPageViewModel), new ReportViewItem(new ReportItem())));
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
                await this.ReloadReportsFromCacheAsync();
            }
        }

        private async Task ReloadReportsFromCacheAsync()
        {
            // setup a load operation to populate the collection from the cache...
            using (this.EnterBusy())
            {
                var reports = await ReportItem.GetAllFromCacheAsync();

                // update the model...
                this.Items.Clear();
                foreach (ReportItem report in reports)
                    this.Items.Add(new ReportViewItem(report));

                // go through and initialize...
                var manager = new ReportImageCacheManager();
                foreach (var item in this.Items)
                    await item.InitializeAsync(manager);

                // update the badge...
                var badge = new BadgeNotificationBuilder(this.Items.Count);
                badge.Update();

                // update the tile...
                string message = "1 report";
                if (this.Items.Count != 1)
                    message = string.Format("{0} reports", this.Items.Count);
                var tile = new TileNotificationBuilder(new string[] { "StreetFoo", message },
                    TileTemplateType.TileWidePeekImage01);
                tile.ImageUris.Add("ms-appx:///Assets/Toast.jpg");

                // update...
                tile.UpdateAndReplicate(TileTemplateType.TileSquarePeekImageAndText02);
            }
        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            // do we have spooled reports?
            var force = false;
            if (await ReportItem.HasSpooledReportsAsync())
                force = true;

            // refresh...
            await DoRefresh(force);
        }
    }
}
