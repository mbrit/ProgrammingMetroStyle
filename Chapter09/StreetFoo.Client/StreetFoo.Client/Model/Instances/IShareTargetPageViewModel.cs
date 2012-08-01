using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace StreetFoo.Client
{
    public interface IShareTargetPageViewModel : IViewModel
    {
        string Title { get; }
        string Description { get; }
        string Comment { get; set; }

        string SharedText { get; }
        BitmapImage SharedImage { get; }

        bool ShowImage { get; }
        bool SupportsComment { get; }
        bool Sharing { get; }

        ICommand ShareCommand { get; }

        Task SetupShareDataAsync(ShareOperation operation);
    }
}
