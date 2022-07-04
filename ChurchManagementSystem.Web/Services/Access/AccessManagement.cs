using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using ChurchManagementSystem.Web.Extentions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChurchManagementSystem.Web.Services.Access
{
    public class AccessManagement : IAccessManagement
    {
        private readonly string _baseUrl;
        private readonly Lazy<HttpClient> _httpClient;

        public AccessManagement()
        {
            _baseUrl = AppsettingsManager.Fetch("BaseUrl");

            _httpClient = new Lazy<HttpClient>(CreateHttpClient);
        }
                                                                                                                                                                                            
        private HttpClient Client => _httpClient.Value;

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var contentString = $"grant_type=password&scope={loginRequest.Login.Scope}&client_id={loginRequest.Login.ClientId}" +
                $"&client_secret={loginRequest.Login.ClientSecret}&username={loginRequest.Login.Email}&password={loginRequest.Login.Password}";

            var content = new StringContent(contentString, Encoding.UTF8, "application/x-www-form-urlencoded");

            var request = await Client
                .PostAsync("/connect/token", content)
                .ConfigureAwait(false);
                                                                           
            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (request.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<LoginResponse>(response);
            }
        }

        public async Task<Response> Logout(LogoutRequest logoutRequest)
        {
            var content = new StringContent(JObject.FromObject(logoutRequest).ToString(), Encoding.UTF8, "application/json");

            var request = await Client
                .PostAsync("/api/Access/Logout", content)
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