using MetroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client
{
    public class SynchronizeReportsTask : BackgroundTask
    {
        public SynchronizeReportsTask()
        {
        }

        protected override async Task DoRun(IBackgroundTaskInstance instance)
        {
            if(!(await this.RestoreLogonAsync()))
                return;

            // send...
            await ReportItem.SendLocalChangesAsync();

            // get new items...
            bool changed = await ReportItem.UpdateCacheFromServerAsync();

            // if we've changed, we need to signal the UI...
            if (changed)
                await new NewReportsSignal().EnqueueAsync();
        }
    }
}
