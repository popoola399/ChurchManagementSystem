using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using ChurchManagementSystem.Web.Extentions;
using ChurchManagementSystem.Web.Services.Users.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChurchManagementSystem.Web.Services.Users
{
    public class UserManagement : IUserManagement
    {
        private readonly string _baseUrl;
        private readonly Lazy<HttpClient> _httpClient;

        public UserManagement()
        {
            _baseUrl = AppsettingsManager.Fetch("BaseUrl");

            _httpClient = new Lazy<HttpClient>(CreateHttpClient);
        }

        private HttpClient Client => _httpClient.Value;

        public async Task<Response<CreateUserResponse>> CreateUser(CreateUserRequest createUserRequest, string token)
        {
            if (!string.IsNullOrEmpty(token))
                return null;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var content = new StringContent(JObject.FromObject(createUserRequest).ToString(), Encoding.UTF8, "application/json");

            var request = await Client
                .PostAsync("/api/User/CreateUser", content)
                .ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Response<CreateUserResponse>>(response);
        }

        public async Task<Response> EditUser(EditUserRequest edithUserRequest, string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            Client.DefaultRequestHeaders.Clear();
            //Client.DefaultRequestHeaders.Remove("Authorization");
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var content = new StringContent(JObject.FromObject(edithUserRequest).ToString(), Encoding.UTF8, "application/json");

            var request = await Client
                .PutAsync("/api/Users/EditUserByAdmin", content)
                .ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Response>(response);
        }

        public async Task<Response<PagedResponse<GetUsersResponse>>> GetAllUsers(GetUsersRequestDto model)
        {
            if (string.IsNullOrEmpty(model.Token))
                return null;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + model.Token);

            var request = await Client.GetAsync($"/api/Users/GetAllUsers?PageNumber={model.PageNumber}&PageSize={model.PageSize}&Search={model.Search}").ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Response<PagedResponse<GetUsersResponse>>>(response);
        }

        public async Task<Response<List<GetUsersNameResponse>>> GetAllUsersName(string token, bool useToken = true)
        {
            if (useToken)
            {
                if (string.IsNullOrEmpty(token))
                    return null;

                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            var request = await Client.GetAsync($"/api/Users/GetAllUsersName").ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Response<List<GetUsersNameResponse>>>(response);
        }

        public async Task<Response<GetUsersResponse>> GetUserById(string token, int id)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var request = await Client.GetAsync($"/api/Users/GetUserById/{id}").ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Response<GetUsersResponse>>(response);
        }

        public async Task<Response<List<UserRolesRequest>>> GetUserRoles(string token, bool use = true)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            if (use)
            {
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            var request = await Client.GetAsync("/api/roleclaim/GetAllRoles").ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Response<List<UserRolesRequest>>>(response);
        }

        public async Task<Response<GetUsersOverviewSummaryResponse>> GetUsersOverviewSummary(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var request = await Client.GetAsync($"/api/summary/get-users-overview-summary").ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            var usersOverviewSummary = JsonConvert.DeserializeObject<Response<GetUsersOverviewSummaryResponse>>(response);

            return usersOverviewSummary;
        }


        public async Task<Response<GetUserIndividualSummaryResponse>> GetUserIndividualSummary(string token, int userId)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var request = await Client.GetAsync($"/api/summary/get-user-individual-summary?UserId={userId}").ConfigureAwait(false);

            var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

            var summary = JsonConvert.DeserializeObject<Response<GetUserIndividualSummaryResponse>>(response);

            return summary;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseUrl) };

            return client;
        }
    }
}