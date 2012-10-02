using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace StreetFoo.Client
{
    public abstract class IocBase<T>
    {
        // lookup table for the handlers...
        private Dictionary<Type, Type> Handlers { get; set; }

        protected IocBase()
        {
            this.Handlers = new Dictionary<Type, Type>();
        }

        public void SetHandler(Type interfaceType, Type concreteType)
        {
            // some some defensive checks...
            var info = interfaceType.GetTypeInfo();
            if(!(info.IsInterface))
                throw new InvalidOperationException(string.Format("Type '{0}' is not an interface.", interfaceType));

            // more checks...
            info = concreteType.GetTypeInfo();
            if (info.IsInterface)
                throw new InvalidOperationException(string.Format("Type '{0}' is an interface.", concreteType));
            if(info.IsAbstract)
                throw new InvalidOperationException(string.Format("Type '{0}' is abstract.", concreteType));

            // set...
            Handlers[interfaceType] = concreteType;
        }

        public Type GetConcreteType<U>()
        {
            return GetConcreteType(typeof(U));
        }

        // gets the concrete type for a given interface type...
        public Type GetConcreteType(Type interfaceType)
        {
            // do we know how to do this?
            if (this.Handlers.ContainsKey(interfaceType))
                return this.Handlers[interfaceType];
            else
                throw new InvalidOperationException(string.Format("A handler for '{0}' was not found.", interfaceType));
        }
   
        protected U GetHandlerInternal<U>(params object[] args)
            where U : T
        {
            Type concreteType = GetConcreteType(typeof(U));
            return (U)Activator.CreateInstance(concreteType, args);
        }
    }
}
