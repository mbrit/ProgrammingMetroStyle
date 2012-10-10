using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                // create some objects...
                var mapper = JsonMapperFactory.GetMapper<ReportItem>();
                List<ReportItem> reports = mapper.LoadArray(asString);

                // return...
                return new GetReportsByUserResult(reports);
            }
            else
                return new GetReportsByUserResult(executeResult);
        }
    }
}
