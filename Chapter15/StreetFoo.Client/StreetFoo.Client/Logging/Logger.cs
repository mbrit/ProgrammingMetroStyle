using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetFoo.Client
{
    internal class Logger
    {
        private Type OwnerType { get; set; }

        internal Logger(Type type)
        {
            this.OwnerType = type;
        }

        internal static Logger GetLogger(ILoggable loggable)
        {
            return new Logger(loggable.GetType());
        }

        internal bool IsErrorEnabled
        {
            get
            {
                return true;
            }
        }

        internal void Error(string message, Exception ex)
        {
            Log(LogLevel.Error, message, ex);
        }

        private void Log(LogLevel logLevel, string message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
