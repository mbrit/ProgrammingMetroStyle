using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client.Tests
{
    public class FakeRegisterServiceProxy : IRegisterServiceProxy
    {
        public Task<RegisterResult> RegisterAsync(string username, string email, string password, 
            string confirm)
        {
            if (username == "mbrit")
                return Task.FromResult<RegisterResult>(new RegisterResult(Guid.NewGuid().ToString()));
            else
                throw new NotImplementedException("This operation has not been implemented.");
        }
    }
}
