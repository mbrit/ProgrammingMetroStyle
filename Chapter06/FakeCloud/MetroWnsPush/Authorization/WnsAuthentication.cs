using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MetroWnsPush
{
    public class WnsAuthentication
    {
        public string Scheme { get; private set; }
        public string Token { get; private set; }

        internal WnsAuthentication(string scheme, string json)
        {
            // atm, this is fixed to understand bundle only...
            if(scheme != "bearer")
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", scheme));

            this.Scheme = "Bearer";
            this.Token = json;
        }

        internal HttpClient GetHttpClient()
        {
            // create a client and pass in the authentication...
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.Scheme, this.Token);

            return client;
        }
    }
}
