using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{
    public class DelegateCommand : ICommand
    {
        private Action<object> Handler { get; set; }

        public event EventHandler CanExecuteChanged = null;

        public DelegateCommand(Action<object> handler)
        {
            this.Handler = handler;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Handler(parameter);
        }
    }
}
