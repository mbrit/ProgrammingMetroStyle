using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace StreetFoo.Client
{
    public class ToastNotificationBuilder : NotificationBuilder<ToastNotification>
    {
        // what we're trying to show...
        private ToastTemplateType Type { get; set; }
        private List<string> Texts { get; set; }

        // the engine used to update it...
        private static ToastNotifier Updater { get; set; }

        public ToastNotificationBuilder(string text, ToastTemplateType type = ToastTemplateType.ToastText01)
            : this(new string[] { text })
        {
        }

        static ToastNotificationBuilder()
        {
            Updater = ToastNotificationManager.CreateToastNotifier();
        }

        public ToastNotificationBuilder(IEnumerable<string> texts, ToastTemplateType type = ToastTemplateType.ToastText01)
        {
            this.Texts = new List<string>(texts);
            this.Type = type;
        }

        public override Task<ToastNotification> SendAsync()
        {
            var toast = this.GetNotification();
            Updater.Show(toast);

            // return...
            return Task.FromResult<ToastNotification>(toast);
        }

        protected override ToastNotification GetNotification()
        {
            var xml = ToastNotificationManager.GetTemplateContent(this.Type);

            // walk and combine elements...
            var textElements = xml.SelectNodes("//text");
            for (int index = 0; index < textElements.Count; index++)
            {
                if (index >= this.Texts.Count)
                    break;

                // set...
                textElements[index].InnerText = this.Texts[index];
            }

            // return...
            return new ToastNotification(xml);
        }
    }
}
