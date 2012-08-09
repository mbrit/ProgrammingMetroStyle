using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;

namespace StreetFoo.Client
{
    public class HelpPaneViewModel : ViewModel, IHelpPaneViewModel
    {
        // commands...
        public ICommand DismissCommand { get { return this.GetValue<ICommand>(); } set { this.SetValue(value); } }
        public ICommand WebHelpCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        public HelpPaneViewModel(IViewModelHost host)
            : base(host)
        {
            WebHelpCommand = new DelegateCommand(async (args) => {
                await Launcher.LaunchUriAsync(new Uri("http://programmingwindows8apps.com/"));
            });
            DismissCommand = new DelegateCommand(async (args) => {
                await this.Host.ShowAlertAsync("Uh?");
            });
        }

        // property for holding the markup...
        public string Markup { get { return this.GetValue<string>(); } set { this.SetValue(value); } }

        // loads the markup from disk when we're activated...
        public override async void Activated(object args)
        {
            base.Activated(args);

            // load...
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/HelpText.txt"));
            this.Markup = await FileIO.ReadTextAsync(file).AsTask();
        }
    }
}
