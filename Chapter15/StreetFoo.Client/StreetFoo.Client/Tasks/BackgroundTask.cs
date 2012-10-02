using MetroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client
{
    public abstract class BackgroundTask
    {
        public BackgroundTask()
        {
        }

        public async void Run(IBackgroundTaskInstance instance)
        {
            var deferral = instance.GetDeferral();
            Task<LogWriteOperation[]> errorTask = null;
            try
            {
                try
                {
                    if (!(StreetFooRuntime.IsStarted))
                        await StreetFooRuntime.StartForTasks();

                    // run...
                    await this.DoRun(instance);
                }
                catch (Exception ex)
                {
                    // report the problem...
                    var logger = LogManagerFactory.DefaultLogManager.GetLogger<BackgroundTask>();
                    if (logger.IsFatalEnabled)
                    {
                        // we need to wait for it to complete...
                        errorTask = ((ILoggerAsync)logger).FatalAsync(string.Format("Execution of task '{0}' failed.", this.GetType()), ex);
                    }
                }

                // wait for errors to be written...
                if (errorTask != null)
                    await errorTask;
            }
            finally
            {
                // signal to Windows that we're finished...
                deferral.Complete();
            }
        }
        
        protected abstract Task DoRun(IBackgroundTaskInstance instance);

        // Registers a background task, on the assumption that the task
        // is actual in a different assembly with a different name...
        public static BackgroundTaskBuilder RegisterTask<T>(Action<BackgroundTaskBuilder> configureCallback)
            where T : BackgroundTask
        {
            // de-register any old tasks (this is really important)...
            DeregisterTask<T>();

            // get the name...
            string name = GetRealTaskName<T>();

            // create...
            var builder = new BackgroundTaskBuilder();
            builder.Name = name;
            builder.TaskEntryPoint = name;
            
            // setup...
            configureCallback(builder);

            // register...
            builder.Register();

            // log...
            var logger = LogManagerFactory.DefaultLogManager.GetLogger<BackgroundTask>();
            if (logger.IsInfoEnabled)
                logger.Info("Registered task '{0}.", typeof(T));

            // return...
            return builder;
        }

        private static string GetRealTaskName<T>()
            where T : BackgroundTask
        {
            // it's the name of the class, with a different namespace, suffixed with "Runner"...
            return string.Format("StreetFoo.Client.Tasks.{0}Runner", typeof(T).Name);
        }

        private static void DeregisterTask<T>()
            where T : BackgroundTask
        {
            // need to walk and remove old tasks with this name...
            var name = GetRealTaskName<T>();
            foreach (var task in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (task.Name == name)
                    task.Unregister(true);
            }
        }

        public async Task<bool> RestoreLogonAsync()
        {
            var model = new LogonPageViewModel(new NullViewModelHost());
            return await model.RestorePersistentLogonAsync();
        }
    }
}
