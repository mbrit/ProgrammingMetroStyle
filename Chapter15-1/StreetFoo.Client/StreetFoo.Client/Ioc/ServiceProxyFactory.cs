using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class ServiceProxyFactory : IocBase<IServiceProxy>
    {
        // private field to hold singleton instance...
		private static ServiceProxyFactory _current = new ServiceProxyFactory();

        private ServiceProxyFactory()
		{
		}

		// returns the singleton instance...				
        public static ServiceProxyFactory Current
		{
			get
			{
				return _current;
			}
		}

        // specialised GetHandler implementation...
        public U GetHandler<U>()
            where U : IServiceProxy
        {
            return GetHandlerInternal<U>();
        }
    }
}
