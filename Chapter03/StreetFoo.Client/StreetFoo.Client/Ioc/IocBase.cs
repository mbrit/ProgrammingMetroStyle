//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace StreetFoo.Client
//{
//    public abstract class IocBase<T>
//    {
//        // lookup table for the handlers...
//        private Dictionary<Type, Type> Handlers { get; set; }

//        protected IocBase()
//        {
//            this.Handlers = new Dictionary<Type, Type>();
//        }

//        public void SetHandler(Type interfaceType, Type concrete)
//        {
//            // in proper .NET you could do lots of defensive checks here, but apparently
//            // most of the stuff you need for that is missing in .NETCore...
//            Handlers[interfaceType] = concrete;
//        }

//        // gets the concrete type for a given interface type...
//        public Type GetConcreteType(Type interfaceType)
//        {
//            // do we know how to do this?
//            if (this.Handlers.ContainsKey(interfaceType))
//                return this.Handlers[interfaceType];
//            else
//                throw new InvalidOperationException(string.Format("A handler for '{0}' was not found.", interfaceType));
//        }
   
//        protected U GetHandlerInternal<U>(params object[] args)
//            where U : T
//        {
//            Type concreteType = GetConcreteType(typeof(U));
//            return (U)Activator.CreateInstance(concreteType, args);
//        }
//    }
//}
