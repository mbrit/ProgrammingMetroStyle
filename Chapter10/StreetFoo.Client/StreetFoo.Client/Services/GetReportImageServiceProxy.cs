using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace StreetFoo.Client
{
    public class GetReportImageServiceProxy : ServiceProxy, IGetReportImageServiceProxy
    {
        public GetReportImageServiceProxy()
            : base("GetReportImage")
        {
        }

        public async Task<GetReportImageResult> GetReportImageAsync(string nativeId)
        {
            var input = new JsonObject();
            input.Add("nativeId", nativeId);
            var executeResult = await this.Execute(input); 

            // did it work?
            if (!(executeResult.HasErrors))
            {
                // get the reports...
                var asString = executeResult.Output.GetNamedString("image");

                // bytes...
                var bs = Convert.FromBase64String(asString);
                return new GetReportImageResult(bs);
            }
            else
                return new GetReportImageResult(executeResult);
        }
    }
}
