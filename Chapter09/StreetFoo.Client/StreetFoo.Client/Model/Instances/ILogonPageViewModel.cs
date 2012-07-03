using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    // exposes the map of public binding properties on LogonPage's view-model...
    public interface ILogonPageViewModel : IViewModel
    {
        string Username
        {
            get;
            set;
        }

        string Password
        {
            get;
            set;
        }

        bool RememberMe
        {
            get;
            set;
        }

        ICommand LogonCommand
        {
            get;
        }

        ICommand RegisterCommand
        {
            get;
        }

        Task<bool> RestorePersistentLogonAsync();
    }
}
