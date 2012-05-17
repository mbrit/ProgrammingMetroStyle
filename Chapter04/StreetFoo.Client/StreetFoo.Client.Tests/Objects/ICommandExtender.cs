using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client.Tests
{
    public static class ICommandExtender
    {
        // method that executes the command and blocks on the reuslt...
        public static void ExecuteAndBlock(this ICommand command)
        {
            // create a context and run...
            CommandExecutionContext context = new CommandExecutionContext();
            command.Execute(context);

            // wait...
            context.WaitUntilComplete();
        }
    }
}
