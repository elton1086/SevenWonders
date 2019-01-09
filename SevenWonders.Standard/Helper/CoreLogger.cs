using NLog;
using SevenWonders.Contracts;
using System;

namespace SevenWonders.Helper
{
    public class CoreLogger : ICoreLogger
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message, params object[] args)
        {
            logger.Debug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            logger.Error(exception, message, args);
        }
    }
}
