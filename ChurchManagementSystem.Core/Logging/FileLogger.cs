using ChurchManagementSystem.Core.Common;
using ChurchManagementSystem.Core.Converters;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ChurchManagementSystem.Core.Logging
{
    public class FileLogger : ILogger
    {
        public string Logpath { get; set; }

        public FileLogger(string logpath)
        {
            Logpath = logpath;
        }

        public void Log(string log, params object[] data)
        {
            var path = $"{Logpath}/log{DateTime.Now.ToString("d").Replace("/", "")}{Constants.FileTypes.TextFile}";

            if (!Directory.Exists(Logpath))
            {
                Directory.CreateDirectory(Logpath);
            }

            var serialized = "";

            foreach (var @object in data)
            {
                serialized += $" {JsonConvert.SerializeObject(@object, Formatting.None, new PhiMaskJsonConverter())}";
            }

            if (!string.IsNullOrEmpty(serialized))
            {
                log += $" \n DATA => {serialized}";
            }

            var text = string.Concat(
                DateTime.Now.ToLongDateString(),
                " ",
                DateTime.Now.ToLongTimeString(),
                Environment.NewLine,
                log,
                Environment.NewLine,
                new string('_', 50),
                Environment.NewLine);

            File.AppendAllText(path, text);
        }
    }
}