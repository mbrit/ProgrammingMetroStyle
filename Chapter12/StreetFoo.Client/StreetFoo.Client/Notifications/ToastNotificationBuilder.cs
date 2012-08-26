using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace StreetFoo.Client
{
    public class ToastNotificationBuilder : NotificationWithTextBuilder<ToastNotification>
    {
        // what we're trying to show...
        private ToastTemplateType _type;
        private bool TypeSet { get; set; }

        // the image to display...
        public string ImageUri { get; set; }

        // the engine used to update it...
        private static ToastNotifier Notifier { get; set; }

        public ToastNotificationBuilder(string text)
            : this(new string[] { text })
        {
        }

        public ToastNotificationBuilder(IEnumerable<string> texts)
            : base(texts)
        {
        }

        static ToastNotificationBuilder()
        {
            Notifier = ToastNotificationManager.CreateToastNotifier();
        }

        public ToastTemplateType Type
        {
            get
            {
                if (this.TypeSet)
                    return _type;
                else
                {
                    if (this.Texts.Count <= 1)
                    {
                        if (this.HasImageUri)
                            return ToastTemplateType.ToastImageAndText01;
                        else
                            return ToastTemplateType.ToastText01;   // just one line...
                    }
                    else if (this.Texts.Count == 2)
                    {
                        if (this.HasImageUri)
                            return ToastTemplateType.ToastImageAndText02;
                        else
                            return ToastTemplateType.ToastText02;   // 1 - bold, next normal
                    }
                    else
                    {
                        if (this.HasImageUri)
                            return ToastTemplateType.ToastImageAndText04;
                        else
                            return ToastTemplateType.ToastText04;   // 1 - bold, 2-3 normal
                    }
                }
            }
            set
            {
                _type = value;
                this.TypeSet = true;
            }
        }

        public override ToastNotification Update()
        {
            var toast = this.GetNotification();
            Notifier.Show(toast);

            // return...
            return toast;
        }

        private bool HasImageUri
        {
            get
            {
                return !(string.IsNullOrEmpty(this.ImageUri));
            }
        }

        protected override ToastNotification GetNotification()
        {
            var xml = ToastNotificationManager.GetTemplateContent(this.Type);
            UpdateTemplateText(xml);

            // do we have an image?
            if (this.HasImageUri)
            {
                var imageElement = (XmlElement)xml.SelectSingleNode("//image");
                imageElement.Attributes.GetNamedItem("src").NodeValue = this.ImageUri;
            }

            // return...
            return new ToastNotification(xml);
        }
    }
}
