using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace StreetFoo.Client
{
    public abstract class NotificationBuilder<T>
    {
        protected NotificationBuilder()
        {
        }

        protected abstract T GetNotification();

        public abstract Task<T> SendAsync();
    }
}
