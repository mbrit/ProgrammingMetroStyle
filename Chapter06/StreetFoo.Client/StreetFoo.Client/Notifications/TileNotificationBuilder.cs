using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace StreetFoo.Client
{
    public class TileNotificationBuilder : NotificationWithTextBuilder<TileNotification>
    {
        public TileTemplateType Type { get; private set; }
        public List<string> ImageUris { get; private set; }

        private static TileUpdater Updater { get; set; }

        public TileNotificationBuilder(IEnumerable<string> texts, TileTemplateType type)
            : base(texts)
        {
            this.ImageUris = new List<string>();
            this.Type = type;
        }

        static TileNotificationBuilder()
        {
            Updater = TileUpdateManager.CreateTileUpdaterForApplication();
        }

        public override TileNotification Update()
        {
            var tile = this.GetNotification();
            Updater.Update(tile);

            return tile;
        }

        protected override TileNotification GetNotification()
        {
            var xml = TileUpdateManager.GetTemplateContent(this.Type);
            this.UpdateTemplateText(xml);

            // images...
            if (this.ImageUris.Any())
            {
                var imageElements = xml.SelectNodes("//image");
                for (int index = 0; index < imageElements.Count; index++)
                {
                    var attr = imageElements[index].Attributes.GetNamedItem("src");

                    // set...
                    if (index < this.ImageUris.Count)
                        attr.NodeValue = this.ImageUris[index];
                    else
                        attr.NodeValue = string.Empty;
                }
            }

            // return...
            return new TileNotification(xml);
        }

        private TileNotificationBuilder Replicate(TileTemplateType newType)
        {
            var newBuilder = new TileNotificationBuilder(this.Texts, newType);
            newBuilder.ImageUris = new List<string>(this.ImageUris);

            return newBuilder;
        }

        public TileNotification UpdateAndReplicate(TileTemplateType replicaType)
        {
            // update this one...
            var result = Update();

            // then copy...
            var replica = this.Replicate(replicaType);
            replica.Update();

            // return...
            return result;
        }
    }
}
