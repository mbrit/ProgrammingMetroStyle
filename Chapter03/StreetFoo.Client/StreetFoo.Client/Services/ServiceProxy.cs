using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        protected void ConfigureInputArgs(JsonObject data)
        {
            // all the requests need an API key...
            data.Add("apiKey", ApiKey);
        }

        public async Task<ServiceExecuteResult> ExecuteAsync(JsonObject input)
        {
            // set the api key...
            ConfigureInputArgs(input);

             // package it us as json...
            var json = input.Stringify();
            var content = new StringContent(json);

            // client...
            var client = new HttpClient();
            var response = await client.PostAsync(this.Url, content);

            // load it up...
            var outputJson = await response.Content.ReadAsStringAsync();
            JsonObject output = JsonObject.Parse(outputJson);

            // did the server return an error?
            bool isOk = output.GetNamedBoolean("isOk");
            if (isOk)
                return new ServiceExecuteResult(output);
            else
            {
                // we have an error returned from the server, so return that...
                string error = output.GetNamedString("error");
                return new ServiceExecuteResult(output, error);
            }
        }
    }
}
