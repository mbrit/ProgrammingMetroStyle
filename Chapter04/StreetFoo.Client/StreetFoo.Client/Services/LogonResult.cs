using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class LogonResult
    {
        public string Token { get; private set; }

        public LogonResult(string token)
        {
            this.Token = token;
        }
    }
}
