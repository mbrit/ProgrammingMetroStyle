using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client
{
    public static class TaskHelper
    {
        // registers a task with the given name...
        public static void RegisterTask<T>(string name, Action<BackgroundTaskBuilder> configureCallback)
            where T : TaskBase
        {
            // unregister any old one...
            UnregisterTask(name);

            // register the new one...
            var builder = new BackgroundTaskBuilder();
            builder.Name = name;

            // entry point is StreetFoo.Client.Tasks.<Name>Facade
            builder.TaskEntryPoint = string.Format("StreetFoo.Client.Tasks.{0}Facade", typeof(T).Name);

            // configure...
            configureCallback(builder);

            // register it...
            builder.Register();
        }

        // unregisters a task with the given name...
        private static void UnregisterTask(string name)
        {
            // find it, and unregister it...
            var existing = BackgroundTaskRegistration.AllTasks.Values.Where(v => v.Name == name).FirstOrDefault();
            if (existing != null)
                existing.Unregister(true);
        }

        public static async Task RequestLockScreenAsync()
        {
            // lock?
            await BackgroundExecutionManager.RequestAccessAsync();
        }
    }
}
