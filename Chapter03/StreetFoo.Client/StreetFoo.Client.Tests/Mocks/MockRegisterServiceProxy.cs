using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Tests
{
    public class MockRegisterServiceProxy : IRegisterServiceProxy
    {
        public Task Register(string username, string email, string password, string confirm, Action<RegisterResult> success, 
            FailureHandler failure, Action complete = null)
        {
            // create a task that simulates a call up to the server...
            return Task.Factory.StartNew(() =>
            {
                // validate the data...
                ErrorBucket errors = new ErrorBucket();
                if (string.IsNullOrEmpty(username))
                    errors.AddError("Username is required.");
                if (string.IsNullOrEmpty(email))
                    errors.AddError("Emailis required.");
                if (string.IsNullOrEmpty(password))
                    errors.AddError("Password is required.");
                if (string.IsNullOrEmpty(confirm))
                    errors.AddError("Confirm password is required.");

                // match?
                if (!(string.IsNullOrEmpty(password)) && password != confirm)
                    errors.AddError("The passwords do not match.");

                // if?
                if(!(errors.HasErrors))
                {
                    // raise a success result...
                    RegisterResult result = new RegisterResult(Guid.NewGuid().ToString());
                    success(result);
                }

                // fail?
                if(errors.HasErrors)
                    failure(this, errors);

                // complete...
                if (complete != null)
                    complete();

            });
        }
    }
}
