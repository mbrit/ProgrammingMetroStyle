using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    // concrete implementation of the RegisterPage's view-model...
    public class RegisterPageViewModel : ViewModel, IRegisterPageViewModel
    {
        // commands...
        public ICommand RegisterCommand { get; private set; }

        public RegisterPageViewModel(IViewModelHost host)
            : base(host)
        {
            // set RegisterCommand to 
            this.RegisterCommand = new DelegateCommand((args) => DoRegistration());
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

        public string Email
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

        public string Confirm
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

        private void DoRegistration()
        {
            // validate...
            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // the real call to the server will return an ID here - we'll fake it for now...
                string userId = Guid.NewGuid().ToString();

                // call the success handler... 
                this.Host.ShowAlertAsync(string.Format("Created user: {0}", userId));
            }

            // errors?
            if(errors.HasErrors)
                this.Host.ShowAlertAsync(errors);
        }

        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(Username))
                errors.AddError("Username is required.");
            if (string.IsNullOrEmpty(Email))
                errors.AddError("Email is required.");
            if (string.IsNullOrEmpty(Password))
                errors.AddError("Password is required.");
            if (string.IsNullOrEmpty(Confirm))
                errors.AddError("Confirm password is required.");

            // check the passwords...
            if (!(string.IsNullOrEmpty(Password)) && this.Password != this.Confirm)
                errors.AddError("The passwords do not match.");
        }
    }
}
