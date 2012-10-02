using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace StreetFoo.Client
{
    public interface IEditReportPageViewModel : IViewModelSingleton<ReportViewItem>
    {
        ICommand TakePhotoCommand { get; }
        ICommand CaptureLocationCommand { get; }

        // are we new?
        bool IsNew { get; }

        // image presentation...
        BitmapImage Image { get; }
        bool HasImage { get; }
    }
}
