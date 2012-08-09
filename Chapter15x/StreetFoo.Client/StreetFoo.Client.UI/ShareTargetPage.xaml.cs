using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Share Target Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234241

namespace StreetFoo.Client.UI
{
    /// <summary>
    /// This page allows other applications to share content through this application.
    /// </summary>
    public sealed partial class ShareTargetPage : StreetFoo.Client.UI.Common.LayoutAwarePage
    {
        private IShareTargetPageViewModel Model { get; set; }

        /// <summary>
        /// Provides a channel to communicate with Windows about the sharing operation.
        /// </summary>
        //private Windows.ApplicationModel.DataTransfer.ShareTarget.ShareOperation _shareOperation;

        public ShareTargetPage()
        {
            this.InitializeComponent();

            this.Model = ViewModelFactory.Current.GetHandler<IShareTargetPageViewModel>(this);
            this.InitializeModel(this.Model);
        }

        /// <summary>
        /// Invoked when another application wants to share content through this application.
        /// </summary>
        /// <param name="args">Activation data used to coordinate the process with Windows.</param>
        public async void Activate(ShareTargetActivatedEventArgs args)
        {
            // give it to the view-model...
            await this.Model.SetupShareDataAsync(args.ShareOperation);

            // show...
            Window.Current.Content = this;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when the user clicks the Share button.
        /// </summary>
        /// <param name="sender">Instance of Button used to initiate sharing.</param>
        /// <param name="e">Event data describing how the button was clicked.</param>
        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DefaultViewModel["Sharing"] = true;
            //this._shareOperation.ReportStarted();

            // TODO: Perform work appropriate to your sharing scenario using
            //       this._shareOperation.Data, typically with additional information captured
            //       through custom user interface elements added to this page such as 
            //       this.DefaultViewModel["Comment"]

            //this._shareOperation.ReportCompleted();
        }
    }
}
