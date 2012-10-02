using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    public static class ILoggableExtender
    {
        public static bool IsErrorEnabled(this ILoggable loggable)
        {
            return Logger.GetLogger(loggable).IsErrorEnabled;
        }

        public static void Error(this ILoggable loggable, string message, Exception ex = null)
        {
            Logger.GetLogger(loggable).Error(message, ex);
        }
    }
}
