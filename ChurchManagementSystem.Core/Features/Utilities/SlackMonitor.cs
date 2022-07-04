
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.Utilities
{
    public static class SlackMonitor
    {
        public static async Task<string> MakeSlackRequest(string message)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var requestData = new StringContent($"{{'text':'{DateTime.Now} {message}'}}", Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"https://hooks.slack.com/services/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", requestData);

                    var result = await response.Content.ReadAsStringAsync();

                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
