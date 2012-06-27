using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Tests
{
    public class MockLogonServiceProxy : ILogonServiceProxy
    {
        public Task<LogonResult> LogonAsync(string username, string password)
        {
            //// create a task that simulates a call up to the server...
            //return Task.Factory.StartNew(() =>
            //{
            //    // raise a success result...
            //    if (success != null)
            //    {
            //        LogonResult result = new LogonResult(Guid.NewGuid().ToString());
            //        success(result);
            //    }

            //    // complete...
            //    if (complete != null)
            //        complete();

            //});

            throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
