using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public abstract class ServiceProxy : IServiceProxy
    {
        // the URL that the proxy connects to...
        private string Url { get; set; }

        // api key available from https://streetfoo.apphb.com/
        // *** YOU MUST CHANGE THIS FOR USE IN YOUR OWN APPS ***
        internal const string ApiKey = "4f41463a-dfc7-45dd-8d95-bf339f040933";

        protected ServiceProxy(string handler)
        {
            this.Url = StreetFooRuntime.ServiceUrlBase + "Handle" + handler + ".ashx";
        }

        protected void ConfigureInputArgs(JsonObject data)
        {
            // all the requests need an API key...
            data.Add("apiKey", ApiKey);

            // are we logged on?
            if (StreetFooRuntime.HasLogonToken)
                data.Add("logonToken", StreetFooRuntime.LogonToken);
        }

        public async Task<ServiceExecuteResult> Execute(JsonObject input)
        {
            // set the api key...
            ConfigureInputArgs(input);

            // create a request...
            HttpWebRequest request = HttpWebRequest.CreateHttp(this.Url);
            request.Method = "POST";

            // make a request for a stream to post the data up...
            using (var stream = await request.GetRequestStreamAsync())
            {
                // get the data to send...
                string outboundJson = input.Stringify();
                byte[] bs = Encoding.UTF8.GetBytes(outboundJson);
                    
                // send it...
                stream.Write(bs, 0, bs.Length);
            }

            // now, the response...
            var response = await request.GetResponseAsync();

            // unpack...
            string inboundJson = null;
            using (Stream stream = response.GetResponseStream())
            {
                // unpack the json...
                StreamReader reader = new StreamReader(stream);
                inboundJson = reader.ReadToEnd();
            }

            // load it up...
            JsonObject output = JsonObject.Parse(inboundJson);

            // did the server return an error?
            bool isOk = output.GetNamedBoolean("isOk");
            if (isOk)
                return new ServiceExecuteResult(output);
            else
            {
                // we have an error returned from the server, so return that...
                string error = output.GetNamedString("error");

                // do we have more?
                const string extendedKey = "generalFailure";
                if (output.ContainsKey(extendedKey))
                {
                    string extended = output.GetNamedString(extendedKey);
                    error = string.Format("{0} -> {1}", error, extended);
                }

                // return...
                return new ServiceExecuteResult(output, error);
            }
        }
    }
}
