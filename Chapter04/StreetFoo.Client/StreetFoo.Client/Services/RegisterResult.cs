using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class RegisterResult
    {
        public string UserId { get; private set; }

        public RegisterResult(string userId)
        {
            this.UserId = userId;
        }
    }
}
