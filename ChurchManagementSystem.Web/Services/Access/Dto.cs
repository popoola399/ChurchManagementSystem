using System.ComponentModel.DataAnnotations;

namespace ChurchManagementSystem.Web.Services.Access
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}