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
            this.LogonCommand = new DelegateCommand((args) => DoLogon(args as CommandExecutionContext));
            this.RegisterCommand = new NavigateCommand<IRegisterPageViewModel>(host);
        }

        public string Username
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public string Password
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        private void DoLogon(CommandExecutionContext context)
        {
            // validate...
            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                ILogonServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<ILogonServiceProxy>();

                // call...
                this.EnterBusy();
                proxy.Logon(this.Username, this.Password, async (result) =>
                {
                    // show a message to say that a user has been created... (this isn't a helpful message, 
                    // included for illustration...)
                    await this.Host.ShowAlertAsync("The user is now logged on.");

                }, this.GetFailureHandler(), this.GetCompleteHandler(true)).AddToContext(context);
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
