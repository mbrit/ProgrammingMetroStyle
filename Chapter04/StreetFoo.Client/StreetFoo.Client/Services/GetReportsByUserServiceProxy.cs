using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class GetReportsByUserServiceProxy : ServiceProxy, IGetReportsByUserServiceProxy
    {
        public GetReportsByUserServiceProxy()
            : base("GetReportsByUser")
        {
        }

        public async Task<GetReportsByUserResult> GetReportsByUserAsync()
        {
            var input = new JsonObject();
            var executeResult = await this.Execute(input); 

            // did it work?
            if (!(executeResult.HasErrors))
            {
                // get the reports...
                string asString = executeResult.Output.GetNamedString("reports");

                // use JSON.NET to create the reports...
                var reports = JsonConvert.DeserializeObject<List<ReportItem>>(asString);

                // return...
                return new GetReportsByUserResult(reports);
            }
            else
                return new GetReportsByUserResult(executeResult);
        }
    }
}
