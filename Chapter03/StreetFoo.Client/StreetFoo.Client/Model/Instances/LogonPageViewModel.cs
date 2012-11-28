using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;

namespace StreetFoo.Client
{
    // concrete implementation of the LogonPage's view-model...
    public class LogonPageViewModel : ViewModel, ILogonPageViewModel
    {
        // commands...
        public ICommand LogonCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }

        public LogonPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

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

        private async void DoLogon(CommandExecutionContext context)
        {
            // validate...
            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                var proxy = TinyIoCContainer.Current.Resolve<ILogonServiceProxy>();

                // call...
                using (this.EnterBusy())
                {
                    var result = await proxy.LogonAsync(this.Username, this.Password);
                    if (!(result.HasErrors))
                    {
                        //// logon... pass through the username as each user gets their own database...
                        //await StreetFooRuntime.LogonAsync(this.Username, result.Token);

                        //// while we're here - store a setting containing the logon name of the user...
                        //await SettingItem.SetValueAsync(LastUsernameKey, this.Username);

                        //// show the reports page...
                        //this.Host.ShowView(typeof(IReportsPageViewModel));

                        await this.Host.ShowAlertAsync("Logon OK!");
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
    }
}
