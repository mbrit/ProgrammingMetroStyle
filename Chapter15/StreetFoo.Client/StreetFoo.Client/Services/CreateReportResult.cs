using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class CreateReportResult : ErrorBucket
    {
        public string NativeId { get; private set; }

        internal CreateReportResult(string nativeId)
        {
            this.NativeId = nativeId;
        }

        internal CreateReportResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
