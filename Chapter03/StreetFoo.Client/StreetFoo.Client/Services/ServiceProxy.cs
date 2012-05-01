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
        private const string ApiKey = "4f41463a-dfc7-45dd-8d95-bf339f040933";

        protected ServiceProxy(string handler)
        {
            this.Url = StreetFooRuntime.ServiceUrlBase + "Handle" + handler + ".ashx";
        }

        public Task Execute(JsonObject input, Action<JsonObject> processor, FailureHandler failure)
        {
            // create a request...
            HttpWebRequest request = HttpWebRequest.CreateHttp(this.Url);
            request.Method = "POST";

            // make a request for a stream to post the data up...
            var grsTask = request.GetRequestStreamAsync();
            grsTask.ContinueWith((grsResult) => {

                // the result holds the stream...
                using (Stream stream = grsResult.Result)
                {
                    // get the data to send...
                    string json = input.Stringify();
                    byte[] bs = Encoding.UTF8.GetBytes(json);
                    
                    // send it...
                    stream.Write(bs, 0, bs.Length);

                }

                // now, the response...
                var grTask = request.GetResponseAsync();
                grTask.ContinueWith((grResult) =>
                {
                    // unpack...
                    string json = null;
                    using (Stream stream = grResult.Result.GetResponseStream())
                    {
                        // unpack the json...
                        StreamReader reader = new StreamReader(stream);
                        json = reader.ReadToEnd();
                    }

                    // load it up...
                    JsonObject output = JsonObject.Parse(json);

                    // did the server return an error?
                    bool isOk = output.GetNamedBoolean("isOk");
                    if (isOk)
                    {
                        // run the delegate that processes the results...
                        processor(output);
                    }
                    else
                    {
                        // we have an error returned from the server...
                        string error = output.GetNamedString("error");

                        // create a bucket and pass it through...
                        ErrorBucket bucket = new ErrorBucket();
                        bucket.AddError(error);
                        failure(this, bucket);
                    }

                }).ChainExceptionHandler(failure);

            }).ChainExceptionHandler(failure);

            // return...
            return grsTask;
        }

        protected void ConfigureInputArgs(JsonObject data)
        {
            // all the requests need an API key...
            data.Add("apiKey", ApiKey);
        }
    }
}
