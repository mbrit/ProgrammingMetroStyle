using MetroLog;
using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StreetFoo.Client
{
    public abstract class TaskBase 
    {
        private ILogger _logger;

        // runs the operation...
        public async Task RunAsync(IBackgroundTaskInstance instance)
        {
            // configure the logging system to use a streaming target...
            try
            {
                LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal,
                    new FileStreamingTarget());
            }
            catch
            {
                // no-op... waiting for a change in MetroLog to see if the config
                // has already been done...
            }

            // logging is a bit tricky as we have to gather all of the messages
            // and flush them out...
            var logTasks = new List<Task<LogWriteOperation[]>>();

            // do some logging...
            var asyncLogger = (ILoggerAsync)this.Logger;
            logTasks.Add(asyncLogger.InfoAsync("Started background task '{0}' (#{1})...",
                instance.Task.Name, instance.Task.TaskId));

            // run...
            try
            {
                // start the app...
                await StreetFooRuntime.Start("Tasks");

                // defer...
                await DoRunAsync(instance);
            }
            catch (Exception ex)
            {
                logTasks.Add(asyncLogger.FatalAsync(string.Format("Background task '{0}' (#{1}) failed.",
                    instance.Task.Name, instance.Task.TaskId), ex));
            }

            // finish...
            logTasks.Add(asyncLogger.InfoAsync("Finished background task '{0}' (#{1}).",
                instance.Task.Name, instance.Task.TaskId));

            // wait...
            await Task.WhenAll(logTasks);
        }

        // actual runner...
        protected abstract Task DoRunAsync(IBackgroundTaskInstance instance);

        // log...
        protected ILogger Logger
        {
            get
            {
                if(_logger == null)
                    _logger = LogManagerFactory.DefaultLogManager.GetLogger(this.GetType());
                return _logger;
            }
        }

        // Registers a background task, on the assumption that the task
        // is actual in a different assembly with a different name...
        public static BackgroundTaskBuilder RegisterTask<T>(Action<BackgroundTaskBuilder> configureCallback)
            where T : TaskBase
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
            var logger = LogManagerFactory.DefaultLogManager.GetLogger<TaskBase>();
            if (logger.IsInfoEnabled)
                logger.Info("Registered task '{0}.", typeof(T));

            // return...
            return builder;
        }

        private static string GetRealTaskName<T>()
            where T : TaskBase
        {
            // it's the name of the class, with a different namespace, suffixed with "Runner"...
            return string.Format("StreetFoo.Client.Tasks.{0}Facade", typeof(T).Name);
        }

        private static void DeregisterTask<T>()
            where T : TaskBase
        {
            // need to walk and remove old tasks with this name...
            var name = GetRealTaskName<T>();
            foreach (var task in BackgroundTaskRegistration.AllTasks.Values)
            {
                if (task.Name == name)
                    task.Unregister(true);
            }
        }
    }
}
