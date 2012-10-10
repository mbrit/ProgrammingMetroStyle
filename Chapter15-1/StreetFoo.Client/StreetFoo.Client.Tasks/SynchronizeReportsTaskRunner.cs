using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client.Tasks
{
    public sealed class SynchronizeReportsTaskRunner : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var task = new SynchronizeReportsTask();
            task.Run(taskInstance);
        }
    }
}
