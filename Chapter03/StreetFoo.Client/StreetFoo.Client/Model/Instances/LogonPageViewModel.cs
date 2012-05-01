using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    // concrete implementation of the LogonPage's view-model...
    public class LogonPageViewModel : ViewModel, ILogonPageViewModel
    {
        // commands...
        public ICommand LogonCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }

        public LogonPageViewModel(IViewModelHost host)
            : base(host)
        {
            // set RegisterCommand to defer to the DoRegistration method...
            this.LogonCommand = new DelegateCommand((args) => DoLogon(args as TaskWrapper));
            this.RegisterCommand = new NavigateCommand<IRegisterPageViewModel>(host);
        }

        public string Username
        {
            get
            {
                // the magic CallerMemberNameAttribute automatically maps this to a
                // hash key of "Username"...
                return this.GetStringValue();
            }
            set
            {
                // likewise, CallerMemberNameAttribute works here too...
                this.SetValue(value);
            }
        }

        public string Password
        {
            get
            {
                return this.GetStringValue();
            }
            set
            {
                this.SetValue(value);
            }

        }

        private void DoLogon(TaskWrapper wrapper)
        {
            // if we don't have a wrapper, create one...
            if (wrapper == null)
                wrapper = new TaskWrapper();

            // validate...
            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                ILogonServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<ILogonServiceProxy>();

                // call...
                wrapper.Task = proxy.Logon(this.Username, this.Password, async (result) =>
                {
                    // if the wrapper has a callback, call it...
                    wrapper.CallSuccessSafe(result);

                    // show a message to say that a user has been created... (this isn't a helpful message, 
                    // included for illustration...)
                    await this.Host.ShowAlertAsync("The user is now logged on.");

                    // now what?

                }, this.Host.GetFailureHandler());
            }

            // errors?
            if (errors.HasErrors)
                this.Host.ShowAlertAsync(errors);
        }

        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(Username))
                errors.AddError("Username is required.");
            if (string.IsNullOrEmpty(Password))
                errors.AddError("Password is required.");
        }
    }
}
