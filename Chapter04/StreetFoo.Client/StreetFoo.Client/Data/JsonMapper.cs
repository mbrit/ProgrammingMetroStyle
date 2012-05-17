using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class JsonMapper<T> : IJsonMapper
        where T : new()
    {
        private Dictionary<string, JsonMapperField> Fields { get; set; }
        private object _lock = new object();

        internal JsonMapper()
        {
            // set...
            this.Fields = new Dictionary<string, JsonMapperField>();

            // walk the properties on the type...
            foreach (PropertyInfo prop in typeof(T).GetTypeInfo().DeclaredProperties)
            {
                // find the json mapping attribute...
                var attr = prop.GetCustomAttributes<JsonMappingAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    // create the bidirectional mapping...
                    this.Fields[attr.JsonName] = new JsonMapperField(prop);
                }
            }
        }

        public List<T> LoadArray(string json)
        {
            JsonArray jsonObjects = JsonArray.Parse(json);
            return LoadArray(jsonObjects);
        }

        private List<T> LoadArray(JsonArray jsons)
        {
            List<T> items = new List<T>();
            foreach(JsonValue json in jsons)
                items.Add(Load(json.GetObject()));

            return items;
        }

        public T Load(string json)
        {
            JsonObject jsonObject = JsonObject.Parse(json);
            return Load(jsonObject);
        }

        public T Load(JsonObject json)
        {
            // create an instance...
            T newItem = new T();
            Populate(newItem, json);

            // return...
            return newItem;
        }

        private void Populate(T item, JsonObject json)
        {
            foreach (string jsonName in this.Fields.Keys)
            {
                // get the value...
                IJsonValue value = null;
                if (json.TryGetValue(jsonName, out value))
                {
                    // set...
                    var field = this.Fields[jsonName];
                    field.SetValue(item, value);
                }
            }
        }
    }
}
