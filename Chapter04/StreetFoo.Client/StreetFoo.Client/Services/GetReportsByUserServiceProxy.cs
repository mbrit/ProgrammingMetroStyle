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

        public void GetReportsByUser(Action<GetReportsByUserResult> success, FailureHandler failure, Action completed = null)
        {
            this.Execute(new JsonObject(), async (output) =>
            {
                // get the reports...
                string asString = output.GetNamedString("reports");

                // somewhere to put the results...
                GetReportsByUserResult result = new GetReportsByUserResult();

                // create some objects...
                var mapper = JsonMapperFactory.GetMapper<ReportItem>();
                List<ReportItem> reports = mapper.LoadArray(asString);

                // dump the reports in the database...
                var conn = StreetFooRuntime.GetUserDatabase();
                foreach (ReportItem report in reports)
                {
                    // load the existing one...
                    ReportItem existing = (await conn.Table<ReportItem>().Where(v => v.NativeId == report.NativeId).ToListAsync()).FirstOrDefault();
                    if (existing != null)
                        await conn.DeleteAsync(existing);

                    // create...
                    await conn.InsertAsync(report);
                }

                // success...
                if (success != null)
                    success(result);

            }, failure, completed);
        }
    }
}
