using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace StreetFoo.Client
{
    public class BadgeNotificationBuilder : NotificationBuilder<BadgeNotification>
    {
        // what we're trying to show...
        public int Number { get; set; }

        // the engine used to update it...
        private static BadgeUpdater Updater { get; set; }

        public BadgeNotificationBuilder(int number)
        {
            this.Number = number;
        }

        static BadgeNotificationBuilder()
        {
            Updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
        }

        public override Task<BadgeNotification> SendAsync()
        {
            // create the notification and send it...
            var badge = GetNotification();
            Updater.Update(badge);

            // return...
            return Task.FromResult<BadgeNotification>(badge);
        }

        protected override BadgeNotification GetNotification()
        {
            var xml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            xml.FirstChild.Attributes.GetNamedItem("value").NodeValue = this.Number.ToString();

            return new BadgeNotification(xml);
        }
    }
}
