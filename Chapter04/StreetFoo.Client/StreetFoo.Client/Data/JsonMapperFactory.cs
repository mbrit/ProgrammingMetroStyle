using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public static class JsonMapperFactory
    {
        private static Dictionary<Type, IJsonMapper> Mappers { get; set; }
        private static object _mappersLock = new object();

        static JsonMapperFactory()
        {
            Mappers = new Dictionary<Type, IJsonMapper>();
        }

        public static JsonMapper<T> GetMapper<T>()
            where T : new()
        {
            lock (_mappersLock)
            {
                Type type = typeof(T);
                if (!(Mappers.ContainsKey(type)))
                    Mappers[type] = new JsonMapper<T>();

                // return...
                return (JsonMapper<T>)Mappers[type];
            }
        }
    }
}
