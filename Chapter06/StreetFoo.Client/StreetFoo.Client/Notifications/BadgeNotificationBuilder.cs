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
        public int Number { get; private set; }
        public BadgeGlyph Glyph { get; private set; }
        private bool HasGlyph { get; set; }

        // the engine used to update it...
        private static BadgeUpdater Updater { get; set; }

        public BadgeNotificationBuilder(int number)
        {
            this.Number = number;
        }

        public BadgeNotificationBuilder(BadgeGlyph glyph)
        {
            this.Glyph = glyph;
            this.HasGlyph = true;
        }

        static BadgeNotificationBuilder()
        {
            Updater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
        }

        public override BadgeNotification Update()
        {
            // create the notification and send it...
            var badge = GetNotification();
            Updater.Update(badge);

            // return...
            return badge;
        }

        protected override BadgeNotification GetNotification()
        {
            var xml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);

            // glyph?
            var attr = xml.FirstChild.Attributes.GetNamedItem("value");
            if (this.HasGlyph)
            {
                string asString = this.Glyph.ToString();
                attr.NodeValue = asString.Substring(0, 1).ToString() + asString.Substring(1);
            }
            else
                attr.NodeValue = this.Number.ToString();

            return new BadgeNotification(xml);
        }
    }
}
