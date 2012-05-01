using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Tests
{
    public class MockRegisterServiceProxy : IRegisterServiceProxy
    {
        public Task Register(string username, string email, string password, string confirm, Action<RegisterResult> success, FailureHandler failure)
        {
            // create a task that simulates a call up to the server...
            return Task.Factory.StartNew(() =>
            {
                // validate the data...

                // raise a success result...
                RegisterResult result = new RegisterResult(Guid.NewGuid().ToString());
                success(result);

            });
        }
    }
}
