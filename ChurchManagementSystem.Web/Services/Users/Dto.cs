namespace ChurchManagementSystem.Web.Services.Users
{
    public class CreateUserRequestDto
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }

    public class GetUsersRequestDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string Token { get; set; }
    }
}