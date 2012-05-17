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

        // defines the username settings key...
        internal const string LastUsernameKey = "LastUsername";

        public LogonPageViewModel(IViewModelHost host)
            : base(host)
        {
            // set RegisterCommand to defer to the DoRegistration method...
            this.LogonCommand = new DelegateCommand((args) => DoLogon(args as CommandExecutionContext));
            this.RegisterCommand = new NavigateCommand<IRegisterPageViewModel>(host);
        }

        public string Username
        {
            get
            {
                // the magic CallerMemberNameAttribute automatically maps this to a
                // hash key of "Username"...
                return this.GetValue<string>();
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
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }

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
                var failure = this.GetFailureHandler();
                var complete = this.GetCompleteHandler();
                this.EnterBusy();
                proxy.Logon(this.Username, this.Password, (result) =>
                {
                    // logon... pass through the username as each user gets their own database...
                    StreetFooRuntime.Logon(this.Username, result.Token, () =>
                    {
                        // while we're here - store a setting containing the logon name of the user...
                        SettingItem.SetValueAsync(LastUsernameKey, this.Username, () =>
                        {
                            // show the reports page...
                            this.Host.ShowView(typeof(IReportsPageViewModel));

                        }, failure, complete);

                    }, failure, complete);

                }, failure, complete).AddToContext(context);
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

        public override void Activated()
        {
            base.Activated();

            // restore the setting...
            SettingItem.GetValueAsync(LastUsernameKey, (value) =>
            {
                // set the loaded value...
                this.Username = value;

            }, this.GetFailureHandler());
        }
    }
}
