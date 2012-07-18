using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using MetroLog.Layouts;
using MetroLog.Targets;

namespace StreetFoo.Client
{
    internal abstract class StreamingTarget : Target
    {
        private StreamingQueue<LogEventInfo> Queue { get; set; }

        internal StreamingTarget(Layout layout)
            : base(layout)
        {
            this.Queue = new StreamingQueue<LogEventInfo>();
            this.Queue.ItemDequeued += Queue_ItemDequeued;
        }

        protected override sealed void Write(LogEventInfo entry)
        {
            this.Queue.Enqueue(entry);
        }

        void Queue_ItemDequeued(object sender, DequeuedEventArgs<LogEventInfo> e)
        {
            this.WriteQueuedItem(e.Item);
        }

        protected abstract void WriteQueuedItem(LogEventInfo entry);
    }
}
