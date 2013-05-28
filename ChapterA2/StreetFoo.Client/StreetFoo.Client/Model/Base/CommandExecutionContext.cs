using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    // defines a class that provides context to a command...
    public class CommandExecutionContext
    {
        // holds a reference to created tasks...
        public List<Task> Tasks { get; private set; }

        public CommandExecutionContext()
        {
            this.Tasks = new List<Task>();
        }

        internal void AddTask(Task task)
        {
            this.Tasks.Add(task);
        }

        public void WaitUntilComplete()
        {
            // ask TPL to wait for all the tasks to finish...
            Task.WaitAll(this.Tasks.ToArray());
        }
    }
}
