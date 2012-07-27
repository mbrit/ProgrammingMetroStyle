using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MetroWnsPush
{
    public class WnsPusher
    {
        public Task<WnsPushResult> PushAsync(WnsAuthentication authentication, string uri, string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return PushAsync(authentication, uri, doc);
        }

        public Task<WnsPushResult> PushAsync(WnsAuthentication authentication, string uri, XmlDocument doc)
        {
            var type = InferNotificationType(doc);
            return PushAsync(authentication, uri, doc, type);
        }

        public async Task<WnsPushResult> PushAsync(WnsAuthentication authentication, string uri, XmlDocument doc, NotificationType type)
        {
            // create...
            var content = new StringContent(doc.OuterXml);
            content.Headers.ContentType.MediaType = "text/xml";

            // if...
            if(type == NotificationType.Toast)
                content.Headers.Add("X-WNS-Type", "wns/toast");
            else if (type == NotificationType.Tile)
                content.Headers.Add("X-WNS-Type", "wns/tile");
            else if (type == NotificationType.Badge)
                content.Headers.Add("X-WNS-Type", "wns/badge");
            else if (type == NotificationType.Raw)
                content.Headers.Add("X-WNS-Type", "wns/badge");
            else
                throw new NotSupportedException(string.Format("Cannot handle '{0}'.", type));

            // ok...
            var client = authentication.GetHttpClient();
            var response = await client.PostAsync(uri, content);

            // what happened?
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // what happened?
                var all = response.Headers.Where(v => v.Key == "X-WNS-NOTIFICATIONSTATUS").FirstOrDefault();
                if(string.IsNullOrEmpty(all.Key))
                    throw new InvalidOperationException("'X-WNS-NOTIFICATIONSTATUS' header not returned.");
                return (WnsPushResult)Enum.Parse(typeof(WnsPushResult), all.Value.First(), true);
            }
            else
                throw await WnsAuthenticator.CreateRequestException("Failed to post notification.", response);
        }

        private NotificationType InferNotificationType(XmlDocument doc)
        {
            var first = (XmlElement)doc.FirstChild;
            if (first.Name == "toast")
                return NotificationType.Toast;
            else if (first.Name == "tile")
                return NotificationType.Tile;
            else if (first.Name == "badge")
                return NotificationType.Badge;
            else
                return NotificationType.Raw;
        }
    }
}
