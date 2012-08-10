using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace StreetFoo.Client
{
    public static class SettingsInteractionHelper
    {
        public static async Task ShowPrivacyStatementAsync()
        {
            // this will just take the user off to a web page... 
            // this isn't a real privacy statement, btw...
            await Launcher.LaunchUriAsync(new Uri("http://programmingwindows8apps.com/"));
        }

        internal static async Task ShowWebHelpAsync()
        {
            // again, not a real website...
            await Launcher.LaunchUriAsync(new Uri("http://programmingwindows8apps.com/"));
        }
    }
}
