using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;

namespace StreetFoo.Client.Tests
{
    internal class MockViewModelHost : IViewModelHost
    {
        // create some properties so we can track what happens...
        internal int NumMessages { get; private set; }
        internal int NumErrorBucketMessages { get; private set; }
        internal string LastMessage { get; private set; }
        internal Exception Fatal { get; private set; }
        internal List<Type> PageChanges { get; private set; }

        // single instance field...
        private static MockViewModelHost _current = new MockViewModelHost();

        public MockViewModelHost()
        {
            this.Reset();
        }

        internal static MockViewModelHost Current
        {
            get
            {
                return _current;
            }
        }

        internal void Reset()
        {
            // prepare for a new test...
            this.NumMessages = 0;
            this.NumErrorBucketMessages = 0;
            this.LastMessage = null;
            this.Fatal = null;
            this.PageChanges = new List<Type>();
        }

        public IAsyncOperation<IUICommand> ShowAlertAsync(ErrorBucket errors)
        {
            // log and defer...
            this.NumErrorBucketMessages++;
            return this.ShowAlertAsync(errors.GetErrorsAsString());
        }

        public IAsyncOperation<IUICommand> ShowAlertAsync(string message)
        {
            // log...
            this.NumMessages++;
            this.LastMessage = message;

            // return a dummy handler...
            return new DummyAsyncOperation();
        }

        private class DummyAsyncOperation : IAsyncOperation<IUICommand>
        {
            public AsyncOperationCompletedHandler<IUICommand> Completed
            {
                get
                {
                    return null;
                }
                set
                {
                }
            }

            public IUICommand GetResults()
            {
                return null;
            }

            public void Cancel()
            {
            }

            public void Close()
            {
            }

            public Exception ErrorCode
            {
                get
                {
                    return null;
                }
            }

            public uint Id
            {
                get
                {
                    return 1;
                }
            }

            public AsyncStatus Status
            {
                get
                {
                    return AsyncStatus.Completed;
                }
            }
        }

        public FailureHandler GetFailureHandler()
        {
            // return the default handler...
            return DefaultFailureHandler;
        }

        private void DefaultFailureHandler(object sender, ErrorBucket errors)
        {
            // our assumption is that if we get here, we have fatal errors...
            if (!(errors.HasFatal))
                throw new InvalidOperationException("The error bucket did not have a fatal error.");

            // get...
            this.Fatal = errors.Fatal;
        }

        public void ShowView(Type viewModelInterfaceType)
        {
            this.PageChanges.Add(viewModelInterfaceType);
        }

        internal int NumPageChanges
        {
            get
            {
                return this.PageChanges.Count;
            }
        }

        internal Type LastPageChange
        {
            get
            {
                if (this.PageChanges.Count > 0)
                    return this.PageChanges.Last<Type>();
                else
                    return null;
            }
        }
    }
}
