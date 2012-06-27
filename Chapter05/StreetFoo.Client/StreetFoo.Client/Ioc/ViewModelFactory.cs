using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class ViewModelFactory : IocBase<IViewModel>
    {
		// private field to hold singleton instance...
		private static ViewModelFactory _current = new ViewModelFactory();
				
		private ViewModelFactory()
		{
		}

		// returns the singleton instance...				
		public static ViewModelFactory Current
		{
			get
			{
				return _current;
			}
		}

        // specialised GetHandler implementation...
        public U GetHandler<U>(IViewModelHost host)
            where U : IViewModel
        {
            return GetHandlerInternal<U>(host);
        }
    }
}
