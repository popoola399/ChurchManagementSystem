using ChurchManagementSystem.Core.Configuration;
using ChurchManagementSystem.Core.Converters;
using Exceptionless;
using Newtonsoft.Json;

namespace ChurchManagementSystem.Core.Logging
{
    public class ExceptionlessLogger : ILogger
    {
        private readonly CommonSettings _commonSettings;

        public ExceptionlessLogger(CommonSettings commonSettings)
        {
            _commonSettings = commonSettings;
        }

        public void Log(string log, params object[] data)
        {
            if (!_commonSettings.Exceptionless.UseExceptionless) return;

            var exceptionlessBuilder = ExceptionlessClient.Default.CreateLog(log);

            foreach (var @object in data)
            {
                var serialized = JsonConvert.SerializeObject(@object, Formatting.None, new PhiMaskJsonConverter());

                exceptionlessBuilder.AddObject(serialized);
            }

            exceptionlessBuilder.Submit();
        }
    }
}