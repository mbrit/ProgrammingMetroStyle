using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class TaskWrapper
    {
        public Task Task { get; set; }
        public bool SuccessCalled { get; private set; }
        private Action<object> Success { get; set; }

        public TaskWrapper()
        {
        }

        public TaskWrapper(Action<object> success)
            : this()
        {
            this.Success = success;
        }

        public void CallSuccessSafe(object args)
        {
            // flag that we tried...
            this.SuccessCalled = true;

            // call the delegate if we have one...
            if(this.Success != null)
                this.Success(args);
        }

        public void WaitIfTaskAvailable()
        {
            if (this.Task != null)
                this.Task.Wait();
        }

        public void WaitIfTaskAvailableAndSurfaceExceptions()
        {
            if (this.Task != null)
            {
                // wait...
                this.Task.Wait();

                // what happened?
                if (this.Task.Status == TaskStatus.Faulted)
                    throw this.Task.Exception;
            }
        }
    }
}
