using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace StreetFoo.Client
{
    public static class StreetFooRuntime
    {
        // holds a reference to how we started...
        public static string Module { get; private set; }

        // holds a reference to the logon token...
        internal static string LogonToken { get; private set; }

        // holds a refrence to the database connections...
        internal static SQLiteConnectionSpecification SystemDatabaseSpecification = null;
        internal static SQLiteConnectionSpecification UserDatabaseSpecification = null;

        // defines the base URL of our services...
        internal const string ServiceUrlBase = "http://streetfoo.apphb.com/handlers/";

        // starts the application/sets up state...
        public static async void Start(string module)
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

            // set the system database...
            SystemDatabaseSpecification = SQLiteConnectionSpecification.CreateForAsync("StreetFoo-system.db");

            // initialize the system database... a rare move to do this synchronously as we're booting up...
            var conn = GetSystemDatabase();
            await conn.CreateTableAsync<SettingItem>();
        }

        internal static bool HasLogonToken
        {
            get
            {
                return !(string.IsNullOrEmpty(LogonToken));
            }
        }

        internal static void Logon(string username, string token, Action success, FailureHandler failure, Action complete = null)
        {
            // set the database to be a user specific one... (assumes the username doesn't have evil chars in it
            // - for production you may prefer to use a hash)...
            UserDatabaseSpecification = SQLiteConnectionSpecification.CreateForAsync(string.Format("StreetFoo-user-{0}.db", username));

            // store the logon token...
            LogonToken = token;

            // initialize the database - has to be done async...
            var conn = GetUserDatabase();
            conn.CreateTableAsync<ReportItem>().ContinueWith((result) =>
            {
                if(success != null)
                    success();
                if (complete != null)
                    complete();

            }).ChainFailureHandler(failure, complete);
        }

        internal static SQLiteAsyncConnection GetSystemDatabase()
        {
            return new SQLiteAsyncConnection(SystemDatabaseSpecification);
        }

        internal static SQLiteAsyncConnection GetUserDatabase()
        {
            return new SQLiteAsyncConnection(UserDatabaseSpecification);
        }
    }
}
