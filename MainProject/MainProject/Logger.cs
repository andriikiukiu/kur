using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainProject
{
     static class MyLogger
    {
        private static Logger logger = LogManager.GetLogger("MyLogger");

        public static void LogError(string message)
        {
            logger.Error(message);
        }

        public static void LogInfo(string message)
        {
            logger.Info(message);
        }
    }
}
