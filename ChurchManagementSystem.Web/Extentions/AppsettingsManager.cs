
using Microsoft.Extensions.Configuration;

namespace ChurchManagementSystem.Web.Extentions
{
    public static class AppsettingsManager
    {
        public static IConfiguration Configuration { get; set; }

        public static string Fetch(string fetchingKey)
        {
            string stringValue = "";

            try
            {
                stringValue = Configuration[$"Appsettings:{fetchingKey}"] ?? "";
            }
            catch
            {
                // ignored
            }
            return stringValue;
        }
    }
}