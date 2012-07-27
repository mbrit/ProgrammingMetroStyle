using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.Networking.PushNotifications;

namespace StreetFoo.Client
{
    public static class StreetFooRuntime
    {
        // holds a reference to how we started...
        public static string Module { get; private set; }

        // holds a reference to the logon token...
        internal static string LogonToken { get; private set; }

        // holds a refrence to the database connections...
        internal const string SystemDatabaseConnectionString = "StreetFoo-system.db";
        internal static string UserDatabaseConnectionString = null;

        // defines the base URL of our services...
        internal const string ServiceUrlBase = "http://streetfoo.apphb.com/handlers/";

        // starts the application/sets up state...
        public static async Task Start(string module)
        {
            Module = module;

            // setup the default IoC handlers for the view models...
            ViewModelFactory.Current.SetHandler(typeof(IRegisterPageViewModel), typeof(RegisterPageViewModel));
            ViewModelFactory.Current.SetHandler(typeof(ILogonPageViewModel), typeof(LogonPageViewModel));
            ViewModelFactory.Current.SetHandler(typeof(IReportsPageViewModel), typeof(ReportsPageViewModel));

            // ...and then for the service proxies...
            ServiceProxyFactory.Current.SetHandler(typeof(IRegisterServiceProxy), typeof(RegisterServiceProxy));
            ServiceProxyFactory.Current.SetHandler(typeof(ILogonServiceProxy), typeof(LogonServiceProxy));
            ServiceProxyFactory.Current.SetHandler(typeof(IEnsureTestReportsServiceProxy), typeof(EnsureTestReportsServiceProxy));
            ServiceProxyFactory.Current.SetHandler(typeof(IGetReportsByUserServiceProxy), typeof(GetReportsByUserServiceProxy));

            // initialize the system database... a rare move to do this synchronously as we're booting up...
            var conn = GetSystemDatabase();
            await conn.CreateTableAsync<SettingItem>();
		}

        public static bool HasLogonToken
        {
            get
            {
                return !(string.IsNullOrEmpty(LogonToken));
            }
        }

        internal static async Task LogonAsync(string username, string token)
        {
            // set the database to be a user specific one... (assumes the username doesn't have evil chars in it
            // - for production you may prefer to use a hash)...
            UserDatabaseConnectionString = string.Format("StreetFoo-user-{0}.db", username);

            // store the logon token...
            LogonToken = token;

            // initialize the database...
            var conn = GetUserDatabase();
            await conn.CreateTableAsync<ReportItem>();
            await conn.CreateTableAsync<SettingItem>();

            // redo our notification...
            await SetupNotificationChannelAsync();
        }

        public static async Task SetupNotificationChannelAsync()
        {
            // get the notification channel...
            var manager = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            Debug.WriteLine("Channel: " + manager.Uri);

            // if either the URI we sent or the username we sent has changed, resend it...
            var lastUri = await SettingItem.GetValueAsync("NotificationUri");
            var lastToken = await SettingItem.GetValueAsync("NotificationToken");
            if (lastUri != manager.Uri || lastToken != LogonToken)
            {
                // send it...
                Debug.WriteLine("*** This is where you asynchronously send it! ***");

                // store it...
                await SettingItem.SetValueAsync("NotificationUri", manager.Uri);
                await SettingItem.SetValueAsync("NotificationToken", LogonToken);
            }
            else
                Debug.WriteLine("URI not changed.");
        }

        internal static SQLiteAsyncConnection GetSystemDatabase()
        {
            return new SQLiteAsyncConnection(SystemDatabaseConnectionString);
        }

        internal static SQLiteAsyncConnection GetUserDatabase()
        {
            return new SQLiteAsyncConnection(UserDatabaseConnectionString);
        }
    }
}
