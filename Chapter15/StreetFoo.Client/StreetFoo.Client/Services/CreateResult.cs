using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class CreateResult : ErrorBucket
    {
        public string ReportId { get; set; }

        public CreateResult(string reportId)
        {
            this.ReportId = reportId;
        }

        public CreateResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
