using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class SignalItem
    {
        // key field...
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        // other fields...
        [Unique]
        public string Name { get; set; }
        public string Data { get; set; }

        internal SignalBase ToSignal()
        {
            var type = Type.GetType(Name);
            return (SignalBase)JsonConvert.DeserializeObject(this.Data, type);
        }
    }
}
