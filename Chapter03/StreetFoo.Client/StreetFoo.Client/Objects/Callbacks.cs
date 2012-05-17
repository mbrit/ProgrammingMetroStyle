using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class Callbacks<T>
    {
        public Action<T> Success { get; private set; }
        public FailureHandler Failure { get; private set; }
        public Action Complete { get; private set; }

        public Callbacks(Action<T> success, FailureHandler failure, Action complete)
        {
            this.Success = success;
            this.Failure = failure;
            this.Complete = complete;
        }

        public void RaiseSuccess(T arg)
        {
            if(this.Success != null)
                this.Success(arg);
        }

        public void RaiseFailure(object sender, ErrorBucket errors)
        {
            if (this.Failure != null)
                this.Failure(sender, errors);
        }

        public void RaiseComplete()
        {
            if (this.Complete != null)
                this.Complete();
        }

        public IDisposable GetCompleteWrapper()
        {
            return new CompleteWrapper<T>(this);
        }

        // class that allows for a using statement to be used to wrap a "complete" callback...
        private class CompleteWrapper<U> : IDisposable
            where U : T
        {
            private Callbacks<U> Owner { get; set; }

            internal CompleteWrapper(Callbacks<U> owner)
            {
                this.Owner = owner;
            }

            public void Dispose()
            {
                this.Owner.RaiseComplete();
            }
        }
    }
}
