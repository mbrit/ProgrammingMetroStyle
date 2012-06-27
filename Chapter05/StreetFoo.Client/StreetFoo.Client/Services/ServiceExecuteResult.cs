using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class ServiceExecuteResult : ErrorBucket
    {
        public JsonObject Output { get; private set; }

        internal ServiceExecuteResult(JsonObject output)
        {
            this.Output = output;
        }

        internal ServiceExecuteResult(JsonObject output, string error)
            : this(output)
        {
            this.AddError(error);
        }
    }
}
