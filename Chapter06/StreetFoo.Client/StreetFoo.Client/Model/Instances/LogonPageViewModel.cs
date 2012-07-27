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
        internal const string LogonTokenKey = "LogonToken";

        public LogonPageViewModel(IViewModelHost host)
            : base(host)
        {
            // set RegisterCommand to defer to the DoRegistration method...
            this.LogonCommand = new DelegateCommand((args) => DoLogon(args as CommandExecutionContext));
            this.RegisterCommand = new NavigateCommand<IRegisterPageViewModel>(host);

            // remember...
            this.RememberMe = true;
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

        public bool RememberMe
        {
            get
            {
                return this.GetValue<bool>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        private async void DoLogon(CommandExecutionContext context)
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
                using(this.EnterBusy())
                {
                    var result = await proxy.LogonAsync(this.Username, this.Password);
                    if (!(result.HasErrors))
                    {
                        // logon... pass through the username as each user gets their own database...
                        await StreetFooRuntime.LogonAsync(this.Username, result.Token);

                        // while we're here - store a setting containing the logon name of the user...
                        await SettingItem.SetValueAsync(LastUsernameKey, this.Username);

                        // remember the user?
                        if (this.RememberMe)
                            await SettingItem.SetValueAsync(LogonTokenKey, result.Token);

                        // show the reports page...
                        this.Host.ShowView(typeof(IReportsPageViewModel));
                    }
                    else
                        errors.CopyFrom(result);
                }
            }

            // errors?
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }

        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(Username))
                errors.AddError("Username is required.");
            if (string.IsNullOrEmpty(Password))
                errors.AddError("Password is required.");
        }

        public override async void Activated()
        {
            base.Activated();

            // restore the setting...
            this.Username = await SettingItem.GetValueAsync(LastUsernameKey);
        }

        public async Task<bool> RestorePersistentLogonAsync()
        {
            var token = await SettingItem.GetValueAsync(LogonTokenKey);
            if (!(string.IsNullOrEmpty(token)))
            {
                var username = await SettingItem.GetValueAsync(LastUsernameKey);
                if (!(string.IsNullOrEmpty(username)))
                {
                    // logon...
                    await StreetFooRuntime.LogonAsync(username, token);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
