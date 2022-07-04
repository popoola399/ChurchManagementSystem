using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChurchManagementSystem.Web.Extentions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChurchManagementSystem.Web.Services.UserProfile
{
    public class UserProfileManagement : IUserProfileManagement
    {
        private readonly string _baseUrl;
        private readonly Lazy<HttpClient> _httpClient;

        public UserProfileManagement()
        {
            _baseUrl = AppsettingsManager.Fetch("FeelBaseUrl");

            _httpClient = new Lazy<HttpClient>(CreateHttpClient);
        }

        private HttpClient Client => _httpClient.Value;

        public async Task<Response> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var content = new StringContent(JObject.FromObject(forgotPasswordRequest).ToString(), Encoding.UTF8, "application/json");

            var request = await Client
                .PostAsync("/api/UserProfile/UserForgotPassword", content)
                .ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (request.StatusCode == HttpStatusCode.InternalServerError || request.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<Response>(response);
            }
        }

        public async Task<Response> ResetForgotPassword(ResetForgotPasswordRequest resetForgotPasswordRequest)
        {
            var content = new StringContent(JObject.FromObject(resetForgotPasswordRequest).ToString(), Encoding.UTF8, "application/json");

            var request = await Client
                .PutAsync("/api/UserProfile/UserResetForgotPassword", content)
                .ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (request.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<Response>(response);
            }
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };

            return client;
        }
    }
}