using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroLog
{
    public static class ILoggableExtender
    {
        public static void LogTrace(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Trace, message, ex);
        }

        public static void LogTrace(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Trace, message, ps);
        }

        public static void LogDebug(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Debug, message, ex);
        }

        public static void LogDebug(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Debug, message, ps);
        }

        public static void LogInfo(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Info, message, ex);
        }

        public static void LogInfo(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Info, message, ps);
        }

        public static void LogWarn(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Warn, message, ex);
        }

        public static void LogWarn(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Warn, message, ps);
        }

        public static void LogError(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Error, message, ex);
        }

        public static void LogError(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Error, message, ps);
        }

        public static LogEventInfo LogFatal(this ILoggable loggable, string message, Exception ex = null)
        {
            var logger = loggable.GetLogger();
            return logger.Log(LogLevel.Fatal, message, ex);
        }

        public static void LogFatal(this ILoggable loggable, string message, params string[] ps)
        {
            var logger = loggable.GetLogger();
            logger.Log(LogLevel.Fatal, message, ps);
        }

        public static Logger GetLogger(this ILoggable loggable, LoggingConfiguration config = null)
        {
            return LogManager.GetLogger(loggable, config);
        }

        public static bool IsTraceEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Trace);
        }

        public static bool IsDebugEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Debug);
        }

        public static bool IsInfoEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Info);
        }

        public static bool IsWarnEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Warn);
        }

        public static bool IsErrorEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Error);
        }

        public static bool IsFatalEnabled(this ILoggable loggable)
        {
            return loggable.IsLevelEnabled(LogLevel.Fatal);
        }

        public static bool IsLevelEnabled(this ILoggable loggable, LogLevel level)
        {
            var logger = loggable.GetLogger();
            return logger.IsEnabled(level);
        }
    }
}
