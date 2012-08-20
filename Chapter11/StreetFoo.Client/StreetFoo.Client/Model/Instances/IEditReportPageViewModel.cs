using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace StreetFoo.Client
{
    public interface IEditReportPageViewModel : IViewModelSingleton<ReportViewItem>
    {
        ICommand TakePhotoCommand { get; }

        string Caption { get; }
        bool HasImage { get; }
        BitmapImage Image { get; }
    }
}
