using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public abstract class ViewModelSingleton<T> : ViewModel, IViewModelSingleton<T>
        where T : ModelItem
    {
        // save and cancel commands
        public ICommand SaveCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand CancelCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        // holds the base item that we're mapped to...
        private T _item;

        protected ViewModelSingleton(IViewModelHost host)
            : base(host)
        {
            this.SaveCommand = new DelegateCommand(async (args) => await SaveAsync());
            this.CancelCommand = new DelegateCommand(async (args) => await CancelAsync());
        }

        public T Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                this.OnPropertyChanged();

                // reload...
                this.ItemChanged();
            }
        }

        protected virtual void ItemChanged()
        {
            // no-op...
        }

        public override void Activated(object args)
        {
            base.Activated(args);

            // check...
            if (args == null)
                throw new InvalidOperationException("An item was not supplied.");
            if(!(typeof(T).GetTypeInfo().IsAssignableFrom(args.GetType().GetTypeInfo())))
            {
                throw new InvalidOperationException(string.Format("An item of type '{0}' was supplied, but an item of type '{1}' was required.",
                    args.GetType(), typeof(T)));
            }

            // are our arguments initializing our item?
            this.Item = (T)args;
        }

        protected virtual Task<ErrorBucket> ValidateAsync()
        {
            // return an empty error bucket (i.e. "success")...
            return Task.FromResult<ErrorBucket>(new ErrorBucket());
        }

        protected async Task<bool> SaveAsync()
        {
            this.EnterBusy();
            try
            {
                // validate...
                var result = await ValidateAsync();
                if (result.HasErrors)
                {
                    await this.Host.ShowAlertAsync(result);
                    return false;
                }

                // ok...
                await DoSaveAsync();
                return true;
            }
            finally
            {
                this.ExitBusy();
            }
        }

        protected virtual Task DoSaveAsync()
        {
            return Task.FromResult<bool>(true);
        }

        protected virtual Task CancelAsync()
        {
            // go back...
            this.Host.GoBack();

            // return...
            return Task.FromResult<bool>(true);
        }
    }
}
