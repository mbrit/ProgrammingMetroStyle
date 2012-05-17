using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class JsonMappingAttribute : Attribute
    {
        public string JsonName { get; private set; }

        public JsonMappingAttribute(string jsonName)
        {
            this.JsonName = jsonName;
        }
    }
}
