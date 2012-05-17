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

        public Task EnsureTestReports(Action success, FailureHandler failure, Action complete = null)
        {
            // input...
            JsonObject input = new JsonObject();

            // run...
            return this.Execute(input, (output) =>
            {
                // nothing to do but just call it...
                if(success != null)
                    success();

            }, failure, complete);
        }
    }
}
