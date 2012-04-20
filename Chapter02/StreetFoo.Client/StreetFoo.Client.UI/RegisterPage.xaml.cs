using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace StreetFoo.Client.UI
{
    public sealed partial class RegisterPage : StreetFoo.Client.UI.Common.LayoutAwarePage
    {
        // hold the model...
        private IRegisterPageViewModel Model { get; set; }

        public RegisterPage()
        {
            this.InitializeComponent();

            // obtain a real instance of a model... we'll do this via dependency injection soon...
            this.Model = new RegisterPageViewModel(this);

            // pass the model through to the DataContext (for databinding)...
            this.DataContext = this.Model;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        //private void HandleRegisterClick(object sender, RoutedEventArgs e)
        //{
        //    // ask the model to handle this... pass in callbacks for success, and a general callback for
        //    // failure...
        //    this.Model.DoRegistration((result) =>
        //    {
        //        this.ShowAlertAsync("Great - we got a result back.");  

        //    }, this.FailureSink);
        //}

        //private void FailureSink(ErrorBucket bucket, object callbackArgs)
        //{
        //    this.ShowAlertAsync(bucket);
        //}

    }
}
