
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace StreetFoo.Client
{
    public abstract class NotificationWithTextBuilder<T> : NotificationBuilder<T>
    {
        protected List<string> Texts { get; set; }

        protected NotificationWithTextBuilder(IEnumerable<string> texts)
        {
            this.Texts = new List<string>(texts);
        }

        protected void UpdateTemplateText(XmlDocument xml)
        {
            // walk and combine elements...
            var textElements = xml.SelectNodes("//text");
            for (int index = 0; index < textElements.Count; index++)
            {
                if (index < this.Texts.Count)
                    textElements[index].InnerText = this.Texts[index];
                else
                    textElements[index].InnerText = string.Empty;
            }
        }
    }
}
