using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client.Tasks
{
    public sealed class BackgroundSyncTaskFacade : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Hello, world.");

            var deferral = taskInstance.GetDeferral();
            try
            {
                // defer...
                var task = new BackgroundSyncTask();
                await task.RunAsync(taskInstance);
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
