namespace ChurchManagementSystem.Web.Services.Access
{
    public class AccessModel
    {
    }

    public class LoginRequest
    {
        public Login Login { get; set; }
    }

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Scope { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class LoginResponse
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
        public string Error { get; set; }
        public string Error_description { get; set; }
        public string County { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool HasWebPortalAccess { get; set; }
    }

    public class LogoutRequest
    {
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}