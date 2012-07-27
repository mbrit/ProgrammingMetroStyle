using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace MetroWnsPush
{
    public class WnsAuthenticator
    {
        public async Task<WnsAuthentication> AuthenticateAsync(string sid, string secret)
        {
            // create some content...
            var body = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com",
                    HttpUtility.UrlEncode(sid).Trim(), HttpUtility.UrlEncode(secret).Trim());
            var content = new StringContent(body);

            // set the type...
            content.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";

            // send it...
            var client = new HttpClient();
            var response = await client.PostAsync("https://login.live.com/accesstoken.srf", content);

            // check...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // get it...
                string json = await response.Content.ReadAsStringAsync();
                var asObject = JObject.Parse(json);

                // get...
                string scheme = (string)asObject["token_type"];
                string token = (string)asObject["access_token"];
                return new WnsAuthentication(scheme, token);
            }
            else
                throw await CreateRequestException(string.Format("Invalid status code received: {0}.", response.StatusCode), response);
        }

        internal static async Task<Exception> CreateRequestException(string message, HttpResponseMessage response)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(message);
            try
            {
                builder.Append("\r\nHeaders: ");
                foreach (var pair in response.Headers)
                {
                    builder.Append("\r\n");
                    builder.Append(pair.Key);
                    builder.Append(": ");
                    if (pair.Value is IEnumerable<string>)
                    {
                        bool first = true;
                        foreach(var buf in (IEnumerable<string>)pair.Value)
                        {
                            if (first)
                                first = false;
                            else
                                builder.Append(", ");
                            builder.Append(buf);
                        }
                    }
                    else
                        builder.Append(pair.Value);
                }

                // content?
                await response.Content.ReadAsStringAsync();
            }
            catch 
            {
                builder.Append(" (Could not get additional information)");
            }

            // return...
            return new InvalidOperationException(builder.ToString());
        }
    }
}
