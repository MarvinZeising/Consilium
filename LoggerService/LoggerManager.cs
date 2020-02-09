using System;
using NLog;
using Server.Contracts;

namespace Server.LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger ();

        public void LogDebug (string message)
        {
            logger.Debug (message);
            Console.WriteLine ("DEBUG: " + message);
        }

        public void LogError (string message)
        {
            logger.Error (message);
            Console.WriteLine ("FAIL: " + message);
        }

        public void LogInfo (string message)
        {
            logger.Info (message);
            Console.WriteLine ("INFO: " + message);
        }

        public void LogWarn (string message)
        {
            logger.Warn (message);
            Console.WriteLine ("WARN: " + message);
        }
    }
}
