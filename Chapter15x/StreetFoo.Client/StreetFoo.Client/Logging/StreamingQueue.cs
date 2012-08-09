using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog;
using Windows.System.Threading;

namespace StreetFoo.Client
{
    internal class StreamingQueue<T>
    {
        private Queue<T> InnerQueue { get; set; }
        private object _lock = new object();
        private ThreadPoolTimer Timer { get; set; }

        internal event EventHandler<DequeuedEventArgs<T>> ItemDequeued;

        internal StreamingQueue()
        {
            this.InnerQueue = new Queue<T>();
        }

        internal void Enqueue(T item)
        {
            lock (_lock)
            {
                // track the item...
                this.InnerQueue.Enqueue(item);

                // set the timer up...
                this.SetupTimer();
            }
        }

        protected virtual void OnItemDequeued(DequeuedEventArgs<T> args)
        {
            if (this.ItemDequeued != null)
                this.ItemDequeued(this, args);
        }

        private void SetupTimer()
        {
            lock (_lock)
            {
                if (this.Timer == null)
                {
                    // kick off an operation in three seconds time...
                    this.Timer = ThreadPoolTimer.CreateTimer(new TimerElapsedHandler(TimerTick), TimeSpan.FromSeconds(3));
                }
            }
        }

        private void TimerTick(ThreadPoolTimer timer)
        {
            // reset...
            try
            {
                // get the queue items and emit them...
                var toWrite = new Queue<T>();
                lock (_lock)
                {
                    while (this.InnerQueue.Any())
                        toWrite.Enqueue(this.InnerQueue.Dequeue());
                }

                // exhaust the local queue...
                while (toWrite.Any())
                {
                    var item = toWrite.Dequeue();
                    this.OnItemDequeued(new DequeuedEventArgs<T>(item));
                }

                // now that we've finished, set-up the timer again...
                SetupTimer();
            }
            finally
            {
                lock (_lock)
                {
                    this.Timer = null;

                    // if we have items in the queue (i.e. those that appeared while we were working), 
                    // setup a new timer...
                    if (this.InnerQueue.Any())
                        this.SetupTimer();
                }
            }
        }
    }
}
