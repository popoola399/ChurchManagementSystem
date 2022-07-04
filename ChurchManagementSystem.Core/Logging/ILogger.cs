namespace ChurchManagementSystem.Core.Logging
{
    public interface ILogger
    {
        void Log(string log, params object[] data);
    }
}