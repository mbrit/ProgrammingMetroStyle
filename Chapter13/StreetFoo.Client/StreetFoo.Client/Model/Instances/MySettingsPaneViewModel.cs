using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreetFoo.Client
{ 
    public class MySettingsPaneViewModel : ViewModel, IMySettingsPaneViewModel
    {
        public ICommand DismissCommand { get { return this.GetValue<ICommand>(); } set { this.SetValue(value); } }

        public string Foobar { get { return this.GetValue<string>(); } set { this.SetValue(value); } }

        public MySettingsPaneViewModel(IViewModelHost host)
            : base(host)
        {
        }
    }
}
