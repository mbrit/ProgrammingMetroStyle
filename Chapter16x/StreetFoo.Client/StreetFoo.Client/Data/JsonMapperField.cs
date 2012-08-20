using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    internal class JsonMapperField
    {
        internal PropertyInfo Property { get; private set; }

        internal JsonMapperField(PropertyInfo prop)
        {
            this.Property = prop;
        }

        internal void SetValue(object item, IJsonValue value)
        {
            // clunky and basic primitive conversion...
            object toSet = null;
            if (value.ValueType == JsonValueType.String)
                toSet = value.GetString();
            else if (value.ValueType == JsonValueType.Number)
                toSet = value.GetNumber();
            else if (value.ValueType == JsonValueType.Boolean)
                toSet = value.GetBoolean();
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", value.ValueType));

            // convert to the target type and set...
            toSet = Convert.ChangeType(toSet, this.Property.PropertyType);
            this.Property.SetValue(item, toSet);
        }
    }
}
