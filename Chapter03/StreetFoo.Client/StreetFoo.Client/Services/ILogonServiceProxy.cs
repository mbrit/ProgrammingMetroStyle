using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface ILogonServiceProxy : IServiceProxy
    {
        Task Logon(string username, string password, Action<LogonResult> success,
            FailureHandler failure);
    }
}
