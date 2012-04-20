using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public interface IRegisterPageViewModel
    {
        string Username
        {
            get;
            set;
        }

        string Email
        {
            get;
            set;
        }

        string Password
        {
            get;
            set;
        }

        string Confirm
        {
            get;
            set;
        }

        ICommand RegisterCommand
        {
            get;
        }
    }
}
