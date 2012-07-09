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
        string Title { get; set; }
        string Description { get; set; }
        string Comment { get; set; }

        bool ShowImage { get; set; }
        bool SupportsComment { get; set; }
        bool Sharing { get; set; }

        ICommand ShareCommand { get; }

        void SetupShareData(ShareOperation operation);
    }
}
