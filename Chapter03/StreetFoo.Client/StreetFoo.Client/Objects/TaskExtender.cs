using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public static class TaskExtender
    {
        public static Task ChainExceptionHandler(this Task task, FailureHandler failure)
        {
            // create a handler that will raise an exception if an operation fails...
            return task.ContinueWith((t) =>
            {
                // create a fatal error bucket...
                ErrorBucket fatal = ErrorBucket.CreateFatalBucket(t.Exception);
                failure(task, fatal);

            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
