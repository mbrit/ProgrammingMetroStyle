using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class EnsureTestReportsServiceProxy : ServiceProxy, IEnsureTestReportsServiceProxy
    {
        public EnsureTestReportsServiceProxy()
            : base("EnsureTestReports")
        {
        }

        public async Task EnsureTestReportsAsync()
        {
            // run... don't need to tell it anything, or get anything back...
            JsonObject input = new JsonObject();
            await this.Execute(input);
        }
    }
}
