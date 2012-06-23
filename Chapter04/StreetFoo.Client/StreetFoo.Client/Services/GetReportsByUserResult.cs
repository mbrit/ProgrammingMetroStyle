using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class GetReportsByUserResult : ErrorBucket
    {
        internal List<ReportItem> Reports { get; set; }

        internal GetReportsByUserResult(IEnumerable<ReportItem> items)
        {
            this.Reports = new List<ReportItem>();
            this.Reports.AddRange(items);
        }

        internal GetReportsByUserResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
