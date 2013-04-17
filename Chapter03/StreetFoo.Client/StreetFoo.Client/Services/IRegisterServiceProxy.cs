using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public interface IRegisterServiceProxy : IServiceProxy
    {
        Task<RegisterResult> RegisterAsync(string username, string email, string password, string confirm);
    }
}
    