using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MetroWnsPush
{
    public class WnsAuthorization : DelegatingHandler
    {
        public string Scheme { get; private set; }
        public string Token { get; private set; }

        internal WnsAuthorization(string scheme, string json)
            : base(new HttpClientHandler())
        {
            // atm, this is fixed to understand bundle only...
            if(scheme != "bearer")
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", scheme));

            this.Scheme = "Bearer";
            this.Token = json;
        }

        internal HttpClient GetHttpClient()
        {
            return new HttpClient(this);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(this.Scheme, this.Token);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
