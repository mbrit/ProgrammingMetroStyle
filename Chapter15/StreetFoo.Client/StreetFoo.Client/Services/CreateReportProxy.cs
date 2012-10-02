using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class CreateReportProxy : ServiceProxy
    {
        public CreateReportProxy()
            : base("CreateReport")
        {
        }

        public async Task<CreateResult> CreateReportAsync(string title, string description, decimal longitude, decimal latitude)
        {
            // package up the request...
            var input = new JsonObject();
            input.Add("title", title);
            input.Add("description", description);
            input.Add("longitude", longitude.ToString());
            input.Add("latitude", latitude.ToString());

            // call...
            var executeResult = await this.Execute(input);

            // get the user ID from the server result...
            if (!(executeResult.HasErrors))
            {
                var reportId = executeResult.Output.GetNamedString("reportId");
                return new CreateResult(reportId);
            }
            else
                return new CreateResult(executeResult);
        }
    }
}
