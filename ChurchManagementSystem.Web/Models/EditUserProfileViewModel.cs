namespace ChurchManagementSystem.Web.Models
{
    public class EditUserProfileViewModel
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string RoleType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public bool DisplayName { get; set; }
    }
}
