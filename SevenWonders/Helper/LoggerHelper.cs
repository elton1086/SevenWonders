#define FULL
#if FULL
using log4net;
#endif
using System;

namespace SevenWonder.Helper
{
    public static class LoggerHelper
    {
#if FULL
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        public static void Debug(string message, Exception exception = null)
        {
#if FULL
            logger.Debug(message, exception);
#endif
        }

        public static void DebugFormat(string format, params object[] args)
        {
#if FULL
            logger.DebugFormat(format, args);
#endif
        }

        public static void Info(string message)
        {
#if FULL
            logger.Info(message);
#endif
        }

        public static void InfoFormat(string format, params object[] args)
        {
#if FULL
            logger.InfoFormat(format, args);
#endif
        }

        public static void Error(string message, Exception exception)
        {
#if FULL
            logger.Error(message, exception);
#endif
        }
    }
}
