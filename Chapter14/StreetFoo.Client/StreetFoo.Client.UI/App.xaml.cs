using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace StreetFoo.Client.UI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // start up our runtime...
            await StreetFooRuntime.Start("Client");

            // remember the user?
            var logonViewModel = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(new NullViewModelHost());
            var targetPage = typeof(LogonPage);
            if (await logonViewModel.RestorePersistentLogonAsync())
                targetPage = typeof(ReportsPage);

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            if (!rootFrame.Navigate(targetPage))
            {
                throw new Exception("Failed to create initial page");
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            
            // register for data transfer...
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;

            // search...
            var search = SearchPane.GetForCurrentView();
            search.PlaceholderText = "Report title";
            search.SuggestionsRequested += search_SuggestionsRequested;
            search.ResultSuggestionChosen += search_ResultSuggestionChosen;

            // settings...
            var settings = SettingsPane.GetForCurrentView();
            settings.CommandsRequested += settings_CommandsRequested;
        }

        void settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("PrivacyStatement", "Privacy Statement",
                async (e) => { await SettingsInteractionHelper.ShowPrivacyStatementAsync(); }));
            args.Request.ApplicationCommands.Add(new SettingsCommand("MySettings", "My Settings",
                (e) => {
                    var flyout = new BasicFlyout(new MySettingsPane());
                    flyout.Width = BasicFlyoutWidth.Wide;
                    flyout.Show();
                }));
            args.Request.ApplicationCommands.Add(new SettingsCommand("Help", "Help", (e) => { ShowHelp(); }));
        }

        internal static void ShowHelp()
        {
            var flyout = new BasicFlyout(new HelpPane());
            flyout.Width = BasicFlyoutWidth.Wide;
            flyout.Show();
        }

        async void search_ResultSuggestionChosen(SearchPane sender, SearchPaneResultSuggestionChosenEventArgs args)
        {
            var item = await ReportItem.GetByIdAsync(int.Parse(args.Tag));
            var viewItem = new ReportViewItem(item);
         
            // show...
            var frame = (Frame)Window.Current.Content;
            var handler = ViewFactory.Current.GetConcreteType<IReportPageViewModel>();
            frame.Navigate(handler, viewItem);
        }

        async void search_SuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            try
            {
                await SearchInteractionHelper.PopulateSuggestionsAsync(args.QueryText, 
                    args.Request.SearchSuggestionCollection);
            }
            finally
            {
                deferral.Complete();
            }
        }

        static void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // find the view model and dereference...
            if (Window.Current != null)
            {
                var viewModel = Window.Current.GetViewModel();
                if (viewModel != null)
                    viewModel.ShareDataRequested(sender, args);
            }
        }

        private class NullViewModelHost : IViewModelHost
        {
            public IAsyncOperation<Windows.UI.Popups.IUICommand> ShowAlertAsync(ErrorBucket errors)
            {
                return null;
            }

            public IAsyncOperation<Windows.UI.Popups.IUICommand> ShowAlertAsync(string message)
            {
                return null;
            }

            public void ShowView(Type viewModelInterfaceType, object args = null)
            {
            }

            public void ShowAppBar()
            {
            }

            public void HideAppBar()
            {
            }

            public void GoBack()
            {
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is activated as the target of a sharing operation.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected override async void OnShareTargetActivated(Windows.ApplicationModel.Activation.ShareTargetActivatedEventArgs args)
        {
            try
            {
                // start...
                await StreetFooRuntime.Start("Client");

                // logon?
                var logon = ViewModelFactory.Current.GetHandler<ILogonPageViewModel>(new NullViewModelHost());
                if (await logon.RestorePersistentLogonAsync())
                {
                    var shareTargetPage = new ShareTargetPage();
                    shareTargetPage.Activate(args);
                }
                else
                {
                    var notLoggedOnPage = new NotLoggedOnPage();
                    notLoggedOnPage.Activate(args);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            StreetFoo.Client.UI.SearchResultsPage.Activate(args.QueryText, args.PreviousExecutionState);
        }
    }
}
