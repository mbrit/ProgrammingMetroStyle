using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public class GetReportImageResult : ErrorBucket
    {
        public byte[] ImageBytes { get; private set; }

        internal GetReportImageResult(byte[] bs)
        {
            this.ImageBytes = bs;
        }

        internal GetReportImageResult(ErrorBucket errors)
            : base(errors)
        {
        }
    }
}
