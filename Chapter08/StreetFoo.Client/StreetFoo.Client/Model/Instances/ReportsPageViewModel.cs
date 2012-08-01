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

namespace StreetFoo.Client
{
    /// <summary>
    /// Concrement implementation of the view-model for the Reports page.
    /// </summary>
    public class ReportsPageViewModel : ViewModel, IReportsPageViewModel
    {
        public ObservableCollection<ReportViewItem> Items { get; private set; }
        private List<ReportViewItem> SelectedItems { get; set; }

        public ICommand CreateTestReportsCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand DumpSelectionCommand { get; private set; }
        public ICommand SelectionCommand { get; private set; }

        public ReportsPageViewModel(IViewModelHost host)
            : base(host)
        {
            // setup...
            this.Items = new ObservableCollection<ReportViewItem>();
            this.SelectedItems = new List<ReportViewItem>();

            // commands...
            this.RefreshCommand = new DelegateCommand(async (e) =>
            {
                this.Host.HideAppBar();
                await this.DoRefresh(true);
                
                // toast...
                string message = "I found 1 report.";
                if (this.Items.Count != 1)
                    message = string.Format("I found {0} reports.", this.Items.Count);
                var toast = new ToastNotificationBuilder(new string[] { "Reports refreshed.", message });
                toast.ImageUri = "ms-appx:///Assets/Toast.jpg";
                toast.Update();
            });

            // update any selection that we were given...
            this.SelectionCommand = new DelegateCommand((args) =>
            {
                // update the selection...
                this.SelectedItems.Clear();
                foreach (ReportViewItem item in (IEnumerable<object>)args)
                    this.SelectedItems.Add(item);

                // raise...
                this.OnPropertyChanged("SelectedItems");
                this.OnPropertyChanged("HasSelectedItems");
            });

            // dump the state...
            this.DumpSelectionCommand = new DelegateCommand(async (e) =>
            {
                if (this.SelectedItems.Count > 0)
                {
                    var builder = new StringBuilder();
                    foreach (var item in this.SelectedItems)
                    {
                        if (builder.Length > 0)
                            builder.Append("\r\n");
                        builder.Append(item.Title);
                    }

                    // show...
                    await this.Host.ShowAlertAsync(builder.ToString());
                }
                else
                    await this.Host.ShowAlertAsync("(No selection)");
            });
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

        public override async void Activated()
        {
            await DoRefresh(false);
        }

        public bool HasSelectedItems
        {
            get
            {
                return this.SelectedItems.Count > 0;
            }
        }

        public override void ShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // do we have a selection?
            if (!(this.HasSelectedItems))
                return;

            // share the first item...
            var report = this.SelectedItems.First();

            // set the basics...
            var data = args.Request.Data;
            data.Properties.Title = string.Format("StreetFoo report '{0}'", report.Title);
            data.Properties.Description = string.Format("Sharing problem report #{0}", report.NativeId);

            // set the basics...
            data.SetText(string.Format("{0}: {1}", report.Title, report.Description));
            data.SetUri(new Uri(report.PublicUrl));

            // tell the caller that we'll get back to them...
            if (report.HasImage)
            {
                var reference = RandomAccessStreamReference.CreateFromUri(new Uri(report.ImageUri));
                data.Properties.Thumbnail = reference;
                data.SetBitmap(reference);
            }
        }
    }
}
