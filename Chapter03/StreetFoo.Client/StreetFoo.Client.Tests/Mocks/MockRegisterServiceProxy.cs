using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Tests
{
    public class MockRegisterServiceProxy : IRegisterServiceProxy
    {
        public Task<RegisterResult> RegisterAsync(string username, string email, string password, string confirm)
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
            if (!(errors.HasErrors))
                return Task.FromResult<RegisterResult>(new RegisterResult(Guid.NewGuid().ToString()));
            else
                return Task.FromResult<RegisterResult>(new RegisterResult(errors));
        }
    }
}
