using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public class ReportPageViewModel : ViewModelSingleton<ReportViewItem>, IReportPageViewModel
    {
        public ICommand OpenMapCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        public ReportPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.OpenMapCommand = new DelegateCommand(async (args) => {
                var from = new AdHocMappablePoint(51.9972M, -0.7422M, "Bletchley");
                await LocationHelper.OpenMapsAppAsync(from, this.Item);
            });
        }

        protected override async void ItemChanged()
        {
            // setup our image...
            var manager = new ReportImageCacheManager();
            await this.Item.InitializeAsync(manager);
        }
    }
}
