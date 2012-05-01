using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface IRegisterServiceProxy : IServiceProxy
    {
        Task Register(string username, string email, string password, string confirm, Action<RegisterResult> success,
            FailureHandler failure);
    }
}
