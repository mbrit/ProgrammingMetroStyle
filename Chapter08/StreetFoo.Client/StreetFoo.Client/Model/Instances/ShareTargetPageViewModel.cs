using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace StreetFoo.Client
{
    public class ShareTargetPageViewModel : ViewModel, IShareTargetPageViewModel
    {
        private ShareOperation ShareOperation { get; set; }

        public string Title { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Description { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Comment { get { return this.GetValue<string>(); } set { this.SetValue(value); } }

        public bool ShowImage { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }
        public bool SupportsComment { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }
        public bool Sharing { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }

        public ICommand ShareCommand { get; private set; }

        public ShareTargetPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.ShowImage = false;
            this.Sharing = false;
            this.SupportsComment = true;

            this.ShareCommand = new DelegateCommand(async (args) => await HandleShareCommandAsync());
        }

        private async Task HandleShareCommandAsync()
        {
            await this.Host.ShowAlertAsync("Shared.");
        }

        public void SetupShareData(ShareOperation share)
        {
            // store the share operation - we need to do this to hold a 
            // reference otherwise the sharing subsystem will assume 
            // that we've finished...
            this.ShareOperation = share;

            // get the properties out...
            var data = share.Data;
            var props = data.Properties;
            this.Title = props.Title;
            this.Description = props.Description;
        }
    }
}
