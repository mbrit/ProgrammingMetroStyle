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
            // raise a success result...
            return Task.FromResult<LogonResult>(new LogonResult(Guid.NewGuid().ToString()));
        }
    }
}
