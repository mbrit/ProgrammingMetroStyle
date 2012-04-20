using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class RegistrationResult
    {
        public string UserId { get; private set; }

        internal RegistrationResult(string userId)
        {
            this.UserId = userId;
        }
    }
}
