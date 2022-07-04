namespace ChurchManagementSystem.Core.Logging
{
    public class NoLogger : ILogger
    {
        public void Log(string log, params object[] data)
        {
        }
    }
}