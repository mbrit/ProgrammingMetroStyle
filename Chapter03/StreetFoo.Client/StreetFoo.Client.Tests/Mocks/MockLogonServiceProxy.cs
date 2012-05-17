using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Tests
{
    public class MockLogonServiceProxy : ILogonServiceProxy
    {
        public Task Logon(string username, string password, Action<LogonResult> success, FailureHandler failure, 
            Action complete = null)
        {
            // create a task that simulates a call up to the server...
            return Task.Factory.StartNew(() =>
            {
                // validate the data...

                // raise a success result...
                LogonResult result = new LogonResult(Guid.NewGuid().ToString());
                success(result);

                // complete?
                if (complete != null)
                    complete();

            });
        }
    }
}
