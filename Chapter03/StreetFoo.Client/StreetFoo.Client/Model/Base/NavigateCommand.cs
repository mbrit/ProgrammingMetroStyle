using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public class NavigateCommand<T> : ICommand
        where T : IViewModel
    {
        private IViewModelHost Host { get; set; }

        public event EventHandler CanExecuteChanged;

        public NavigateCommand(IViewModelHost host)
        {
            this.Host = host;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        protected void OnCanExecuteChanged(EventArgs e)
        {
            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, e);
        }

        public void Execute(object parameter)
        {
            this.Host.ShowView(typeof(T));
        }
    }
}
