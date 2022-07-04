using System.Collections.Generic;

namespace ChurchManagementSystem.Core.Logging
{
    public class CompositeLogger : ILogger
    {
        private readonly IEnumerable<ILogger> _loggers;

        public CompositeLogger(IEnumerable<ILogger> loggers)
        {
            _loggers = loggers;
        }

        public void Log(string log, params object[] data)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(log, data);
            }
        }
    }
}