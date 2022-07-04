using Newtonsoft.Json;

namespace ChurchManagementSystem.Web.Services.Users.Models
{
    public class GetUsersOverviewSummaryResponse
    {
        public int TotalUsers { get; set; }
        public int GiftRecipientCount { get; set; }
        public int InActiveUserCount { get; set; }
    }

    public class UsersOverviewSummaryViewModel
    {
        public int TotalUsers { get; set; }
        public int InActiveUserCount { get; set; }
       
    }

    public class UsersOverviewViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("membershipDate")]
        public string MembershipDate { get; set; }    
    }
}