using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client
{
    public class BackgroundSyncTask : TaskBase
    {
        private const string SyncExpirationKey = "SyncExpiration";

        protected override async Task DoRunAsync(IBackgroundTaskInstance instance)
        {
            // should we run?
            if (!(await CanRunAsync()))
                return;

            // send up changes...
            await ReportItem.PushServerUpdatesAsync();
        }

        private async Task<bool> CanRunAsync()
        {
            // do we have connectivity?
            if (!(StreetFooRuntime.HasConnectivity))
            {
                this.Logger.Info("No connectivity - skipping...");

                // clear the expiration period...
                await SettingItem.SetValueAsync(SyncExpirationKey, string.Empty);

                // return...
                return false;
            }

            // skip the check if we're debugging... (otherwise it's hard to see what's
            // going on...)
            if (!(Debugger.IsAttached))
            {
                // check the expiration...
                var asString = await SettingItem.GetValueAsync(SyncExpirationKey);
                if (!(string.IsNullOrEmpty(asString)))
                {
                    // parse...
                    var expiration = DateTime.ParseExact(asString, "o", CultureInfo.InvariantCulture).ToUniversalTime();

                    // if the expiration time is in the future - do nothing...
                    if (expiration > DateTime.UtcNow)
                    {
                        this.Logger.Info("Not expired (expiration is '{0}') - skipping...", expiration);
                        return false;
                    }
                }
            }

            // we're ok - set the new expiration period...
            var newExpiration = DateTime.UtcNow.AddMinutes(5);
            await SettingItem.SetValueAsync(SyncExpirationKey, newExpiration.ToString("o"));

            // try and log the user in...
            var model = new LogonPageViewModel(new NullViewModelHost());
            return await model.RestorePersistentLogonAsync();
        }

        public static void Configure()
        {
            // setup the maintenance task...
            TaskHelper.RegisterTask<BackgroundSyncTask>("BackgroundSyncMaintenance", (builder) =>
            {
                // every 15 minutes, continuous...
                builder.SetTrigger(new MaintenanceTrigger(15, false));
            });

            // setup the time task...
            TaskHelper.RegisterTask<BackgroundSyncTask>("BackgroundSyncTime", (builder) =>
            {
                // every 15 minutes, continuous...
                builder.SetTrigger(new TimeTrigger(15, false));
            });

            // setup the connectivity task...
            TaskHelper.RegisterTask<BackgroundSyncTask>("BackgroundSyncConnectivity", (builder) =>
            {
                // whenever we get connectivity...
                builder.SetTrigger(new SystemTrigger(SystemTriggerType.InternetAvailable, false));
            });
        }
    }
}
