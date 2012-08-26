using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public static class JsonObjectExtender
    {
        // extension method that adds a primitive value...
        public static void Add(this JsonObject json, string key, string value)
        {
            json.Add(key, JsonValue.CreateStringValue(value));
        }

        public static void Add(this JsonObject json, string key, bool value)
        {
            json.Add(key, JsonValue.CreateBooleanValue(value));
        }

        public static void Add(this JsonObject json, string key, double value)
        {
            json.Add(key, JsonValue.CreateNumberValue(value));
        }
    }
}
